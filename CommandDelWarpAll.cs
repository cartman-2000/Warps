using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;

namespace Warps
{
    public class CommandDelWarpAll : IRocketCommand
    {
        public static string help = "Deletes all warps from a specific map.";
        public static string syntax = "<\"Map name\">";
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
            get { return "delwarpall"; }
        }

        public List<string> Permissions
        {
            get { return new List<string> { "Warps.delwarpall" }; }
        }

        public string Syntax
        {
            get { return syntax; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 0 || command.Length > 1)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("delwarpall_help"));
                return;
            }
            if (command.Length == 1)
            {
                int count = Warps.warpsData.RemoveWarpAll(command[0]);
                if (count == 0)
                {
                    UnturnedChat.Say(caller, Warps.Instance.Translate("warps_none_found"));
                    return;
                }
                UnturnedChat.Say(caller, Warps.Instance.Translate("delwarpall_removed", count));
            }
        }
    }
}
