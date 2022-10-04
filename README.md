# DESRU - DOOM Eternal Speedrun Utility
![DESRU Icon](https://user-images.githubusercontent.com/26034933/191414580-21e0c691-cf32-43c3-b45a-a48bc0b234b6.png)  
DESRU is an all-in-one utility program designed to make your DOOM Eternal speedrunning experience a lot smoother.

You are allowed, and **encouraged**, to use this program during your speedruns. Eventually DESRU will be **required** for any speedruns done on Windows PCs, so stay tuned to the [MDSR Discord](https://discord.gg/dtDa9VZ) for updates.

## Features
- [Freescroll Emulation Macro](https://github.com/henyK/doom-eternal-macro)
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
  - By default, DOOM Eternal only allows its dynamic resolution scaling to drop down to a minimum of 50% of your current resolution.
  - If you use this unlocker, you can set this minimum all the way down to 1% if you'd like
  - You can also automatically enable dynamic scaling and set its target FPS through DESRU
    - The Res. Scale Toggle hotkey will change the minimum resolution between 100% and the value you input in DESRU
- Version Changing
  - If you have multiple installations of DOOM Eternal (due to downpatching), you can set up their folder structure in a way that DESRU will detect them and allow you to quickly switch between them
    - The required folder name for an extra installation is `DOOMEternal <version>`. Replace `<version>` with the version of that installation
      - Valid version names can be found in the included `validVersions.txt` file
- Firewall rule creation/deletion
  - DOOM Eternal automatically downloads a server-side balance update on launch
  - This can effect the campaign, so speedruns require a firewall rule to block this
- [meath00k](https://github.com/brongo/m3337ho0o0ok)
  - meath00k is bundled with DESRU
  - Use the Cheats button to install/uninstall meath00k to enable the use of cheat protected commands, along with adding features like noclip/notarget/entitySpawning

All this, and more, can be read about in the HELP page of DESRU.

## Installation
.NET 6.0 is ***required*** for the use of this program. Download the ***Desktop x64*** version found [here](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime).

Once .NET 6.0 is installed, download the latest **DESRU.zip** file from the releases page [here](https://github.com/bowsr/DESRU/releases/latest). Extract ***all files*** from this zip to your desired folder.  
When updating, download the latest **DESRU.zip** and extract all files to the same folder, replacing **all of them**.

When you first run DESRU, Windows may show a popup saying the program is unsafe.  
This can be safely ignored by pressing "More Info" then "Run Anyway"

If you're worried about DESRU doing anything malicious, the full source code is available in this repository.
## Usage
You can find instructions for usage of this utility by clicking the **HELP** button in DESRU.
### Feedback and Questions
If you have any feedback or questions about the program, please let me know in the MDSR Discord, **@bowsr#0238**.
