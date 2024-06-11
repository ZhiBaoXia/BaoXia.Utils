using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class BytesOperatorTest
	{
		const byte TestByte = 128;
		const bool TestBoolean = true;
		const uint TestUInt = uint.MaxValue;
		const ulong TestULong = ulong.MaxValue;
		const int TestInt = int.MaxValue;
		const long TestLong = long.MaxValue;
		const float TestFloat = float.MaxValue;
		const double TestDouble = double.MaxValue;
		const string TestString_UTF8 = "UTF-8：This is a Test text. 这是一个测试文本。";
		const string TestString_UTF32 = "UTF-32：This is a Test text . 这是一个测试文本。";
		static readonly DateTime TestDateTime = DateTime.Now;

		void WriteValuesToBytesWithBytesOperator(BytesOperator bytesOperator)
		{
			bytesOperator.Clear();

			bytesOperator.Write(TestByte);
			bytesOperator.Write(TestBoolean);
			bytesOperator.Write(TestUInt);
			bytesOperator.Write(TestULong);
			bytesOperator.Write(TestInt);
			bytesOperator.Write(TestLong);
			bytesOperator.Write(TestFloat);
			bytesOperator.Write(TestDouble);
			bytesOperator.Write(TestString_UTF8);
			bytesOperator.Write(TestString_UTF32, System.Text.Encoding.UTF32);
			bytesOperator.Write(TestDateTime);
			bytesOperator.Write(TestString_UTF32, System.Text.Encoding.UTF32);
			bytesOperator.Write(TestString_UTF8);
			bytesOperator.Write(TestDouble);
			bytesOperator.Write(TestFloat);
			bytesOperator.Write(TestLong);
			bytesOperator.Write(TestInt);
		}


		void ReadValuesToBytesWithBytesOperator(BytesOperator bytesOperator)
		{
			bytesOperator.CursorPosition = 0;

			Assert.IsTrue(
				bytesOperator.TryToReadByte(out var byteValue)
				&& byteValue == TestByte);
			Assert.IsTrue(
				bytesOperator.TryToReadBool(out var boolValue)
				&& boolValue == TestBoolean);
			Assert.IsTrue(
				bytesOperator.TryToReadUInt(out var uintValue)
				&& uintValue == TestUInt);
			Assert.IsTrue(
				bytesOperator.TryToReadULong(out var ulongValue)
				&& ulongValue == TestULong);
			Assert.IsTrue(
				bytesOperator.TryToReadInt(out var intValue)
				&& intValue == TestInt);
			Assert.IsTrue(
				bytesOperator.TryToReadLong(out var longValue)
				&& longValue == TestLong);
			Assert.IsTrue(
				bytesOperator.TryToReadFloat(out var floatValue)
				&& floatValue == TestFloat);
			Assert.IsTrue(
				bytesOperator.TryToReadDouble(out var doubleValue)
				&& doubleValue == TestDouble);
			Assert.IsTrue(
				bytesOperator.TryToReadString(out var stringValueUtf8)
				&& stringValueUtf8 == TestString_UTF8);
			Assert.IsTrue(
				bytesOperator.TryToReadString(
					out var stringValueUtf32,
					System.Text.Encoding.UTF32)
				&& stringValueUtf32 == TestString_UTF32);
			Assert.IsTrue(
				bytesOperator.TryToReadDateTime(out var dateTimeValue)
				&& dateTimeValue.EqualsInMillisecond(TestDateTime));
			Assert.IsTrue(
				bytesOperator.TryToReadString(
					out stringValueUtf32,
					System.Text.Encoding.UTF32)
				&& stringValueUtf32 == TestString_UTF32);
			Assert.IsTrue(
				bytesOperator.TryToReadString(out stringValueUtf8)
				&& stringValueUtf8 == TestString_UTF8);
			Assert.IsTrue(
				bytesOperator.TryToReadDouble(out doubleValue)
				&& doubleValue == TestDouble);
			Assert.IsTrue(
				bytesOperator.TryToReadFloat(out floatValue)
				&& floatValue == TestFloat);
			Assert.IsTrue(
				bytesOperator.TryToReadLong(out longValue)
				&& longValue == TestLong);
			Assert.IsTrue(
				bytesOperator.TryToReadInt(out intValue)
				&& intValue == TestInt);
		}

		void WriteAndReadValuesWithBytesOperator(BytesOperator bytesOperator)
		{
			WriteValuesToBytesWithBytesOperator(bytesOperator);
			ReadValuesToBytesWithBytesOperator(bytesOperator);
		}

		[TestMethod]
		public void WriteAndReadTest()
		{
			var bytesOperator = new BytesOperator(new BytesBuffer());
			// !!!
			WriteAndReadValuesWithBytesOperator(bytesOperator);
			// !!!

			bytesOperator = new BytesOperator();
			// !!!
			WriteAndReadValuesWithBytesOperator(bytesOperator);
			// !!!
		}


		[TestMethod]
		public void SetBytesTest()
		{
			var bytesOperator = new BytesOperator(new BytesBuffer());
			// !!!
			WriteValuesToBytesWithBytesOperator(bytesOperator);
			// !!!
			var testBytes = bytesOperator.Bytes.ToArray();


			bytesOperator = new BytesOperator(new BytesBuffer());
			{
				bytesOperator.Bytes = testBytes;
			}
			// !!!
			ReadValuesToBytesWithBytesOperator(bytesOperator);
			// !!!


			var testBytesOffset = 10;
			var testBytes2 = new byte[testBytesOffset + testBytes.Length];
			{
				Array.Copy(
					testBytes,
					0,
					testBytes2,
					testBytesOffset,
					testBytes.Length);

				bytesOperator.Bytes = new ArraySegment<byte>(
					testBytes2,
					testBytesOffset,
					testBytes.Length);
			}
			// !!!
			ReadValuesToBytesWithBytesOperator(bytesOperator);
			// !!!

		}
	}
}
