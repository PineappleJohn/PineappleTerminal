# Pineapple Console
Pineapple console adds a CLI to Another Axiom's flagship game *Gorilla Tag* <br>
It allows you to run commands, change settings, and interact with the game in an easy way. There are many possibilities for commands.
You are able to make addons for it, adding more commands, namespaces, and functionality to the console.
# Built-in Commands
## Debug
- debug test - The original test command used<br>
## Room
- Room disconnect - Disconnects you from the current room<br>
~~- Room join [^1] - Joins a room with the given code<br>~~
~~- Room gm [^3] [^2] - Sets the current gamemode<br>~~
- Room info - Provides info on the current connected room.<br>
[^1]: The room code (String)
[^2]: Wether or not the room should be modded. (True / False)
[^3]: The gamemode to set (String)
## Player
- Player info [^4] - Provides info on the player specifified. Might return an error.
- Player colour - Not yet implemented properly
- Player name - Not yet implemented properly
[^4]: The players name
## Mods
- Mods check [^5] - Views the players custom properties for a mod in the modlist.
[^5]: The mod name
## Coming soon
- Grate integration - Allowing the terminal to enable / disable Grate modules.
- Gorilla Pronouns intergration - Allowing the terminal to change your pronouns.
# Installation & Usage
**How to install:** <br>
> Download the Dll from the latest release and place it in the plugins folder of Gorilla Tag. <br>
> Run the game. <br>

**How to use:** <br>

> Open the console by <Insert Controls here> <br>
> Close the console by <Insert Controls here> <br>
> Run commands by typing them in the following format and then clicking enter: <br>
```<Namespace> [Command] (Arguments)``` <br>
Do not include symbols.
# For developers
Read the [wiki](https://github.com/PineappleJohn/PineappleTerminal/wiki)! <br> <br> <br>
Terminal in-game
![20B5F61](https://github.com/user-attachments/assets/9e691447-52d7-4a1d-a6a7-5c434c8b9e7a)
