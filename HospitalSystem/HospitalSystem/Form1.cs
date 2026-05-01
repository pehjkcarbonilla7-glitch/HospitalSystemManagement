using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HospitalSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

           
          
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password");
                return;
            }

            var data = new
            {
                username = username,
                password = password
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 🔥 IMPORTANT (SSL fix)
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        (s, cert, chain, sslPolicyErrors) => true;

                    var response = await client.PostAsync("https://localhost:7029/api/Users/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Login Success!");

                        DashboardForm dash = new DashboardForm();
                        dash.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void rdbShow_CheckedChanged(object sender, EventArgs e)
        {

            if (rdbShow.Checked)
            {
                txtPassword.PasswordChar = '\0'; // show password
            }
            else
            {
                txtPassword.PasswordChar = '*'; // hide password
            }
        }
    }
}
