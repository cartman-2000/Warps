using Rocket.Core.Logging;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Warps
{
    public class WarpDataManager
    {
        private static Dictionary<string, Warp> WarpsData = new Dictionary<string, Warp>();


        public WarpDataManager()
        {
            Load();
        }

        private void Load()
        {
            // Try to load the records from the config file to the Warps Dictionary.
            foreach (Warp warpData in Warps.Instance.Configuration.Instance.Warps)
            {
                try
                {
                    WarpsData.Add(warpData.World.ToLower() + "." + warpData.Name.ToLower(), warpData);
                }
                catch
                {
                    Logger.LogError("Error: Unable to load a warp record.");
                }
            }
        }

        public void Unload()
        {
            WarpsData.Clear();
        }

        private void SaveWarps()
        {
            // Save the warps out to the config file.
            Warps.Instance.Configuration.Instance.Warps = WarpsData.Values.ToList();
            Warps.Instance.Configuration.Save();

        }

        public List<Warp> SearchWarps(string name)
        {
            if (name == null)
            {
                return WarpsData.Values.Where(warpData => warpData.World.ToLower() == Warps.MapName).OrderBy(warp => warp.Name).ToList();
            }
            return WarpsData.Values.Where(warpData => warpData.Name.Contains(name.ToLower()) && warpData.World.ToLower() == Warps.MapName).OrderBy(warp => warp.Name).ToList();
        }

        public List<Warp> SearchWarps(CSteamID cSteamID)
        {
            return WarpsData.Values.Where(warpData => warpData.SetterCSteamID == cSteamID && warpData.World.ToLower() == Warps.MapName).OrderBy(warp => warp.Name).ToList();
        }

        public Warp GetWarp(string name)
        {
            return WarpsData.Values.FirstOrDefault(warpData => warpData.Name.ToLower() == name.ToLower() && warpData.World.ToLower() == Warps.MapName);
        }

        public bool SetWarp(Warp warpData)
        {
            try
            {
                if (WarpsData.ContainsKey(WarpsKey(warpData.Name)))
                {
                    WarpsData.Remove(WarpsKey(warpData.Name));
                }
                WarpsData.Add(WarpsKey(warpData.Name), warpData);
                SaveWarps();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error: Unable to set warp.");
                return false;
            }
        }

        public bool RemoveWarp(string name)
        {
            if (WarpsData.ContainsKey(WarpsKey(name)))
            {
                WarpsData.Remove(WarpsKey(name));
                SaveWarps();
                return true;
            }
            return false;
        }

        public int RemoveWarpAll(string mapName)
        {
            List<Warp> list = WarpsData.Values.Where(warpData => warpData.World.ToLower() == mapName.ToLower()).ToList();
            if (list.Count > 0)
            {
                foreach ( Warp entry in list)
                {
                    WarpsData.Remove(entry.World.ToLower() + "." + entry.Name.ToLower());
                }
                SaveWarps();
                return list.Count;
            }
            return 0;
        }

        private static string WarpsKey(string name)
        {
            return Warps.MapName + "." + name.ToLower();
        }
    }
}
