using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CustomDomain.Classes
{
    public class AccessControlList
    {
        internal static void AddUrl(string Url)
        {
            string cmd = "/c netsh http add urlacl url=" + Url + " user=everyone";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = cmd;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
        }
    }
}