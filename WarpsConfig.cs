using System;
using Rocket.API;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Warps
{
    public class WarpsConfig : IRocketPluginConfiguration
    {
        public bool WarpsEnable;
        [XmlArray("Warps"), XmlArrayItem(ElementName = "Warp")]
        public List<Warp> Warps;
        public void LoadDefaults()
        {
            WarpsEnable = true;
            Warps = new List<Warp>();
        }
    }
}