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

		public static void ChangeTarantool(string newConn)
		{
			Current.Tarantool = newConn;
		}
		
		public static void ChangePostgres(string newConn)
		{
			Current.Postgres = newConn;
		}
		
		public string Postgres { get; private set; }
		public string Tarantool { get; private set; }
	}
}