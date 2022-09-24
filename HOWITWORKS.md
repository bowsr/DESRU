This file explains how DESRU works under the hood.

If you're looking for instructions on how to use DESRU, click the **HELP** button in DESRU. That page will explain basically everything you need to know regarding how to use it.
## Memory
DESRU monitors and modifies DOOMEternal's memory, almost exactly like LiveSplit does (literally using their code lol).  
cvars modified:
```
[c] == Cheat Protected

com_adaptiveTickMaxHz [c] - Max Frequency of Game Updates (FPS)
 - Used to limit framerate

com_showConsumerPerfMetrics - Performance Metrics Display (Top-Right of Screen)
 - Forced to 2 (Medium) for OSD

rs_enable - Enable Dynamic Resolution Scaling
rs_minimumResolution [c] - Minimum Dynamic Resolution %
rs_raiseMilliseconds [c] - Minimum frametime before raising the resolution scale (95% of target FPS)
rs_dropMilliseconds [c] - Maximum frametime before lowering the resolution scale (99% of target FPS)
 - Above four cmds used for the Resolution Scaling Settings
```  
DESRU also modifies the Medium Perf Metrics strings (for the OSD), and an array of 32 resolution % values (for the res unlocker).

## On-Screen Display
As mentioned above, performance metrics are forced to Medium (2). The text that shows up there is modified to show the state of various things.

### Row 1
If an asterisk (\*) shows up by the FPS counter (`250 FPS*`), that means DESRU has detected an update is available and the user chose to run an older version.

### Row 2
Under the FPS counter, the version of the game will show up.  
Some version names are shortened as there's a limited amount of text that can be shown in this row.  
*(e.g. 6.66r2 means 6.66 Rev 2)*

Some letters representing flags will show up next to the game version in parentheses `(MFRS)`.
```
M - Bundled Freescroll Macro is enabled
F - Firewall rule is detected and enabled
R - ReShade is detected
S - Slopeboosts are disabled [pm_allowRampJumping 0]; 1.0 only
```  
### Row 3
Below the version, a few things can show up.
```
CHEATS ENABLED -> meath00k is detected
RESTART GAME   -> DESRU was started after DOOMEternal or the Trainer was detected at some point
```  
This row will also display a scroll pattern tracker.  
Format: `X (Y.YYms)` X == # of scroll inputs; Y == avg time between scroll inputs  
*note: short scroll strings may have some error in the time delta. This works itself out very quickly with more scroll inputs.  
(e.g. using the macro to scroll 3 times could show a delta of 10.8ms, despite the macro frequency being 100hz (10.0ms))*

### Row 4+
Rest of the rows are set to a blank string.  
*Non-NVIDIA GPU users may find one of these rows to be unmodified. Nothing I can do about this since I only have NVIDIA GPUs.*

## Other Info
### Running DESRU
DESRU has to be started before the game, otherwise it can't correctly detect the state of some things.  
*(e.g. firewall rule could be active but the balance updates were already DL'd since the user enabled the rule after the game was started)*

### Macro
The Freescroll Emulation Macro is bundled with DESRU's download. DESRU handles running the macro so the user never needs to interact with it.  
If the macro is enabled in DESRU while the game is running, `M` is displayed.  
If the user modifies the macro executable, this will be detected and the macro will not be ran.  
DESRU will also periodically check for the Freescroll Macro running externally and will kill any of those processes.

### Firewall
DESRU will check the user's firewall outbound rules for a rule blocking traffic from their DOOMEternalx64vk.exe installation.  
If this is found but not enabled, DESRU will enable it.

Clicking the firewall button will either delete this rule, or create a new one named "DOOM Eternal (SR Utility).

### ReShade
For ReShade detection, DESRU searches for a Registry Value in the `HKEY_LOCAL_MACHINE\SOFTWARE\Khronos\Vulkan\ImplicitLayers\` Key.  
If a ReShade value is found, DESRU searches `C:\ProgramData\ReShade` for a file named `ReShadeApps.ini`.  
If DOOMEternal's directory is found within, `R` is displayed.  
If this file isn't found at all, `R` is still displayed as there's no way to know if the user is or isn't using ReShade.

### Resolution Unlocking
DOOM Eternal has an array of 32 percentage values that determine the steps the dynamic resolution scaler takes.

Step increases when frametime is lower than 95% of the target FPS frametime. (resolution increases)  
Step decreases when frametime is higher than 99% of the target FPS frametime. (resolution decreases)

Since the default resolution steps only go down to a minimum of 0.5x (50%) resolution, DESRU can modify this 32 value array so these steps reach a minimum of 0.01x (1%).  
If these values are modified, the game needs to be resized for them to take effect. DESRU will send two `ALT+ENTER` inputs when the game is in focus to ensure the values are used.

#### 6.66 Rev 2
The command `rs_minimumResolution` does not exist on version 6.66 Rev 2. For this reason, that 32 value array is dynamically generated based on the user's desired minimum resolution.  
This means the window resizing needs to happen every time it changes, so the 2x `ALT+ENTER` inputs will be sent again.

### Slopeboosting
While DESRU can detect if slopeboosting is disabled (via `pm_allowRampJumping 0`), the option to actually enable/disable this isn't included as the autosplitter handles this.  
Having the option stay in the autosplitter allows the user to have it disabled only for certain split files they have open. So they could have slopeboosting disabled for Limited runs done on 1.0, but enabled for Unrestricted runs, all without having to change any setting in DESRU.

