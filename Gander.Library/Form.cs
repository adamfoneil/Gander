﻿using System.Xml.Serialization;

namespace Gander.Library
{
    /// <summary>
    /// Describes an interaction with an HTML form by setting one or more fields and then submitting
    /// </summary>
    public class Form
    {
        /// <summary>
        /// URL containing the form to submit
        /// </summary>
        [XmlAttribute("url")]
        public string Url { get; set; }

        /// <summary>
        /// Enables Gander to find the form on a page by its action attribute of the HTML form element
        /// </summary>
        [XmlAttribute("action")]
        public string Action { get; set; }

        /// <summary>
        /// Enables Gander to find the form on a page by the Id attribute of the HTML form element
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }

        public class Field
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }


        }
    }
}