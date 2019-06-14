using Newtonsoft.Json;

namespace PonyApiClient.Models
{
	public class MakeMoveRequest
	{
		[JsonProperty(PropertyName = "direction")]
		public string Direction { get; set; }
	}
}
