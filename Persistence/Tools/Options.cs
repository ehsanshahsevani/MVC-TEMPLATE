namespace Persistence.Tools
{
	public class Options : object
	{
		public Options(string connectionString) : base()
		{
			ConnectionString = connectionString;
		}

		// **********
		public Enums.Provider Provider { get; set; }
		// **********

		// **********
		public string ConnectionString { get; set; }
		// **********
	}
}
