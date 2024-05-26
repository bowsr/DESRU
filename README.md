# DESRU - DOOM Eternal Speedrun Utility
![DESRU Icon](https://user-images.githubusercontent.com/26034933/191414580-21e0c691-cf32-43c3-b45a-a48bc0b234b6.png)  
DESRU is an all-in-one utility program designed to make your DOOM Eternal speedrunning experience a lot smoother.

You are **required** to use this program during your speedruns if you are running on a Windows PC.

### Table of Contents
- [Features](https://github.com/bowsr/DESRU#features)
- [Installation](https://github.com/bowsr/DESRU#installation)
- [Usage](https://github.com/bowsr/DESRU#usage)
  - [Known Issues](https://github.com/bowsr/DESRU#known-issues)
    - [Modbot Input Duplication](https://github.com/bowsr/DESRU#modbot-input-duplication)
  - [Feedback and Questions](https://github.com/bowsr/DESRU#feedback-and-questions)

## Features
- [Freescroll Emulation Macro](https://github.com/bowsr/desru-macro)
  - The macro comes bundled with DESRU
  - If enabled in DESRU, it will automatically run in the background when DOOM Eternal is running
- FPS Limiter
  - DESRU will automatically cap DOOM Eternal's framerate via the `com_adaptiveTickMaxHz` console command which is normally inaccessible without cheats
    - Change the `Max FPS` value in DESRU to your desired limit
    - DESRU does not allow values higher than 250, as that is the required limit for speedruns
- Various Hotkeys
  - Hotkeys for toggling Resolution Scaling and FPS Limits are included
    - Three FPS hotkeys are shown for quick access, but there's a button to show 12 more if you need them
      - More can also be added via the `fpskeys.json` file if absolutely needed.  
      **NOTE**: These will not be shown in DESRU, but the duplicate hotkey check will take them into account
  - You can also change your macro bindings directly from DESRU. No need to open the `bindings.txt` file and figure out which keycode to add
- Resolution Scaling Unlocker
  - By default, DOOM Eternal only allows its resolution scaling to drop down to a minimum of 50% of your current resolution.
  - If you use this unlocker, you can set this minimum all the way down to 1% if you'd like
  - You can also automatically enable static or dynamic scaling and even set the dynamic scaling target FPS through DESRU
    - The Res. Scale Toggle hotkey will toggle resolution scaling, using the method you selected
    - The Res. Scale hotkeys in the extra hotkey section will change the current minimum between the default and the hotkey's value
  - NOTE: Static scaling cannot go lower than roughly 10%, no matter what value is set. This is a limitation with how the game's scaler works.
- Version Changing
  - If you have multiple installations of DOOM Eternal (due to downpatching), you can set up their folder structure in a way that DESRU will detect them and allow you to quickly switch between them
    - The required folder name for an extra installation is `DOOMEternal <version>`. Replace `<version>` with the version of that installation
      - Valid version names can be found in the included `validVersions.txt` file
- Firewall rule toggling
  - DOOM Eternal automatically downloads a server-side balance update on launch
  - This can affect the campaign, so speedruns **require** a firewall rule to block this
- [meath00k](https://github.com/brongo/m3337ho0o0ok)
  - meath00k is bundled with DESRU
  - Use the Cheats button to install/uninstall meath00k to enable the use of cheat protected commands, along with adding features like noclip/notarget/entitySpawning
- Trainer
  - DESRU will read and display the slayer's position, rotation, and velocity  
    *note: the position shown is the position of the camera*
    - You can enable a speedometer which can be captured by recording software like OBS. [Example OBS Filter](https://cdn.discordapp.com/attachments/1014057314510196776/1068588673504923649/image.png)
  - You must have cheats **_enabled_** to use this Trainer
  - This trainer does *not* include an option to teleport. If you wish to do so, bind a key to each of the commands listed below  
  `getviewpos`  
  `setviewpos last`

All this, and more, can be read about in the HELP page of DESRU.

## Installation
.NET 6.0 is ***required*** for the use of this program. Download the ***Desktop x64*** version found [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime).

Once .NET 6.0 is installed, download the latest **DESRU.zip** file from the releases page [here](https://github.com/bowsr/DESRU/releases/latest/download/DESRU.zip). Extract ***all files*** from this zip to your desired folder.  
If manually updating, download the latest **DESRU.zip** and extract all files to the same folder, replacing **all of them**.

When you first run DESRU, Windows may show a popup saying the program is unsafe.  
This can be safely ignored by pressing "More Info" then "Run Anyway"

If you're worried about DESRU doing anything malicious, the full source code is available in this repository.
## Usage
You can find instructions for usage of this utility by clicking the **HELP** button in DESRU.
### Known Limitations
#### Modbot Input Duplication
If your melee/interact action is bound to `E` or `Q`, interacting with a Modbot may duplicate the input, causing the mod selection menu to tab over to another weapon. If you use either of these keys for your interact action, you will either need to rebind it to another key, or disable the Max FPS Limit in DESRU's options.  
If you disable the Max FPS Limit, you are **REQUIRED** to limit your framerate to 250 (or lower) through external means. Your options include Rivatuner Statistics Server (RTSS), NVIDIA Control Panel, among others. Make sure that this external limit is running **at all times**. NVIDIA Control Panel is recommended for this reason as you'll only need to set it once.
### Feedback and Questions
If you have any feedback or questions about the program, please let me know in the [MDSR Discord](https://discord.com/invite/dtDa9VZ), **@bowsr**.
