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
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            koneksi.Open();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(new MySqlCommand("Select m.nim, m.nama_mahasiswa, " + "m.alamat, m.jenis_kel, m.tgl_lahir, p.nama_prodi From pabd.mahasiswa m " + "join pabd.prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            this.customerbindingSource.DataSource = ds.Tables[0];

        }
    }
}
