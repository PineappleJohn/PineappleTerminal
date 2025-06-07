using System;
using System.Collections.Generic;
using PineappleMod.Tools;

namespace PineappleMod.ConsoleCommands.Commands.Room
{
    public class Disconnect : Command
    {
        public override string GetCommandName()
        {
            return "disconnect";
        }
        public override string GetOutput()
        {
            return "Disconnected";
        }

        public override bool IsEnabled()
        {
            return true;
        }

        public override void Execute(object[] args)
        {
            if (args.Length > 0)
            {
                ThrowError(ErrorType.InvalidArguments, 0);
                return;
            }

            if (NetworkSystem.Instance.InRoom)
            {
                NetworkSystem.Instance.ReturnToSinglePlayer();
            }
            else
            {
                ThrowError(ErrorType.CommandNotAllowed, 0);
                Logging.Fatal("You are not in a room to disconnect from.");
                return;
            }
        }
    }
}
