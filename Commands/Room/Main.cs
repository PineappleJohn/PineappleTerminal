using GorillaNetworking;
using PineappleMod.Console;
using PineappleMod.Menu;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace PineappleMod.Commands.Room
{
    public class Disconnect : Command
    {
        public override void OnExecute(string[] args)
        {
            NetworkSystem.Instance.ReturnToSinglePlayer();
            MenuManager.instance.DisableMenu();
        }

        public override string GetCommandName() => "disconnect";

        public override string GetOutput() => "Disconnected";
    }

    public class Gamemode : Command
    {
        public string[] gamemodes = { "Casual", "Infection", "FreezeTag", "Guardian" };
        public override int RequiredArgs => 2;
        public override string GetCommandName() => "gm";

        public override string GetOutput() => "Set gamemode";

        public override void OnExecute(string[] args)
        {
            string gamemode = args[0].ToLower().Capitalize();
            bool modded = args[1].ToLower() == "modded";

            if (!gamemodes.Contains(gamemode)) throw new System.ArgumentException($"Invalid gamemode '{gamemode}'. Valid options are: {string.Join(", ", gamemodes)}");

            GorillaComputer.instance.SetGameModeWithoutButton($"{(modded ? "MODDED_" : "")}{gamemode}");
        }
    }
}
