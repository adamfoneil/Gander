using System.Xml.Serialization;

namespace Gander.Library
{
    public class Credential
    {
        /// <summary>
        /// The type of user, see <see cref="Role.Name"/>
        /// </summary>
        [XmlAttribute("role")]
        public string Role { get; set; }

        /// <summary>
        /// User name to login as
        /// </summary>
        [XmlAttribute("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Password to use
        /// </summary>
        [XmlAttribute("password")]
        public string Password { get; set; }
    }
}