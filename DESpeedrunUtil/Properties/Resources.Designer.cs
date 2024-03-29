﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DESpeedrunUtil.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DESpeedrunUtil.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon DESRU {
            get {
                object obj = ResourceManager.GetObject("DESRU", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] gameVersions {
            get {
                object obj = ResourceManager.GetObject("gameVersions", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Info Panel will show you the status of DOOM Eternal and DESRU.
        ///
        ///Starting from the top, this panel will show you what version of DOOM Eternal is currently running.
        ///Your current FPS limit will be displayed below that.
        ///
        ///The status of the Freescroll Emulation Macro will also be shown. As a note, you&apos;ll see this toggle between Running and Stopped if you tab out of the game.
        ///
        ///If dynamic resolution scaling is enabled, this panel will display that, along with the current FPS target.
        ///It will also display [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_InfoPanel {
            get {
                return ResourceManager.GetString("HelpPage_InfoPanel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the top left of the main DESRU window, you&apos;ll find a section where you can bind various actions to keys.
        ///
        ///In order to bind an action to a key, click the box next to the label of your desired action, then press a key or mouse button that you wish to bind that action to.
        ///Most keys can be bound, with notable exceptions being the ESCAPE and Tilde keys, and the Left and Right mouse buttons.
        ///
        ///For the FPS Hotkey actions, you can also specify what FPS limit you want to toggle with that key. This limit can b [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_Keybinds {
            get {
                return ResourceManager.GetString("HelpPage_Keybinds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Due to the fact that some speedtech in DOOM Eternal directly benefits from quick inputs, the Freescroll Emulation Macro was designed to emulate the scroll wheel of a freescrolling mouse (e.g. G502) for those that do not own one.
        ///
        ///This macro sends scroll inputs, a single direction at a time, to your OS at a frequency of 100hz.
        ///It is included in your DESRU download, and will run in the background whenever DOOM Eternal is in focus.
        ///
        ///This is currently allowed for use during speedruns in any situation where [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_Macro {
            get {
                return ResourceManager.GetString("HelpPage_Macro", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In the bottom left of the DESRU window, there&apos;s a section with a few miscellaneous options.
        ///
        ///You can enable/disable the macro and hotkeys from here, instead of removing binds.
        ///You can specify your desired maximum FPS that DOOM Eternal will run at.
        ///You can toggle the On-Screen Display here, but make sure it is enabled during speedruns, otherwise your run won&apos;t be valid for leaderboard submission. If the OSD takes too much space on your screen, you can enable the Minimal OSD option so the info is shown on [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_Options {
            get {
                return ResourceManager.GetString("HelpPage_Options", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to While DESRU is running, the performance metrics display in the top right of DOOM Eternal will be modified to display the state of a few things.
        ///This is here to help with the moderation of leaderboard submitted speedruns.
        ///
        ///If DESRU is detected to be out of date, an asterisk (*) will be displayed next to the FPS counter. DESRU is required to be up to date during speedruns.
        ///
        ///Under the FPS counter, the version of the game will be displayed, along with a few flags.
        ///These flags are:
        ///M - Freescroll Macro En [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_OSD {
            get {
                return ResourceManager.GetString("HelpPage_OSD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Under the Keybinds section, you&apos;ll find some options for unlocking the resolution scaling in DOOM Eternal.
        ///
        ///By default, DOOM Eternal only allows the resolution scaling to scale down to a minimum of 50% of your selected resolution. DESRU can unlock this, allowing a minimum scale of as low as 1%.
        ///
        ///In order to unlock your resolution scaling, when DOOM Eternal is open you&apos;ll need to click the Unlock button, or have the &quot;Unlock on Startup&quot; checkbox checked when starting the game. After a few seconds of the g [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_ResScaling {
            get {
                return ResourceManager.GetString("HelpPage_ResScaling", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DESRU also includes a Trainer that&apos;ll read and display the slayer&apos;s position and velocity.
        ///
        ///In order to use the Trainer, you must have cheats enabled first.
        ///
        ///Your position and velocity will be displayed in DESRU, but you can also enable an option to show this info in-game, in place of the normal DESRU OSD.
        ///
        ///There is also an optional visual speedometer included that you can use, and can even capture separately to show in recordings.
        ///The top number of this speedometer is your horizontal velocity.
        ///The  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_Trainer {
            get {
                return ResourceManager.GetString("HelpPage_Trainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to To the right of the resolution scaling section are some options to change the version of DOOM Eternal you have currently loaded.
        ///
        ///DESRU requires a specific folder structure for this feature to work.
        ///The name of a folder, besides the main one, should be &quot;DOOMEternal &lt;version&gt;&quot;. A list of valid version names can be found in the &quot;validVersions.txt&quot; file.
        ///Your folder structure should then look something like the picture below.
        ///
        ///Once it&apos;s set up, click the Refresh button in DESRU, or restart DESRU.
        ///Changi [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string HelpPage_VersionChanger {
            get {
                return ResourceManager.GetString("HelpPage_VersionChanger", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] offsets {
            get {
                object obj = ResourceManager.GetObject("offsets", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
