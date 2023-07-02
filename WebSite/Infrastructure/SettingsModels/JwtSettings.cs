namespace Infrastructure.ApplicationSettings;

	public class JwtSettings : object
	{
		public JwtSettings() : base()
		{
		}

		public string? SecretKey { get; set; }
		public int TokenExpiresInMinutes { get; set; }
	}
