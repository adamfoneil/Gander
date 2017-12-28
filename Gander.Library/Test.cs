using OpenQA.Selenium;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library
{
    [XmlInclude(typeof(Form))]
    public class Test
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates that login for the Accept and Fail roles is required to run this test. True most of the time
        /// </summary>
        [XmlAttribute("authenticated")]
        public bool IsAuthenticated { get; set; } = true;

        /// <summary>
        /// What roles should pass this test?
        /// </summary>
        public string[] PassRoles { get; set; }

        /// <summary>
        /// What roles should fail this test?
        /// </summary>
        public string[] FailRoles { get; set; }

        /// <summary>
        /// Indicates that user must logout to run this test
        /// </summary>
        [XmlAttribute("anonymous")]
        public bool IsAnonymous { get; set; }

        public TestStep[] Steps { get; set; }

        internal void Execute(IWebDriver driver, Application application, Environment environment)
        {
            if (!IsAuthenticated && !IsAnonymous) throw new InvalidOperationException("Must have either Authenticated and/or Anonymous test.");

            if (IsAuthenticated)
            {
                if (PassRoles == null) PassRoles = application.Roles?.Select(r => r.Name).ToArray();

                foreach (string role in PassRoles)
                {
                    application.Login(driver, environment.Name, role);

                    foreach (var step in Steps)
                    {
                        step.Execute(role, driver, application, environment);
                    }

                    application.Logout(driver);
                }
            }

            if (IsAnonymous)
            {
                application.Logout(driver);

                foreach (var step in Steps)
                {
                    step.Execute("<anonymous>", driver, application, environment);
                }
            }
        }
    }
}