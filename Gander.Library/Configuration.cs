using Gander.Library.Environments;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library
{
    [XmlInclude(typeof(SqlEnvironment))]
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
        public Form LoginForm { get; set; }

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
        /// List of Roles available to this test configuration
        /// </summary>
        public Role[] Roles { get; set; }

        public IEnumerable<string> GetEnvironmentNames()
        {
            return Environments.Select(e => e.Name);
        }
        
        public Environment this[string name]
        {
            get { return Environments.Single(e => e.Name.Equals(name)); }
        }
    }
}