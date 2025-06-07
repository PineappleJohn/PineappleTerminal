using PineappleMod.Console;
using PineappleMod.Tools;
using System;
using TMPro;
using UnityEngine;

namespace PineappleMod.ConsoleCommands
{
    public abstract class Command : MonoBehaviour
    {
        public static Command instance;
        public TextMeshPro output;

        protected virtual void Awake()
        {
            instance = this;
            output = ConsoleManager.Instance.returnText;
        }
        /// <summary>
        /// This is the command name, very simple.
        /// </summary>
        /// <returns>The command name</returns>
        public abstract string GetCommandName(); // Keep it abstract :D
        /// <summary>
        /// This is shown when the command is *succesfully* executed.
        /// </summary>
        /// <returns>The output</returns>
        public abstract string GetOutput();
        /// <summary>
        /// This makes the command only run if it's enabled. You can use this if you want to limit commands to a specific user id or something similar.
        /// </summary>
        /// <returns>Wether or not the command is enabled and can be used</returns>
        public abstract bool IsEnabled();
        /// <summary>
        /// Executes the command with the provided arguments. Args will always be a string, so you will need to parse it if you want to use it as a different type.
        /// </summary>
        /// <param name="args"></param>
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
        /// <summary>
        /// Throws an error to the output console. If you want to throw it to the Log, use Logging.Fatal() instead.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="pos"></param>
        public virtual void ThrowError(ErrorType errorType, int pos)
        {
            output.color = Color.red;
            output.text = $"{errorType} in command {GetCommandName()} at char {pos}";
        }
    }
    /// <summary>
    /// All possible error types.
    /// </summary>
    public enum ErrorType
    {
        InvalidNamespace,
        InvalidCommand,
        InvalidArguments,
        CommandNotAllowed
    }
}
