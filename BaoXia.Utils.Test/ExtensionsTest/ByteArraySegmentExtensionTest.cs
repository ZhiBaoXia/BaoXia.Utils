using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test.ExtensionsTest
{
        [TestClass]
        public class ByteArraySegmentExtensionTest
        {
                protected ArraySegment<byte> BytesBoxOfBytes(byte[] bytes)
                {
                        var bytesOffset = Random.Shared.Next(10);
                        var bytesSuffixBytesCount = Random.Shared.Next(10);
                        var bytesBox = new byte[bytesOffset + bytes.Length + bytesSuffixBytesCount];
                        {
                                Array.Copy(bytes, 0, bytesBox, bytesOffset, bytes.Length);
                        }
                        return new ArraySegment<byte>(
                                bytesBox,
                                bytesOffset,
                                bytes.Length);
                }

                protected ArraySegment<byte> ByteArraySegmentOfString(string? text)
                {
                        if (string.IsNullOrEmpty(text))
                        {
                                return new ArraySegment<byte>();
                        }

                        var textBytes = text.ToUtf8Bytes();
                        { }
                        return BytesBoxOfBytes(textBytes);
                }

                [TestMethod]
                public void ToMD532()
                {
                        foreach (var testValue in ByteArrayExtensionTest_Data.ShortTexts)
                        {
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
                                        .ToMD532String()
                                        .ToUpper();
                                // !!!
                                Assert.IsTrue(hashCode?.Equals(testValue.MD532_UpperCase));
                                // !!!
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
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
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
                                        .ToMD516String()
                                        .ToUpper();
                                // !!!
                                Assert.IsTrue(hashCode?.Equals(testValue.MD516_UpperCase));
                                // !!!
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
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
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
                                        .ToSHA256()
                                        .ToUpper();
                                // !!!
                                Assert.IsTrue(hashCode?.Equals(testValue.SHA256_UpperCase));
                                // !!!
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
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
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
                                        .ToSHA512()
                                        .ToUpper();
                                // !!!
                                Assert.IsTrue(hashCode?.Equals(testValue.SHA512_UpperCase));
                                // !!!
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var hashCode
                                        = ByteArraySegmentOfString(testValue.Text)
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
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithBrotli();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithBrotli();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
                                        // !!!
                                        Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                        // !!!
                                }
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithBrotli();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithBrotli();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
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
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithGZip();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithGZip();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
                                        // !!!
                                        Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                        // !!!
                                }
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithGZip();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithGZip();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
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
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithDeflate();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithDeflate();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
                                        // !!!
                                        Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                        // !!!
                                }
                        }

                        foreach (var testValue in ByteArrayExtensionTest_Data.LongTexts)
                        {
                                var testVaueBytes = ByteArraySegmentOfString(testValue.Text);

                                var testVaueBytesCompressed = testVaueBytes.BytesByCompressWithDeflate();
                                var testValueBytesDecompressed = testVaueBytesCompressed?.BytesByDecompressWithDeflate();
                                var testValueDecompressed = StringExtension.StringWithUtf8Bytes(testValueBytesDecompressed?.ToArray());
                                // !!!
                                Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                // !!!

                                testVaueBytes = BytesBoxOfBytes(testValueBytesDecompressed!.Value.ToArray());
                                {
                                        testValueDecompressed = StringExtension.StringWithUtf8Bytes(testVaueBytes);
                                        // !!!
                                        Assert.IsTrue(testValueDecompressed.Equals(testValue.Text));
                                        // !!!
                                }
                        }
                }
        }
}
