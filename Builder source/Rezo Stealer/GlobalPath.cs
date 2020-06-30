using System;
using System.IO;

namespace Rezo_Stealer
{
    public static class GlobalPath
    {
        public static readonly string CurrDir = Environment.CurrentDirectory;
        public static readonly string PathDark = Path.Combine(CurrDir, "Dark");
        public static readonly string CLI_Confuser = Path.Combine(PathDark, "Confuser.CLI.exe");
        public static readonly string DarkConfig = Path.Combine(PathDark, "temp.darkpr");
    }
}