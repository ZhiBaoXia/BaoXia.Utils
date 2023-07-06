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
		// @“Upload”相关方法。
		////////////////////////////////////////////////

		#region “Upload”相关方法

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			Dictionary<string, string?>? headers,
			CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrEmpty(requestUri))
			{
				return null;
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
			if (formKeyValues?.Count > 0
				|| formKeyByteArrays?.Count > 0)
			{
				var requestContent = new MultipartFormDataContent();
				if (formKeyValues?.Count > 0)
				{
					requestContent.Add(new FormUrlEncodedContent(formKeyValues));
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

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				formKeyValues,
				formKeyByteArrays,
				null,
				cancellationToken);
		}

		public static async Task<string?> UploadToGetStringAsync(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, ByteArray> formKeyByteArrays,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				null,
				formKeyByteArrays,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			Dictionary<string, string?>? headers,
			JsonSerializerOptions? jsonSerializerOptions,
			CancellationToken cancellationToken = default)
		{
			var responseString = await HttpClientUploadExtension.UploadToGetStringAsync(
				httpClient,
				requestUri,
				formKeyValues,
				formKeyByteArrays,
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

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, string>? formKeyValues,
			Dictionary<string, ByteArray> formKeyByteArrays,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				formKeyValues,
				formKeyByteArrays,
				null,
				null,
				cancellationToken);
		}

		public static async Task<ObjectType?> UploadToGetObjectAsync<ObjectType>(
			this HttpClient httpClient,
			string? requestUri,
			Dictionary<string, ByteArray> formKeyByteArrays,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientUploadExtension.UploadToGetObjectAsync<ObjectType>(
				httpClient,
				requestUri,
				null,
				formKeyByteArrays,
				null,
				null,
				cancellationToken);
		}

		#endregion
	}
}