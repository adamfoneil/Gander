using System.Data;
using System.Xml.Serialization;

namespace Gander.Library
{
    /// <summary>
    /// Defines a set of tests that share the same context
    /// </summary>
    public class TestSuite
    {
        /// <summary>
        /// Values passed to tests in this suite that are the same across all tests
        /// </summary>
        public Const[] Constants { get; set; }

        public class Const
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }
        }
    }
}