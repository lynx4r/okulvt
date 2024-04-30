using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Okul_VT_96
{
    public partial class TumOgrencilerForm : Form
    {
        string baglanti = "Server=localhost;Database=hnk_okul;Uid=root;Pwd='';";
        public TumOgrencilerForm()
        {
            InitializeComponent();
        }

        private void dgwTumOgrenciler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwTumOgrenciler.SelectedRows.Count > 0)
            {
                txtAd.Text = dgwTumOgrenciler.SelectedRows[0].Cells["ad"].Value.ToString();
                txtSoyad.Text = dgwTumOgrenciler.SelectedRows[0].Cells["soyad"].Value.ToString();
                txtOkulNo.Text = dgwTumOgrenciler.SelectedRows[0].Cells["okul_no"].Value.ToString();
                dtTarih.Value = Convert.ToDateTime(dgwTumOgrenciler.SelectedRows[0].Cells["dtarih"].Value.ToString());
                cbMezun.Checked = Convert.ToBoolean(dgwTumOgrenciler.SelectedRows[0].Cells["mezun_durum"].Value);
                cmbCinsiyet.SelectedValue = dgwTumOgrenciler.SelectedRows[0].Cells["cinsiyet"].Value.ToString();
            }
        }

        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM ogrenci;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dgwTumOgrenciler.DataSource = dt;

            }
        }

        private void TumOgrencilerForm_Load(object sender, EventArgs e)
        {
            DgwDoldur();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgwTumOgrenciler.SelectedRows[0];

            int okulNo = Convert.ToInt32(dr.Cells[0].Value);

            DialogResult cevap = MessageBox.Show("Öğrenciyi silmek istediğinizden emin misiniz?",
                                                 "Öğrenciyi sil", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (cevap == DialogResult.Yes)
            {

                using (MySqlConnection baglan = new MySqlConnection(baglanti))
                {
                    baglan.Open();
                    string sorgu = "DELETE FROM ogrenci WHERE okul_no=@okulNo;";
                    MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@okulNo", okulNo);
                    cmd.ExecuteNonQuery();

                    DgwDoldur();
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "UPDATE ogrenci SET  ad=@ad, soyad = @soyad, dtarih = @tarih, cinsiyet=@cinsiyet, mezun_Durum= @mezun WHERE okul_no =@okul_no;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@okulno", txtOkulNo.Text);
                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                cmd.Parameters.AddWithValue("@tarih", dtTarih.Value);
                cmd.Parameters.AddWithValue("@cinsiyet", cmbCinsiyet.SelectedValue);
                cmd.Parameters.AddWithValue("@mezun", cbMezun.Checked);

                int no = Convert.ToInt32(dgwTumOgrenciler.SelectedRows[0].Cells["okul_no"].Value);
                cmd.Parameters.AddWithValue("@okul_no", no);

                cmd.ExecuteNonQuery();

                DgwDoldur();

            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string sorgu = " SELECT * FROM ogrenci WHERE ad LIKE @aranan;";
            if (rbAd.Checked)
            {
                sorgu = "SELECT * FROM ogrenci WHERE ad LIKE @aranan;";
            }

            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@aranan", "%" + txtArama.Text + "%");
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgwTumOgrenciler.DataSource = dt;
            }
        }
    }
}
