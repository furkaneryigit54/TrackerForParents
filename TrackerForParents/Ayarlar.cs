using ScottPlot.Drawing.Colormaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerForParents
{
    public partial class Ayarlar : Form
    {
        public Ayarlar()
        {
            InitializeComponent();
        }
        public int kullaniciID=1;
        private string kullaniciAd = "";
        private string sifre;
        private string mail;
        private int wantsmail = 0;
        private void Ayarlar_Load(object sender, EventArgs e)
        {
            button1.PerformClick();
            timer1.Start();
            bilgileriGetir();
        }

        private string eskisifre;
        public void bilgileriGetir()
        {
           SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from Kullanicilar where id=" + kullaniciID + "", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            kullaniciAd = dataGridView1.Rows[0].Cells[1].Value.ToString();
            eskisifre = dataGridView1.Rows[0].Cells[2].Value.ToString();
            mail = dataGridView1.Rows[0].Cells[5].Value.ToString();
            wantsmail = Convert.ToInt32(dataGridView1.Rows[0].Cells[7].Value.ToString());
            con.Close();
            txtKullaniciAdi.Text = kullaniciAd;
            txtmail.Text = mail;
            if (wantsmail==1)
            {
                cmbWantsMail.SelectedIndex = 0;
            }
            else
            {
                cmbWantsMail.SelectedIndex = 1;
            }

            txtEskiSifre.Text = "";
            txtYeniSifreOnay.Text = "";
            txtYeniSifre.Text = "";

        }
       
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            if (txtEskiSifre.Text==eskisifre)
            {
                if (txtYeniSifre.Text==txtYeniSifreOnay.Text&txtYeniSifre.Text!=""&txtYeniSifreOnay.Text!="")
                {
                    string yeniSifre=txtYeniSifre.Text;
                    SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                    SQLiteCommand cmd = new SQLiteCommand("update Kullanicilar set kullaniciSifre=$sifre where id=$id", con);
                    cmd.Parameters.AddWithValue("$sifre", yeniSifre);
                    cmd.Parameters.AddWithValue("$id", kullaniciID);
                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Bilgiler Başarıyla Güncellendi!");
                        bilgileriGetir();
                        button3.Focus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bir Hata Oluştu\n" + ex, "HATA");
                    }
                }
                else
                {
                    MessageBox.Show("Yeni Şifreler Uyuşmuyor!", "HATA");
                }
            }
            else
            {
                MessageBox.Show("Eski Şifreniz Uyuşmuyor!", "HATA");
            }



            
        }
        private int[] sayilar = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string onayKodu = "";
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox5.Text==onayKodu)
            {
               
                SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                SQLiteCommand cmd = new SQLiteCommand("update Kullanicilar set mail=$mail where id=$id", con);
                cmd.Parameters.AddWithValue("$mail", txtmail.Text.Trim());
                cmd.Parameters.AddWithValue("$id", kullaniciID);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bilgiler Başarıyla Güncellendi!");
                    pnlmailOnay.Visible = false;
                    pnlmailOnay.Enabled = false;
                    pnlGuvenlik.Visible = true;
                    pnlGuvenlik.Enabled = true;
                    bilgileriGetir();
                    button3.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir Hata Oluştu\n" + ex, "HATA");
                }
                con.Close();
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(84, 86, 95);

        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            panel6.BackColor = Color.FromArgb(68, 215, 182);

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (seciliEkran == 1)
            {
                button1.BackColor = Color.FromArgb(67, 210, 178);
                button1.ForeColor = Color.White;
            }
            else
            {
                button1.BackColor = Color.FromArgb(24, 30, 45);
                
            }
            if (seciliEkran==2)
            {
                button3.BackColor = Color.FromArgb(67, 210, 178);
                button3.ForeColor = Color.White;
            }
            else
            {
                button3.BackColor = Color.FromArgb(24, 30, 45);
                
            }

            if (seciliEkran == 3)
            {
                button4.BackColor = Color.FromArgb(67, 210, 178);
                button4.ForeColor = Color.White;
            }
            else
            {
                button4.BackColor = Color.FromArgb(24, 30, 45);
                
            }
        }


        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor= Color.FromArgb(67, 210, 178);
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor=Color.White;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.ForeColor= Color.FromArgb(67, 210, 178);
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.ForeColor=Color.White;
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.ForeColor= Color.FromArgb(67, 210, 178);
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.ForeColor=Color.White;
        }


        private int seciliEkran = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            seciliEkran = 1;
            pnlTemelBilgiler.Enabled = true;
            pnlTemelBilgiler.Visible = true;
            pnlBildirimler.Enabled = false;
            pnlBildirimler.Visible=false;
            pnlGuvenlik.Enabled = false;
            pnlGuvenlik.Visible=false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            seciliEkran = 2;
            pnlTemelBilgiler.Enabled = false;
            pnlTemelBilgiler.Visible = false;
            pnlBildirimler.Enabled = false;
            pnlBildirimler.Visible = false;
            pnlGuvenlik.Enabled = true;
            pnlGuvenlik.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            seciliEkran = 3;
            pnlTemelBilgiler.Enabled = false;
            pnlTemelBilgiler.Visible = false;
            pnlBildirimler.Enabled = true;
            pnlBildirimler.Visible = true;
            pnlGuvenlik.Enabled = false;
            pnlGuvenlik.Visible = false;
        }

        private void pnlTemelBilgiler_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtKullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text!="")
            {
                panel12.BackColor= Color.FromArgb(67, 210, 178);
            }
            else
            {
                panel12.BackColor=Color.FromArgb(84, 86, 95);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (cmbWantsMail.SelectedIndex==0)
            {
                wantsmail = 1;
            }
            else
            {
                wantsmail = 0;
            }
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            SQLiteCommand cmd = new SQLiteCommand("update Kullanicilar set wantsmail=$wantsmail where id=$id", con);
            cmd.Parameters.AddWithValue("$wantsmail", wantsmail);
            cmd.Parameters.AddWithValue("$id", kullaniciID);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bilgiler Başarıyla Güncellendi!");
                bilgileriGetir();
                button4.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Oluştu\n" + ex, "HATA");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text!="")
            {
                kullaniciAd = txtKullaniciAdi.Text.Trim();
                SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                SQLiteCommand cmd = new SQLiteCommand("update Kullanicilar set kullaniciAd=$ad where id=$id", con);
                cmd.Parameters.AddWithValue("$ad", kullaniciAd);
                cmd.Parameters.AddWithValue("$id", kullaniciID);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bilgiler Başarıyla Güncellendi!");
                    bilgileriGetir();
                    button1.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir Hata Oluştu\n" + ex, "HATA");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (txtmail.Text!="")
            {
                Random rnd = new Random();
                onayKodu = "";
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
                    string konustr = "Tracker For Parents Bilgilerinin Güncellenmesi";
                    string icerik = "Bilgilerinizi Güncellemek İçin Onay Kodunuz: " + onayKodu + "";
                    smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "SİFRE");
                    MailMessage mailonay = new MailMessage();
                    mailonay.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
                    mailonay.To.Add(txtmail.Text.Trim());
                    mailonay.Subject = konustr;
                    mailonay.IsBodyHtml = true;
                    mailonay.Body = icerik;
                    smtp.Send(mailonay);
                    pnlGuvenlik.Visible = false;
                    pnlGuvenlik.Enabled = false;
                    pnlmailOnay.Enabled = true;
                    pnlmailOnay.Visible = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Bir Hata Oluştu\n" + exception, "HATA");
                }
            }
        }

        private void txtEskiSifre_TextChanged(object sender, EventArgs e)
        {
            if (txtEskiSifre.Text != "")
            {
                panel2.BackColor = Color.FromArgb(67, 210, 178);
            }
            else
            {
                panel2.BackColor = Color.FromArgb(84, 86, 95);
            }
        }

        private void txtYeniSifre_TextChanged(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text != "")
            {
                panel3.BackColor = Color.FromArgb(67, 210, 178);
            }
            else
            {
                panel3.BackColor = Color.FromArgb(84, 86, 95);
            }
        }

        private void txtYeniSifreOnay_TextChanged(object sender, EventArgs e)
        {
            if (txtYeniSifre.Text == txtYeniSifreOnay.Text)
            {
                panel10.BackColor = Color.FromArgb(67, 210, 178);
            }
            else
            {
                panel10.BackColor = Color.FromArgb(84, 86, 95);
            }
        }

        private void txtmail_TextChanged(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text != "")
            {
                panel4.BackColor = Color.FromArgb(67, 210, 178);
            }
            else
            {
                panel4.BackColor = Color.FromArgb(84, 86, 95);
            }
        }
    }
}
