using System;
using System.Collections.Generic;
using System.Text;

namespace ArxLibertatisProcGenTools
{
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public DescriptionAttribute(string desc) { this.Description = desc; }
    }
}
