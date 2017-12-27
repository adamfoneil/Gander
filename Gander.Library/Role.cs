using System.Xml.Serialization;

namespace Gander.Library
{
    /// <summary>
    /// Describes a type of user (normally based on role or permission level) that is logged in by Gander to execute tests
    /// </summary>
    public class Role
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        /// <summary>
        /// One or more assertions that verify this Role's claimed permissions (i.e. database queries that check permissions)
        /// </summary>
        public Assertion[] Assertions { get; set; }
    }
}