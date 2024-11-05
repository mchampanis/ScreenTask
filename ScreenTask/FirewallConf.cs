using System;
using System.Diagnostics;
using NetFwTypeLib;

namespace ScreenTask {
    class FirewallConf {
        public void AddRule(
            String name,
            String description,
            String ip,
            int port,
            String appname = "ScreenTask"
        ) {
            Type Policy = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 FwPolicy = (INetFwPolicy2)Activator.CreateInstance(Policy);

            Type RuleType = Type.GetTypeFromProgID("HNetCfg.FWRule");
            INetFwRule rule = (INetFwRule)Activator.CreateInstance(RuleType);

            DeleteRule(name, ip, port);

            rule.Name = name;
            rule.Description = description;
            rule.Protocol = 6; // TCP/IP
            rule.LocalPorts = port.ToString();
            rule.RemoteAddresses = "localsubnet";
            rule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
            rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            //rule.ApplicationName = appname;
            rule.InterfaceTypes = "All";
            rule.Enabled = true;
            
            FwPolicy.Rules.Add(rule);
            addACL(ip, port);
        }

        public void addACL(String ip, int port) {
            runCMD($"netsh http add urlacl url=http://{ip}:{port.ToString()}/ user=Everyone listen=yes", true); 
        }

        public void delACL(String ip, int port) {
            runCMD($"netsh http delete urlacl url=http://{ip}:{port.ToString()}/", true);
        }

        public void DeleteRule(String name, String ip, int port) {
            Type Policy = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 FwPolicy = (INetFwPolicy2)Activator.CreateInstance(Policy);
            FwPolicy.Rules.Remove(name);
            delACL(ip, port);
        }

        private string runCMD(string cmd, bool requireAdmin = false) {
            Process proc = new Process();

            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/C " + cmd;
            proc.StartInfo.CreateNoWindow = true;
            
            if (requireAdmin) {
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
                return null;
            } else {
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                string res = proc.StandardOutput.ReadToEnd();
                proc.StandardOutput.Close();
                proc.Close();
                return res;
            }
        }
    }
}
