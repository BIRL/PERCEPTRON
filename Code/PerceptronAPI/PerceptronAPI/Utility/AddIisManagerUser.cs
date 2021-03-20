using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PerceptronAPI.Utility
{
    public class AddIisManagerUser
    {
        public void CreatingUser(string UserName, string Password, string FtpSiteName, string FtpPathDir)
        {
            try
            {

                string AdminConfigIISFile = @"C:\Windows\System32\inetsrv\config\administration.config";
                string AdminConfigIISExpressFile = @"C:\Program Files\IIS Express\config\administration.config";

                // ONLY NEED THIS ( File.Copy(AdminConfigIISFile, AdminConfigIISExpressFile, true);  ) BELOW CODE WHILE TESTING LOCALLY
                File.Copy(AdminConfigIISFile, AdminConfigIISExpressFile, true);   //Copying file from "inetsrv\config"  to   "IIS Express\config" for editting

                // create Powershell runspace
                string scriptText = "[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.Web.Management');" +
                    "[Microsoft.Web.Management.Server.ManagementAuthentication]::CreateUser('" + UserName + "','" + Password + "');" +
                    "[Microsoft.Web.Management.Server.ManagementAuthorization]::Grant('" + UserName + "','" + FtpSiteName + "', $FALSE)";

                Runspace runspace = RunspaceFactory.CreateRunspace();

                runspace.Open();

                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript(scriptText);

                pipeline.Commands.Add("Out-String");

                Collection<PSObject> results = pipeline.Invoke();
                runspace.Close();

                // ONLY NEED THIS ( File.Copy(AdminConfigIISExpressFile, AdminConfigIISFile, true); ) BELOW CODE WHILE TESTING LOCALLY
                File.Copy(AdminConfigIISExpressFile, AdminConfigIISFile, true);   //Copying BACK EDITTED FILE from  "IIS Express\config" to "inetsrv\config"  

                string strCmdText = "C:/Windows/System32/inetsrv/appcmd.exe set config " +
                    FtpSiteName + " - section:system.ftpServer / security / authorization / +'[accessType='Allow',users='" + UserName + "',permissions='Read, Write']' / commit:apphost";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);

                string directory = FtpPathDir + UserName;
                Directory.CreateDirectory(directory);
            }
            catch (Exception e)
            {

            }   
        }
    }
}