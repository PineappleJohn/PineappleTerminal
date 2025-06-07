using PineappleMod.Console;
using PineappleMod.Tools;
using System;
using TMPro;
using UnityEngine;

namespace PineappleMod.ConsoleCommands
{
    public abstract class Command : MonoBehaviour
    {
        public TextMeshPro output;

        protected virtual void Awake()
        {
            output = ConsoleManager.Instance.returnText;
        }

        public abstract string GetCommandName();
        public abstract string GetOutput();
        public abstract bool IsEnabled();

        public virtual void Execute(object[] args)
        {
            try
            {
                if (!IsEnabled())
                {
                    ThrowError(ErrorType.CommandNotAllowed, 0);
                    return;
                }
                output.color = Color.white;
                output.text = GetOutput();
            }
            catch (Exception error)
            {
                Logging.Fatal($"Error in command {GetCommandName()}: {error.Message}", error);
            }
        }
        public virtual void ThrowError(ErrorType errorType, int pos)
        {
            output.color = Color.red;
            output.text = $"{errorType} in command {GetCommandName()} at char {pos}";
        }
    }

    public enum ErrorType
    {
        InvalidNamespace,
        InvalidCommand,
        InvalidArguments,
        CommandNotAllowed
    }
}
