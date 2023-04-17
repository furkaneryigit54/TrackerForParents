using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TrackerForParents
{
    public partial class ebeveynKontrol : Form
    {
        public ebeveynKontrol()
        {
            InitializeComponent();
        }

        private void ebeveynKontrol_Load(object sender, EventArgs e)
        {
            //Güncel tarih ve saat için başlatılan timer
            timer1.Start();
            timerButonRenkler.Start();
            btnGirisEkrani.PerformClick();
        }

        public int KullaniciID = 0;
        public void Acilis(string ad)
        {
            //Form açıldığında giriş yapan kişinin isminin yazdığı label'ı görünür hale getirme
            label1.Visible = true;
            label1.Text = ad;
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Her saniye güncel tarih ve saati labellara yazma
            label2.Text ="Tarih: " + DateTime.Now.ToString("dd MMMM yyyy");
            label3.Text ="Saat: " + DateTime.Now.ToString("HH:mm");
        }

        private void btnGirilenSiteler_Click(object sender, EventArgs e)
        {
            seciliEkran = 4;
            lblBaslik.Text = "Girilen Siteler";
            this.pnlFormLoader.Controls.Clear();
            Gecmis frmGecmis = new Gecmis() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmGecmis.FormBorderStyle = FormBorderStyle.None;
            frmGecmis.ekleyen = KullaniciID;
            this.pnlFormLoader.Controls.Add(frmGecmis);
            frmGecmis.Show();
        }

        private void btnKullaniciDuzenle_Click(object sender, EventArgs e)
        {
            seciliEkran = 3;
            lblBaslik.Text = "Kullanıcı Düzenle";
            this.pnlFormLoader.Controls.Clear();
            kullaniciDuzenle frmKullaniciDuzenle = new kullaniciDuzenle() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmKullaniciDuzenle.FormBorderStyle = FormBorderStyle.None;
            frmKullaniciDuzenle.ekleyen = KullaniciID;
            this.pnlFormLoader.Controls.Add(frmKullaniciDuzenle);
            frmKullaniciDuzenle.Show();
        }

        private void btnKullaniciEkle_Click_1(object sender, EventArgs e)
        {
            seciliEkran = 2;
            lblBaslik.Text = "Kullanıcı Ekleme";
            this.pnlFormLoader.Controls.Clear();
            KayitEkrani frmKayitEkrani = new KayitEkrani() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmKayitEkrani.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmKayitEkrani);
            frmKayitEkrani.kullaniciID = KullaniciID;
            frmKayitEkrani.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGirisEkrani_Click(object sender, EventArgs e)
        {
            seciliEkran = 1;
            lblBaslik.Text = "Giriş Ekranı";
            this.pnlFormLoader.Controls.Clear();
            anaGiris frmAnaGiris = new anaGiris() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmAnaGiris.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmAnaGiris);
            frmAnaGiris.ekleyen = KullaniciID;
            frmAnaGiris.Show();
        }

        private int seciliEkran = 0;

        private void btnGirisEkrani_MouseEnter(object sender, EventArgs e)
        {
            if (seciliEkran!=1)
            {
                btnGirisEkrani.ForeColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void btnKullaniciEkle_MouseEnter(object sender, EventArgs e)
        {
            if (seciliEkran!=2)
            {
                btnKullaniciEkle.ForeColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void btnKullaniciDuzenle_MouseEnter(object sender, EventArgs e)
        {
            if (seciliEkran!=3)
            {
                btnKullaniciDuzenle.ForeColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void btnGirilenSiteler_MouseEnter(object sender, EventArgs e)
        {
            if (seciliEkran!=4)     
            {
                btnGirilenSiteler.ForeColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void btnGirisEkrani_MouseLeave(object sender, EventArgs e)
        {
            btnGirisEkrani.ForeColor = Color.White;
        }

        private void btnKullaniciEkle_MouseLeave(object sender, EventArgs e)
        {
            btnKullaniciEkle.ForeColor = Color.White;
        }

        private void btnKullaniciDuzenle_MouseLeave(object sender, EventArgs e)
        {
            btnKullaniciDuzenle.ForeColor = Color.White;
        }

        private void btnGirilenSiteler_MouseLeave(object sender, EventArgs e)
        {
            btnGirilenSiteler.ForeColor = Color.White;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private bool mouseDown = false;
        private Point offset;
        private void panel7_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void panel7_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void panel7_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void timerButonRenkler_Tick(object sender, EventArgs e)
        {
            if (seciliEkran==1)
            {
                btnGirisEkrani.BackColor = Color.FromArgb(67, 210, 178);
                btnGirisEkrani.ForeColor = Color.White;
                btnGirisEkrani.Image = TrackerForParents.Properties.Resources.homeWhite;
            }
            else
            {
                btnGirisEkrani.BackColor = Color.FromArgb(24, 30, 45);
                btnGirisEkrani.Image = TrackerForParents.Properties.Resources.homeicon1;
            }

            if (seciliEkran==2)
            {
                btnKullaniciEkle.BackColor= Color.FromArgb(67, 210, 178);
                btnKullaniciEkle.ForeColor = Color.White;
                btnKullaniciEkle.Image = TrackerForParents.Properties.Resources.addiconwhite;
            }
            else
            {
                btnKullaniciEkle.BackColor = Color.FromArgb(24, 30, 45);
                btnKullaniciEkle.Image = TrackerForParents.Properties.Resources.addicon;
            }

            if (seciliEkran==3)
            {
                btnKullaniciDuzenle.BackColor = Color.FromArgb(67, 210, 178);
                btnKullaniciDuzenle.ForeColor = Color.White;
                btnKullaniciDuzenle.Image= TrackerForParents.Properties.Resources.editiconwhite;
            }
            else
            {
                btnKullaniciDuzenle.BackColor = Color.FromArgb(24, 30, 45);
                btnKullaniciDuzenle.Image = TrackerForParents.Properties.Resources.editicon;
            }

            if (seciliEkran==4)
            {
                btnGirilenSiteler.BackColor = Color.FromArgb(67, 210, 178);
                btnGirilenSiteler.ForeColor = Color.White;
                btnGirilenSiteler.Image= TrackerForParents.Properties.Resources.historywhite1;
            }
            else
            {
                btnGirilenSiteler.BackColor = Color.FromArgb(24, 30, 45);
                btnGirilenSiteler.Image = TrackerForParents.Properties.Resources.historyicon;
            }

            if (seciliEkran==5)
            {
                button4.BackColor = Color.FromArgb(67, 210, 178);
                button4.ForeColor = Color.White;
                button4.Image = TrackerForParents.Properties.Resources.settingswhite;
            }
            else
            {
                button4.BackColor = Color.FromArgb(24, 30, 45);
                button4.Image = TrackerForParents.Properties.Resources.settings;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            seciliEkran = 5;
            lblBaslik.Text = "Ayarlar";
            this.pnlFormLoader.Controls.Clear();
            Ayarlar frmAyarlar = new Ayarlar() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmAyarlar.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmAyarlar);
            frmAyarlar.kullaniciID = KullaniciID;
            frmAyarlar.Show();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            if (seciliEkran != 5)
            {
                button4.ForeColor = Color.FromArgb(67, 205, 175);
            }
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.ForeColor = Color.White;
        }

       

        private void ebeveynKontrol_Resize(object sender, EventArgs e)
        {
            this.MaximizedBounds = Screen.GetWorkingArea(this);
        }
    }
}
