using BaoXia.Utils.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace BaoXia.Utils.Test.SecurityTest.CryptographyTest;

[TestClass]
public class AesCtrTest
{
	[TestMethod]
	public void EncryptAndDecryptBytesTest()
	{
		// 测试用例
		var plaintext = "Hello, AES CTR in .NET Core!";
		byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
		byte[] keyBytes = RandomNumberGenerator.GetBytes(32); // 32字节（AES-256）

		// 加密
		byte[] ciphertextBytes = AesCtr.EncryptBytes(plaintextBytes, keyBytes, null, out byte[] nonce);

		// 解密
		byte[] plaintextBytesByDecrypted = AesCtr.DecryptBytes(ciphertextBytes, keyBytes, nonce);

		// 验证结果
		string plaintextByDecrypted = Encoding.UTF8.GetString(plaintextBytesByDecrypted);
		System.Diagnostics.Trace.WriteLine($"原始明文: {plaintext}");
		System.Diagnostics.Trace.WriteLine($"解密结果: {plaintextByDecrypted}"); // 应与原始明文完全一致

		// !!!
		Assert.AreEqual(plaintext, plaintextByDecrypted);
		// !!!
	}

	[TestMethod]
	public void EncryptAndDecryptStringTest()
	{
		// 测试用例
		var key = "4341F7D1FD8F4D8B88873D1CD7E387B1";
		var plaintext = "Hello World！";

		// 加密
		var ciphertext = AesCtr.EncryptString(plaintext, key, null, out var nonce);

		// 验证结果
		string plaintextByDecrypted = AesCtr.DecryptString(ciphertext, key, nonce);

		System.Diagnostics.Trace.WriteLine($"原始明文: {plaintext}");
		System.Diagnostics.Trace.WriteLine($"解密结果: {plaintextByDecrypted}"); // 应与原始明文完全一致

		// !!!
		Assert.AreEqual(plaintext, plaintextByDecrypted);
		// !!!
	}

	[TestMethod]
	public void EncryptAndDecryptStringWithNonceSpecifiedTest()
	{
		// 测试用例
		var key = "4341F7D1FD8F4D8B88873D1CD7E387B1";
		var plaintext = "Hello World！";
		var nonce = "ELMqoE+WfRbD4bRzWn8UVw==";
		//var ciphertext = "jyUYItmrPR28GPnJyQI=";

		// 加密
		var ciphertext = AesCtr.EncryptString(plaintext, key, nonce, out _);

		// 验证结果
		string plaintextByDecrypted = AesCtr.DecryptString(ciphertext, key, nonce);

		System.Diagnostics.Trace.WriteLine($"原始明文: {plaintext}");
		System.Diagnostics.Trace.WriteLine($"解密结果: {plaintextByDecrypted}"); // 应与原始明文完全一致

		// !!!
		Assert.AreEqual(plaintext, plaintextByDecrypted);
		// !!!
	}

	[TestMethod]
	public void DecryptStringTest()
	{
		// 测试用例
		var key = "F11144532E6F45F5A3D66D97B61E652F67116502E17547C0AB6B8C8ADE0155F2";
		var ciphertext = "L1vRH7Ay0EJyNx8=";
		var nonce = "ZUht7UQHEQYAlZqPN5oxlg==";
		var plaintext = "16645670001";


		// 验证结果DecryptString
		string plaintextByDecrypted = AesCtr.DecryptString(ciphertext, key, nonce);

		System.Diagnostics.Trace.WriteLine($"解密结果: {plaintextByDecrypted}"); // 应与原始明文完全一致

		// !!!
		Assert.AreEqual(plaintext, plaintextByDecrypted);
		// !!!
	}
}