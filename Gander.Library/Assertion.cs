using System.Data;
using System.Xml.Serialization;

namespace Gander.Library
{
    public abstract class Assertion
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("failMessage")]
        public string FailMessage { get; set; }

        public abstract bool Evaluate(Application application, Environment environment);
    }
}