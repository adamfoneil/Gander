using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library
{
    /// <summary>
    /// Base URL where tests can be run
    /// </summary>
    public abstract class Environment
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Literal connection string, or filename if prefixed with @
        /// </summary>
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Base URL for all tests in this environment
        /// </summary>
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// Account information for the various user roles that will be running tests
        /// </summary>
        public Credential[] Credentials { get; set; }

        /// <summary>
        /// Filename where encrypted credentials are saved to and loaded from
        /// </summary>
        public string EncryptedCredentialSource { get; set; }

        public void EncryptCredentials(string fileName)
        {
        }

        public void DecryptCredentials()
        {
        }

        public void EncryptConnectionString(string fileName)
        {
        }

        public abstract IDbConnection GetConnection();

        public Variable[] Variables { get; set; }

        public class Variable
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }
        }

        public class Results
        {
            public IEnumerable<string> Passed { get; set; }
            public IEnumerable<TestException> Failed { get; set; }
        }

        internal Results RunTests(Application application)
        {
            // any vars defined by application that don't have enviro-specific values?
            var missingValues = application?.VariableNames.Where(name => !Variables?.Any(var => var.Name.Equals(name)) ?? false);

            if (missingValues?.Any() ?? false)
            {
                throw new Exception($"The {Name} environment is missing values for the variable name(s): {string.Join(", ", missingValues)}");
            }

            if (!application?.DriverFactories?.Any() ?? false)
            {
                throw new Exception("Application must have at least one driver factory.");
            }

            List<string> passed = new List<string>();
            List<TestException> failed = new List<TestException>();

            foreach (var invoker in application.DriverFactories)
            {
                var driver = invoker.Invoke();
                
            }

            return new Results() { Passed = passed, Failed = failed };
        }
    }
}