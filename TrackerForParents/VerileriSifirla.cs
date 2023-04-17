using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerForParents
{
    public partial class VerileriSifirla : Form
    {
        public VerileriSifirla()
        {
            InitializeComponent();
        }

  
        private void button2_Click(object sender, EventArgs e)
        {
            
        }
        private bool mouseDown = false;
        private Point offset;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void txtKullaniciAdi_Enter(object sender, EventArgs e)
        {
            panel3.BackColor= Color.FromArgb(68, 215, 182);
        }

        private void txtKullaniciAdi_Leave(object sender, EventArgs e)
        {
            panel3.BackColor= Color.FromArgb(84, 86, 95);
        }


        private List<string> kullanicilarID = new List<string>();
        List<string> kullanicilar = new List<string>();
        List<string> sifreler = new List<string>();
        List<string> mailler = new List<string>();
        private void VerileriSifirla_Load(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from Kullanicilar", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                kullanicilarID.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                kullanicilar.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                sifreler.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                mailler.Add(dataGridView1.Rows[i].Cells[5].Value.ToString());
             
            }
            con.Close();
        }
        private int[] sayilar = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text!="")
            {
                if (mailler.Contains(txtEmail.Text.Trim()))
                {
                    int kullanciAdiInd=0;
                    for (int i = 0; i < mailler.Count; i++)
                    {
                        if (mailler[i]==txtEmail.Text.Trim())
                        {
                            kullanciAdiInd = i;
                        }
                    }
                    try
                    {
                        Random rnd = new Random();

                        string yeniSifre="";
                        for (int i = 0; i < 6; i++)
                        {
                            yeniSifre += sayilar[rnd.Next(0, 10)].ToString();
                        }
                        SmtpClient smtp = new SmtpClient();
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        string konustr = "Tracker For Parents Giriş Bilgileri!";
                        string icerik = "Uygulamayı Kullanmak İçin Giriş Bilgileriniz\nKullanıcı Adı:" + kullanicilar[kullanciAdiInd] +"\nŞifre:"+yeniSifre+" ";
                        smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "SİFRE");
                        MailMessage mailonay = new MailMessage();
                        mailonay.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
                        mailonay.To.Add(txtEmail.Text.Trim());
                        mailonay.Subject = konustr;
                        mailonay.IsBodyHtml = true;
                        mailonay.Body = icerik;
                        smtp.Send(mailonay);
                        SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                        SQLiteCommand cmd = new SQLiteCommand("update kullanicilar set kullaniciSifre=$yenisifre where id=$id", con);
                        cmd.Parameters.AddWithValue("$yenisifre", yeniSifre);
                        cmd.Parameters.AddWithValue("$id", kullanicilarID[kullanciAdiInd]);
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
                    catch (Exception exception)
                    {
                        MessageBox.Show("Bir Hata Oluştu\n" + exception, "HATA");
                    }

                    txtEmail.Visible = false;
                    panel3.Visible = false;
                    btnGirisYap.Visible = false;
                    linkLabel1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = true;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Eğer hiçbir giriş bilginizi hatırlamıyorsanız uygulamayı sıfırlayabilirsiniz.\nUygulamayı sıfırlamak istediğinize emin misiniz?", "UYARI", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists(@"C:\TFPDB\TFP.sqlite"))
                {

                    SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
                    SQLiteCommand cmd = new SQLiteCommand("delete from kullanicilar", con);
                    SQLiteCommand cmd2 = new SQLiteCommand("delete from history", con);
                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        cmd2.ExecuteNonQuery();
                        GirisEkrani frmgiGirisEkrani = new GirisEkrani();
                        this.Hide();
                        frmgiGirisEkrani.ShowDialog();
                        this.Close();
                    }
                    catch (Exception)
                    {

                    }
                    con.Close();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GirisEkrani frmGirisEkrani = new GirisEkrani();
            this.Hide();
            frmGirisEkrani.ShowDialog();
            this.Close();
        }
    }
}
