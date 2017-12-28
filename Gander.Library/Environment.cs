using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            public IEnumerable<PassedTest> Passed { get; set; }
            public IEnumerable<TestException> Failed { get; set; }
        }

        public class PassedTest
        {
            public string Name { get; set; }
            public long MillesecondsElapsed { get; set; }
        }

        internal Results Execute(Application application)
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

            if (!application.Tests?.Any() ?? false)
            {
                throw new Exception("Application must have at least one test defined.");
            }

            List<PassedTest> passed = new List<PassedTest>();
            List<TestException> failed = new List<TestException>();

            foreach (var invoker in application.DriverFactories)
            {
                var driver = invoker.Invoke();
                foreach (var test in application.Tests)
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        test.Execute(driver, application, this);
                        sw.Stop();
                        passed.Add(new PassedTest() { Name = test.Name, MillesecondsElapsed = sw.ElapsedMilliseconds });
                    }
                    catch (TestException exc)
                    {
                        sw.Stop();
                        exc.MillesecondsElapsed = sw.ElapsedMilliseconds;
                        failed.Add(exc);
                    }
                }
            }

            return new Results() { Passed = passed, Failed = failed };
        }
    }
}