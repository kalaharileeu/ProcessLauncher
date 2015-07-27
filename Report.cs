/**************************************************************************************
http://nz-hwlab-ws1:8000/dashboard/update/?script=My%20Script&status=run&progress=.8536

Script is the name of your script. Be sure to urlencode if it has spaces (these should become %20)
Status should be "Running", "Failed", or "Complete"
Progress is optional. It is a float from 0.0 (0%) to 1.0 (100%)

***************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace PyLauncher
{
    class Report
    {
        string varstatus = "unknown";
        string varerror = ".";
        HttpWebRequest req;
        HttpWebResponse resp;
        int count = 0;
        string vartestname;

        public Report()
        {
            //varstatus = "unknown";
            //varerror = ".";
            vartestname = "default";
            req = null;
            resp = null;
        }

        public string Reporttoserver(string status, string testname, ref string error, string emailaddress)
        {
           vartestname = testname;
           varstatus = status;
           varerror = error;
           count += 1;
           if(status == "Complete" && emailaddress.Length > 5)
               email(emailaddress);
           string responsestring = ".";
            try
            {
                req = (HttpWebRequest)WebRequest.Create("http://nz-hwlab-ws1:80/dashboard/update/?script=" + testname + "&status=" + status); //Complete
                //req.Timeout = 200;
                resp = (HttpWebResponse)req.GetResponse();
                Stream istrm = resp.GetResponseStream();
                int ch;
                for (int ij = 1; ; ij++)
                {
                    ch = istrm.ReadByte();
                    if (ch == -1) break;
                    responsestring += ((char)ch);
                }
                resp.Close();
                //email();
                return responsestring + " : from server";
            }
            catch (System.Net.WebException e)
            {
                error = e.Message;
                System.Console.WriteLine(error);
                string c = count.ToString();
                return "ServerError " + c;
            }
        }

        public void email(string emailaddr)
        {
            try 
            {
                var fromAddress = new MailAddress("m!#$@gmail.com", "mjohny");
                var toAddress = new MailAddress(emailaddr, "Tester");
                const string fromPassword = "monkey";
                string subject = "Test done: " + vartestname;
                const string body = "This email is sent because a test has been done.";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch(Exception e)
            {
                if(e is System.Net.Mail.SmtpException || e is System.Net.Mail.SmtpFailedRecipientException)
                    return;
            }
        }
    }
}
