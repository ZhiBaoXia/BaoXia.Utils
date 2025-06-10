using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace BaoXia.Utils.Security.Cryptography;

public class AES
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public const int KeyBytesCount = 32;

	#endregion


	////////////////////////////////////////////////
	// @静态变量
	////////////////////////////////////////////////

	#region 静态变量

	// 计数器初始值设为随机数，降低不同实例碰撞的概率
	private static int _nonceCounter = RandomNumberGenerator.GetInt32(0, int.MaxValue);

	#endregion

	/// <summary>
	/// 通用的密钥字节数组获取方法。
	/// </summary>
	/// <param name="key">密钥字符串。</param>
	/// <returns>密钥字符串对应、有效的密钥字节数组。</returns>

	public static byte[] GetKeyBytesWithKeyString(string key)
	{
		byte[] keyBytes = UTF8Encoding.UTF8.GetBytes(key);
		if (keyBytes.Length != KeyBytesCount)
		{
			var keyFullBytes = new byte[KeyBytesCount];
			// !!!
			Array.Copy(
				keyBytes,
				keyFullBytes,
				keyBytes.Length > KeyBytesCount
				? KeyBytesCount
				: keyBytes.Length);
			// !!!
			keyBytes = keyFullBytes;
		}
		return keyBytes;
	}


	////////////////////////////////////////////////
	// “CBC”模式下的加密方法：
	////////////////////////////////////////////////

	public static byte[] EncryptToBytesWithCBC(
		byte[] plaintextBytes,
		string key,
		byte[] ivBytes)
	{
		if (plaintextBytes == null
			|| plaintextBytes.Length < 1
			|| key == null
			|| key.Length < 1
			|| ivBytes == null
			|| ivBytes.Length < 1)
		{
			return [];
		}

		byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);
		using var aes = Aes.Create();
		{
			aes.Key = keyBytes;
			aes.IV = ivBytes;
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
		}
		;
		using var encryptor = aes.CreateEncryptor(
			keyBytes,
			ivBytes);
		var encryptedBytes = encryptor.TransformFinalBlock(
			plaintextBytes,
			0,
			plaintextBytes.Length);
		{ }
		return encryptedBytes;
	}

	public static byte[] DecryptToBytesWithCBC(
		string ciphertext,
		string key,
		byte[] ivBytes)
	{
		if (string.IsNullOrEmpty(ciphertext))
		{
			return [];
		}

		byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);

		if (keyBytes == null
			|| keyBytes.Length < 1
			|| ivBytes == null
			|| ivBytes.Length < 1)
		{
			return [];
		}

		using var aes = Aes.Create();
		{
			aes.Key = keyBytes;
			aes.IV = ivBytes;
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
		}
		;
		using var decryptor = aes.CreateDecryptor(
			keyBytes,
			ivBytes);
		var inputBytes = Convert.FromBase64String(ciphertext);
		var encryptedBytes = decryptor.TransformFinalBlock(
			inputBytes,
			0,
			inputBytes.Length);
		{ }
		return encryptedBytes;
	}

	////////////////////////////////////////////////
	// “CTR”模式下的加密方法：
	////////////////////////////////////////////////

	private static byte[] NormalizeKeyBytes(byte[] keyBytes)
	{
		if (keyBytes == null) throw new ArgumentNullException(nameof(keyBytes));

		// AES 密钥必须为 16/24/32 字节（AES-128/192/256）
		return keyBytes.Length switch
		{
			16 => keyBytes,
			24 => keyBytes,
			32 => keyBytes,
			_ => throw new ArgumentException("密钥长度必须为 16、24 或 32 字节（AES-128/192/256）", nameof(keyBytes))
		};
	}

	private static byte[] GenerateCtrNonce()
	{
		byte[] nonce = new byte[16]; // 128位 Nonce（符合 CTR 标准）

		// 前 12 字节：密码学安全的随机数（96位）
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(nonce, 0, 12);
		}

		// 后 4 字节：原子递增的计数器（32位，大端序）
		var counterValue = Interlocked.Increment(ref _nonceCounter);
		byte[] counterBytes = BitConverter.GetBytes(counterValue);
		if (BitConverter.IsLittleEndian)
		{
			// 转换为大端序
			Array.Reverse(counterBytes);
		}
		Buffer.BlockCopy(counterBytes, 0, nonce, 12, 4);

		return nonce;
	}

	private static void IncrementCounter(byte[] counter)
	{
		for (int i = counter.Length - 1; i >= 0; i--)
		{
			if (++counter[i] != 0) // 递增当前字节，处理进位
				break; // 无进位则停止
		}
	}

	public static byte[] EncryptToBytesWithCTR(
		byte[] plaintextBytes,
		byte[] keyBytes,
		byte[]? nonceBytes,
		out byte[] finalNonceBytes)
	{
		// 参数校验
		if (plaintextBytes == null) throw new ArgumentNullException(nameof(plaintextBytes));
		if (plaintextBytes.Length == 0) throw new ArgumentException("明文不能为空", nameof(plaintextBytes));
		keyBytes = NormalizeKeyBytes(keyBytes); // 确保密钥长度合法

		// 生成唯一 Nonce
		finalNonceBytes = GenerateCtrNonce();
		byte[] counter = (byte[])finalNonceBytes.Clone(); // 初始计数器 = Nonce

		// 初始化 AES（ECB 模式，无填充）
		using var aes = Aes.Create();

		aes.Key = keyBytes;
		aes.Mode = CipherMode.ECB;
		aes.Padding = PaddingMode.None;

		using var encryptor = aes.CreateEncryptor();

		byte[] encryptedBytes = new byte[plaintextBytes.Length];
		int blockSize = aes.BlockSize / 8; // 16 字节（AES 块大小）

		for (int i = 0; i < plaintextBytes.Length; i += blockSize)
		{
			// 1. 生成当前计数器块（16字节）
			byte[] counterBlock = new byte[blockSize];
			Buffer.BlockCopy(counter, 0, counterBlock, 0, blockSize);

			// 2. 加密计数器块生成密钥流
			byte[] keystream = encryptor.TransformFinalBlock(counterBlock, 0, blockSize);

			// 3. 明文与密钥流异或生成密文
			int bytesToProcess = Math.Min(blockSize, plaintextBytes.Length - i);
			for (int j = 0; j < bytesToProcess; j++)
			{
				encryptedBytes[i + j] = (byte)(plaintextBytes[i + j] ^ keystream[j]);
			}

			// 4. 递增计数器（大端序全16字节递增）
			IncrementCounter(counter);
		}

		return encryptedBytes;
	}

	public static string EncryptToBytesWithCTR(
		string plaintext,
		string key,
		string? nonce,
		out string finalNonce)
	{
		var plaintextBytes = UTF8Encoding.UTF8.GetBytes(plaintext);
		var keyBytes = UTF8Encoding.UTF8.GetBytes(key);
		var nonceBytes = nonce != null ? Convert.FromBase64String(nonce) : null;
		{ }
		var ciphertextBytes = EncryptToBytesWithCTR(
			plaintextBytes,
			keyBytes,
			nonceBytes,
			out var finalNonceBytes);
		var ciphertext = Convert.ToBase64String(ciphertextBytes) ?? string.Empty;
		{
			finalNonce = Convert.ToBase64String(finalNonceBytes);
		}
		return ciphertext;
	}

	public static byte[] DecryptToBytesWithCTR(
		byte[] ciphertextBytes,
		byte[] keyBytes,
		byte[] nonceBytes)
	{
		// 参数校验
		if (ciphertextBytes == null) throw new ArgumentNullException(nameof(ciphertextBytes));
		if (ciphertextBytes.Length == 0) throw new ArgumentException("密文不能为空", nameof(ciphertextBytes));
		keyBytes = NormalizeKeyBytes(keyBytes); // 确保密钥长度合法
		if (nonceBytes == null || nonceBytes.Length != 16) throw new ArgumentException("Nonce 必须为16字节", nameof(nonceBytes));

		byte[] counter = (byte[])nonceBytes.Clone(); // 初始计数器 = Nonce

		// 初始化 AES（ECB 模式，无填充）
		using (Aes aes = Aes.Create())
		{
			aes.Key = keyBytes;
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;

			using (ICryptoTransform encryptor = aes.CreateEncryptor()) // 解密复用加密器生成密钥流
			{
				byte[] decryptedBytes = new byte[ciphertextBytes.Length];
				int blockSize = aes.BlockSize / 8; // 16 字节

				for (int i = 0; i < ciphertextBytes.Length; i += blockSize)
				{
					// 1. 生成当前计数器块（与加密时一致）
					byte[] counterBlock = new byte[blockSize];
					Buffer.BlockCopy(counter, 0, counterBlock, 0, blockSize);

					// 2. 加密计数器块生成密钥流（与加密时逻辑一致）
					byte[] keystream = encryptor.TransformFinalBlock(counterBlock, 0, blockSize);

					// 3. 密文与密钥流异或生成明文（异或自逆）
					int bytesToProcess = Math.Min(blockSize, ciphertextBytes.Length - i);
					for (int j = 0; j < bytesToProcess; j++)
					{
						decryptedBytes[i + j] = (byte)(ciphertextBytes[i + j] ^ keystream[j]);
					}

					// 4. 递增计数器（与加密时完全一致）
					IncrementCounter(counter);
				}

				return decryptedBytes;
			}
		}
	}

	public static string DecryptToBytesWithCTR(
		string encryptedText,
		string key,
		string nonce)
	{
		byte[] ciphertextBytes = Convert.FromBase64String(encryptedText);
		byte[] keyBytes = Convert.FromBase64String(key);
		byte[] nonceBytes = Convert.FromBase64String(nonce);

		var plaintextBytes = DecryptToBytesWithCTR(
			ciphertextBytes,
			keyBytes,
			nonceBytes);
		var plaintext = UTF8Encoding.UTF8.GetString(plaintextBytes) ?? string.Empty;
		{ }
		return plaintext;
	}


	////////////////////////////////////////////////
	// “ECB”模式下的加密方法：
	////////////////////////////////////////////////

	[Obsolete("ECB由于相同铭文，永远加密出相同的密文，因此可通过重复明文的方式进行破解，推荐使用“EncryptToBytesWithCTR”方法替代。")]
	public static byte[] EncryptToBytesWithECB(
		byte[] plaintextBytes,
		string key)
	{
		if (plaintextBytes == null
			|| plaintextBytes.Length < 1
			|| key == null
			|| key.Length < 1)
		{
			return [];
		}

		byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);
		using var aes = Aes.Create();
		{
			aes.Key = keyBytes;
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.PKCS7;
		}
		;
		using var encryptor = aes.CreateEncryptor();
		var ciphertextBytes = encryptor.TransformFinalBlock(
			plaintextBytes,
			0,
			plaintextBytes.Length);
		{ }
		return ciphertextBytes;
	}

	[Obsolete("ECB由于相同铭文，永远加密出相同的密文，因此可通过重复明文的方式进行破解，推荐使用“DecryptToBytesWithCTR”方法替代。")]
	public static byte[] DecryptToBytesWithECB(
		string ciphertext,
		string key)
	{
		if (ciphertext == null
			|| ciphertext.Length < 1
			|| key == null
			|| key.Length < 1)
		{
			return [];
		}

		// !!! 注意，要针对URL规则进行以下特殊字符的替换 !!!
		ciphertext = ciphertext.Replace(" ", "+");

		byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);
		byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

		using var aes = Aes.Create();
		{
			aes.Key = keyBytes;
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.PKCS7;
		}
		;
		using var decryptor = aes.CreateDecryptor();
		byte[] plaintextBytes = decryptor.TransformFinalBlock(
			ciphertextBytes,
			0,
			ciphertextBytes.Length);
		{ }
		return plaintextBytes;
	}
}
