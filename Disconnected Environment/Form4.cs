using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Disconnected_Environment
{
    public partial class Form4 : Form
    {
        private string connectionString = "Server=localhost;Database=blob;Uid=root;Pwd=BSBPfeb1234_;";
        private MySqlConnection koneksi;
        public Form4()
        {
            InitializeComponent();
            koneksi = new MySqlConnection(connectionString);
            refreshform();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }

        private void dataGridView()
        {
            koneksi.Open();
            string str = "select * from pabd.status_mahasiswa";
            MySqlDataAdapter da = new MySqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void cbNama()
        {
            koneksi.Open();
            string str ="select nama_mahasiswa from pabd.mahasiswa where " + 
                "not EXISTS(select id_status from pabd.status_mahasiswa where " + 
                "status_mahasiswa.nim = mahasiswa.nim)";
            MySqlCommand cmd = new MySqlCommand(str, koneksi);
            MySqlDataAdapter da = new MySqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            cbxNama.DisplayMember = "nama_mahasiswa";
            cbxNama.ValueMember = "NIM";
            cbxNama.DataSource = ds.Tables[0];
        }

        private void cbTahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i=0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTahunMasuk.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + 1;
                    cbxTahunMasuk.Items.Add(l.ToString());
                }
            }
        }

        private void cbxNama_SelectedIndexChanged(Object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from pabd.mahasiswa where nama_mahasiswa = @nm";
            MySqlCommand cm = new MySqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new MySqlParameter("@nm", cbxNama.Text));
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read()) ;
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            koneksi.Open();

            string str = "select count (*) from pabd.status_mahasiswa";
            MySqlCommand cm = new MySqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();
            if (count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string queryString = "select Max(id_status) from pabd.status_mahasiswa";
                MySqlCommand cmStatusMahasiswaSum = new MySqlCommand(str, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                kodeStatus = Convert.ToString(finalKodeStatusInt);
            }
            string queryString = "insert into pabd.status_mahasiswa (id_status,nim, " +
                "status_mahasiswa, tahun_masuk)" + "values(@ids, @NIM, @sm, @tm)";
            MySqlCommand cmd = new MySqlCommand(queryString, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new MySqlParameter("ids", kodeStatus));
            cmd.Parameters.Add(new MySqlParameter("NIM", nim));
            cmd.Parameters.Add(new MySqlParameter("sm", statusMahasiswa));
            cmd.Parameters.Add(new MySqlParameter("tm", tahunMasuk));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
