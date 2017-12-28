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
            var app = new Application()
            {
                LoginForm = new Form()
                {
                    ElementId = "frmLogin",
                    Fields = new Form.Field[]
                    {
                        new Form.Field() { ElementId = "Email" },
                        new Form.Field() { ElementId = "Password" }
                    }
                },
                LoginUserElementId = "Email",
                LoginPasswordElementId = "Password",
                Roles = new Role[] { new Role() { Name = "Normal User" } },
                Environments = new SqlServerEnvironment[]
                {
                    new SqlServerEnvironment()
                    {
                        Name = "Dev",
                        Url = "http://localhost:53679/",
                        ConnectionString = "@myconnection.xml",
                        Credentials = new Credential[]
                        {
                            new Credential() { Role = "Normal User", UserName = "test.user@nowhere.org", Password = "Hello.1234" }
                        }
                    },
                    new SqlServerEnvironment()
                    {
                        Name = "Prod",
                        Url = "http://ginseng.azurewebsites.net/",
                        ConnectionString = "@myconnection.xml"
                    }
                },
                LogoffUrl = "Account/Logoff"
        };

            //var config = XmlSerializerHelper.Load<Application>(@"C:\Users\Adam\Desktop\Gander\Config.xml");
            XmlSerializerHelper.Save(app, @"C:\Users\Adam\Desktop\Gander\Ginseng.xml");
        }

        private static void Main2(string[] args)
        {
            var config = new Application();
            config.LoginForm = new Form()
            {
                ElementId = "frmLogin"
            };

            config.Environments = new SqlServerEnvironment[]
            {
                new SqlServerEnvironment()
                {
                    Name = "Dev",
                    Url = "http://localhost:53679/",
                    ConnectionString = "@myconnection.xml",
                    Credentials = new Credential[] 
                    {
                        new Credential() { Role = "Regular User", UserName = "test.user@nowhere.org", Password = "Hello.1234" }
                    }
                },
                new SqlServerEnvironment()
                {
                    Name = "Prod",
                    Url = "http://ginseng.azurewebsites.net/",
                    ConnectionString = "@myconnection.xml"
                }
            };

            config.LogoffUrl = "Account/Logoff";

            config.Roles = new Role[]
            {
                new Role() { Name = "Regular User" },
                new Role() { Name = "Power User" }
            };

            XmlSerializerHelper.Save(config, @"C:\Users\Adam\Desktop\Gander\Config.xml");
        }
    }
}