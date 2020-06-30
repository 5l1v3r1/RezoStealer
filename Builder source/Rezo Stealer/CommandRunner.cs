using System;
using System.Diagnostics;

namespace Rezo_Stealer
{
    public static class CommandRunner
    {
        public static void RunFile(string filename, string args)
        {
            if (!string.IsNullOrWhiteSpace(filename))
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        Arguments = args,
                        FileName = filename,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                    };
                    using var info = Process.Start(startInfo);
                    info.Refresh();
                    info.WaitForExit();
                }
                catch (Exception ex) { throw new Exception("Error Running Obfuscation", ex); }
            }
        }
    }
}