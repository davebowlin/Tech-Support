/*
 * Tech Support Request System
 * Version 1.1
 *      -- 28 February 2019
 *      -- Updated the GMail information.
 * Version 1.0
 *      -- 2 September 2016
 *      -- Uses GMail account to send email.
 */


using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HLC_Tech_Support
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Populate the form.
            txtUserName.Text = Environment.UserName;
            txtComputerName.Text = Environment.MachineName;
            string[] lineOfContents = File.ReadAllLines(@"issues.sup");
            foreach (var line in lineOfContents)
            {
                cboHelpArea.Items.Add(line);
            }
            cboHelpArea.SelectedIndex = 0;
        }

        private void cboHelpArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Focus();
        }

        private void btnStartOver_Click(object sender, EventArgs e)
        {
            CleanUp();
        }

        private void btnHelpl_Click(object sender, EventArgs e)
        {
            try
            {

                string smtpAddress = "smtp.gmail.com";
                int portNumber = 587;
                bool enableSSL = true;

                string emailFrom = "fromw@gmail.com";
                string password = "password";
                string emailTo = "to@gmail.com";
                string textTo = "1234567890@vtext.com";

                string subject = "HELP: " + cboHelpArea.SelectedItem;
                string body = "Username: " + txtUserName.Text + "<br /> " +"Computer: " + txtComputerName.Text + "<p>" + txtDescription.Text + "<p>" + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");
                string result = body
                    .Replace(Environment.NewLine, "<p>");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);

                    // if checked, send SMS as well as email
                    if (chkSMS.Checked)
                    {
                        mail.To.Add(textTo);
                    }

                    mail.Subject = subject;
                    mail.Body = result;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress,portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);

                        MessageBox.Show("Thank you. Your help request has been sent." + Environment.NewLine + "Please be patient. Your request will be" + Environment.NewLine + "answered as quickly as possible." + Environment.NewLine  + Environment.NewLine + "You do not need to resend this request.","Request Sent",MessageBoxButtons.OK,MessageBoxIcon.Information);

                        CleanUp();
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error has prevented your help request." + Environment.NewLine + "Make sure you are online, and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CleanUp()
        {
            cboHelpArea.SelectedIndex = 0;
            txtDescription.Clear();
            txtDescription.Focus();
        }
    }
}
