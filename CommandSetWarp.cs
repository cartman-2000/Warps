using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace Warps
{
    public class CommandSetWarp : IRocketCommand
    {
        public static string syntax = "<\"warpname\">";
        public static string help = "Sets a warp on the server at player location.";

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Help
        {
            get { return help; }
        }

        public string Name
        {
            get { return "setwarp"; }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "Warps.setwarp" }; }
        }

        public string Syntax
        {
            get { return syntax; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("setwarp_help"));
                return;
            }
            string warpName = command[0].Sanitze().Trim();
            if (warpName == string.Empty)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("setwarp_not_set"));
                return;
            }

            Warp warpData = new Warp();
            UnturnedPlayer unturnedCaller = (UnturnedPlayer)caller;
            warpData.Name = warpName.Sanitze();
            warpData.SetterCharName = unturnedCaller.CharacterName.Sanitze();
            warpData.SetterSteamName = unturnedCaller.SteamName.Sanitze();
            warpData.SetterCSteamID = unturnedCaller.CSteamID;
            warpData.World = Warps.MapName;
            warpData.Rotation = unturnedCaller.Rotation;
            warpData.Location = unturnedCaller.Position;

            if (Warps.CheckUconomy())
                if (Warps.Instance.Configuration.Instance.SetWarpChargeEnable && Warps.Instance.Configuration.Instance.SetWarpCost > 0.00m)
                    if (!Warps.TryCharge(caller, Warps.Instance.Configuration.Instance.SetWarpCost))
                        return;
            if (Warps.warpsData.SetWarp(warpData))
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("setwarp_set"));
                return;
            }
            else
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("setwarp_not_set"));
                return;
            }
        }
    }
}