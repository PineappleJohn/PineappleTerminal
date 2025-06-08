using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PineappleMod.Tools
{
    public static class Formatting
    {
        public static string FormatGamemode(string gm) 
        {
            bool modded = gm.Contains("MODDED_");
            string formattedGamemode = gm.Replace("MODDED_", "");

            return $"{(modded ? "Modded" : "")} {formattedGamemode}";
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
