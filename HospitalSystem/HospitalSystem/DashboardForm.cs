using Newtonsoft.Json;
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

namespace HospitalSystem
{
    public partial class DashboardForm : Form
    {


        string API = "https://localhost:7029/api";
        public DashboardForm()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void btnDoctors_Click(object sender, EventArgs e)
        {
            Doctors doctors   = new Doctors();
            doctors.Show();
            this.Hide();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            AddNewPatient addNewPatient =   new AddNewPatient();
            addNewPatient.Show();
            this.Hide();
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Show();
            this.Hide();
        }

        private async void DashboardForm_Load(object sender, EventArgs e)
        {
            await LoadDashboard();
        }

        private async Task LoadDashboard()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // SSL fix
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        (sender, cert, chain, sslPolicyErrors) => true;

                    // ======================
                    // PATIENTS
                    // ======================
                    var patientRes = await client.GetStringAsync(API + "/Patient");
                    var patients = JsonConvert.DeserializeObject<List<object>>(patientRes);
                    lblPatients.Text = patients.Count.ToString();

                    // ======================
                    // DOCTORS
                    // ======================
                    var doctorRes = await client.GetStringAsync(API + "/Doctors");
                    var doctors = JsonConvert.DeserializeObject<List<object>>(doctorRes);
                    lblDoctors.Text = doctors.Count.ToString();

                    // ======================
                    // APPOINTMENTS
                    // ======================
                    var apptRes = await client.GetStringAsync(API + "/Appointments");
                    var appointments = JsonConvert.DeserializeObject<List<object>>(apptRes);
                    lblAppointments.Text = appointments.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading dashboard: " + ex.Message);
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            AddNewPatient addNewPatient = new AddNewPatient();
            addNewPatient.Show();
            this.Hide();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Appointment appointment     = new Appointment();
            appointment.Show();
            this.Hide();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Doctors doctors = new Doctors();
            doctors.Show();
            this.Hide();
        }
    }
}
