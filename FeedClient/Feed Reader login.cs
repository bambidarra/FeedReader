using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace FeedClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string answer = new WebClient().UploadString("http://localhost:1846/Api/GetFeeds", "POST", "username=" + textBox1.Text + "&password=" + textBox2.Text);

            if (answer == "Login Failed")
            {
                MessageBox.Show("You fail go fuck a pig.");
            }
            else
            {
                MessageBox.Show("Hai piggy I lovded you piggy");
            }
        }
    }
}
