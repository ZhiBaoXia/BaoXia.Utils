using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class ByteArrayExtensionTest
	{
		[TestMethod]
		public void ToMD532()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					?.ToMD532String()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.MD532_UpperCase));
				// !!!
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToMD532String()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.MD532_UpperCase));
				// !!!
			}
		}


		[TestMethod]
		public void ToMD516()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToMD516String()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.MD516_UpperCase));
				// !!!
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToMD516String()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.MD516_UpperCase));
				// !!!
			}
		}


		[TestMethod]
		public void ToSHA256()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToSHA256()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.SHA256_UpperCase));
				// !!!
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToSHA256()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.SHA256_UpperCase));
				// !!!
			}
		}

		[TestMethod]
		public void ToSHA512()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToSHA512()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.SHA512_UpperCase));
				// !!!
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var hashCode = testValue
					.Text
					?.ToUtf8Bytes()
					.ToSHA512()
					.ToUpper();
				// !!!
				Assert.IsTrue(hashCode?.Equals(testValue.SHA512_UpperCase));
				// !!!
			}
		}

		[TestMethod]
		public void BytesByCompressWithBrotli()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithBrotli();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithBrotli();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithBrotli(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
							testVaueBytesCompressed!,
							0,
							testVaueBytesCompressed2,
							1,
							testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithBrotli(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithBrotli();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithBrotli();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithBrotli(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
							testVaueBytesCompressed!,
							0,
							testVaueBytesCompressed2,
							1,
							testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithBrotli(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}
		}

		[TestMethod]
		public void BytesByCompressWithGZip()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithGZip();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithGZip();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithGZip(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
								testVaueBytesCompressed!,
								0,
								testVaueBytesCompressed2,
								1,
								testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithGZip(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithGZip();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithGZip();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithGZip(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
							testVaueBytesCompressed!,
							0,
							testVaueBytesCompressed2,
							1,
							testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithGZip(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}
		}

		[TestMethod]
		public void BytesByCompressWithDeflate()
		{
			foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithDeflate();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithDeflate();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithDeflate(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
							testVaueBytesCompressed!,
							0,
							testVaueBytesCompressed2,
							1,
							testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithDeflate(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}

			foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
			{
				var testVaueBytes = testValue.Text?.ToUtf8Bytes();

				var testVaueBytesCompressed = testVaueBytes?.BytesByCompressWithDeflate();
				var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithDeflate();
				var testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
				// !!!
				Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
				// !!!

				if (testVaueBytes != null)
				{
					var testVaueBytes2 = new byte[testVaueBytes.Length + 1];
					{
						Array.Copy(
							testVaueBytes,
							0,
							testVaueBytes2,
							1,
							testVaueBytes.Length);
					}

					testVaueBytesCompressed = testVaueBytes2?.BytesByCompressWithDeflate(1, testVaueBytes.Length);
					var testVaueBytesCompressed2 = new byte[testVaueBytesCompressed!.Length + 1];
					{
						Array.Copy(
							testVaueBytesCompressed!,
							0,
							testVaueBytesCompressed2,
							1,
							testVaueBytesCompressed.Length);
					}
					testValueBytesDecompressed = testVaueBytesCompressed2?.BytesByDecompressWithDeflate(
						1,
						testVaueBytesCompressed.Length);
					testValueDecompressed = StringUtil.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
					// !!!
					Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
					// !!!
				}
			}
		}
	}
}
