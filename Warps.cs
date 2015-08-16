using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Warps
{
    public class Warps : RocketPlugin<WarpsConfig>
    {
        public static Warps Instance;
        internal static WarpDataManager warpsData;
        public static readonly string MapName = Steam.map.ToLower();

        protected override void Load()
        {
            Instance = this;
            if (Instance.Configuration.Instance.WarpsEnable)
            {
                warpsData = new WarpDataManager();
            }
        }

        protected override void Unload()
        {
            warpsData.Unload();
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    { "warp_help", CommandWarp.syntax + " - " + CommandWarp.help },
                    { "setwarp_help", CommandSetWarp.syntax + " - " + CommandSetWarp.help },
                    { "delwarp_help", CommandDelWarp.syntax + " - " + CommandDelWarp.help },
                    { "warps_help", CommandWarps.syntax + " - " + CommandWarps.help },
                    { "warps_disabled", "Error: Warps arn't enabled on this server." },
                    { "admin_warp", "You have teleported player: {0}, to warp: {1}." },
                    { "admin_warp_log", "Admin: {0}({1}), has teleported player: {2}, to warp:{3}" },
                    { "player_warp", "You have been teleported to warp: {0}." },
                    { "warp_cant_find_player", "Error: Cant find the player to warp." },
                    { "warp_cant_find_warp", "Error: A warp by the name of {0}, wasn't found." },
                    { "warp_other_not_allowed", "Error: Warping other players not allowed." },
                    { "warp_console_no_player", "Error: Can't use warp command without a player from the console." },
                    { "setwarp_set", "Warp has been set." },
                    { "setwarp_not_set", "Error: Wasn't able to set warp." },
                    { "delwarp_removed", "Warp has been removed Removed." },
                    { "delwarp_not_found", "Error: A warp by that name doesn't exist." },
                    { "warps_none_found", "Error: Sorry, there were no records found."},
                    { "warps_list_header", "List of warps, There are {0} warps set."},
                    { "warps_list", "Warps: {0}."}
                };
            }
        }
    }
}
