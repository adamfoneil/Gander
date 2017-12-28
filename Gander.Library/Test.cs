using OpenQA.Selenium;
using System.Xml.Serialization;

namespace Gander.Library
{
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

        internal void Execute(IWebDriver driver, Application application, Environment environment)
        {
        }
    }
}