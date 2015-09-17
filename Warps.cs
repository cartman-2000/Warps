using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using unturned.ROCKS.Uconomy;

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
            Instance.Configuration.Save();
        }

        protected override void Unload()
        {
            warpsData.Unload();
        }

        internal static bool CheckUconomy()
        {
            if (Instance.Configuration.Instance.UconomyEnable)
            {
                if (Type.GetType("unturned.ROCKS.Uconomy.DatabaseManager,Uconomy") != null)
                {
                    return true;
                }
                Logger.LogWarning("Warps: Error: Uconomy plugin wasn't found on the server.");
            }
            return false;
        }

        internal static bool TryCharge(IRocketPlayer player, decimal ammount)
        {
            if (Uconomy.Instance.State == PluginState.Loaded)
            {
                try
                {
                    if (player.HasPermission("warpcharge.overide") || player is ConsolePlayer)
                        return true;
                    decimal balance = Uconomy.Instance.Database.GetBalance(player.Id);
                    if (balance < ammount)
                        UnturnedChat.Say(player, Instance.Translate("insufficient_funds", Uconomy.Instance.Configuration.Instance.MoneyName));
                    else
                    {
                        if (Uconomy.Instance.Database.IncreaseBalance(player.Id, -ammount) < 1)
                        {
                            Logger.LogWarning("Warps: Error: Wasn't able to set the player balance.");
                        }
                        else
                        {
                            UnturnedChat.Say(player, Instance.Translate("charge_success", ammount, Uconomy.Instance.Configuration.Instance.MoneyName, balance - ammount, Uconomy.Instance.Configuration.Instance.MoneyName));
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Warps: Error: There was an error trying to charge the player.");
                    return true;
                }
            }
            else
            {
                Logger.LogWarning("Warps: Error: The Uconomy plugin isn't enabled.");
                return true;
            }
            return false;
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
                    { "warp_cant_warp_in_car", "Error: Can't warp, player is in a car." },
                    { "warp_other_not_allowed", "Error: Warping other players not allowed." },
                    { "warp_console_no_player", "Error: Can't use warp command without a player from the console." },
                    { "setwarp_set", "Warp has been set." },
                    { "setwarp_not_set", "Error: Wasn't able to set warp." },
                    { "delwarp_removed", "Warp has been removed Removed." },
                    { "delwarp_not_found", "Error: A warp by that name doesn't exist." },
                    { "warps_none_found", "Error: Sorry, there were no records found."},
                    { "warps_list_header", "List of warps, There are {0} warps set."},
                    { "warps_list", "Warps: {0}."},
                    { "insufficient_funds", "Error: You don't have enough {0}s to use this command." },
                    { "charge_success", "You have been charged {0} {1}s, your balance is now {2} {3}s." }
                };
            }
        }
    }
}
