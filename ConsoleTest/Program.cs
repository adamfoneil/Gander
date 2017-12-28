using AdamOneilSoftware;
using Gander.Library;
using Gander.Library.Environments;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main()
        {
            var app = XmlSerializerHelper.Load<Application>(@"C:\Users\Adam\Desktop\Gander\Ginseng.xml");
            app.Tests = new Test[]
            {
                XmlSerializerHelper.Load<Test>(@"C:\Users\Adam\Desktop\Gander\Tests\SubmitRequest.xml")
            };
            var results = app.RunTests("Dev");
            Console.ReadLine();
        }

        private static void CreateTest()
        {
            var test = new Test()
            {
                Name = "Submit Request",
                IsAuthenticated = true,
                Steps = new TestStep[]
                {                    
                    new Form()
                    {
                        Url = "Request/Create",
                        Fields = new Form.Field[]
                        {
                            new Form.Field() { ElementId = "ApplicationId", Value = 3 }, // Ginseng app
                            new Form.Field() { ElementId = "Title", Value = "Sample Request" },
                            new Form.Field() { ElementId = "TypeId", Value = 7 }, // Change
                            new Form.Field() { ElementId = "TextBody", Value = "This is a sample request" }
                        }
                    }
                }
            };

            XmlSerializerHelper.Save(test, @"C:\Users\Adam\Desktop\Gander\SubmitRequest.xml");
        }

        private static void CreateApp()
        {
            var app = new Application()
            {
                LoginForm = new Form()
                {
                    ElementId = "frmLogin",
                    Url = "Account/Login",
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

            XmlSerializerHelper.Save(app, @"C:\Users\Adam\Desktop\Gander\Ginseng.xml");
        }        
    }
}