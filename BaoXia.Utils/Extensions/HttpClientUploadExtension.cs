using BaoXia.Utils.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
	public static class HttpClientUploadExtension
	{

		////////////////////////////////////////////////
		// @“Send”相关方法。
		////////////////////////////////////////////////

		#region “Send”相关方法

		public static async Task<string?> SendToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			HttpMethod httpMethod,
			Dictionary<string, string?>? requestHeaders,
			HttpContent? requestBody,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}

			var requestMessage = new HttpRequestMessage(httpMethod, requestUri);
			if (requestHeaders?.Count > 0)
			{
				foreach (var headerKeyValue in requestHeaders)
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
			{
				requestMessage.Content = requestBody;
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


		public static async Task<ObjectType?> SendToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			HttpMethod httpMethod,
			string? requestUri,
			Dictionary<string, string?>? requestHeaders,
			HttpContent? requestBody,
			JsonSerializerOptions? jsonSerializerOptions = null,
			CancellationToken cancellationToken = default)
		{
			var responseString = await HttpClientUploadExtension.SendToGetStringAsync(
				httpClient,
				requestUri,
				httpMethod,
				requestHeaders,
				requestBody,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}

			jsonSerializerOptions ??= BaoXia.Utils.Environment.JsonSerializerOptions;

			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		#endregion


		////////////////////////////////////////////////
		// @“Upload”相关方法。
		////////////////////////////////////////////////

		#region “Upload”相关方法

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? requestHeaders,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
			}

			HttpContent? reqeustBody = null;
			if (formKeyValues?.Count > 0
				|| formKeyByteArrays?.Count > 0)
			{
				var requestContent = new MultipartFormDataContent();
				if (formKeyValues?.Count > 0)
				{
					if (isFormKeyValuesUrlEncoded)
					{
						requestContent.Add(new FormUrlEncodedContent(formKeyValues));
					}
					else
					{
						foreach (var formKeyValue in formKeyValues)
						{
							requestContent.Add(
								new StringContent(formKeyValue.Value),
								formKeyValue.Key);
						}
					}
				}
				if (formKeyByteArrays?.Count > 0)
				{
					foreach (var formKeyByteArray in formKeyByteArrays)
					{
						var formKey = formKeyByteArray.Key;
						if (formKey?.Length > 0)
						{
							var formByteArray = formKeyByteArray.Value;

							var formByteBytes = formByteArray.Bytes;
							if (formByteBytes?.Length > 0)
							{
								var byteArrayContent = new ByteArrayContent(formByteBytes);
								if (formByteArray.FileName?.Length > 0)
								{
									requestContent.Add(
										byteArrayContent,
										formKey,
										formByteArray.FileName);
								}
								else
								{
									requestContent.Add(
										byteArrayContent,
										formKey);
								}
							}
						}
					}
				}
				// !!!
				reqeustBody = requestContent;
				// !!!
			}

			return await SendToGetStringAsync(
				httpClient,
				requestUri,
				HttpMethod.Post,
				requestHeaders,
				reqeustBody,
				cancellationToken);
		}

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				null,
				formKeyValues,
				formKeyByteArrays,
				isFormKeyValuesUrlEncoded,
				cancellationToken);
		}

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, ByteArray> formKeyByteArrays,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				null,
				null,
				formKeyByteArrays,
				isFormKeyValuesUrlEncoded,
				cancellationToken);
		}

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string?>? requestHeaders,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			JsonSerializerOptions? jsonSerializerOptions = null,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			var responseString = await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				requestHeaders,
				formKeyValues,
				formKeyByteArrays,
				isFormKeyValuesUrlEncoded,
				cancellationToken);
			if (responseString == null
				|| responseString.Length < 1)
			{
				return default;
			}

			jsonSerializerOptions ??= BaoXia.Utils.Environment.JsonSerializerOptions;

			var @object = responseString.ToObjectByJsonDeserialize<ObjectType>(
				jsonSerializerOptions);
			{ }
			return @object;
		}

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				null,
				formKeyValues,
				formKeyByteArrays,
				null,
				isFormKeyValuesUrlEncoded,
				cancellationToken);
		}

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, ByteArray> formKeyByteArrays,
			bool isFormKeyValuesUrlEncoded = false,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				null,
				null,
				formKeyByteArrays,
				null,
				isFormKeyValuesUrlEncoded,
				cancellationToken);
		}

		#endregion
	}
}