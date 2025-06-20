using System;

namespace ArxLibertatisProcGenTools
{
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public DescriptionAttribute(string desc) { this.Description = desc; }
    }
}
