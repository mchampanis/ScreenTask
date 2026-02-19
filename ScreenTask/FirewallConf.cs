using System;
using System.Diagnostics;

namespace ScreenTask {
    class FirewallConf {
        private const int NET_FW_ACTION_ALLOW = 1;
        private const int NET_FW_RULE_DIR_IN = 1;

        public void AddRule(
            string name,
            string description,
            string ip,
            int port
        ) {
            dynamic fwPolicy = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            dynamic rule = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));

            DeleteRule(name, ip, port);

            rule.Name = name;
            rule.Description = description;
            rule.Protocol = 6; // TCP/IP
            rule.LocalPorts = port.ToString();
            rule.RemoteAddresses = "localsubnet";
            rule.Action = NET_FW_ACTION_ALLOW;
            rule.Direction = NET_FW_RULE_DIR_IN;
            rule.InterfaceTypes = "All";
            rule.Enabled = true;

            fwPolicy.Rules.Add(rule);
            AddACL(ip, port);
        }

        public void AddACL(string ip, int port) {
            RunCMD($"netsh http add urlacl url=http://{ip}:{port}/ user=Everyone listen=yes", true);
        }

        public void DeleteACL(string ip, int port) {
            RunCMD($"netsh http delete urlacl url=http://{ip}:{port}/", true);
        }

        public void DeleteRule(string name, string ip, int port) {
            dynamic fwPolicy = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            fwPolicy.Rules.Remove(name);
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
