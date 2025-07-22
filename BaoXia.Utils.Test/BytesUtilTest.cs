using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test;

[TestClass]
public class BytesUtilTest
{
	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	[TestMethod]
	public void CreateRandomBytesInLengthTest()
	{
		for (var bytesLength = 0;
			bytesLength <= 100;
			bytesLength++)
		{
			var randomBytes = BytesUtil.CreateRandomBytesInLength(bytesLength);
			// !!!
			Assert.AreEqual(bytesLength, randomBytes.Length);
			// !!!
		}

		var testBytesLength = 100;
		var testBytesList = new List<byte[]>();
		for (var testIndex = 0;
			testIndex < 100;
			testIndex++)
		{
			var testBytes = BytesUtil.CreateRandomBytesInLength(testBytesLength);
			{
				foreach (var testBytesExisted
					in
					testBytesList)
				{
					Assert.IsFalse(Array.Equals(testBytes, testBytesExisted));
				}
				testBytesList.Add(testBytes);
			}
		}
		// !!!
		Assert.AreEqual(testBytesLength, testBytesList.Count);
		// !!!
	}

	[TestMethod]
	public void Test()
	{
		var bytes = new byte[6];
		{
			bytes[0] = 0;
			bytes[1] = 1;
			bytes[2] = 2;
			bytes[3] = (byte)'A';
			bytes[4] = (byte)'b';
			bytes[5] = (byte)'c';
		}

		var base64 = BytesUtil.CreateBase64StringOfBytes(bytes);
		{
			Assert.AreEqual("AAECQWJj", base64);
		}
		var bytesFromBase64 = BytesUtil.CreateBytesFromBase64String(base64);
		{
			Assert.AreEqual(0, bytesFromBase64[0]);
			Assert.AreEqual(1, bytesFromBase64[1]);
			Assert.AreEqual(2, bytesFromBase64[2]);
			Assert.AreEqual((byte)'A', bytesFromBase64[3]);
			Assert.AreEqual((byte)'b', bytesFromBase64[4]);
			Assert.AreEqual((byte)'c', bytesFromBase64[5]);
		}
	}

	#endregion
}