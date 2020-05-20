using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

namespace Warps
{
    public class WarpsPlayerComponent : UnturnedPlayerComponent
    {
        private bool doWarp = false;
        private DateTime startTime;
        private Warp wData;
        private UnityEngine.Vector3 lastLocation;
        private float timetoWait = 10;

        internal void DoWarp(Warp WData)
        {
            // Set the warp info up.
            startTime = DateTime.Now;
            wData = WData;
            lastLocation = Player.Position;

            // Grab time to wait.
            List<RocketPermissionsGroup> groups = R.Permissions.GetGroups(Player, false);
            if (groups.Count != 0)
            {
                if (!Warps.WaitGroups.TryGetValue(groups[0].Id, out timetoWait))
                    if (!Warps.WaitGroups.TryGetValue("all", out timetoWait))
                    {
                        Logger.LogWarning("Error: Was Unable to get time to wait on player, using default of 10 seconds.");
                        timetoWait = 10;
                    }
            }
            else
            {
                Logger.LogWarning("Error: Was Unable to get time to wait on player, using default of 10 seconds.");
                timetoWait = 10;
            }
            if (Warps.Instance.Configuration.Instance.EnableMovementRestriction)
                UnturnedChat.Say(Player, Warps.Instance.Translate("warp_wait_nomovement", wData.Name, timetoWait));
            else
                UnturnedChat.Say(Player, Warps.Instance.Translate("warp_wait", wData.Name, timetoWait));
            doWarp = true;
        }

        public void FixedUpdate()
        {
            if (doWarp)
            {
                // Fail if the player moved with movement restriction enabled.
                if (Warps.Instance.Configuration.Instance.EnableMovementRestriction && lastLocation != Player.Position)
                {
                    UnturnedChat.Say(Player, Warps.Instance.Translate("warp_fail_player_moved", wData.Name));
                    doWarp = false;
                    return;
                }
                // Fail if the player died.
                if (Player.Health == 0 || Player.Dead)
                {
                    UnturnedChat.Say(Player, Warps.Instance.Translate("warp_fail_player_died", wData.Name));
                    doWarp = false;
                    return;
                }
                // Handle the warp after the wait time has gone by.
                if ((DateTime.Now - startTime).Seconds >= timetoWait)
                {
                    if (Warps.CheckUconomy())
                        if (Warps.Instance.Configuration.Instance.WarpCargeEnable && Warps.Instance.Configuration.Instance.WarpCost > 0.00m)
                            if (!Warps.TryCharge(Player, Warps.Instance.Configuration.Instance.WarpCost))
                            {
                                doWarp = false;
                                return;
                            }
                    if (!Player.Player.teleportToLocation(wData.Location, wData.Rotation))
                    {
                        if (Player.IsAdmin)
                        {
                            Player.Player.teleportToLocationUnsafe(wData.Location, wData.Rotation);
                            doWarp = false;
                            return;
                        }
                        UnturnedChat.Say(Player, Warps.Instance.Translate("warp_obstructed"));
                        doWarp = false;
                        return;
                    }
                    UnturnedChat.Say(Player, Warps.Instance.Translate("player_warp", wData.Name));
                    doWarp = false;
                }
            }
        }
    }
}
