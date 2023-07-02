using Newtonsoft.Json;

namespace Models.ErrorModel;

	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string? Message { get; set; }
		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
