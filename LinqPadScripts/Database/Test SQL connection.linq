<Query Kind="Program" />

void Main()
{
	CanConnect("Data Source=Server;Initial Catalog=Database;Integrated Security=SSPI;").Dump();
}

public Result CanConnect(string connectionString)
{
	var r = new Result();
	
	try
	{
		var cb = new SqlConnectionStringBuilder(connectionString);
		
		//Change to something less than default 20 seconds because this just a test.
		cb.ConnectTimeout = 3;
		
		var cs = cb.ToString();
		
		using (var con = new SqlConnection(cs))
		{
			con.Open();

			using (var cmd = new SqlCommand("SELECT 1 AS Scalar;", con))
			{
				var obj = cmd.ExecuteScalar();
			}
		}

		r.IsSuccessful = true;
	}
	catch (Exception ex)
	{
		r.IsSuccessful = false;
		r.ErrorMessage = ex.Message;
	}
	
	return r;
}

public class Result
{ 
	public bool IsSuccessful { get; set; }

	public string ErrorMessage { get; set; }
}
