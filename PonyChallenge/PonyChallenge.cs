using PonyApiClient;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PonyApiClient.Constants;
using PonyApiClient.Models;
using PonyChallenge.Constants;
using PonyChallenge.PathFinding;

namespace PonyChallenge
{
	public class PonyChallenge
	{
		private readonly MazeSettings _mazeSettings;

		private static readonly IPathFinder PathFinder = new PathFinder();
		private static readonly IPonyApiClient ApiClient = new PonyApiClient.PonyApiClient();

		public PonyChallenge(MazeSettings mazeSettings)
		{
			_mazeSettings = mazeSettings;
		}

		/// <summary>
		/// Idea is to recalculate path to the exit avoiding the monster on each step.
		/// If path does not exist, just stay and wait for the destiny.
		/// Apparently monster can't see pony if it stays on the place (probably a bug).
		/// Thanks to that pony always can reach the end and win.
		/// </summary>
		/// <returns></returns>
		public async Task StartAsync()
		{
			var mazeId = await CreateNewMazeAsync();

			await VisualizeMazeAsync(mazeId);

			var mazeStateModel = await GetMazeStateAsync(mazeId);
			
			var state = mazeStateModel.GameState.State.ToLowerInvariant();

			while (state == State.Active)
			{
				//Initialize maze grid
				var maze = Maze.Build(
					mazeStateModel.Pony.First(),
					mazeStateModel.Domokun.First(),
					mazeStateModel.EndPoint.First(),
					mazeStateModel.Data,
					_mazeSettings.Width,
					_mazeSettings.Height);

				//Find a path by A* algorithm
				var path = PathFinder.Find(maze);

				//Get next direction
				var ponyThought = PonyThought.SafeAndSound;
				string nextDirection;
				if (path.Any())
				{
					var directions = Maze.GetDirections(maze.Start, path);
					nextDirection = directions.First();
				}
				else
				{
					//No path exists avoiding the monster
					nextDirection = Direction.Stay;
					ponyThought = PonyThought.BadFate;
				}
				
				var makeMoveModel = await MakeMoveAsync(mazeId, nextDirection);
			
				mazeStateModel = await GetMazeStateAsync(mazeId);

				Debug.WriteLine(mazeStateModel.GameState.State + " " + mazeStateModel.GameState.StateResult);

				await VisualizeMazeAsync(mazeId, $"{_mazeSettings.PonyName}: {ponyThought}");

				state = makeMoveModel.State;

				switch (makeMoveModel.State)
				{
					case State.Over:
						Console.ForegroundColor = ConsoleColor.Red;
						break;
					case State.Won:
						Console.ForegroundColor = ConsoleColor.Green;
						break;
				}

				if (makeMoveModel.State != State.Active)
				{
					Console.WriteLine();
					Console.WriteLine(makeMoveModel.StateResult);
				}
			}

			Console.ReadLine();
		}

		private async Task<Guid> CreateNewMazeAsync()
		{
			var request = new CreateMazeRequest
			{
				MazeWidth = _mazeSettings.Width,
				MazeHeight = _mazeSettings.Height,
				Difficulty = _mazeSettings.Difficulty,
				MazePlayerName = _mazeSettings.PonyName
			};

			var createMazeResponse = await ApiClient.CreateMazeAsync(request);

			ThrowIfNotSuccess(createMazeResponse);

			return createMazeResponse.Value.MazeId;
		}

		private async Task VisualizeMazeAsync(Guid mazeId, string ponyThought = null)
		{
			var visualizeResponseInit = await ApiClient.VisualizeAsync(mazeId);

			ThrowIfNotSuccess(visualizeResponseInit);

			Render(new[] { visualizeResponseInit.Value, ponyThought });
		}

		private async Task<MazeStateModel> GetMazeStateAsync(Guid mazeId)
		{
			var mazeStateResponse = await ApiClient.GetMazeStateAsync(mazeId);

			ThrowIfNotSuccess(mazeStateResponse);

			return mazeStateResponse.Value;
		}

		private async Task<MakeMoveModel> MakeMoveAsync(Guid mazeId, string direction)
		{
			var makeMoveRequest = new MakeMoveRequest
			{
				Direction = direction
			};

			var makeMoveResponse = await ApiClient.MakeMoveAsync(mazeId, makeMoveRequest);

			ThrowIfNotSuccess(makeMoveResponse);

			return makeMoveResponse.Value;
		}

		private void ThrowIfNotSuccess<T>(ApiResponse<T> apiResponse)
		{
			if (!apiResponse.IsSuccess)
			{
				throw new Exception($"Api error: {apiResponse.ErrorMessage}");
			}
		}

		private void Render(string[] lines)
		{
			Console.SetCursorPosition(0, 0);

			foreach (var line in lines)
			{
				Console.WriteLine(line);
			}
		}
	}
}
