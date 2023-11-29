using BaoXia.Utils.Extensions;
using BaoXia.Utils.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AppIdType = System.Int32;
using ClientAddressPortType = System.Int32;
using LoginTimeStampType = System.Int64;
using TokenTypeByteType = System.Byte;
using UseIdType = System.Int32;

namespace BaoXia.Utils.Test.SecurityTest.CryptographyTest
{
	[TestClass]
	public class AESTest
	{
		const string AesKey = "6fea19fbaad04cfc89acd5f57ab0e174";

		protected class SessionToken
		{
			////////////////////////////////////////////////
			// @静态常量
			////////////////////////////////////////////////


			//  2147483647
			// -2147483648

			/// <summary>
			/// "255.255.255.255.255.255".length;
			/// </summary>
			public const int IpV6SectionsCount = 6;

			/// <summary>
			/// "255.255.255.255.255.255".length;
			/// </summary>
			public const int IpV6BytesCount = IpV6SectionsCount;


			/// <summary>
			/// 会话令牌字节数。
			/// </summary>
			public static readonly int SessionTokenBytesCount
				= sizeof(TokenTypeByteType) // TokenType
				+ sizeof(AppIdType) // AppId
				+ sizeof(UseIdType) // UserId
				+ SessionToken.IpV6BytesCount // ClientAddressIp
				+ sizeof(ClientAddressPortType) // ClientAddressPort
				+ sizeof(LoginTimeStampType); // loginTimeStamp


			////////////////////////////////////////////////
			// @自身属性
			////////////////////////////////////////////////

			public TokenTypeByteType TokenType { get; set; } = 1;

			public AppIdType AppId { get; set; }

			public UseIdType UserId { get; set; }

			/// <summary>
			/// “IP v6”地址，去除“.”后共需要18位。
			/// </summary>
			public byte[] ClientAddressIp { get; init; } = new byte[IpV6BytesCount];

			public ClientAddressPortType ClientAddressPort;

			public LoginTimeStampType LoginTimeStamp;

			////////////////////////////////////////////////
			// @自身实现
			////////////////////////////////////////////////

			public SessionToken(
				AppIdType appId,
				UseIdType userId,
				string clientAddressIp,
				ClientAddressPortType clientAddressPort,
				DateTime loginTime)
			{
				this.AppId = appId;
				this.UserId = userId;
				if (clientAddressIp?.Length > 0)
				{
					var ipStringSections = clientAddressIp.Split(".");
					if (ipStringSections?.Length > 0)
					{
						for (var ipSectionIndex = 0;
							ipSectionIndex < SessionToken.IpV6SectionsCount;
							ipSectionIndex++)
						{
							int ipNumberSection = 0;
							if (ipSectionIndex < ipStringSections.Length)
							{
								_ = int.TryParse(
									ipStringSections[ipSectionIndex],
									out ipNumberSection);
							}
							this.ClientAddressIp[ipSectionIndex]
								= (byte)ipNumberSection;
						}
					}

				}
				this.ClientAddressPort = clientAddressPort;

				this.LoginTimeStamp = loginTime.MillisecondsFrom1970();
			}

			public SessionToken(string tokenString)
			{
				if (tokenString == null
					|| tokenString.Length < 1)
				{
					return;
				}

				var sessionTokenBytes = AES.DecryptToBytesWithECB(
					tokenString,
					AESTest.AesKey);
				if (sessionTokenBytes == null
					|| sessionTokenBytes.Length < SessionToken.SessionTokenBytesCount)
				{
					return;
				}

				var sessionTokenPlaintextBytesReadIndex = 0;

				this.TokenType = sessionTokenBytes[sessionTokenPlaintextBytesReadIndex];
				sessionTokenPlaintextBytesReadIndex += sizeof(TokenTypeByteType);


				this.AppId = BitConverter.ToInt32(
					sessionTokenBytes,
					sessionTokenPlaintextBytesReadIndex);
				sessionTokenPlaintextBytesReadIndex += sizeof(AppIdType);

				this.UserId = BitConverter.ToInt32(
					sessionTokenBytes,
					sessionTokenPlaintextBytesReadIndex);
				sessionTokenPlaintextBytesReadIndex += sizeof(UseIdType);

				Array.Copy(
					sessionTokenBytes,
					sessionTokenPlaintextBytesReadIndex,
					this.ClientAddressIp,
					0,
					this.ClientAddressIp.Length);
				sessionTokenPlaintextBytesReadIndex += SessionToken.IpV6BytesCount;

				this.ClientAddressPort = BitConverter.ToInt32(
					sessionTokenBytes,
					sessionTokenPlaintextBytesReadIndex);
				sessionTokenPlaintextBytesReadIndex += sizeof(ClientAddressPortType);

				this.LoginTimeStamp = BitConverter.ToInt64(
					sessionTokenBytes,
					sessionTokenPlaintextBytesReadIndex);
				// sessionTokenPlaintextBytesReadIndex += sizeof(ClientAddressPortType);
			}

			public override string? ToString()
			{
				var tokenTypeBytes = new byte[] { this.TokenType };
				var appIdBytes = BitConverter.GetBytes(this.AppId);
				var userIdBytes = BitConverter.GetBytes(this.UserId);
				var clientAddressIpBytes = this.ClientAddressIp;
				var clientPortBytes = BitConverter.GetBytes(this.ClientAddressPort);
				var loginTimeStampBytes = BitConverter.GetBytes(this.LoginTimeStamp);

				var sessionTokenPlaintextBytes
					= new byte[SessionToken.SessionTokenBytesCount];
				{
					var sessionTokenPlaintextBytesWriteIndex = 0;

					Array.Copy(
						tokenTypeBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						tokenTypeBytes.Length);
					sessionTokenPlaintextBytesWriteIndex += tokenTypeBytes.Length;

					Array.Copy(
						appIdBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						appIdBytes.Length);
					sessionTokenPlaintextBytesWriteIndex += appIdBytes.Length;

					Array.Copy(
						userIdBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						userIdBytes.Length);
					sessionTokenPlaintextBytesWriteIndex += userIdBytes.Length;

					Array.Copy(
						clientAddressIpBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						clientAddressIpBytes.Length);
					sessionTokenPlaintextBytesWriteIndex += clientAddressIpBytes.Length;

					Array.Copy(
						clientPortBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						clientPortBytes.Length);
					sessionTokenPlaintextBytesWriteIndex += clientPortBytes.Length;

					Array.Copy(
						loginTimeStampBytes,
						0,
						sessionTokenPlaintextBytes,
						sessionTokenPlaintextBytesWriteIndex,
						loginTimeStampBytes.Length);
				}

				var sessionTokenBytes
					= BaoXia.Utils.Security.Cryptography.AES
					.EncryptToBytesWithECB(
					sessionTokenPlaintextBytes,
					AESTest.AesKey);
				if (sessionTokenBytes == null
					|| sessionTokenBytes.Length < 1)
				{
					return null;
				}

				var sessionToken
					= Convert.ToBase64String(sessionTokenBytes);
				{ }
				return sessionToken;
			}


			////////////////////////////////////////////////
			// @实现“object”
			////////////////////////////////////////////////

			public override bool Equals(object? obj)
			{
				if (obj is SessionToken sessionToken
					&& this.AppId == sessionToken.AppId
					&& this.UserId == sessionToken.UserId
					&& this.ClientAddressIp.IsItemsEqual(sessionToken.ClientAddressIp)
					&& this.ClientAddressPort == sessionToken.ClientAddressPort
					&& this.LoginTimeStamp == sessionToken.LoginTimeStamp)
				{
					return true;
				}
				return false;
			}

			public override int GetHashCode()
			{
				var sessionTokenString = this.ToString();
				if (sessionTokenString?.Length > 0)
				{
					return sessionTokenString.GetHashCode();
				}
				return 0;
			}
		}

		[TestMethod]
		public void AesEncryptTest()
		{
			var random = new Random((int)DateTime.Now.Ticks);
			var sessionToken = new SessionToken(
				random.Next(),
				random.Next(),
				//
				"255.255.255.255.255.255",
				random.Next(),
				DateTime.Now);

			var sessionTokenString = sessionToken.ToString();
			{
				// !!!
				Assert.IsTrue(sessionTokenString?.Length > 0);
				// !!!
			}

			var sessionTokenFromString = new SessionToken(sessionTokenString);
			{
				// !!!
				Assert.IsTrue(sessionToken.Equals(sessionTokenFromString));
				// !!!
			}

			System.Diagnostics.Trace.WriteLine("AESTest.AesEncryptTest:\r\n");
			for (var i = 0; i < 10; i++)
			{
				sessionToken = new SessionToken(
					random.Next(),
					random.Next(),
					//
					"255.255.255.255.255.255",
					random.Next(),
					DateTime.Now);
				//
				System.Diagnostics.Trace.WriteLine(
					(i + 1) + ", " + sessionToken.ToString());
				//
			}
		}
	}
}