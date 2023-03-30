using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerForParents
{
    public partial class KayitEkrani : Form
    {
        public KayitEkrani()
        {
            InitializeComponent();
        }

        public int kullaniciID = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = "";
            string sifre = "";
            int kayitTuru = 0;
            string mail = "";
            //Bilgileri girilen kişiyi veritabanına kaydetme
            if (!kullanicilar.Contains(textBox1.Text.Trim()))
            {
                if (textBox1.Text!=""&textBox2.Text!="")
                {
                    kullaniciAdi = textBox1.Text;
                    kullaniciAdi = kullaniciAdi.Trim();
                    sifre = textBox2.Text;
                    if (textBox3.Text.Contains("@") & textBox3.Text.Contains(".com"))
                    {
                        mail = textBox3.Text;
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir e-posta adresi girin!", "UYARI");
                        goto don;
                    }
                    if (comboBox1.SelectedItem=="Ebeveyn")
                    {
                        kayitTuru = 1;
                    }else if (comboBox1.SelectedItem=="Çocuk")
                    {
                        kayitTuru = 2;
                    }
                    SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
                    SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Kullanicilar (kullaniciAd,kullaniciSifre,tYetkiID,addedBy,mail) VALUES ($ad,$soyad,$tur,$ekleyen,$mail)", con);
                    cmd.Parameters.AddWithValue("$ad", kullaniciAdi);
                    cmd.Parameters.AddWithValue("$soyad", sifre);
                    cmd.Parameters.AddWithValue("$tur", kayitTuru);
                    cmd.Parameters.AddWithValue("$ekleyen", kullaniciID);
                    cmd.Parameters.AddWithValue("$mail", mail);
                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen bütün alanları doldurun!", "UYARI");
                    goto don;
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı sistemde zaten kayıtlı!");
                goto don;
            }

            MessageBox.Show("Kullanıcı başarıyla kaydedildi!");
            GirisEkrani frmkGirisEkrani = new GirisEkrani();
            this.Close();
            don: ;
        }

        public List<string> kullanicilar = new List<string>();
        private void KayitEkrani_Load(object sender, EventArgs e)
        {
           //Kaydedilecek kişi bilgilerinin tekrar edilmemesi için form yüklendiğinde sistemde kayıtlı olan kullanıcıları datagridview'a aktarma
            comboBox1.SelectedIndex = 0;
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            long yetki = 0;
            con.Open();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=TFP.sqlite;Version=3"))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT count(tYetkiID) from Kullanicilar where tYetkiID=1", connection))
                {
                    yetki = (long)command.ExecuteScalar();
                }
                connection.Close();
            }
            SQLiteDataAdapter da = new SQLiteDataAdapter("select kullaniciAd from Kullanicilar", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dataGridView1.DataSource = dt;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                kullanicilar.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            //Eğer ilk defa kayıt olunuyorsa otomatik olarak ebeveyn türü seçimi
            if (yetki<1)
            {
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = false;
            }
            
        }
    }
}
