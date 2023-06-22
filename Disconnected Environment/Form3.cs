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
    public partial class Form3 : Form
    {
        private string connectionString = "Server=localhost;Database=blob;Uid=root;Pwd=BSBPfeb1234_;";
        private MySqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customerbindingSource = new BindingSource();
        public Form3()
        {
            InitializeComponent();
            koneksi = new MySqlConnection(connectionString);
            this.bnMahasiswa.BindingSource = this.customerbindingSource;
            refreshform();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggalLahir.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text;
            nama = txtNama.Text;
            jk = cbxJenisKelamin.Text;
            alamat = txtAlamat.Text;
            tgl = dtTanggalLahir.Value;
            prodi = cbxProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from pabd.prodi where nama_prodi = @dd";
            MySqlCommand cm = new MySqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new MySqlParameter("@dd", prodi));
            MySqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "insert into pabd.mahasiswa (nim, nama_mahasiswa, jenis_kel, alamat, tgl_lahir, id_prodi)" + "values(@NIM, @Nm, @Jk, @Al, @Tgll, @Idp)";
            MySqlCommand cmd = new MySqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new MySqlParameter("NIM", nim));
            cmd.Parameters.Add(new MySqlParameter("Nm", nama));
            cmd.Parameters.Add(new MySqlParameter("JK", jk));
            cmd.Parameters.Add(new MySqlParameter("AL", alamat));
            cmd.Parameters.Add(new MySqlParameter("Tgll", tgl));
            cmd.Parameters.Add(new MySqlParameter("Idp", hs));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

            refreshform();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void FormDataMahasiswa_Load()
        {
            koneksi.Open();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(new MySqlCommand("Select m.nim, m.nama_mahasiswa, " + "m.alamat, m.jenis_kel, m.tgl_lahir, p.nama_prodi From pabd.mahasiswa m " + "join pabd.prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            this.customerbindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "NIM", true));
            this.txtNama.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "alamat", true));
            this.cbxJenisKelamin.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "jenis_kel", true));
            this.dtTanggalLahir.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(
                new Binding("Text", this.customerbindingSource, "nama_prodi", true));
            koneksi.Close();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }

        private void refreshform()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            cbxJenisKelamin.Enabled = false;
            txtAlamat.Enabled = false;
            dtTanggalLahir.Enabled = false;
            cbxProdi.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            clearBinding();
            FormDataMahasiswa_Load();
        }

        private void Prodicbx()
        {
            koneksi.Open();
            string str = "select nama_prodi from pabd.Prodi";
            MySqlCommand cmd = new MySqlCommand(str, koneksi);
            MySqlDataAdapter da = new MySqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
