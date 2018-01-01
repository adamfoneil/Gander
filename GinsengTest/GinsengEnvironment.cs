using Gander.Unit;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace GinsengTest
{
	public class GinsengCredentials : Credentials
	{
		public override Form MapToForm(Form loginForm)
		{
			loginForm["Email"] = UserName;
			loginForm["Password"] = Password;
			return loginForm;
		}
	}

	public class GinsengEnvironment : Gander.Unit.Abstract.Environment
	{
		public GinsengEnvironment(string name, string url) : base(name, url)
		{
		}

		public override Credentials DefaultCredentials()
		{
			return new GinsengCredentials() { UserName = "test.user@nowhere.org", Password = "Hello.1234" };
		}

		public override IDbConnection GetConnection()
		{
			string connectionString = SeleniumHelper.DecryptStringFromFile(@"%mydocs%\ginseng.connection.txt", DataProtectionScope.CurrentUser);
			return new SqlConnection(connectionString);
		}
	}
}