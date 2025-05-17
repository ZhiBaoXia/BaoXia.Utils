using BaoXia.Utils.Constants;
using BaoXia.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaoXia.Utils.Extensions;

public static class StringPrivacyExtension
{
	public static List<PrivacyInfo>? GetPrivacyInfes(
		this string originalString,
		bool isIgnoreInvisibleChars = true,
		int phoneNumberLengthMin = 8,
		int englishAccountLengthMin = 6,
		int cnIdCardNumberLengthMin = 15)
	{
		List<PrivacyInfo>? privacyInfes = null;
		var privacyContentBuffer = new StringBuilder();

		var chars = originalString;
		var charsCount = chars.Length;
		for (int charIndex = 0;
			charIndex < charsCount;
			charIndex++)
		{
			var character = chars[charIndex];
			////////////////////////////////////////////////
			// 1/，电话号码判断：
			////////////////////////////////////////////////
			if (CharUtil.IsPhoneNumberChar(character))
			{
				// !!!
				privacyContentBuffer.Clear();
				privacyContentBuffer.Append(character);
				var privacyContentBeginCharIndex = charIndex;
				var privacyContentEndCharIndex = privacyContentBeginCharIndex;
				// !!!

				////////////////////////////////////////////////
				// 1/2，获取隐私信息的起始、结束字符索引值。
				////////////////////////////////////////////////
				for (int nextCharIndex = charIndex + 1;
					nextCharIndex < charsCount;
					nextCharIndex++)
				{
					var nextChar = chars[nextCharIndex];
					if (CharUtil.IsPhoneNumberChar(nextChar))
					{
						// !!!
						privacyContentBuffer.Append(nextChar);
						privacyContentEndCharIndex = nextCharIndex + 1;
						// !!!
					}
					else if (CharUtil.IsVisibleChar(nextChar)
						|| isIgnoreInvisibleChars != true)
					{
						// !!!
						break;
						// !!!
					}
				}

				////////////////////////////////////////////////
				// 2/2，获取隐私信息：电话号码。
				////////////////////////////////////////////////
				if (privacyContentBuffer.Length >= cnIdCardNumberLengthMin)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.CNIdCardNumber,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else if (privacyContentBuffer.Length >= phoneNumberLengthMin)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.PhoneNumber,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else
				{
					// !!! ⚠
					// charIndex = charIndex;
					// !!! ⚠
				}
			}
			////////////////////////////////////////////////
			// 2/，电子邮箱判断：
			////////////////////////////////////////////////
			else if (character == '@')
			{
				// !!!
				privacyContentBuffer.Clear();
				privacyContentBuffer.Append(character);
				var emailSymbolCharIndex = charIndex;
				var privacyContentBeginCharIndex = emailSymbolCharIndex;
				var privacyContentEndCharIndex = emailSymbolCharIndex;
				// !!!


				////////////////////////////////////////////////
				// 1/3，获取邮箱开始字符索引值：
				////////////////////////////////////////////////
				var emailNameLength = 0;
				for (int prevCharIndex = charIndex - 1;
					prevCharIndex >= 0;
					prevCharIndex--)
				{
					var prevChar = chars[prevCharIndex];
					if (CharUtil.IsEMailChar(prevChar))
					{
						// !!!
						privacyContentBuffer.Insert(0, prevChar);
						privacyContentBeginCharIndex = prevCharIndex;
						emailNameLength++;
						// !!!
					}
					// 注意邮箱的名称（前缀），不允许有不可见字符，和其他字符。
					else // if (CharUtil.IsVisibleChar(prevChar))
					     // || isIgnoreInvisibleChars != true)
					{
						// !!!
						break;
						// !!!
					}
				}

				////////////////////////////////////////////////
				// 2/3，获取邮箱结束字符索引值：
				////////////////////////////////////////////////
				var dotsCountInEMailDomain = 0;
				var emailLastDomainLength = 0;
				for (int nextCharIndex = charIndex + 1;
					nextCharIndex < charsCount;
					nextCharIndex++)
				{
					var nextChar = chars[nextCharIndex];
					if (CharUtil.IsEMailChar(nextChar))
					{
						// !!!
						privacyContentBuffer.Append(nextChar);
						privacyContentEndCharIndex = nextCharIndex + 1;
						//
						if (nextChar == '.')
						{
							dotsCountInEMailDomain++;
							emailLastDomainLength = 0;
						}
						else
						{
							emailLastDomainLength++;
						}
						// !!!
					}
					else if (CharUtil.IsVisibleChar(nextChar)
						|| isIgnoreInvisibleChars != true)
					{
						// !!!
						break;
						// !!!
					}
				}


				////////////////////////////////////////////////
				// 3/3，获取隐私信息：电子邮箱。
				////////////////////////////////////////////////
				if (emailNameLength > 0
					&& dotsCountInEMailDomain > 0
					&& emailLastDomainLength > 0)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.EMail,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else
				{
					// !!! ⚠
					// charIndex = charIndex;
					// !!! ⚠
				}
			}
			////////////////////////////////////////////////
			// 3/，英文账号判断：
			////////////////////////////////////////////////
			else if (CharUtil.IsEnglishAccount(character, true))
			{
				// !!!
				privacyContentBuffer.Clear();
				privacyContentBuffer.Append(character);
				var privacyContentBeginCharIndex = charIndex;
				var privacyContentEndCharIndex = privacyContentBeginCharIndex;
				// !!!


				////////////////////////////////////////////////
				// 1/2，获取隐私信息的起始、结束字符索引值。
				////////////////////////////////////////////////
				for (int nextCharIndex = charIndex + 1;
					nextCharIndex < charsCount;
					nextCharIndex++)
				{
					var nextChar = chars[nextCharIndex];
					if (CharUtil.IsEnglishAccount(nextChar))
					{
						// !!!
						privacyContentBuffer.Append(nextChar);
						privacyContentEndCharIndex = nextCharIndex + 1;
						// !!!
					}
					else if (CharUtil.IsVisibleChar(nextChar)
						|| isIgnoreInvisibleChars != true)
					{
						// !!!
						break;
						// !!!
					}
				}


				////////////////////////////////////////////////
				// 2/2，获取隐私信息：英文账号。
				////////////////////////////////////////////////
				if (privacyContentBuffer.Length >= englishAccountLengthMin)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.EnglishAccount,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else
				{
					// !!! ⚠
					// charIndex = charIndex;
					// !!! ⚠
				}
			}
			////////////////////////////////////////////////
			// 4/，中国身份证号：
			////////////////////////////////////////////////
			else if (CharUtil.IsNumberChar(character))
			{
				// !!!
				privacyContentBuffer.Clear();
				privacyContentBuffer.Append(character);
				var privacyContentBeginCharIndex = charIndex;
				var privacyContentEndCharIndex = privacyContentBeginCharIndex;
				// !!!

				////////////////////////////////////////////////
				// 1/2，获取隐私信息的起始、结束字符索引值。
				////////////////////////////////////////////////
				for (int nextCharIndex = charIndex + 1;
					nextCharIndex < charsCount;
					nextCharIndex++)
				{
					var nextChar = chars[nextCharIndex];
					// 注意这里兼容了手机号
					if (CharUtil.IsPhoneNumberChar(nextChar))
					{
						// !!!
						privacyContentBuffer.Append(nextChar);
						privacyContentEndCharIndex = nextCharIndex + 1;
						// !!!
					}
					else if (CharUtil.IsVisibleChar(nextChar)
						|| isIgnoreInvisibleChars != true)
					{
						// !!!
						break;
						// !!!
					}
				}

				////////////////////////////////////////////////
				// 2/2，获取隐私信息：电话号码。
				////////////////////////////////////////////////
				// 注意这里兼容了手机号
				if (privacyContentBuffer.Length >= cnIdCardNumberLengthMin)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.CNIdCardNumber,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else if (privacyContentBuffer.Length >= phoneNumberLengthMin)
				{
					var privacyInfo = new PrivacyInfo(
						PrivacyInfoType.PhoneNumber,
						privacyContentBeginCharIndex,
						privacyContentEndCharIndex,
						privacyContentBuffer.ToString());
					// !!!
					privacyInfes ??= [];
					privacyInfes.Add(privacyInfo);
					//
					// !!! ⚠
					charIndex = privacyContentEndCharIndex - 1;
					// !!! ⚠
				}
				else
				{
					// !!! ⚠
					// charIndex = charIndex;
					// !!! ⚠
				}
			}
		}


		// 电子邮箱账号检测时，会向前检查，
		// 因此遇到电子邮箱时，要向前合并下隐私信息。
		if (privacyInfes != null)
		{
			for (var privacyInfoIndex = privacyInfes.Count - 1;
				privacyInfoIndex > 0;
				privacyInfoIndex--)
			{
				var privacyInfo = privacyInfes[privacyInfoIndex];
				if (privacyInfo.Type == PrivacyInfoType.EMail)
				{
					var emailPrivacyContentBeginCharIndex = privacyInfo.BeginIndex;
					var emailPrivacyContentEndCharIndex = privacyInfo.EndIndex;
					for (var prevPrivacyInfoIndex = privacyInfoIndex - 1;
						prevPrivacyInfoIndex >= 0;
						prevPrivacyInfoIndex--)
					{
						var prevPrivacyInfo = privacyInfes[prevPrivacyInfoIndex];
						if (prevPrivacyInfo.EndIndex > emailPrivacyContentBeginCharIndex
							&& prevPrivacyInfo.BeginIndex < emailPrivacyContentEndCharIndex)
						{
							// !!!
							privacyInfes.RemoveAt(privacyInfoIndex);
							// !!!
						}
					}
				}
			}
		}
		return privacyInfes;
	}

	public static string ToStringByErasePrivacyContent(
		this string originalString,
		Func<string, PrivacyInfo, string?>? toErasePrivacyContent = null)
	{
		var privacyInfes = originalString.GetPrivacyInfes();
		if (privacyInfes == null
			|| privacyInfes.Count < 1)
		{
			return originalString;
		}

		var stringPrivacyInfoErasedBuffer = new StringBuilder();
		var lastTextSectionEndIndex = 0;
		foreach (var privacyInfo in privacyInfes)
		{
			var plaintextSectionBeginIndex = lastTextSectionEndIndex;
			var plaintextSectionEndIndex = privacyInfo.BeginIndex;
			var plaintextSectionLength = plaintextSectionEndIndex - plaintextSectionBeginIndex;
			if (plaintextSectionLength > 0)
			{
				var plaintext = originalString[plaintextSectionBeginIndex..plaintextSectionEndIndex];
				// !!!
				stringPrivacyInfoErasedBuffer.Append(plaintext);
				// !!!
			}

			string? privacyContentErased = null;
			if (toErasePrivacyContent != null)
			{
				privacyContentErased
					= toErasePrivacyContent(privacyInfo.PrivacyContent, privacyInfo);
			}
			else
			{
				var privacyContent = privacyInfo.PrivacyContent;
				switch (privacyInfo.Type)
				{
					default:
					case PrivacyInfoType.Unknow:
					case PrivacyInfoType.PhoneNumber:
						{
							privacyContentErased
								= privacyInfo.PrivacyContent.ToPrivacyStringForPhoneNumber(null);
						}
						break;
					case PrivacyInfoType.EnglishAccount:
						{
							privacyContentErased
								= privacyInfo.PrivacyContent.ToPrivacyStringForAccount(null);
						}
						break;
					case PrivacyInfoType.EMail:
						{
							privacyContentErased
								= privacyInfo.PrivacyContent.ToPrivacyStringForEMail(null);
						}
						break;
					case PrivacyInfoType.CNIdCardNumber:
						{
							privacyContentErased
								= privacyInfo.PrivacyContent.ToPrivacyStringForCNIdCardNumber(null);
						}
						break;
				}
			}
			if (privacyContentErased?.Length > 0)
			{
				// !!!
				stringPrivacyInfoErasedBuffer.Append(privacyContentErased);
				// !!!
			}
			// !!!
			lastTextSectionEndIndex = privacyInfo.EndIndex;
			// !!!
		}
		if (lastTextSectionEndIndex < originalString.Length)
		{
			// !!!
			var lastPlaintext = originalString[lastTextSectionEndIndex..];
			//
			stringPrivacyInfoErasedBuffer.Append(lastPlaintext);
			// !!!
		}
		return stringPrivacyInfoErasedBuffer.ToString();
	}
}