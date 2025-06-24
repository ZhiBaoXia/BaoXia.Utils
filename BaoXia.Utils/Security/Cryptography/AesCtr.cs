using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace BaoXia.Utils.Security.Cryptography;

public class AesCtr
{
	////////////////////////////////////////////////
	// @静态变量
	////////////////////////////////////////////////

	#region 静态变量

	// 计数器初始值设为随机数，降低不同实例碰撞的概率
	private static int _nonceCounter = RandomNumberGenerator.GetInt32(0, int.MaxValue);

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	private static byte[] NormalizeKeyBytes(byte[] keyBytes)
	{
		// AES 密钥必须为 16/24/32 字节（AES-128/192/256）
		byte[] keyBytesNormalized;
		var keyBytesLength = keyBytes.Length;
		if (keyBytesLength < 16)
		{
			keyBytesNormalized = new byte[16];
			{
				Array.Copy(keyBytes, keyBytesNormalized, keyBytesLength);
			}
			return keyBytesNormalized;
		}
		else if (keyBytesLength == 16)
		{
			return keyBytes;
		}
		else if (keyBytesLength < 24)
		{
			keyBytesNormalized = new byte[24];
			{
				Array.Copy(keyBytes, keyBytesNormalized, keyBytesLength);
			}
			return keyBytesNormalized;
		}
		else if (keyBytesLength == 24)
		{
			return keyBytes;
		}
		else if (keyBytesLength == 32)
		{
			return keyBytes;
		}

		keyBytesNormalized = new byte[32];
		{
			Array.Copy(keyBytes, keyBytesNormalized, Math.Min(keyBytesNormalized.Length, keyBytesLength));
		}
		return keyBytesNormalized;
	}

	private static byte[] GenerateNonceBytes()
	{
		var nonceLength_Random = 12;
		var nonceLength_Counter = 4;
		var nonceLength = nonceLength_Random + nonceLength_Counter;
		byte[] nonce = new byte[nonceLength]; // 128位 Nonce（符合 CTR 标准）

		// 前 12 字节：密码学安全的随机数（96位）
		using var rng = RandomNumberGenerator.Create();
		//
		rng.GetBytes(nonce, 0, nonceLength_Random);
		//

		// 后 4 字节：原子递增的计数器（32位，大端序）
		var counterValue = Interlocked.Increment(ref _nonceCounter);
		byte[] counterBytes = BitConverter.GetBytes(counterValue);
		if (BitConverter.IsLittleEndian)
		{
			// 转换为大端序
			Array.Reverse(counterBytes);
		}
		Buffer.BlockCopy(counterBytes, 0, nonce, nonceLength_Random, nonceLength_Counter);

		return nonce;
	}

	private static void IncrementCounter(byte[] counter)
	{
		for (int counterByteIndex = counter.Length - 1;
			counterByteIndex >= 0;
			counterByteIndex--)
		{
			// 递增当前字节，处理进位
			if (++counter[counterByteIndex] != 0)
			{
				// 无进位则停止
				break;
			}
		}
	}

	public static byte[] EncryptBytes(
		byte[] plaintextBytes,
		byte[] keyBytes,
		byte[]? nonceBytes,
		out byte[] finalNonceBytes)
	{
		if (plaintextBytes.Length < 1)
		{
			// !!!
			finalNonceBytes = [];
			// !!!
			return [];
		}

		// 参数校验
		// 确保密钥长度合法
		keyBytes = NormalizeKeyBytes(keyBytes);

		// 生成唯一 Nonce
		if (nonceBytes?.Length > 0)
		{
			if (nonceBytes.Length != 16)
			{
				throw new ArgumentException($"“{nameof(nonceBytes)}”字段的长度必须位“16”（自动截断会影响“nonce”的唯一性）。");
			}
		}
		else
		{
			nonceBytes = GenerateNonceBytes();
		}
		// !!!
		finalNonceBytes = nonceBytes;
		// !!!
		// 初始计数器 = Nonce，注意计数器会被修改，
		// 所以这里需要“Clone”。
		var counter = (byte[])finalNonceBytes.Clone();


		// 初始化 AES（ECB 模式，无填充）
		using var aes = Aes.Create();
		aes.Key = keyBytes;
		aes.Mode = CipherMode.ECB;
		aes.Padding = PaddingMode.None;

		using var encryptor = aes.CreateEncryptor();
		byte[] encryptedBytes = new byte[plaintextBytes.Length];
		// 16 字节（AES 块大小）
		int blockSize = aes.BlockSize / 8;

		for (int plaiantextBlockByteIndex = 0;
			plaiantextBlockByteIndex < plaintextBytes.Length;
			plaiantextBlockByteIndex += blockSize)
		{
			////////////////////////////////////////////////
			// 1/4. 生成当前计数器块（16字节）。
			////////////////////////////////////////////////
			byte[] counterBlock = new byte[blockSize];
			Buffer.BlockCopy(counter, 0, counterBlock, 0, blockSize);

			////////////////////////////////////////////////
			// 2/4. 加密计数器块生成密钥流。
			////////////////////////////////////////////////
			byte[] keystream = encryptor.TransformFinalBlock(counterBlock, 0, blockSize);

			////////////////////////////////////////////////
			// 3/4. 明文与密钥流异或生成密文。
			////////////////////////////////////////////////
			int bytesToProcess = Math.Min(blockSize, plaintextBytes.Length - plaiantextBlockByteIndex);
			for (int byteIndexToProcess = 0;
				byteIndexToProcess < bytesToProcess;
				byteIndexToProcess++)
			{
				encryptedBytes[plaiantextBlockByteIndex + byteIndexToProcess]
					= (byte)(plaintextBytes[plaiantextBlockByteIndex + byteIndexToProcess] ^ keystream[byteIndexToProcess]);
			}

			////////////////////////////////////////////////
			// 4/4. 递增计数器（大端序全16字节递增）。
			////////////////////////////////////////////////
			//
			IncrementCounter(counter);
			//
		}
		return encryptedBytes;
	}

	public static byte[] DecryptBytes(
		byte[] ciphertextBytes,
		byte[] keyBytes,
		byte[] nonceBytes)
	{
		if (ciphertextBytes.Length < 1)
		{
			return [];
		}

		// 确保密钥长度合法
		keyBytes = NormalizeKeyBytes(keyBytes);
		if (nonceBytes.Length != 16)
		{
			throw new ArgumentException($"“{nameof(nonceBytes)}”字段的长度必须位“16”（自动截断会影响“nonce”的唯一性）。");
		}
		// 初始计数器 = Nonce，注意计数器会被修改，
		// 所以这里需要“Clone”。
		byte[] counter = (byte[])nonceBytes.Clone();

		// 初始化 AES（ECB 模式，无填充）
		using Aes aes = Aes.Create();

		aes.Key = keyBytes;
		aes.Mode = CipherMode.ECB;
		aes.Padding = PaddingMode.None;

		// 解密复用加密器生成密钥流
		using var encryptor = aes.CreateEncryptor();
		byte[] decryptedBytes = new byte[ciphertextBytes.Length];
		int blockSize = aes.BlockSize / 8; // 16 字节

		for (int ciphertextBlockByteIndex = 0;
			ciphertextBlockByteIndex < ciphertextBytes.Length;
			ciphertextBlockByteIndex += blockSize)
		{
			// 1. 生成当前计数器块（与加密时一致）
			byte[] counterBlock = new byte[blockSize];
			Buffer.BlockCopy(counter, 0, counterBlock, 0, blockSize);

			// 2. 加密计数器块生成密钥流（与加密时逻辑一致）
			byte[] keystream = encryptor.TransformFinalBlock(counterBlock, 0, blockSize);

			// 3. 密文与密钥流异或生成明文（异或自逆）
			int bytesToProcess = Math.Min(blockSize, ciphertextBytes.Length - ciphertextBlockByteIndex);
			for (int byteIndexToProcess = 0;
				byteIndexToProcess < bytesToProcess;
				byteIndexToProcess++)
			{
				decryptedBytes[ciphertextBlockByteIndex + byteIndexToProcess]
					= (byte)(ciphertextBytes[ciphertextBlockByteIndex + byteIndexToProcess] ^ keystream[byteIndexToProcess]);
			}

			// 4. 递增计数器（与加密时完全一致）
			IncrementCounter(counter);
		}

		return decryptedBytes;
	}

	public static string EncryptString(
		string plaintext,
		string key,
		string? nonceInBase64,
		out string finalNonceInBase64)
	{
		var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
		var keyBytes = Encoding.UTF8.GetBytes(key);
		var nonceBytes = nonceInBase64 != null ? Convert.FromBase64String(nonceInBase64) : null;
		{ }
		var ciphertextBytes = EncryptBytes(
			plaintextBytes,
			keyBytes,
			nonceBytes,
			out var finalNonceBytes);
		var ciphertext = Convert.ToBase64String(ciphertextBytes) ?? string.Empty;
		{
			finalNonceInBase64 = Convert.ToBase64String(finalNonceBytes);
		}
		return ciphertext;
	}

	public static string DecryptString(
		string ciphertextInBase64,
		string key,
		string nonceInBase64)
	{
		byte[] ciphertextBytes = Convert.FromBase64String(ciphertextInBase64);
		byte[] keyBytes = Encoding.UTF8.GetBytes(key);
		byte[] nonceBytes = Convert.FromBase64String(nonceInBase64);

		var plaintextBytes = DecryptBytes(
			ciphertextBytes,
			keyBytes,
			nonceBytes);
		var plaintext = Encoding.UTF8.GetString(plaintextBytes) ?? string.Empty;
		{ }
		return plaintext;
	}

	#endregion


}