using Newtonsoft.Json;

namespace PonyApiClient.Models
{
	public class GameState
	{
		[JsonProperty(PropertyName = "state")]
		public string State { get; set; }

		[JsonProperty(PropertyName = "state-result")]
		public string StateResult { get; set; }
	}
}
