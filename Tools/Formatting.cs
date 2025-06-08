using GorillaNetworking;
using System.Collections.Generic;
using UnityEngine;

namespace PineappleMod.Tools
{
    public static class Formatting
    {
        public static string FormatGamemode(string gm) 
        {
            bool modded = gm.Contains("MODDED_");
            string formattedGamemode = gm.Replace("MODDED_", "").Replace("DEFAULT", "");

            return $"{(modded ? "Modded" : "")} {formattedGamemode}";
        }

        public static NetPlayer GetNetPlayerFromName(string name)
        {
            name = name.ToUpper();
            if (string.IsNullOrEmpty(name) || !NetworkSystem.Instance.InRoom) return null;
            var netplayers = NetworkSystem.Instance.AllNetPlayers;
            if (name == GorillaComputer.instance.currentName) return NetworkSystem.Instance.LocalPlayer;
            if (netplayers == null || netplayers.Length == 0) return null;

            FormatPlayerlist(netplayers).TryGetValue(name, out NetPlayer player);
            return player;
        }

        public static VRRig GetVRRigFromName(string name)
        {
            name = name.ToUpper();
            if (string.IsNullOrEmpty(name) || !NetworkSystem.Instance.InRoom) return null;
            var rigs = GorillaParent.instance.vrrigs;
            if (name == GorillaComputer.instance.currentName) return GorillaTagger.Instance.offlineVRRig;

            foreach (var rig in rigs)
            {
                if (rig.playerText1.text.ToUpper() == name || rig.playerText2.text.ToUpper() == name)
                {
                    return rig;
                }
            }
            return null;
        }

        public static Dictionary<string, NetPlayer> FormatPlayerlist(NetPlayer[] players) 
        {
            Dictionary<string, NetPlayer> playerList = new Dictionary<string, NetPlayer>();
            foreach (var player in players)
            {
                if (player == null) continue;
                playerList.Add(player.SanitizedNickName, player);
            }
            return playerList;
        }

        public static string FormatColour(Color clr) 
        {
            return $"[{clr.r * 9}, {clr.g * 9}, {clr.b * 9} | {clr.r * 255}, {clr.g * 255}, {clr.b * 255}]";
        }
    }
}
