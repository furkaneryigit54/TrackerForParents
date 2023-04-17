using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Policy;
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

        public int ilkKayit = 0;
        public List<string> kullanicilar = new List<string>();
        public List<string> mailler = new List<string>();
        private void KayitEkrani_Load(object sender, EventArgs e)
        {
           //Kaydedilecek kişi bilgilerinin tekrar edilmemesi için form yüklendiğinde sistemde kayıtlı olan kullanıcıları datagridview'a aktarma
            comboBox1.SelectedIndex = 0;
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            long yetki = 0;
            con.Open();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3"))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT count(tYetkiID) from Kullanicilar where tYetkiID=1", connection))
                {
                    yetki = (long)command.ExecuteScalar();
                }
                connection.Close();
            }
            SQLiteDataAdapter da = new SQLiteDataAdapter("select kullaniciAd,mail from Kullanicilar", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            dataGridView1.DataSource = dt;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                kullanicilar.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                mailler.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }
            //Eğer ilk defa kayıt olunuyorsa otomatik olarak ebeveyn türü seçimi
            if (yetki<1)
            {
                comboBox1.SelectedIndex = 0;
                comboBox1.Enabled = false;
            }
            
        }

        private int[] sayilar = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string kullaniciAdi = "";
        string sifre = "";
        int kayitTuru = 0;
        string mail = "";
        string onayKodu = "";
        private void button1_Click_1(object sender, EventArgs e)
        {
           
            //Bilgileri girilen kişiyi veritabanına kaydetme
            if (!kullanicilar.Contains(textBox1.Text.Trim()))
            {
                if (!mailler.Contains(textBox3.Text.Trim()))
                {
                    if (textBox1.Text != "" & textBox2.Text != "")
                    {
                        kullaniciAdi = textBox1.Text.Trim();
                        sifre = textBox2.Text;
                        if (textBox3.Text.Contains("@") & textBox3.Text.Contains(".com"))
                        {
                            mail = textBox3.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Lütfen geçerli bir e-posta adresi girin!", "UYARI");
                            goto don;
                        }
                        if (comboBox1.SelectedItem == "Ebeveyn")
                        {
                            kayitTuru = 1;
                        }
                        else if (comboBox1.SelectedItem == "Çocuk")
                        {
                            kayitTuru = 2;
                        }
                        Random rnd = new Random();
                   
                        for (int i = 0; i < 6; i++)
                        {
                            onayKodu += sayilar[rnd.Next(0, 10)].ToString();
                        }

                        try
                        {
                            SmtpClient smtp = new SmtpClient();
                            smtp.Port = 587;
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            string konustr = "Tracker For Parents'a Hoş Geldiniz!";
                            string icerik = "Uygulamayı Kullanmak İçin Onay Kodunuz: "+onayKodu+"";
                            smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "SİFRE");
                            MailMessage mailonay = new MailMessage();
                            mailonay.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
                            mailonay.To.Add(mail);
                            mailonay.Subject = konustr;
                            mailonay.IsBodyHtml = true;
                            mailonay.Body = icerik;
                            smtp.Send(mailonay);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show("Bir Hata Oluştu\n" + exception,"HATA");
                            goto don;
                        }
                        panel4.Visible = false;
                        panel5.Visible = true;
                    
                   
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bütün alanları doldurun!", "UYARI");
                        goto don;
                    }
                }
                else
                {
                    MessageBox.Show("E-Posta Adresi Sistem Zaten Kayıtlı!", "UYARI");
                    goto don;
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı Sistemde Zaten Kayıtlı!","UYARI");
                goto don;
            }

           
           
            don:;
        }



        private void textBox1_Enter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(68, 215, 182);
            if (kullanicilar.Contains(textBox1.Text.Trim()))
            {
                panel1.BackColor = Color.Red;
            }
            else
            {
                panel1.BackColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(84, 86, 95);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(68, 215, 182);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(84, 86, 95);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(68, 215, 182);
            if (mailler.Contains(textBox3.Text.Trim()))
            {
                panel3.BackColor = Color.Red;
            }
            else
            {
                panel3.BackColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(84, 86, 95);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == onayKodu)
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Kullanicilar (kullaniciAd,kullaniciSifre,tYetkiID,addedBy,mail,wantsmail) VALUES ($ad,$soyad,$tur,$ekleyen,$mail,$wantsmail)", con);
                cmd.Parameters.AddWithValue("$ad", kullaniciAdi);
                cmd.Parameters.AddWithValue("$soyad", sifre);
                cmd.Parameters.AddWithValue("$tur", kayitTuru);
                cmd.Parameters.AddWithValue("$ekleyen", kullaniciID);
                cmd.Parameters.AddWithValue("$mail", mail);
                cmd.Parameters.AddWithValue("$wantsmail", 1);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                }
                con.Close();
                MessageBox.Show("Kullanıcı başarıyla kaydedildi!");
                panel5.Visible = false;
                panel4.Visible = true;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                button1.Focus();
                if (ilkKayit==1)
                {
                    GirisEkrani frmGirisEkrani=new GirisEkrani();
                    this.Hide();
                    frmGirisEkrani.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Doğrulama Kodu Uyuşmuyor!");
                panel5.Visible = false;
                panel4.Visible = true;
                textBox5.Text = "";
            }
            
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(67, 205, 175);
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (kullanicilar.Contains(textBox1.Text.Trim()))
            {
                panel1.BackColor=Color.Red;
            }
            else
            {
                panel1.BackColor=Color.FromArgb(67, 205, 175);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (mailler.Contains(textBox3.Text.Trim()))
            {
                panel3.BackColor = Color.Red;
            }
            else
            {
                panel3.BackColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(84, 86, 95);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
