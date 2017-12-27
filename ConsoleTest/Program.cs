using AdamOneilSoftware;
using Gander.Library;
using Gander.Library.Environments;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main()
        {
            var config = XmlSerializerHelper.Load<Configuration>(@"C:\Users\Adam\Desktop\Gander\Config.xml");
        }

        private static void Main2(string[] args)
        {
            var config = new Configuration();
            config.Login = new Form()
            {
                Id = "frmLogin"
            };

            config.Environments = new SqlEnvironment[]
            {
                new SqlEnvironment() { Name = "Dev", Url = "http://localhost:53679/", ConnectionString = "@myconnection.xml" },
                new SqlEnvironment() { Name = "Prod", Url = "http://ginseng.azurewebsites.net/", ConnectionString = "@myconnection.xml" }
            };

            config.LogoffUrl = "Account/Logoff";

            config.Roles = new Configuration.Role[]
            {
                new Configuration.Role() { Name = "Regular User" },
                new Configuration.Role() { Name = "Power User" }
            };

            XmlSerializerHelper.Save(config, @"C:\Users\Adam\Desktop\Gander\Config.xml");
        }
    }
}