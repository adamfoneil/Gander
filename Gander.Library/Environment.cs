using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
