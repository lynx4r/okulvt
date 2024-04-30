using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Okul_VT_96
{
    public partial class OgrenciEkleForm : Form
    {
        string baglanti = "Server=localhost;Database=hnk_okul;Uid=root;Pwd='';";
        public OgrenciEkleForm()
        {
            InitializeComponent();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "INSERT INTO ogrenci VALUES(@okulno,@ad,@soyad,@tarih,@cinsiyet,@mezun)";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@okulno", txtOkulNo.Text).ToString();
                cmd.Parameters.AddWithValue("@ad", txtAd.Text);
                cmd.Parameters.AddWithValue("@soyad", txtSoyad.Text);
                cmd.Parameters.AddWithValue("@tarih", dtTarih.Value);
                cmd.Parameters.AddWithValue("@cinsiyet", cmbCinsiyet.SelectedValue);
                cmd.Parameters.AddWithValue("@mezun", cbMezun.Checked);

                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Kayıt Eklendi");
                    }
                }
                catch (MySqlException ex)
                {
                    
                    if(ex.Number == 1062)
                        MessageBox.Show("Bu okul numarası mevcuttur.");
                    else
                        MessageBox.Show("Bu başka bir hata var..");
                    
                }

                

            }

        }
        
    }
}
