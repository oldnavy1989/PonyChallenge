using System;
using Newtonsoft.Json;

namespace PonyApiClient.Models
{
	public class CreateMazeModel
	{
		[JsonProperty(PropertyName = "maze_id")]
		public Guid MazeId { get; set; }
	}
}
