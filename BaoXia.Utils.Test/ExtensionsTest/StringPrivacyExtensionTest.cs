using BaoXia.Utils.Constants;
using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class StringPrivacyExtensionTest
{
	[TestMethod]
	public void GetPrivacyInfesTest()
	{
		string testString;

		testString = "一二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.IsNull(privacyInfes);
		}

		testString = "1234567一二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.IsNull(privacyInfes);
		}

		testString = "abc_defg1234567一二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(1, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一80906285二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(2, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("80906285", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一+86-80906285二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(2, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("+86-80906285", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(2, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("+86-80906285", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fuwu@baoxiaruanjian.com三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(3, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("+86-80906285", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[2];
			Assert.AreEqual(PrivacyInfoType.EMail, privacyInfo.Type);
			Assert.AreEqual("fuwu@baoxiaruanjian.com", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fu wu@baoxiaruanjian.com三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(3, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("+86-80906285", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[2];
			Assert.AreEqual(PrivacyInfoType.EMail, privacyInfo.Type);
			Assert.AreEqual("wu@baoxiaruanjian.com", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fu wu@baoxiaruanjian .com三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(3, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("+86-80906285", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[2];
			Assert.AreEqual(PrivacyInfoType.EMail, privacyInfo.Type);
			Assert.AreEqual("wu@baoxiaruanjian.com", privacyInfo.PrivacyContent);
		}

		testString = "abc_defg1234567一18812341234二fu wu@baoxiaruanjian .com三四五六七。";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(3, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("18812341234", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[2];
			Assert.AreEqual(PrivacyInfoType.EMail, privacyInfo.Type);
			Assert.AreEqual("wu@baoxiaruanjian.com", privacyInfo.PrivacyContent);
		}


		testString = "abc_defg1234567一18812341234二fu wu@baoxiaruanjian .com三四五六七。123123123456781234";
		{
			var privacyInfes = testString.GetPrivacyInfes();
			Assert.AreEqual(4, privacyInfes!.Count);

			var privacyInfo = privacyInfes[0];
			Assert.AreEqual(PrivacyInfoType.EnglishAccount, privacyInfo.Type);
			Assert.AreEqual("abc_defg1234567", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[1];
			Assert.AreEqual(PrivacyInfoType.PhoneNumber, privacyInfo.Type);
			Assert.AreEqual("18812341234", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[2];
			Assert.AreEqual(PrivacyInfoType.EMail, privacyInfo.Type);
			Assert.AreEqual("wu@baoxiaruanjian.com", privacyInfo.PrivacyContent);

			privacyInfo = privacyInfes[3];
			Assert.AreEqual(PrivacyInfoType.CNIdCardNumber, privacyInfo.Type);
			Assert.AreEqual("123123123456781234", privacyInfo.PrivacyContent);
		}
	}

	[TestMethod]
	public void ToStringByErasePrivacyContentTest()
	{
		string testString;

		testString = "1234567一二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual(testString, stringByErasePrivacyContent);
		}

		testString = "一二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual(testString, stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一二三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一80906285二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一80****85二三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一+86-80906285二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一+86*****6285二三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一+86*****6285二三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fuwu@baoxiaruanjian.com三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一+86*****6285二f*********************m三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fu wu@baoxiaruanjian.com三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一+86*****6285二fu w*******************m三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一+86- 8 0 9 0 6 2 8 5二fu wu@baoxiaruanjian .com三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一+86*****6285二fu w*******************m三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一18812341234二fu wu@baoxiaruanjian .com三四五六七。";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一188****1234二fu w*******************m三四五六七。", stringByErasePrivacyContent);
		}

		testString = "abc_defg1234567一18812341234二fu wu@baoxiaruanjian .com三四五六七。123123123456781234";
		{
			var stringByErasePrivacyContent = testString.ToStringByErasePrivacyContent();
			{ }
			Assert.AreEqual("a*************7一188****1234二fu w*******************m三四五六七。123***********1234", stringByErasePrivacyContent);
		}
	}
}