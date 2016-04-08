using System.Xml.Serialization;

namespace Warps
{
    public class WarpWaitGroups
    {
        public WarpWaitGroups () { }
        internal WarpWaitGroups(string gName, float wTime)
        {
            GroupName = gName;
            WaitTime = wTime;
        }

        [XmlAttribute]
        public string GroupName;
        [XmlAttribute]
        public float WaitTime;
    }
}
