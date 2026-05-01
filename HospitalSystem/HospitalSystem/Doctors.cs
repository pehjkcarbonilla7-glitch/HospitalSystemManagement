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
    public partial class Doctors : Form
    {

        string API = "https://localhost:7029/api/Doctors";
        public Doctors()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddNewPatient addNewPatient = new AddNewPatient();
            addNewPatient.Show();
            this.Hide();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment();
            appointment.Show();
            this.Hide();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private async Task LoadDoctors()
        {
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true;

                var json = await client.GetStringAsync(API);
                var data = JsonConvert.DeserializeObject<List<dynamic>>(json);

                dataGridView1.DataSource = data;
            }
        }

        private async void Doctors_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            await LoadDoctors();
        }

        private async void guna2Button5_Click(object sender, EventArgs e)
        {
            var data = new
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                specialization = txtSpecialization.Text,
                department = txtDepartment.Text,
                phone = txtPhone.Text,
                email = txtEmail.Text
            };

            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (s, cert, chain, sslPolicyErrors) => true;

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(API, content);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Doctor Added!");
                    await LoadDoctors();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show(await res.Content.ReadAsStringAsync());
                }
            }
        }

        int selectedId = 0;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                selectedId = Convert.ToInt32(row.Cells["id"].Value);

                txtFirstName.Text = row.Cells["firstName"].Value.ToString();
                txtLastName.Text = row.Cells["lastName"].Value.ToString();
                txtSpecialization.Text = row.Cells["specialization"].Value.ToString();
                txtDepartment.Text = row.Cells["department"].Value.ToString();
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();

                btnSubmit.Visible = false;
                btnReset.Visible = false;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Select a doctor first!");
                return;
            }

            var confirm = MessageBox.Show("Delete this doctor?", "Confirm", MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes) return;

            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (s, cert, chain, sslPolicyErrors) => true;

                var res = await client.DeleteAsync(API + "/" + selectedId);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Doctor Deleted!");
                    await LoadDoctors();
                    ClearFields();

                    selectedId = 0;
                }
            }

            btnDelete.Visible = false;
            btnUpdate.Visible= false;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var data = new
            {
                id = selectedId,
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                specialization = txtSpecialization.Text,
                department = txtDepartment.Text,
                phone = txtPhone.Text,
                email = txtEmail.Text
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PutAsync(API + "/" + selectedId, content);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Doctor Updated!");
                    await LoadDoctors();
                    ClearFields();

                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                }
                else
                {
                    MessageBox.Show(await res.Content.ReadAsStringAsync());
                }
            }

            btnDelete.Visible = false;
            btnUpdate.Visible = false;
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSpecialization.Clear();
            txtDepartment.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
    }
}
