using PineappleMod.Console;
using PineappleMod.Tools;
using System;

namespace PineappleMod.Commands
{
    public class Debug : Command
    {
        public override void Execute(string[] args)
        {
            Logging.Info("Debug command executed with args: " + string.Join(", ", args));
        }

        public override string GetCommandName() => "test";

        public override string GetOutput() => "YAYAYAY!!";
    }
}
