namespace IntegrationService.Models
{
	public class ConnectionStrings
	{
		public ConnectionStrings(string postgresString, string tarantoolString)
		{
			Postgres = postgresString;
			Tarantool = tarantoolString;
		}

		public static ConnectionStrings Current { get; set; }

		public string Postgres { get; }
		public string Tarantool { get; }
	}
}