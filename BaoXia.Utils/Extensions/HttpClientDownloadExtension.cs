using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions
{
	public static class HttpClientDownloadExtension
	{
		////////////////////////////////////////////////
		// @“GET”相关方法。
		////////////////////////////////////////////////

		#region “GET”相关方法	

		public static async Task<byte[]?> DownloadBytesWithQueryParamsAsync(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string?>? queryParams,
			Dictionary<string, string?>? headers,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			if (httpClient == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
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
			if (response == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			return await responseContent.ReadAsBytesWithProgressActionAsync(
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesAsync(
			this HttpClient httpClient,
			string requestUri,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientDownloadExtension.DownloadBytesWithQueryParamsAsync(
				httpClient,
				requestUri,
				null,
				null,
				toReceiveDownloadProgress,
				cancellationToken);
		}

		#endregion


		////////////////////////////////////////////////
		// @“POST GetString”相关方法。
		////////////////////////////////////////////////

		#region “POST”相关方法	

		public static async Task<byte[]?> DownloadBytesByPostWithHeadersAsync(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string?>? headers,
			string requestBody,
			Encoding? requestBodyEncoding,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			if (httpClient == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
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
			if (requestBody?.Length > 0)
			{
				if (requestBodyEncoding == null)
				{
					requestBodyEncoding = Encoding.UTF8;
				}
				var requestBodyBytes = requestBodyEncoding.GetBytes(requestBody);
				var requestContent = new ByteArrayContent(requestBodyBytes);
				{ }
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			return await responseContent.ReadAsBytesWithProgressActionAsync(
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesByPostAsync(
			this HttpClient httpClient,
			string requestUri,
			string requestBody,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientDownloadExtension.DownloadBytesByPostWithHeadersAsync(
				httpClient,
				requestUri,
				null,
				requestBody,
				null,
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesByPostFormUrlEncodedWithHeadersAsync(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string?>? headers,
			Dictionary<string, string> formUrlEncoded,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			if (httpClient == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
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
			if (formUrlEncoded?.Count > 0)
			{
				var requestContent = new FormUrlEncodedContent(formUrlEncoded);
				{ }
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			return await responseContent.ReadAsBytesWithProgressActionAsync(
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesByPostFormUrlEncodedAsync(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> formUrlEncoded,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientDownloadExtension.DownloadBytesByPostFormUrlEncodedWithHeadersAsync(
				httpClient,
				requestUri,
				null,
				formUrlEncoded,
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesByPostMultipartFormWithHeadersAsync(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string?>? headers,
			Dictionary<string, string> multipartFormData,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			if (httpClient == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
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
			if (multipartFormData?.Count > 0)
			{
				var requestContent = new MultipartFormDataContent();
				{
					requestContent.Add(new FormUrlEncodedContent(multipartFormData));
				}
				requestMessage.Content = requestContent;
			}

			var response = await httpClient.SendAsync(requestMessage, cancellationToken);
			if (response == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}
			var responseContent = response.Content;
			if (responseContent == null)
			{
				toReceiveDownloadProgress?.Invoke(null, 0, 1.0F, null);
				return null;
			}

			return await responseContent.ReadAsBytesWithProgressActionAsync(
				toReceiveDownloadProgress,
				cancellationToken);
		}

		public static async Task<byte[]?> DownloadBytesByPostMultipartForm(
			this HttpClient httpClient,
			string requestUri,
			Dictionary<string, string> multipartFormData,
			Action<string?, long, float, byte[]?>? toReceiveDownloadProgress = null,
			CancellationToken cancellationToken = default)
		{
			return await HttpClientDownloadExtension.DownloadBytesByPostMultipartFormWithHeadersAsync(
				httpClient,
				requestUri,
				null,
				multipartFormData,
				toReceiveDownloadProgress,
				cancellationToken);
		}

		#endregion
	}
}
