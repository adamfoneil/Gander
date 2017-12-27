using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library
{
    public class Configuration
    {
        /// <summary>
        /// Where do test results go? Environment folder names will be automatically created
        /// </summary>
        [XmlAttribute("logPath")]
        public string LogPath { get; set; }

        /// <summary>
        /// Login form to use for all authenticated tests
        /// </summary>
        public Form Login { get; set; }

        /// <summary>
        /// URL to navigate to cause current user to log out
        /// </summary>
        [XmlAttribute("logOffUrl")]
        public string LogoffUrl { get; set; }

        /// <summary>
        /// List of environments available to this test configuration
        /// </summary>
        public Environment[] Environments { get; set; }
        
        /// <summary>
        /// List of user types available to this test configuration
        /// </summary>
        public UserType[] UserTypes { get; set; }

        public IEnumerable<string> GetEnvironmentNames()
        {
            return Environments.Select(e => e.Name);
        }
        
        public Environment this[string name]
        {
            get { return Environments.Single(e => e.Name.Equals(name)); }
        }

        /// <summary>
        /// Base URL where tests can be run
        /// </summary>
        public class Environment
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
        }

        /// <summary>
        /// Describes a type of user (normally based on role or permission level) that is logged in by Gander to execute tests
        /// </summary>
        public class UserType
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("description")]
            public string Description { get; set; }
        }
    }
}