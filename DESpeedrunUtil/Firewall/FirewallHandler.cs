﻿using NetFwTypeLib;
using Serilog;

namespace DESpeedrunUtil.Firewall {
    internal class FirewallHandler {

        /// <summary>
        /// Checks if an outbound firewall rule blocking DOOMEternalx64vk exists.
        /// </summary>
        /// <param name="application">Full path of DOOMEternalx64vk.exe</param>
        /// <param name="delete"><see langword="true"/> to delete the detected rule</param>
        /// <returns><see langword="true"/> if a matching rule is detected</returns>
        public static bool CheckForFirewallRule(string application, bool delete) {
            INetFwPolicy2 policy2 = (INetFwPolicy2) Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            foreach(INetFwRule rule in policy2.Rules) {
                if(rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT && rule.ApplicationName == application) {
                    if(delete) {
                        policy2.Rules.Remove(rule.Name);
                        Log.Information("Firewall rule deleted. name: {Name}", rule.Name);
                    }
                    if(!rule.Enabled && !delete) {
                        Log.Information("Firewall rule detected but not enabled. Enabling...");
                        rule.Enabled = true;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a new outbound firewall rule blocking DOOMEternalx64vk
        /// </summary>
        /// <param name="application">Full path of DOOMEternalx64vk.exe</param>
        public static void CreateFirewallRule(string application) {
            INetFwPolicy2 policy2 = (INetFwPolicy2) Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            INetFwRule2 fwRule = (INetFwRule2) Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            fwRule.Enabled = true;
            fwRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            fwRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            fwRule.Name = "DOOMEternal (SR Utility)";
            fwRule.ApplicationName = application;
            fwRule.Profiles = policy2.CurrentProfileTypes;

            policy2.Rules.Add(fwRule);
            Log.Information("Firewall rule created. name: {Name}", fwRule.Name);
        }

    }
}
