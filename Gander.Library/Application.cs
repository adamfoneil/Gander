using AdamOneilSoftware;
using Gander.Library.Environments;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library
{
    [XmlInclude(typeof(SqlServerEnvironment))]
    public class Application
    {
        private Environment[] _environments;
        private Dictionary<string, Environment> _environmentDictionary;

        public Application()
        {
            // by default we'll test in Chrome only
            DriverFactories = new Func<IWebDriver>[]
            {
                () => new ChromeDriver()
            };
        }

        /// <summary>
        /// Where do test results go? Environment folder names will be automatically created
        /// </summary>
        [XmlAttribute("logPath")]
        public string LogPath { get; set; }

        /// <summary>
        /// Login form to use for all authenticated tests
        /// </summary>
        public Form LoginForm { get; set; }

        [XmlAttribute("loginUserElementId")]
        public string LoginUserElementId { get; set; } = "UserName";

        [XmlAttribute("loginPasswordElementId")]
        public string LoginPasswordElementId { get; set; } = "Password";

        /// <summary>
        /// URL to navigate to cause current user to log out
        /// </summary>
        [XmlAttribute("logOffUrl")]
        public string LogoffUrl { get; set; }

        internal void Login(IWebDriver driver, string environment, string role)
        {
            driver.Url = LoginForm.Url;
            driver.Navigate();

            var env = _environmentDictionary[environment];
            var creds = env.Credentials.Single(c => c.Role.Equals(role));

            var field = driver.FindElement(By.Id(LoginUserElementId));
            field.SendKeys(creds.UserName);

            field = driver.FindElement(By.Id(LoginPasswordElementId));
            field.SendKeys(creds.Password);

            var form = driver.FindElement(By.Id(LoginForm.ElementId));
            form.Submit();
        }

        /// <summary>
        /// List of environments available to this test configuration
        /// </summary>
        public Environment[] Environments
        {
            get { return _environments; }
            set
            {
                _environments = value;
                _environmentDictionary = _environments.ToDictionary(item => item.Name);
            }
        }

        internal void Logout(IWebDriver driver)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// List of Roles available to this test configuration
        /// </summary>
        public Role[] Roles { get; set; }

        public IEnumerable<string> GetEnvironmentNames()
        {
            return Environments.Select(e => e.Name);
        }
        
        public Environment this[string name]
        {
            get { return _environmentDictionary[name]; }
        }

        /// <summary>
        /// Names of variables that all environments must supply values for (e.g. a tenant ID used to isolate data to a particular tenant)
        /// </summary>
        public string[] VariableNames { get; set; }

        /// <summary>
        /// Array of methods that instantiate Selenium WebDrivers used with tests
        /// </summary>
        [XmlIgnore]
        public Func<IWebDriver>[] DriverFactories { get; set; }

        /// <summary>
        /// Tests that go with this application. Set internally during the <see cref="RunTests(string)"/> method by reading the <see cref="TestFilePath"/>
        /// </summary>
        [XmlIgnore]
        public Test[] Tests { get; set; }

        /// <summary>
        /// Local path from where test files are loaded
        /// </summary>
        [XmlAttribute("testFilePath")]
        public string TestFilePath { get; set; }

        /// <summary>
        /// Extension used with test source files
        /// </summary>
        [XmlAttribute("testFileSearchPattern")]
        public string TestFileSearchPattern { get; set; } = "*.xml";

        public Environment.Results RunTests(string environment)
        {
            if (Directory.Exists(TestFilePath))
            {
                var fileList = Directory.GetFiles(TestFilePath, TestFileSearchPattern, SearchOption.AllDirectories);

                List<Test> tests = new List<Test>();
                foreach (string fileName in fileList)
                {
                    var test = XmlSerializerHelper.Load<Test>(fileName);
                    test.Name = fileName.Substring(TestFilePath.Length);
                    tests.Add(test);
                }
                Tests = tests.ToArray();
            }

            var env = this[environment];
            var results = env.Execute(this);

            // todo: save results log

            return results;
        }
    }
}