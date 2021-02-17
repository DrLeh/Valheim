using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;

namespace ValheimBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            var worldName = @"CockyLittleFreaks";
            var basePath = @"C:\Users\Devon\AppData\LocalLow\IronGate\Valheim\worlds\";
            var db = Path.Combine(basePath, $"{worldName}.db");
            var fwl = Path.Combine(basePath, $"{worldName}.fwl");

            var backupLocation = @"C:\Projects\Valheim\worlds";
            var newDbPath = Path.Combine(backupLocation, $"{worldName}.db");
            var newFwlPath = Path.Combine(backupLocation, $"{worldName}.fwl");

            File.Copy(db, newDbPath, true);
            File.Copy(fwl, newFwlPath, true);

            GitCommitAndPush();
            Console.WriteLine("Done");
            Console.ReadLine();

        }


        public static void GitCommitAndPush()
        {
            var basePath = @"C:\Projects\Valheim\";
            using (var powershell = PowerShell.Create())
            {
                // this changes from the user folder that PowerShell starts up with to your git repository
                powershell.AddScript($"cd {basePath}");

                powershell.AddScript(@"git init");
                powershell.AddScript(@"git add *");
                powershell.AddScript($@"git commit -m '{DateTime.Now.ToShortDateString()} Backup'");
                powershell.AddScript(@"git push");

                var results = powershell.Invoke();
            }
        }
    }

}
