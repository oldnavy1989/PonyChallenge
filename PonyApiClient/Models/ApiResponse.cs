namespace PonyApiClient.Models
{
	public class ApiResponse<TResponseModel>
	{
		public TResponseModel Value { get; set; }

		public bool IsSuccess { get; set; }
		public string ErrorMessage { get; set; }
	}
}
