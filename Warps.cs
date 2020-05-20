using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using fr34kyn01535.Uconomy;
using System.Collections.Generic;

namespace Warps
{
    public class Warps : RocketPlugin<WarpsConfig>
    {
        public static Warps Instance;
        internal static WarpDataManager warpsData = null;
        internal static readonly string MapName = Provider.map.ToLower();
        internal static Dictionary<string, float> WaitGroups = new Dictionary<string, float>();

        protected override void Load()
        {
            Instance = this;
            warpsData = new WarpDataManager();
            // Populate Wait Groups, if they're empty.
            Instance.Configuration.Instance.LoadDefaults();
            foreach (WarpWaitGroups group in Instance.Configuration.Instance.WaitGroups)
            {
                if (!WaitGroups.ContainsKey(group.GroupName))
                {
                    if (group.WaitTime >= 0)
                        WaitGroups.Add(group.GroupName, group.WaitTime);
                    else
                        Logger.LogWarning("Error: Negative wait time value in group: " + group.GroupName);
                }
                else
                {
                    Logger.LogWarning("Error: Duplicate group name in wait groups.");
                }
            }
            Instance.Configuration.Save();
        }

        protected override void Unload()
        {
            warpsData = null;
            WaitGroups.Clear();
        }

        internal static bool CheckUconomy()
        {
            if (Instance.Configuration.Instance.UconomyEnable)
            {
                if (Type.GetType("fr34kyn01535.Uconomy.DatabaseManager,Uconomy") != null)
                {
                    return true;
                }
                Logger.LogWarning("Error: Uconomy plugin wasn't found on the server.");
            }
            return false;
        }

        internal static bool TryCharge(IRocketPlayer player, decimal ammount, bool checkOnly = false)
        {
            if (Uconomy.Instance.State == PluginState.Loaded)
            {
                try
                {
                    if (player.HasPermission("warpcharge.overide") || player is ConsolePlayer)
                        return true;
                    decimal balance = Uconomy.Instance.Database.GetBalance(player.Id);
                    if (balance < ammount)
                    {
                        UnturnedChat.Say(player, Instance.Translate("insufficient_funds", Uconomy.Instance.Configuration.Instance.MoneyName));
                        return false;
                    }
                    if (balance > ammount && checkOnly)
                        return true;
                    else
                    {
                        if (Uconomy.Instance.Database.IncreaseBalance(player.Id, -ammount) < 0)
                        {
                            Logger.LogWarning("Warning: Player has negative balance after charge, undoing charge.");
                            Uconomy.Instance.Database.IncreaseBalance(player.Id, ammount);
                            return false;
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
                    Logger.LogException(ex, "Error: There was an error trying to charge the player.");
                    return true;
                }
            }
            else
            {
                Logger.LogWarning("Error: The Uconomy plugin isn't enabled.");
                return true;
            }
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
                    { "delwarpall_help", CommandDelWarpAll.syntax + " - " + CommandDelWarpAll.help },
                    { "warps_help", CommandWarps.syntax + " - " + CommandWarps.help },
                    { "admin_warp", "You have teleported player: {0}, to warp: {1}." },
                    { "admin_warp_log", "Admin: {0}({1}), has teleported player: {2}, to warp: {3}" },
                    { "player_warp", "You have been teleported to warp: {0}." },
                    { "warp_wait", "You'll be warped to {0}, in {1} seconds." },
                    { "warp_wait_nomovement", "You'll be warped to {0}, in {1} seconds, please don't move." },
                    { "warp_fail_player_moved", "Warp to {0} failed because you moved." },
                    { "warp_fail_player_died", "Warp to {0} failed because you died." },
                    { "warp_cant_find_player", "Error: Cant find the player to warp." },
                    { "warp_cant_find_warp", "Error: A warp by the name of {0}, wasn't found." },
                    { "warp_cant_warp_in_car", "Error: Can't warp, player is in a car." },
                    { "warp_other_not_allowed", "Error: Warping other players not allowed." },
                    { "warp_console_no_player", "Error: Can't use warp command without a player from the console." },
                    { "warp_obstructed", "Error: Can't teleport to warp, location obstructed by nearby elements." },
                    { "setwarp_set", "Warp has been set." },
                    { "setwarp_not_set", "Error: Wasn't able to set warp." },
                    { "delwarp_removed", "Warp has been removed." },
                    { "delwarp_not_found", "Error: A warp by that name doesn't exist." },
                    { "delwarpall_removed", "{0} warps with this map name have been removed." },
                    { "warps_none_found", "Error: Sorry, there were no records found."},
                    { "warps_list_header", "List of warps, There are {0} warps set."},
                    { "warps_list", "Warps: {0}."},
                    { "insufficient_funds", "Error: You don't have enough {0}s to use this command." },
                    { "charge_success", "You have been charged {0} {1}, your balance is now {2} {3}." }
                };
            }
        }
    }
}
