using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PonyApiClient.Models
{
	public class MazeStateModel
	{
		[JsonProperty(PropertyName = "pony")]
		public List<int> Pony { get; set; }

		[JsonProperty(PropertyName = "domokun")]
		public List<int> Domokun { get; set; }

		[JsonProperty(PropertyName = "end-point")]
		public List<int> EndPoint { get; set; }

		[JsonProperty(PropertyName = "size")]
		public List<int> Size { get; set; }

		[JsonProperty(PropertyName = "difficulty")]
		public int Difficulty { get; set; }

		/// <summary>
		/// The 'data' contains an array with width*height entries.
		/// Each entry has at most 2 walls, 'west' and 'north'.
		/// If you want to find all walkable directions from place X
		/// you need to use the array entries X, X+1 and X+width to construct all walls around the place X.
		/// </summary>
		[JsonProperty(PropertyName = "data")]
		public List<List<string>> Data { get; set; }

		[JsonProperty(PropertyName = "maze-id")]
		public Guid MazeId { get; set; }

		[JsonProperty(PropertyName = "game-state")]
		public GameState GameState { get; set; }
	}
}
