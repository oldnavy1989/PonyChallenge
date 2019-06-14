using System;
using System.Threading.Tasks;
using PonyApiClient.Models;

namespace PonyApiClient
{
	public interface IPonyApiClient
	{
		Task<ApiResponse<CreateMazeModel>> CreateMazeAsync(CreateMazeRequest createMazeRequest);
		Task<ApiResponse<MazeStateModel>> GetMazeStateAsync(Guid mazeId);
		Task<ApiResponse<MakeMoveModel>> MakeMoveAsync(Guid mazeId, MakeMoveRequest makeMoveRequest);
		Task<ApiResponse<string>> VisualizeAsync(Guid mazeId);
	}
}