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
    public partial class AddNewPatient : Form
    {
        int lastCount = 0;

        string API = "https://localhost:7029/api/Patient";
        int selectedId = 0;
        public AddNewPatient()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
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

        private async void AddNewPatient_Load(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            await LoadPatients();

        }

        private async Task LoadPatients()
        {
            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => true;

                var json = await client.GetStringAsync(API);
                var data = JsonConvert.DeserializeObject<List<dynamic>>(json);

                // REFRESH ONLY IF NAA CHANGE
                if (data.Count != lastCount)
                {
                    dataGridView1.DataSource = data;
                    lastCount = data.Count;
                }
            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            var data = new
            {
                firstName = txtFirstName.Text,
                middleName = txtMiddleName.Text,
                lastName = txtLastName.Text,
                dateOfBirth = dtpDOB.Value.ToString("yyyy-MM-dd"),
                gender = cmbGender.Text,
                civilStatus = cmbStatus.Text,
                address = txtAddress.Text,
                phone = txtPhone.Text,
                email = txtEmail.Text
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync(API, content);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Patient Added!");
                    await LoadPatients();
                    ClearFields();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                var row = dataGridView1.Rows[e.RowIndex];

                selectedId = Convert.ToInt32(row.Cells["id"].Value);

                txtFirstName.Text = row.Cells["firstName"].Value.ToString();
                txtMiddleName.Text = row.Cells["middleName"].Value.ToString();
                txtLastName.Text = row.Cells["lastName"].Value.ToString();
                dtpDOB.Value = Convert.ToDateTime(row.Cells["dateOfBirth"].Value);
                cmbGender.Text = row.Cells["gender"].Value.ToString();
                cmbStatus.Text = row.Cells["civilStatus"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                txtPhone.Text = row.Cells["phone"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();

                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                btnReset.Visible = false;
                btnDelete.Visible = true;
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var data = new
            {
                id = selectedId,
                firstName = txtFirstName.Text,
                middleName = txtMiddleName.Text,
                lastName = txtLastName.Text,
                dateOfBirth = dtpDOB.Value.ToString("yyyy-MM-dd"),
                gender = cmbGender.Text,
                civilStatus = cmbStatus.Text,
                address = txtAddress.Text,
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
                    MessageBox.Show("Patient Updated!");
                    await LoadPatients();
                    ClearFields();

                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                }
            }
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            cmbGender.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();

            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await LoadPatients();
        }

        private async void guna2Button5_Click(object sender, EventArgs e)
        {
            await LoadPatients();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == 0)
            {
                MessageBox.Show("Please select a patient first!");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this patient?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            using (HttpClient client = new HttpClient())
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (s, cert, chain, sslPolicyErrors) => true;

                var res = await client.DeleteAsync(API + "/" + selectedId);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Patient Deleted!");

                    await LoadPatients();   // 🔥 auto refresh
                    ClearFields();

                    btnSubmit.Visible = true;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnReset.Visible = true;

                    selectedId = 0;
                }
                else
                {
                    var err = await res.Content.ReadAsStringAsync();
                    MessageBox.Show("Error: " + err);
                }
            }

        }
    }
}
