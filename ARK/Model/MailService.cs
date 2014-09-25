using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARK.Model;
using System.Net.Mail;


namespace ARK.Model
{
    class MailService
    {
        public static void sendUnhandledException(System.Windows.Threading.DispatcherUnhandledExceptionEventArgs ex)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("aau304@gmail.com");
            mail.To.Add("aau304@gmail.com");
            mail.Subject = "ARK - Exception";
            mail.Body = "Der er sket en fejl på systemet, som kræver din opmærksomhed, følgende data blev samlet:\n";
            mail.Body += "Exception Name: " + ex.Exception.ToString() + "\n";
            mail.Body += "Inner Exception: " + ex.Exception.InnerException + "\n";
            mail.Body += "Exception Besked: " + ex.Exception.Message + "\n";
            mail.Body += "Help Link: " + ex.Exception.HelpLink + "\n";
            mail.Body += "TargetSite: " + ex.Exception.TargetSite + "\n";
            mail.Body += "StackTrace: " + ex.Exception.StackTrace + "\n";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aau304@gmail.com", "aalborguniversitet");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        // Method that sends a mail to admins responsible for longTrips
        public static void sendNewLongTrip()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("aau304@gmail.com");
            using (var db = DB.DbArkContext.GetDbContext())
            {
                foreach (Admin m in db.Admin.Where(x => x.ContactTrip == true))
                {
                    mail.To.Add(m.Member.Email1);
                }
            } 
            mail.Subject = "ARK - Ny Langtur";
            mail.Body = "Der er ansøgt om en ny langtur.\n";
            
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aau304@gmail.com", "aalborguniversitet");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        // Method that sends a mail to admins responsible for boat material
        public static void sendNewDamage()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("aau304@gmail.com");
            
            using (var db = DB.DbArkContext.GetDbContext()){
                foreach (Admin m in db.Admin.Where(x=> x.ContactDamage == true)){
                    mail.To.Add(m.Member.Email1);
                }
            }

            mail.To.Add("adminsBoadDamage@mail.dk");        // not valid mail  
            mail.Subject = "ARK - Ny Bådskade";
            mail.Body = "Der er sket skade på en båd.\n";
            
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("aau304@gmail.com", "aalborguniversitet");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
