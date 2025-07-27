using System;
using System.Security.Cryptography;
using System.Text;

namespace BaoXia.Utils.Security.Cryptography;

public class AES
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public const int KeyBytesCount = 32;

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
	// “ECB”模式下的加密方法：
	////////////////////////////////////////////////

	[Obsolete("ECB由于相同明文，永远加密出相同的密文，因此可通过重复明文的方式进行破解，推荐使用“EncryptToBytesWithCTR”方法替代。")]
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

	[Obsolete("由于“Aes/Ecb算法”，存在安全隐患（相同明文、密钥时，密文永远相同，因此可通过重复明文的方式进行破解），推荐使用“DecryptToBytesWithCTR”方法替代。")]
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
