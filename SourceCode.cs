using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        
        bool Start = false;
        public Form1()
        {
            InitializeComponent();
            button1.Visible = false;
        }

      
        BackgroundWorker backgroundWorker;
        private void Send_Click(object sender, EventArgs e)
        {
            string From = textBox1.Text;
            string To = textBox5.Text;
            string Password = textBox2.Text;
            string Subject = textBox3.Text;
            string Content = textBox4.Text;
            
            if (string.IsNullOrWhiteSpace(From) == false && string.IsNullOrWhiteSpace(To) == false && string.IsNullOrWhiteSpace(Password) == false && string.IsNullOrWhiteSpace(Subject) == false && string.IsNullOrWhiteSpace(Content) == false)
            {
                //Console.WriteLine("Sent an email to " + To + ". Using the email " + From + ", password " + Password + ". With the subject " + Subject + " and it contained " + Content);
                Start = true;
                label3.Text = "Running";
                button1.Visible = true;
                Send.Visible = false;
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += (obj, ea) => TaskAsync(From, To, Password, Subject, Content);
                backgroundWorker.RunWorkerAsync();


            } else
            {
                label3.Text = "Error, one or more fields are empty.";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Send.Visible = true;
            button1.Visible = false;
            Console.WriteLine("Stopping!");
            Start = false;
            label3.Text = "Stopped";
        }



        private async void TaskAsync(string From, string To, string Password, string Subject, string Content)
        {
            // The Email Sender
            while (Start == true)
            {
                try
                {
                    //System.Threading.Thread.Sleep(1000);
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;

                    NetworkCredential Pass = new NetworkCredential(From, Password);
                    MailMessage Mail = new MailMessage();
                    smtpClient.Credentials = Pass;

                    Mail.From = new MailAddress(From);
                    Mail.To.Add(To);
                    Mail.Subject = Subject;
                    Mail.Body = Content;

                    smtpClient.Send(Mail);
                }
                catch
                {
                    this.BeginInvoke((Action)delegate ()
                    {
                        Start = false;
                        Send.Visible = true;
                        button1.Visible = false;
                        label3.Text = "Error, you have entered a field correctly.";
                    });
                }
            }
        }
    }
}
