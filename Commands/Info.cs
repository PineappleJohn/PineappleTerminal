using GorillaNetworking;
using Photon.Pun;
using PineappleMod.Console;
using PineappleMod.Tools;
using System.Linq;
using UnityEngine;

namespace PineappleMod.Commands
{
    public class RoomInfo : Command
    {
        public override void OnExecute(string[] args)
        {
            return;
        }

        public override string GetCommandName() => "info";

        public override string GetOutput()
        {
            return $"Code: {NetworkSystem.Instance.RoomName}\nPlayers: {NetworkSystem.Instance.RoomPlayerCount}\nMaster: {NetworkSystem.Instance.MasterClient.SanitizedNickName}\nGamemode: {Formatting.FormatGamemode(NetworkSystem.Instance.GameModeString)}";
        }

        public override bool Clear => false;
    }

    public class PlayerInfo : Command
    {
        string[] args;
        public override void OnExecute(string[] args1)
        {
            args = args1;
        }
        public override string GetCommandName() => "info";
        public override string GetOutput()
        {
            
            if (args.Length == 0)
                return "No player specified.";
            /*
            var player = Formatting.FormatPlayerlist(NetworkSystem.Instance.AllNetPlayers).TryGetValue(args[0].ToUpper(), out NetPlayer plr);*/
            var plr = Formatting.GetNetPlayerFromName(args[0]);
            if (plr == null)
                return $"Player '{args[0]}' not found.";
            return $"ID: {plr.UserId}\nIs Master: {plr.IsMasterClient}\nColour: {Formatting.FormatColour(GorillaParent.instance.vrrigDict[plr].playerColor)}";
        }

        public override bool Clear => false;
    }

    public class PlayerColour : Command
    {
        public override int RequiredArgs => 3;
        string output = string.Empty;
        public override void OnExecute(string[] args)
        {
            int red = int.Parse(args[0]);
            int green = int.Parse(args[1]);
            int blue = int.Parse(args[2]);

            PlayerPrefs.SetFloat("redValue", red);
            PlayerPrefs.SetFloat("greenValue", green);
            PlayerPrefs.SetFloat("blueValue", blue);
            GorillaTagger.Instance.UpdateColor(red, green, blue);
            PlayerPrefs.Save();

            if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId) || CosmeticWardrobeProximityDetector.IsUserNearWardrobe(PhotonNetwork.LocalPlayer.UserId))
                GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", Photon.Pun.RpcTarget.All, new object[] { red, green, blue });
            else
                output = "You must be near a computer to change your colour";
        }

        public override string GetCommandName() => "colour";

        public override string GetOutput() => output == string.Empty ? "Set color" : output;
    }

    public class PlayerName : Command
    {
        public override int RequiredArgs => 1;
        public override void OnExecute(string[] args)
        {
            string priorname = GorillaComputer.instance.currentName;
            string name = args[0].ToUpper() ?? priorname;

            if (name.Length > 12)
            {
                name = priorname;
            }

            GorillaComputer.instance.currentName = name;
            PhotonNetwork.LocalPlayer.NickName = name;
            GorillaTagger.Instance.offlineVRRig.playerText1.text = name;
            GorillaTagger.Instance.offlineVRRig.playerText2.text = name;
            GorillaComputer.instance.savedName = name;
            PlayerPrefs.SetString("playerName", name);
            PlayerPrefs.Save();
        }

        public override string GetCommandName() => "name";

        public override string GetOutput() => "Set name";
    }

    public class ModChecker : Command // Husky this is a WIP, I am aware it doesn't work yet
    {
        public string[] mods = new string[]
        {
            "PINEAPPLEMOD",
            "GORILLAINFOWATCH",
            "GRATE",
            "GC",
            "GPRONOUNS",
            "CHEESEISGOUDA" // WhosThatMonke
        };

        string output = string.Empty;
        public override int RequiredArgs => 2;
        public override bool Clear => false;
        public override string GetCommandName() => "check";

        public override string GetOutput() => output;

        public override void OnExecute(string[] args)
        {
            if (!mods.Contains(args[1].ToUpper())) throw new System.ArgumentException($"Invalid mod name '{args[1]}'. Valid options are: {string.Join(", ", mods)}");

            if (Formatting.GetNetPlayerFromName(args[0]).GetPlayerRef().CustomProperties.TryGetValue(args[1], out object mod))
            {
                output = $"{args[0]} has {args[1]} installed.";
            }
            else
            {
                output = $"{args[0]} does not have {args[1]} installed.";
            }
        }
    }
}
