using BaoXia.Utils.Extensions;
using BaoXia.Utils.Test.Constants;
using BaoXia.Utils.Test.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static BaoXia.Utils.Test.Constants.TestToolsServiceUrls;

namespace BaoXia.Utils.Test.ExtensionsTest
{
	[TestClass]
	public class HttpClientExtensionTest
	{
		[TestMethod]
		public async Task PostToGetStringAsyncTest()
		{
			var httpClient = new HttpClient();

			var requestHeaders = new Dictionary<string, string?>()
			{
				{"content-type", "application/json" }
			};
			var requestBodyObject = new Dictionary<string, string>()
			{
				{"Name", "testBodyObject" },
				{ "Value", "Abcdefg"}
			};
			var requestBodyObjectString
				= requestBodyObject.ToJsonString(new System.Text.Json.JsonSerializerOptions()
				{
					PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
				});
			var requestEchoResponseString = await httpClient.PostToGetStringAsync(
				TestToolsServiceUrls.ApiUrls.HttpRequestEcho,
				requestBodyObjectString,
				requestHeaders,
				System.Text.Encoding.UTF8);

			Assert.IsTrue(requestEchoResponseString?.Length > 0);
			var requestEchoResponse
				= requestEchoResponseString
				.ToObjectByJsonDeserialize<HttpRequestEchoResponse>(
					new System.Text.Json.JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true,
					});

			Assert.IsTrue(requestBodyObjectString.Equals(requestEchoResponse?.RequestInfo?.HttpBodyString));
			if (requestHeaders?.Count > 0)
			{
				var responseHttpHeaders = requestEchoResponse.RequestInfo?.HttpHeaders;
				Assert.IsTrue(responseHttpHeaders?.Count > 0);
				foreach (var requestHeaderKeyValue in requestHeaders)
				{
					string? responseHttpHeaderValue = null;
					foreach (var responseHttpHeaderKeyValue in responseHttpHeaders)
					{
						if (responseHttpHeaderKeyValue.Key.EqualsIgnoreCase(requestHeaderKeyValue.Key))
						{
							responseHttpHeaderValue = responseHttpHeaderKeyValue.Value;
						}
					}
					Assert.IsTrue(responseHttpHeaderValue?.Length > 0);

					Assert.IsTrue(responseHttpHeaderValue.Contains(
						requestHeaderKeyValue.Value!,
						StringComparison.OrdinalIgnoreCase));
				}
			}
		}

		class PostBody
		{
			public string? Name { get; set; } = "请求参数名称";

			public int NumberValue { get; set; } = 1001;
		}

		[TestMethod]
		public async Task PostToJsonApiAsyncTest()
		{
			var httpClient = new HttpClient();

			var requestBody = new PostBody();
			var resposneBody = await httpClient.PostToJsonApiAsync<HttpRequestEchoResponse>(
				ApiUrls.HttpRequestEcho,
				requestBody,
				null,
				null,
				new System.Text.Json.JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				});
			{ }

			var requestInfoFromRemote = resposneBody?.RequestInfo;
			{
				Assert.IsNotNull(requestInfoFromRemote);
				Assert.IsTrue(requestInfoFromRemote.HttpMethod?.EqualsIgnoreCase("POST"));
			}
			var requestBodyFromRemote
				= requestInfoFromRemote.HttpBodyString?.ToObjectByJsonDeserialize<PostBody>();
			{
				Assert.IsNotNull(requestBodyFromRemote);
				Assert.IsTrue(requestBodyFromRemote.Name?.Equals(requestBody.Name));
				Assert.IsTrue(requestBodyFromRemote.NumberValue.Equals(requestBody.NumberValue));
			}
		}

		[TestMethod]
		public async Task PostToJsonApiWithHeadersAsyncTest()
		{
			var httpClient = new HttpClient();

			var requestBody = new PostBody();
			var resposneBody = await httpClient.PostToJsonApiAsync<HttpRequestEchoResponse>(
				ApiUrls.HttpRequestEcho,
				requestBody,
				new()
				{
			{ "TestHeader-Name", "TestHeader-Value" }
				},
				null,
				new System.Text.Json.JsonSerializerOptions()
				{
					PropertyNameCaseInsensitive = true
				});
			{ }

			var requestInfoFromRemote = resposneBody?.RequestInfo;
			{
				Assert.IsNotNull(requestInfoFromRemote);
				Assert.IsTrue(requestInfoFromRemote.HttpMethod?.EqualsIgnoreCase("POST"));
			}
			var requestHeadersFromRemote
				= requestInfoFromRemote.HttpHeaders;
			{
				string? testHeader_Value = null;
				foreach (var requestHeaderParam in requestHeadersFromRemote)
				{
					if (requestHeaderParam.Key.EqualsIgnoreCase("TestHeader-Name"))
					{
						testHeader_Value = requestHeaderParam.Value;
					}
				}
				Assert.IsTrue(testHeader_Value?.EqualsIgnoreCase("TestHeader-Value")
);
			}
			var requestBodyFromRemote
				= requestInfoFromRemote.HttpBodyString?.ToObjectByJsonDeserialize<PostBody>();
			{
				Assert.IsNotNull(requestBodyFromRemote);
				Assert.IsTrue(requestBodyFromRemote.Name?.Equals(requestBody.Name));
				Assert.IsTrue(requestBodyFromRemote.NumberValue.Equals(requestBody.NumberValue));
			}
		}

	}
}
