using Steamworks;
using System;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace Warps
{
    public class Warp
    {
        public Warp() { }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string SetterCharName { get; set; }
        [XmlAttribute]
        public string SetterSteamName { get; set; }
        [XmlIgnore]
        public CSteamID SetterCSteamID { get; set; }
        [XmlAttribute("SetterCSteamID")]
        public ulong XmlSetterCSteamID
        {
            get
            {
                return (ulong)SetterCSteamID;
            }
            set
            {
                SetterCSteamID = (CSteamID)value;
            }
        }
        [XmlAttribute]
        public string World { get; set; }
        [XmlAttribute]
        public float Rotation { get; set; }
        [XmlIgnore]
        public Vector3 Location { get; set; }
        [XmlAttribute("Location")]
        public string XmlLocation
        {
            get
            {
                return Location.ToString();
            }
            set
            {
                float[] array = value.Substring(1, value.Length - 2).Split(',').Select(Convert.ToSingle).ToArray();
                Location = new Vector3(array[0], array[1], array[2]);
            }

        }
    }
}