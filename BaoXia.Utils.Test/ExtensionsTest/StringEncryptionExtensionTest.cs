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

		var ciphertext = plaintext.StringByEncrypted();
		{
			Assert.IsNotNull(ciphertext);
		}
		var plaintextByDecrypted = ciphertext.StringByDecrypted();
		{
			Assert.AreEqual(plaintext, plaintextByDecrypted);
		}
	}
}