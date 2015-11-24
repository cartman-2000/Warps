using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Warps
{
    public class CommandWarps : IRocketCommand
    {
        public static string syntax = "[\"warpname\"]";
        public static string help = "Gets a list of warps that have been set to the server.";

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public bool AllowFromConsole
        {
            get { return true; }
        }

        public string Help
        {
            get { return help; }
        }

        public string Name
        {
            get { return "warps"; }
        }

        public List<string> Permissions
        {
            get { return new List<string>() { "Warps.warps" }; }
        }

        public string Syntax
        {
            get { return syntax; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string name = command.GetStringParameter(0);
            if (name == "help")
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("warps_help"));
                return;
            }
            List<Warp> WarpsList = Warps.warpsData.SearchWarps(name);

            if (WarpsList.Count == 0)
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("warps_none_found"));
                return;
            }
            else
            {
                UnturnedChat.Say(caller, Warps.Instance.Translate("warps_list_header", WarpsList.Count));
                UnturnedChat.Say(caller, Warps.Instance.Translate("warps_list", string.Join(", ", WarpsList.Select(warp => warp.Name).ToArray())));
            }
        }
    }
}