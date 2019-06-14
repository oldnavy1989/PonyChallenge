using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PonyApiClient.Models;

namespace PonyApiClient
{
	public class PonyApiClient : IPonyApiClient
	{
		private const string BaseEndpoint = "https://ponychallenge.trustpilot.com/pony-challenge";

		private static readonly string CreateMazeEndpoint = $"{BaseEndpoint}/maze";
		private static readonly string GetMazeStateEndpoint = $"{BaseEndpoint}/maze/{{0}}";
		private static readonly string MakeMoveEndpoint = $"{BaseEndpoint}/maze/{{0}}";
		private static readonly string VisualizeEndpoint = $"{BaseEndpoint}/maze/{{0}}/print";

		private static HttpClient HttpClient
		{
			get
			{
				var client = new HttpClient();

				client.DefaultRequestHeaders
					.Accept
					.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				return client;
			}
		}

		public async Task<ApiResponse<CreateMazeModel>> CreateMazeAsync(CreateMazeRequest createMazeRequest)
		{
			return await PostAsync<CreateMazeModel, CreateMazeRequest>(CreateMazeEndpoint, createMazeRequest);
		}

		public async Task<ApiResponse<MazeStateModel>> GetMazeStateAsync(Guid mazeId)
		{
			var uri = string.Format(GetMazeStateEndpoint, mazeId);

			return await GetAsync<MazeStateModel>(uri);
		}

		public async Task<ApiResponse<MakeMoveModel>> MakeMoveAsync(Guid mazeId, MakeMoveRequest makeMoveRequest)
		{
			var uri = string.Format(MakeMoveEndpoint, mazeId);
			
			return await PostAsync<MakeMoveModel, MakeMoveRequest>(uri, makeMoveRequest);
		}

		public async Task<ApiResponse<string>> VisualizeAsync(Guid mazeId)
		{
			var uri = string.Format(VisualizeEndpoint, mazeId);

			return await GetStringAsync(uri);
		}

		#region Private methods

		private async Task EnrichApiResponseAsync<TResponseModel>(ApiResponse<TResponseModel> apiResponse, HttpResponseMessage httpResponseMessage)
		{
			apiResponse.IsSuccess = httpResponseMessage.IsSuccessStatusCode;

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				apiResponse.ErrorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
			}
		}

		private async Task<ApiResponse<TResponseModel>> GetAsync<TResponseModel>(string uri)
		{
			var apiResponse = new ApiResponse<TResponseModel>();

			var httpResponse = await HttpClient.GetAsync(uri);

			await EnrichApiResponseAsync(apiResponse, httpResponse);

			if (apiResponse.IsSuccess)
			{
				var content = await httpResponse.Content.ReadAsStringAsync();
				apiResponse.Value = JsonConvert.DeserializeObject<TResponseModel>(content);
			}

			return apiResponse;
		}

		private async Task<ApiResponse<string>> GetStringAsync(string uri)
		{
			var apiResponse = new ApiResponse<string>();

			var httpResponse = await HttpClient.GetAsync(uri);

			await EnrichApiResponseAsync(apiResponse, httpResponse);

			if (apiResponse.IsSuccess)
			{
				apiResponse.Value = await httpResponse.Content.ReadAsStringAsync();
			}

			return apiResponse;
		}

		private async Task<ApiResponse<TResponseModel>> PostAsync<TResponseModel, TRequestModel>(string uri, TRequestModel requestModel)
		{
			var apiResponse = new ApiResponse<TResponseModel>();

			var httpResponse = await HttpClient.PostAsJsonAsync(uri, requestModel);

			await EnrichApiResponseAsync(apiResponse, httpResponse);

			if (apiResponse.IsSuccess)
			{
				apiResponse.Value = await httpResponse.Content.ReadAsAsync<TResponseModel>();
			}

			return apiResponse;
		}

		#endregion
	}
}
