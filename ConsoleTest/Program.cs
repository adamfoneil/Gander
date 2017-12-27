using AdamOneilSoftware;
using Gander.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Configuration();
            config.Login = new Form()
            {
                Id = "frmLogin"
            };

            config.Environments = new Configuration.Environment[]
            {
                new Configuration.Environment() { Name = "Dev", Url = "http://localhost:53679/", ConnectionString = "@myconnection.xml" },
                new Configuration.Environment() { Name = "Prod", Url = "http://ginseng.azurewebsites.net/", ConnectionString = "@myconnection.xml" }
            };

            config.LogoffUrl = "Account/Logoff";

            XmlSerializerHelper.Save(config, @"C:\Users\Adam\Desktop\Gander\Config.xml");
        }
    }
}
