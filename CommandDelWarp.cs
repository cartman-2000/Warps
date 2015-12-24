using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;

namespace Warps
{
    public class CommandDelWarp : IRocketCommand
    {
        public static string syntax = "<\"warpname\">";
        public static string help = "Deletes a warp on the server.";

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
            get { return "delwarp"; }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "Warps.delwarp" }; }
        }

        public string Syntax
        {
            get { return syntax; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("delwarp_help"));
                return;
            }

            Warp warpData = Warps.warpsData.GetWarp(command[0]);
            if (warpData == null)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("delwarp_not_found"));
                return;
            }
            else
            {
                if (Warps.CheckUconomy())
                    if (Warps.Instance.Configuration.Instance.DelWarpChargeEnable && Warps.Instance.Configuration.Instance.DelWarpCost > 0.00m)
                        if (!Warps.TryCharge(caller, Warps.Instance.Configuration.Instance.DelWarpCost))
                            return;
                Warps.warpsData.RemoveWarp(command[0]);
                UnturnedChat.Say(caller, Warps.Instance.Translate("delwarp_removed"));
                return;
            }
        }
    }
}