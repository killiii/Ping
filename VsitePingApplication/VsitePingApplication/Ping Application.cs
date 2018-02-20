using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace VsitePingApplication
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            this.Text = "VSITE C# PingApp";

        }
 
        private void Button1_Click(object sender, EventArgs e)
        {
            
                if (int.TryParse(textBox2.Text,out int value) && value !=0 && value < 86400)  //parsiramo vrijednost textboxa2 (interval) za timer.
                {
                    timer1.Interval = value * 1000;   //uneseni broj mnozimo s tisucu (Timer.Interval izrazen u milisekundama)
                }
                else
                {
                    textBox2.Text = "Invalid input!";
                    return;
                }
                timer1.Start();  //pokrećemo timer
                PingMethod();    

        }

        private PingResponse DoTheJob(string ip)
        {
            
            Ping isPing = new Ping();
            PingResponse pr = new PingResponse();   //instanciramo objekt naseg tipa PingResponse 

            try { 
            PingReply reply = isPing.Send(ip);   //saljemo ping prema ip-u,rezultat spremamo u objekt tipa PingReply.
            pr.Status =reply.Status == IPStatus.Success;    
            pr.TripTime = reply.RoundtripTime;  
                return pr;
            }
            catch(PingException)  //hvatamo iznimku koju eventualno baca isPing.Send 
            {
                pr.Status = false;
                pr.TripTime = 0;
                return pr;
            }

        }
        private async void PingMethod()
        {
            Task<PingResponse> obj = new Task<PingResponse>(
                () => DoTheJob(textBox1.Text));                     //lambda izrazom pridruzujemo pripadajucu funkciju Task objektu. 
            obj.Start();                                            //osiguravamo da nova nit izvodenja ne barata elementima glavne niti i da ne blokiramo rad aplikacije.
            panel1.Visible = true;
            await obj;                     //cekamo asinkrono izvodenje Metode DoTheJob 
            PingResponse pr = obj.Result;
            ipstatus1.Text = pr.Status ? "Available": "Unavailable";        
            rt1.Text =pr.TripTime.ToString() + " miliseconds";
            }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            ipstatus1.Text = "";
            rt1.Text = "";
            timer1.Stop();

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            PingMethod();                                       //svakih n sekundi intervala osiguravamo izvodenje ping upita.
        }
    }
}
