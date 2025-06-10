using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class StringEncryptionExtensionTest
{
	[TestMethod]
	public void StringByEncryptedTest()
	{
		var plaintext = "Hello, World!你好，世界！";

#pragma warning disable CS0618 // 类型或成员已过时
		var ciphertext = plaintext.StringByEncrypted();
#pragma warning restore CS0618 // 类型或成员已过时
		{
			Assert.IsNotNull(ciphertext);
		}
#pragma warning disable CS0618 // 类型或成员已过时
		var plaintextByDecrypted = ciphertext.StringByDecrypted();
#pragma warning restore CS0618 // 类型或成员已过时
		{
			Assert.AreEqual(plaintext, plaintextByDecrypted);
		}
	}

	[TestMethod]
	public void ToNewCiphertextTest()
	{
		var plaintext = "Hello, World!你好，世界！";
		var ciphertext = plaintext.ToNewCiphertext();
		{
			Assert.IsNotNull(ciphertext);
		}
		var plaintextByDecrypted = ciphertext.ToPlaintext(out var nonceInBase64);
		{
			Assert.AreEqual(plaintext, plaintextByDecrypted);
			Assert.IsNotNull(nonceInBase64);
		}
		var ciphertextByNonce = plaintext.ToNewCiphertext(
			null,
			nonceInBase64);
		{
			Assert.IsNotNull(ciphertextByNonce);
			Assert.AreEqual(ciphertext, ciphertextByNonce);
		}
	}
}