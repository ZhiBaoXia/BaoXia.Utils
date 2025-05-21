using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
	public static class HttpClientExtension
	{

		////////////////////////////////////////////////
		// @“GET”相关方法。
		////////////////////////////////////////////////

		#region “GET”相关方法	

		public static async Task<string?> GetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? queryParams,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}
			if (queryParams?.Count > 0)
			{
				var requestUriQuery = queryParams.ToUriQuery();
				requestUri = requestUri.StringByUriAppendQueryParams(requestUriQuery);
			}

			var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			if (headers?.Count > 0)
			{
				foreach (var headerKeyValue in headers)
				{
					var headerKey = headerKeyValue.Key;
					if (headerKey?.Length > 0)
					{
						requestMessage.Headers.TryAddWithoutValidation(
							headerKey,
							headerKeyValue.Value);
					}
				}
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			var responseContent = response.Content;

			var responseString = await responseContent.ReadAsStringAsync(cancellationToken);
			{ }
			return responseString;
		}

		public static async Task<string?> GetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? queryParams,
			CancellationToken cancellationToken = default)
		{
			return await GetStringAsync(
				httpClient,
				requestUri,
				queryParams,
				null,
				cancellationToken);
		}

		public static async Task<string?> GetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			CancellationToken cancellationToken)
		{
			return await GetStringAsync(
				httpClient,
				requestUri,
				null,
				null,
				cancellationToken);
		}


		////////////////////////////////////////////////


		public static async Task<ObjectType?> GetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? queryParams,
			Dictionary<string, string?>? headers,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var responseString = await httpClient.GetStringAsync(
				requestUri,
				queryParams,
				headers,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}

			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> GetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? queryParams,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			var @object = await httpClient.GetObjectAsync<ObjectType>(
				requestUri,
				queryParams,
				headers,
				null,
				cancellationToken);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> GetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? queryParams,
			CancellationToken cancellationToken = default)
		{
			var @object = await httpClient.GetObjectAsync<ObjectType>(
				requestUri,
				queryParams,
				null,
				null,
				cancellationToken);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> GetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			CancellationToken cancellationToken)
		{
			var @object = await httpClient.GetObjectAsync<ObjectType>(
				requestUri,
				null,
				null,
				null,
				cancellationToken);
			{ }
			return @object;
		}

		#endregion


		////////////////////////////////////////////////
		// @“POST GetString”相关方法。
		////////////////////////////////////////////////

		#region “POST”相关方法	

		public static async Task<string?> PostToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			string? requestBody,
			Dictionary<string, string?>? headers,
			Encoding? requestBodyEncoding,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
			Dictionary<string, string?>? contentHeaders = null;
			if (headers?.Count > 0)
			{
				foreach (var headerKeyValue in headers)
				{
					var headerKey = headerKeyValue.Key;
					if (headerKey?.Length > 0)
					{
						if (!requestMessage.Headers.TryAddWithoutValidation(
							headerKey,
							headerKeyValue.Value))
						{
							contentHeaders ??= [];
							contentHeaders.TryAdd(headerKey, headerKeyValue.Value);
						}
					}
				}
			}
			if (requestBody?.Length > 0)
			{
				requestBodyEncoding ??= Encoding.UTF8;
				var requestBodyBytes = requestBodyEncoding.GetBytes(requestBody);
				var requestContent = new ByteArrayContent(requestBodyBytes);
				{
					if (contentHeaders?.Count > 0)
					{
						foreach (var contentHeader in contentHeaders)
						{
							requestContent.Headers.TryAddWithoutValidation(
								contentHeader.Key,
								contentHeader.Value);
						}
					}
				}
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				return null;
			}

			var responseString = await responseContent.ReadAsStringAsync(cancellationToken);
			{ }
			return responseString;
		}

		public static async Task<string?> PostToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			string? requestBody,
			CancellationToken cancellationToken = default)
		{
			return await PostToGetStringAsync(
				httpClient,
				requestUri,
				requestBody,
				null,
				null,
				cancellationToken);
		}

		public static async Task<string?> PostFormUrlEncodedToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> formUrlEncoded,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
			Dictionary<string, string?>? contentHeaders = null;
			if (headers?.Count > 0)
			{
				foreach (var headerKeyValue in headers)
				{
					var headerKey = headerKeyValue.Key;
					if (headerKey?.Length > 0)
					{
						if (!requestMessage.Headers.TryAddWithoutValidation(
							headerKey,
							headerKeyValue.Value))
						{
							contentHeaders ??= [];
							contentHeaders.TryAdd(headerKey, headerKeyValue.Value);
						}
					}
				}
			}
			if (formUrlEncoded?.Count > 0)
			{
				var requestContent = new FormUrlEncodedContent(formUrlEncoded);
				{
					if (contentHeaders?.Count > 0)
					{
						foreach (var contentHeader in contentHeaders)
						{
							requestContent.Headers.TryAddWithoutValidation(
								contentHeader.Key,
								contentHeader.Value);
						}
					}
				}
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				return null;
			}

			var responseString = await responseContent.ReadAsStringAsync(cancellationToken);
			{ }
			return responseString;
		}

		public static async Task<string?> PostFormUrlEncodedToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> formUrlEncoded,
			CancellationToken cancellationToken = default)
		{
			return await PostFormUrlEncodedToGetStringAsync(
				httpClient,
				requestUri,
				formUrlEncoded,
				null,
				cancellationToken);
		}

		public static async Task<string?> PostFormMultipartToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> multipartFormData,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
			Dictionary<string, string?>? contentHeaders = null;
			if (headers?.Count > 0)
			{
				foreach (var headerKeyValue in headers)
				{
					var headerKey = headerKeyValue.Key;
					if (headerKey?.Length > 0)
					{
						if (!requestMessage.Headers.TryAddWithoutValidation(
							headerKey,
							headerKeyValue.Value))
						{
							contentHeaders ??= [];
							contentHeaders.TryAdd(headerKey, headerKeyValue.Value);
						}
					}
				}
			}
			if (multipartFormData?.Count > 0)
			{
				var requestContent = new MultipartFormDataContent();
				{
					requestContent.Add(new FormUrlEncodedContent(multipartFormData));

					if (contentHeaders?.Count > 0)
					{
						foreach (var contentHeader in contentHeaders)
						{
							requestContent.Headers.TryAddWithoutValidation(
								contentHeader.Key,
								contentHeader.Value);
						}
					}
				}
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				return null;
			}

			var responseString = await responseContent.ReadAsStringAsync(cancellationToken);
			{ }
			return responseString;
		}

		public static async Task<string?> PostFormMultipartToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> multipartFormData,
			CancellationToken cancellationToken = default)
		{
			return await PostFormMultipartToGetStringAsync(
				httpClient,
				requestUri,
				multipartFormData,
				null,
				cancellationToken);
		}

		#endregion


		////////////////////////////////////////////////
		// @“POST GetObject”相关方法。
		////////////////////////////////////////////////

		#region “POST GetObject”相关方法。

		public static async Task<ObjectType?> PostToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			string? requestBody,
			Dictionary<string, string?>? headers,
			Encoding? requestBodyEncoding,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var responseString = await PostToGetStringAsync(
				httpClient,
				requestUri,
				requestBody,
				headers,
				requestBodyEncoding,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}
			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> PostToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			string? requestBody,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			return await PostToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				requestBody,
				headers,
				null,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> PostToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			string? requestBody,
			CancellationToken cancellationToken = default)
		{
			return await PostToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				requestBody,
				null,
				null,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> PostFormUrlEncodedToGetObject<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> formUrlEncoded,
			Dictionary<string, string?>? headers,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var responseString = await PostFormUrlEncodedToGetStringAsync(
				httpClient,
				requestUri,
				formUrlEncoded,
				headers,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}
			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> PostFormUrlEncodedToGetObject<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string> formUrlEncoded,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			return await PostFormUrlEncodedToGetObject<ObjectType>(
				httpClient,
				requestUri,
				formUrlEncoded,
				headers,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> PostFormUrlEncodedToGetObject<ObjectType>(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> formUrlEncoded,
			CancellationToken cancellationToken = default)
		{
			return await PostFormUrlEncodedToGetObject<ObjectType>(
				httpClient,
				requestUri,
				formUrlEncoded,
				null,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> PostFormMultipartToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> multipartFormData,
			Dictionary<string, string?>? headers,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var responseString = await PostFormMultipartToGetStringAsync(
				httpClient,
				requestUri,
				multipartFormData,
				headers,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}
			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> PostFormMultipartToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> multipartFormData,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			return await PostFormMultipartToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				multipartFormData,
				headers,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> PostFormMultipartToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> multipartFormData,
			CancellationToken cancellationToken = default)
		{
			return await PostFormMultipartToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				multipartFormData,
				null,
				null,
				cancellationToken);
		}

		#endregion


		////////////////////////////////////////////////
		// @常用语法糖
		////////////////////////////////////////////////

		#region 常用语法糖

		public static async Task<ResponseObjectType?> PostToJsonApiAsync<ResponseObjectType>(
			this HttpClient httpClient,
			string? apiUri,
			object? requestObject,
			Dictionary<string, string?>? headers,
			Encoding? requestBodyEncoding,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var isNeedContentTypeHeaderParam = true;
			var contentTypeName = System.Net.HttpRequestHeader
				.ContentType
				.ToHttpHeaderParamName();
			if (contentTypeName?.Length > 0)
			{
				if (headers?.Count > 0)
				{
					foreach (var headerKeyValue in headers)
					{
						if (headerKeyValue.Key.EqualsIgnoreCase(contentTypeName))
						{
							// !!!
							isNeedContentTypeHeaderParam = false;
							break;
							// !!!
						}
					}
				}
				if (isNeedContentTypeHeaderParam)
				{
					headers ??= [];
					headers.Add(contentTypeName, "application/json");
				}
			}

			var responseObject = await PostToGetObjectAsync<ResponseObjectType>(
				httpClient,
				apiUri,
				requestObject.ToJsonString(jsonSerializerOptions),
				headers,
				requestBodyEncoding,
				jsonSerializerOptions,
				cancellationToken);
			{ }
			return responseObject;
		}

		public static async Task<ResponseObjectType?> PostToJsonApiAsync<ResponseObjectType>(
			this HttpClient httpClient,
			string? apiUri,
			object? requestObject,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			var responseObject = await PostToJsonApiAsync<ResponseObjectType>(
				httpClient,
				apiUri,
				requestObject,
				headers,
				null,
				null,
				cancellationToken);
			{ }
			return responseObject;
		}

		public static async Task<ResponseObjectType?> PostToJsonApiAsync<ResponseObjectType>(
			this HttpClient httpClient,
			string? apiUri,
			object? requestObject,
			CancellationToken cancellationToken = default)
		{
			var responseObject = await PostToJsonApiAsync<ResponseObjectType>(
				httpClient,
				apiUri,
				requestObject,
				null,
				null,
				null,
				cancellationToken);
			{ }
			return responseObject;
		}

		#endregion
	}
}
