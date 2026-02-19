using System;
using System.Diagnostics;
using NetFwTypeLib;

namespace ScreenTask {
    class FirewallConf {
        public void AddRule(
            string name,
            string description,
            string ip,
            int port
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
            rule.InterfaceTypes = "All";
            rule.Enabled = true;

            FwPolicy.Rules.Add(rule);
            AddACL(ip, port);
        }

        public void AddACL(string ip, int port) {
            RunCMD($"netsh http add urlacl url=http://{ip}:{port}/ user=Everyone listen=yes", true);
        }

        public void DeleteACL(string ip, int port) {
            RunCMD($"netsh http delete urlacl url=http://{ip}:{port}/", true);
        }

        public void DeleteRule(string name, string ip, int port) {
            Type Policy = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 FwPolicy = (INetFwPolicy2)Activator.CreateInstance(Policy);
            FwPolicy.Rules.Remove(name);
            DeleteACL(ip, port);
        }

        private string RunCMD(string cmd, bool requireAdmin = false) {
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
