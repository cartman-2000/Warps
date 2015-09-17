using Rocket.API;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Warps
{
    public class WarpsConfig : IRocketPluginConfiguration
    {
        public bool WarpsEnable;

        public bool UconomyEnable;
        public bool WarpCargeEnable;
        public decimal WarpCost;
        public bool WarpOtherChargeEnable;
        public decimal WarpOtherCost;
        public bool SetWarpChargeEnable;
        public decimal SetWarpCost;
        public bool DelWarpChargeEnable;
        public decimal DelWarpCost;

        [XmlArray("Warps"), XmlArrayItem(ElementName = "Warp")]
        public List<Warp> Warps;

        public void LoadDefaults()
        {
            WarpsEnable = true;

            UconomyEnable = false;
            WarpCargeEnable = true;
            WarpCost = 100.00m;
            WarpOtherChargeEnable = false;
            WarpOtherCost = 200.00m;
            SetWarpChargeEnable = false;
            SetWarpCost = 200.00m;
            DelWarpChargeEnable = false;
            DelWarpCost = 200.00m;

            Warps = new List<Warp>();
        }
    }
}