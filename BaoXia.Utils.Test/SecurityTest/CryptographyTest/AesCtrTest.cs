using BaoXia.Utils.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BaoXia.Utils.Test.SecurityTest.CryptographyTest;

[TestClass]
public class AesCtrTest
{
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

	private static readonly object _nonceLock = new object();
	private static uint _nonceCounter = 0; // 32位计数器（足够大多数场景）

	private static byte[] GenerateCtrNonce()
	{
		byte[] nonce = new byte[16]; // 128位 Nonce（符合 CTR 标准）

		// 前 12 字节：密码学安全的随机数（96位）
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(nonce, 0, 12);
		}

		// 后 4 字节：原子递增的计数器（32位，大端序）
		uint counterValue;
		lock (_nonceLock)
		{
			counterValue = _nonceCounter++;
		}
		byte[] counterBytes = BitConverter.GetBytes(counterValue);
		if (BitConverter.IsLittleEndian) Array.Reverse(counterBytes); // 转换为大端序
		Buffer.BlockCopy(counterBytes, 0, nonce, 12, 4);

		return nonce;
	}

	public static byte[] EncryptToBytesWithCTR(
    byte[] plaintextBytes,
    byte[] keyBytes,
    out byte[] finalNonce)
	{
		// 参数校验
		if (plaintextBytes == null) throw new ArgumentNullException(nameof(plaintextBytes));
		if (plaintextBytes.Length == 0) throw new ArgumentException("明文不能为空", nameof(plaintextBytes));
		keyBytes = NormalizeKeyBytes(keyBytes); // 确保密钥长度合法

		// 生成唯一 Nonce
		finalNonce = GenerateCtrNonce();
		byte[] counter = (byte[])finalNonce.Clone(); // 初始计数器 = Nonce

		// 初始化 AES（ECB 模式，无填充）
		using (Aes aes = Aes.Create())
		{
			aes.Key = keyBytes;
			aes.Mode = CipherMode.ECB;
			aes.Padding = PaddingMode.None;

			using (ICryptoTransform encryptor = aes.CreateEncryptor())
			{
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
		}
	}

	public static byte[] DecryptToBytesWithCTR(
    byte[] ciphertextBytes,
    byte[] keyBytes,
    byte[] nonce)
	{
		// 参数校验
		if (ciphertextBytes == null) throw new ArgumentNullException(nameof(ciphertextBytes));
		if (ciphertextBytes.Length == 0) throw new ArgumentException("密文不能为空", nameof(ciphertextBytes));
		keyBytes = NormalizeKeyBytes(keyBytes); // 确保密钥长度合法
		if (nonce == null || nonce.Length != 16) throw new ArgumentException("Nonce 必须为16字节", nameof(nonce));

		byte[] counter = (byte[])nonce.Clone(); // 初始计数器 = Nonce

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

	private static void IncrementCounter(byte[] counter)
	{
		for (int i = counter.Length - 1; i >= 0; i--)
		{
			if (++counter[i] != 0) // 递增当前字节，处理进位
				break; // 无进位则停止
		}
	}

	[TestMethod]
	public void AesCtrTest2()
	{
		// 测试用例
		byte[] plaintext = Encoding.UTF8.GetBytes("Hello, AES CTR in .NET Core!");
		byte[] key = RandomNumberGenerator.GetBytes(32); // 32字节（AES-256）

		// 加密
		byte[] ciphertext = AES.EncryptToBytesWithCTR(plaintext, key, null, out byte[] nonce);

		// 解密
		byte[] decrypted = AES.DecryptToBytesWithCTR(ciphertext, key, nonce);

		// 验证结果
		string decryptedText = Encoding.UTF8.GetString(decrypted);
		System.Diagnostics.Trace.WriteLine($"原始明文: {Encoding.UTF8.GetString(plaintext)}");
		System.Diagnostics.Trace.WriteLine($"解密结果: {decryptedText}"); // 应与原始明文完全一致
	}
}