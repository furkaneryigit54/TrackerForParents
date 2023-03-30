using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            label2.Text ="Tarih: " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
            label3.Text ="Saat: " + DateTime.Now.ToString("HH:mm");
        }

        private void btnKullaniciEkle_Click(object sender, EventArgs e)
        {
            //Kullanıcı ekleme ekranını açma
            KayitEkrani frmkGirisEkrani = new KayitEkrani();
            frmkGirisEkrani.kullaniciID = KullaniciID;
            frmkGirisEkrani.Show();
        }

        private void btnGirilenSiteler_Click(object sender, EventArgs e)
        {
            //Girilen siteler ekranını açma
            Gecmis frm = new Gecmis();
            frm.ekleyen = KullaniciID;
            frm.ShowDialog();
        }

        private void btnKullaniciDuzenle_Click(object sender, EventArgs e)
        {
            //Kullanıcı düzenleme ekranını açma
            kullaniciDuzenle frm = new kullaniciDuzenle();
            frm.ekleyen = KullaniciID;
            frm.ShowDialog();
        }
    }
}
