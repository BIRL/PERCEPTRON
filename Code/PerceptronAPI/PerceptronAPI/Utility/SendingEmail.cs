using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace PerceptronAPI.Utility
{
    public static class SendingEmail
    {
        public static void SendingEmailMethod(string UserEmailAddress, string StringInfo, string CreationTime, string EmailMessage)// Here StringInfo will behave based on EmailMessage either as JobTitle or UniqueUserGuid
        {
            StreamReader ReadPerceptronEmailAddress = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailAddress.txt");
            StreamReader ReadPerceptronEmailPassword = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailPassword.txt");
            string PerceptronEmailAddress = ReadPerceptronEmailAddress.ReadLine();
            string PerceptronEmailAddressPassword = ReadPerceptronEmailPassword.ReadLine();


            using (var mm = new MailMessage(PerceptronEmailAddress, UserEmailAddress))
            {
                string BaseUrl = "https://perceptron.lums.edu.pk/";


                if (EmailMessage == "Error") // Email Msg for Something Wrong With Entered Query     // StringInfo 
                {
                    mm.Subject = "PERCEPTRON: Protein Search Results";
                    var body = "Dear User,";
                    body += "<br/><br/> Search couldn't complete for protein search query submitted at " + CreationTime + " with job title \"" +
                            StringInfo + "\" Please check your search parameters and data file.";
                    //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                    body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide" +
                        " and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If " +
                        "problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }
                else if (EmailMessage == "QuerySuccessfullySubmitted")
                {
                        mm.Subject = "PERCEPTRON: Protein Search Query Submitted";
                        var body = "Dear User,";
                        body += "<br/><br/> Your protein search query successfully submitted at " + CreationTime + " with job title \"" +
                                StringInfo + "\" Please check your query status at <a href=\'" + BaseUrl + "index.html#/history \'>User Search History</a>.";
                        body += "</br> If you need help check out the <a href=\'" + BaseUrl + "index.html#/getting \'>Getting Started</a> guide " +
                        "and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>.";

                        body += "</br></br>Thank You for using Perceptron.";
                        body += "</br><b>The PERCEPTRON Team</b>";
                        body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                        mm.Body = body;
                }
                else if (EmailMessage == "PerceptronSdkEmailVerification")
                {
                    mm.Subject = "PERCEPTRON SDK: Email Verification";
                    var body = "Dear User,";
                    body += "<br/> To complete your Calling PERCEPTRON API sign up, we just need to verify your email address: " + UserEmailAddress +
                        ". So, please copy the below line ";
                    body += "<br/> UserUniqueId = '" + StringInfo + "'" + 
                        "<br/>and paste into the function of VerfiyingEmailAddress as instructed you there.";
                    body += "</br>Once verified, you can start using Calling PERCEPTRON API for proteoform search.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }

                mm.IsBodyHtml = true;
                var networkCred = new NetworkCredential(PerceptronEmailAddress, PerceptronEmailAddressPassword);
                var smtp = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Credentials = networkCred,
                    Port = 587
                };
                try
                {
                    smtp.Send(mm);
                }
                catch (Exception e)
                {
                    if (e is System.Net.Mail.SmtpException)
                        UserEmailAddress = "das bad";

                }
            }
        }
    }
}