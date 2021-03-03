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
                // create Powershell runspace
                string scriptText = @"[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.Web.Management')" + "\n" +
                    "[Microsoft.Web.Management.Server.ManagementAuthentication]::CreateUser('" + UserName + "','" + Password + "')" + "\n" +
                     "[Microsoft.Web.Management.Server.ManagementAuthorization]::Grant('" + UserName + "','" + FtpSiteName + "', $FALSE)";

                Runspace runspace = RunspaceFactory.CreateRunspace();

                // open it

                runspace.Open();

                // create a pipeline and feed it the script text

                Pipeline pipeline = runspace.CreatePipeline();
                pipeline.Commands.AddScript(scriptText);

                // add an extra command to transform the script
                // output objects into nicely formatted strings

                // remove this line to get the actual objects
                // that the script returns. For example, the script

                // "Get-Process" returns a collection
                // of System.Diagnostics.Process instances.

                pipeline.Commands.Add("Out-String");

                // execute the script
                Collection<PSObject> results = pipeline.Invoke();

                // close the runspace
                runspace.Close();

                // convert the script result into a single string
                //StringBuilder stringBuilder = new StringBuilder();
                //foreach (PSObject obj in results)
                //{

                //    Console.WriteLine(stringBuilder.AppendLine(obj.ToString()));
                //}

                string strCmdText = "C:/Windows/System32/inetsrv/appcmd.exe set config " +
                    FtpSiteName + " - section:system.ftpServer / security / authorization / +'[accessType='Allow',users='" + UserName + "',permissions='Read, Write']' / commit:apphost";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);

                string directory = FtpPathDir + UserName;
                Directory.CreateDirectory(directory);
            }
            catch(Exception e)
            {
                
            }
            
        }
    }
}