using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;

namespace Warps
{
    public class CommandWarp : IRocketCommand
    {
        public static string syntax = "<\"warpname\"> [\"playername\"]";
        public static string help = "Goes to a warp point on the server.";

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Help
        {
            get { return help; }
        }

        public string Name
        {
            get { return "warp"; }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "Warps.warp" }; }
        }

        public string Syntax
        {
            get { return syntax; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 2)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("warp_help"));
                return;
            }
            Warp warp = Warps.warpsData.GetWarp(command[0]);
            UnturnedPlayer unturnedTarget = command.GetUnturnedPlayerParameter(1);
            if (warp != null)
            {
                if (unturnedTarget != null && (caller.HasPermission("warp.other") || caller.IsAdmin || caller is ConsolePlayer))
                {
                    if (unturnedTarget.Stance == EPlayerStance.DRIVING || unturnedTarget.Stance == EPlayerStance.SITTING)
                    {
                        UnturnedChat.Say(caller, Warps.Instance.Translate("warp_cant_warp_in_car"));
                        return;
                    }
                    if (Warps.CheckUconomy())
                        if (Warps.Instance.Configuration.Instance.WarpOtherChargeEnable && Warps.Instance.Configuration.Instance.WarpOtherCost > 0.00m)
                            if (!Warps.TryCharge(caller, Warps.Instance.Configuration.Instance.WarpOtherCost))
                                return;
                    unturnedTarget.Player.teleportToLocationUnsafe(warp.Location, warp.Rotation);
                    UnturnedChat.Say(caller, Warps.Instance.Translate("admin_warp", unturnedTarget.CharacterName, warp.Name));
                    Logger.Log(Warps.Instance.Translate("admin_warp_log", caller.DisplayName, caller.Id, unturnedTarget.CharacterName, warp.Name));
                    UnturnedChat.Say(unturnedTarget, Warps.Instance.Translate("player_warp", warp.Name));
                    return;
                }
                else if (unturnedTarget != null)
                {
                    UnturnedChat.Say(caller, Warps.Instance.Translate("warp_other_not_allowed"));
                    return;
                }
                if (unturnedTarget == null && command.Length == 2)
                {
                    UnturnedChat.Say(caller, Warps.Instance.Translate("warp_cant_find_player"));
                    return;
                }
                else if (caller is ConsolePlayer)
                {
                    UnturnedChat.Say(caller, Warps.Instance.Translate("warp_console_no_player"));
                    return;
                }
                else
                {
                    UnturnedPlayer unturnedCaller = (UnturnedPlayer)caller;
                    if (unturnedCaller.Stance == EPlayerStance.DRIVING || unturnedCaller.Stance == EPlayerStance.SITTING)
                    {
                        UnturnedChat.Say(caller, Warps.Instance.Translate("warp_cant_warp_in_car"));
                        return;
                    }
                    if (Warps.Instance.Configuration.Instance.EnableWaitGroups)
                    {
                        if (Warps.CheckUconomy())
                            if (Warps.Instance.Configuration.Instance.WarpCargeEnable && Warps.Instance.Configuration.Instance.WarpCost > 0.00m)
                                if (!Warps.TryCharge(caller, Warps.Instance.Configuration.Instance.WarpCost, true))
                                    return;
                        WarpsPlayerComponent wpc = unturnedCaller.GetComponent<WarpsPlayerComponent>();
                        wpc.DoWarp(warp);
                        return;
                    }
                    else
                    {
                        if (Warps.CheckUconomy())
                            if (Warps.Instance.Configuration.Instance.WarpCargeEnable && Warps.Instance.Configuration.Instance.WarpCost > 0.00m)
                                if (!Warps.TryCharge(caller, Warps.Instance.Configuration.Instance.WarpCost))
                                    return;
                        if (unturnedCaller.Player.teleportToLocation(warp.Location, warp.Rotation))
                        {
                            if (caller.IsAdmin)
                            {
                                unturnedCaller.Player.teleportToLocationUnsafe(warp.Location, warp.Rotation);
                                return;
                            }
                            UnturnedChat.Say(Warps.Instance.Translate("warp_obstructed"));
                            return;
                        }
                        UnturnedChat.Say(caller, Warps.Instance.Translate("player_warp", warp.Name));
                        return;
                    }
                }
            }
            else
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("warp_cant_find_warp", command[0]));
                return;
            }
        }
    }

}