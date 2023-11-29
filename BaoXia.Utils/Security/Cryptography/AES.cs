using System;
using System.Security.Cryptography;
using System.Text;

namespace BaoXia.Utils.Security.Cryptography
{
	public class AES
	{
		public const int KeyBytesCount = 32;

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
				return Array.Empty<byte>();
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
			};
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
				return Array.Empty<byte>();
			}

			byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);

			if (keyBytes == null
				|| keyBytes.Length < 1
				|| ivBytes == null
				|| ivBytes.Length < 1)
			{
				return Array.Empty<byte>();
			}

			using var aes = Aes.Create();
			{
				aes.Key = keyBytes;
				aes.IV = ivBytes;
				aes.KeySize = 256;
				aes.BlockSize = 128;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
			};
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

		public static byte[] EncryptToBytesWithECB(
			byte[] plaintextBytes,
			string key)
		{
			if (plaintextBytes == null
				|| plaintextBytes.Length < 1
				|| key == null
				|| key.Length < 1)
			{
				return Array.Empty<byte>();
			}

			byte[] keyBytes = AES.GetKeyBytesWithKeyString(key);
			using var aes = Aes.Create();
			{
				aes.Key = keyBytes;
				aes.Mode = CipherMode.ECB;
				aes.Padding = PaddingMode.PKCS7;
			};
			using var encryptor = aes.CreateEncryptor();
			var ciphertextBytes = encryptor.TransformFinalBlock(
				plaintextBytes,
				0,
				plaintextBytes.Length);
			{ }
			return ciphertextBytes;
		}

		public static byte[] DecryptToBytesWithECB(
			string ciphertext,
			string key)
		{
			if (ciphertext == null
				|| ciphertext.Length < 1
				|| key == null
				|| key.Length < 1)
			{
				return Array.Empty<byte>();
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
			};
			using var decryptor = aes.CreateDecryptor();
			byte[] plaintextBytes = decryptor.TransformFinalBlock(
				ciphertextBytes,
				0,
				ciphertextBytes.Length);
			{ }
			return plaintextBytes;
		}
	}
}
