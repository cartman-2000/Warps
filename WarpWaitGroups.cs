using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Warps
{
    public class WarpWaitGroups
    {
        public WarpWaitGroups () { }

        [XmlAttribute]
        public string GroupName { get; set; }
        [XmlAttribute]
        public float WaitTime { get; set; }
    }
}
