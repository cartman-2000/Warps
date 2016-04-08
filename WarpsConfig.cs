using Rocket.API;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Warps
{
    public class WarpsConfig : IRocketPluginConfiguration
    {
        public bool UconomyEnable = false;
        public bool WarpCargeEnable = true;
        public decimal WarpCost = 100.00m;
        public bool WarpOtherChargeEnable = false;
        public decimal WarpOtherCost = 200.00m;
        public bool SetWarpChargeEnable = false;
        public decimal SetWarpCost = 200.00m;
        public bool DelWarpChargeEnable = false;
        public decimal DelWarpCost = 200.00m;
        public bool EnableWaitGroups = false;
        public bool EnableMovementRestriction = false;

        [XmlArray("WaitGroups"), XmlArrayItem(ElementName = "Group")]
        public List<WarpWaitGroups> WaitGroups = new List<WarpWaitGroups>();

        [XmlArray("Warps"), XmlArrayItem(ElementName = "Warp")]
        public List<Warp> Warps = new List<Warp>();

        public void LoadDefaults()
        {
            // Populate the WaitGroups list with groups, if it is empty.
            if (WaitGroups.Count == 0)
            {
                WaitGroups = new List<WarpWaitGroups>
                {
                    new WarpWaitGroups("default", 10),
                    new WarpWaitGroups("admin", 5),
                    new WarpWaitGroups("all", 10)
                };
            }
        }
    }
}