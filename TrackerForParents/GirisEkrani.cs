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
    public partial class GirisEkrani : Form
    {
        public GirisEkrani()
        {
            InitializeComponent();
        }

        List<int> kullaniciID = new List<int>();
        List<string> kullanicilar = new List<string>(); 
        List<string> sifreler = new List<string>();
        List<string> tur = new List<string>();
         List<string> siteler = new List<string>();
          List<string> mailler = new List<string>();
          List<int> addedby = new List<int>();
        private void GirisEkrani_Load(object sender, EventArgs e)
        {
            //Yerel dosyalar içerisinde veriseti yoksa verisetini oluşturan konsol uygulaması çalıştırılır.
            if (!File.Exists("TFP.sqlite"))
            {
                DBCreator db = new DBCreator();
                db.Calistir();
                MessageBox.Show(
                    "Program ilk defa çalıştığı için bir ebeveyn eklenmeli aksi taktirde program düzgün çalışmayacaktır!\nEbeveyn ekledikten sonra programı tekrar çalıştırınız!\nİlk eklenen hesap ana yönetici hesabıdır.",
                    "UYARI");
                KayitEkrani frmkKayitEkrani = new KayitEkrani();
                this.Hide();
                frmkKayitEkrani.ShowDialog();
                this.Close();
            }
            //Giriş bilgilerinin kontrolü ve diğer formlara geçiş için kullanıcı bilgilerini datagridview'a aktarma
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            con.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from Kullanicilar", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                kullaniciID.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                kullanicilar.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                sifreler.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                tur.Add(dataGridView1.Rows[i].Cells[3].Value.ToString());
                siteler.Add(dataGridView1.Rows[i].Cells[6].Value.ToString());
                mailler.Add(dataGridView1.Rows[i].Cells[5].Value.ToString());
                addedby.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value.ToString()));
            }
            con.Close();
        }

        

        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            //Giriş yap butonuna tıklandığında bilgiler uyuşursa ebeveyn ise ebeveyn, çocuk ise çocuk formunu açma
            int giris = 0;
            if (txtKullaniciAdi.Text!=""&txtSifre.Text!="")
            {
                for (int i = 0; i < kullanicilar.Count; i++)
                {
                    if (kullanicilar[i] == txtKullaniciAdi.Text.Trim() & sifreler[i]==txtSifre.Text)
                    {
                        if (tur[i]=="1")
                        {
                            ebeveynKontrol frmEbeveynKontrol = new ebeveynKontrol();
                            this.Hide();
                            frmEbeveynKontrol.Acilis(kullanicilar[i]);
                            frmEbeveynKontrol.KullaniciID = kullaniciID[i];
                            frmEbeveynKontrol.ShowDialog();
                            this.Close();
                        }else if (tur[i]=="2")
                        {
                            string mailadress = "";
                            for (int j = 0; j < kullaniciID.Count; j++)
                            {
                                if (addedby[i] == kullaniciID[j])
                                {
                                    mailadress = mailler[j];
                                }
                            }
                            Form1 frm = new Form1();
                            frm.Bilgiler(kullanicilar[i], kullaniciID[i], siteler[i], mailler[0], mailadress);
                            this.Hide();
                            frm.ShowDialog();
                        }
                        giris++;
                        goto don;
                    }
                    
                }
                don:;
                if (giris == 0)
                {
                    MessageBox.Show("Kullanıcı Bulunamadı!","UYARI");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bütün alanları doldurun!", "UYARI");
            }
        }
    }
}
