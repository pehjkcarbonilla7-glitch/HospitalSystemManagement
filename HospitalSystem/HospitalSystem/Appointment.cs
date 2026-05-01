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
    public partial class Appointment : Form
    {

        string API = "https://localhost:7029/api/Appointments";
        public Appointment()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Doctors doctors = new Doctors();    
            doctors.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddNewPatient addNewPatient = new AddNewPatient();
            addNewPatient.Show();
            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private async void Appointment_Load(object sender, EventArgs e)
        {
            await LoadTodayAppointments();
        }

        private async Task LoadTodayAppointments()
        {
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true;

                var json = await client.GetStringAsync(API + "/today");
                var data = JsonConvert.DeserializeObject<List<dynamic>>(json);

                dataGridView1.DataSource = data;
            }
        }

        int selectedId = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                selectedId = Convert.ToInt32(row.Cells["id"].Value);

                string fullName = row.Cells["patientName"].Value.ToString();
                var split = fullName.Split(' ');

                txtFirstName.Text = split.Length > 0 ? split[0] : "";
                txtMiddleName.Text = "";
                txtDoctorName.Text = row.Cells["doctorName"].Value.ToString();

                dtpDate.Value = Convert.ToDateTime(row.Cells["appointmentDate"].Value);
                txtReason.Text = row.Cells["reason"].Value.ToString();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private async void btnComplete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select appointment first!");
                return;
            }

            var data = new
            {
                prescription = txtPrescription.Text
            };

            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (s, cert, chain, sslPolicyErrors) => true;

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PutAsync(API + "/complete/" + selectedId, content);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Appointment Completed!");
                    await LoadTodayAppointments();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error: " + res.StatusCode);
                }
            }
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtDoctorName.Clear();
            txtReason.Clear();
            txtPrescription.Clear();
            selectedId = 0;
        }
    }
}
