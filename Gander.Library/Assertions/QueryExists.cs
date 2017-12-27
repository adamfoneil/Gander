using Dapper;
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace Gander.Library.Assertions
{
    /// <summary>
    /// Describes a success condition of a test based on whether a database query returns records or not
    /// </summary>
    public class QueryExists : Assertion
    {
        [XmlAttribute("query")]
        public string Query { get; set; }

        [XmlAttribute("commandType")]
        public CommandType CommandType { get; set; } = CommandType.Text;

        /// <summary>
        /// Indicates whether the assertion succeeds when records are found or not
        /// </summary>
        [XmlAttribute("anyExpected")]
        public bool AnyExpected { get; set; } = true;

        public override bool Evaluate(Environment environment)
        {
            using (var cn = environment.GetConnection())
            {
                var data = cn.Query(Query);
                return (AnyExpected) ? data.Any() : !data.Any();
            }
        }
    }
}