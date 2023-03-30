using System.Data;
using TrackerForParents.Browsers;
using System.Data.SQLite;
using System.Drawing.Imaging;
using System.Net;
using System.Security.AccessControl;
using Microsoft.VisualBasic.CompilerServices;
using System.Windows.Forms;
using System.Net.Mail;


namespace TrackerForParents;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    public void Form1_Load(object sender, EventArgs e)
    {
        //Çocuk giriþi yapýldýðýnda timer'ý baþlatarak her saniye girilen siteleri çekip veritabanýna ekleme
        timer1.Start();

        //Uygulamaya giriþ yapýldýðýnda admin ve çocuðu ekleyen ebeveyn hesabýna bilgilendirme maili gönderme
        SmtpClient smtp = new SmtpClient();
        smtp.Port = 587;
        smtp.Host = "smtp.gmail.com";
        smtp.EnableSsl = true;
        string konustr = "Tracker For Parents Bilgilendirme";
        string icerik = "Çocuðunuz " + kullaniciad + "; uygulamaya giriþ yapmýþtýr.";
        smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "12345");
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
        mail.To.Add(adminMail);
        mail.Subject = konustr;
        mail.IsBodyHtml = true;
        mail.Body = icerik;
        smtp.Send(mail);
        MailMessage mail2 = new MailMessage();
        mail2.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
        mail2.To.Add(ekleyenMail);
        mail2.Subject = konustr;
        mail2.IsBodyHtml = true;
        mail2.Body = icerik;
        smtp.Send(mail2);

    }
    
    int id=0;
    private string sitelerlist = "";
    private string adminMail = "";
    private string ekleyenMail = "";
    private string kullaniciad = "";
    public void Bilgiler(string isim, int idkullanici,string siteler,string mail1,string mail2)
    {
        //Maile eklenecek deðiþkenlerin giriþ ekranýndan gelen bilgilerini deðiþkenlere atama
        label1.Text = isim + " Ýçin Tarayýcý Geçmiþi Kaydediliyor";
        kullaniciad = isim;
        id = idkullanici;
        sitelerlist = siteler;
        adminMail = mail1;
        ekleyenMail = mail2;
    }
   
    public void timer1_Tick(object sender, EventArgs e)
    {
        //Her timer tickinde 4 farklý tarayýcý için girilen siteleri çeken ve veri tabanýna ekleyen metotlarý çaðýrma
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        con.Open();
        //Her bir tarayýcý için ziyaret edilen siteleri veri setine ekleyen metotlar
        ChromeDBAdd();
        OperaDBAdd();
        FirefoxDBAdd();
        EdgeDBAdd();
        con.Close();
    }

    
     
    public void EdgeDBAdd()
    {
        //Belirlenen tarayýcý için tarayýcý sekme ismini çekme ve eðer veritabanýnda yoksa ekleme varsa sitede geçirilen süreyi güncelleme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        con.Open();
        string sonurl;
        string sontarih;
        using (SQLiteCommand command = new SQLiteCommand("select site from history WHERE id=(SELECT MAX(id) from history where tarayiciID=4)", con))
        {
            sonurl = (string)command.ExecuteScalar();
        }
        using (SQLiteCommand command = new SQLiteCommand("select tarih from history WHERE id=(SELECT MAX(id) from history where tarayiciID=4)", con))
        {
            sontarih = (string)command.ExecuteScalar();
        }
        string eklencekurl;
        MicrosoftEdge edge = new MicrosoftEdge();
        eklencekurl = edge.MsEdgeUrl();
        TimeSpan sureFark = DateTime.Now.Subtract(Convert.ToDateTime(sontarih));
        if (eklencekurl == sonurl)
        {
            string guncelSure = Convert.ToString(Convert.ToInt32(sureFark.TotalMinutes));
            long id;
            using (SQLiteCommand command = new SQLiteCommand("SELECT MAX(id) from history where tarayiciID=4", con))
            {
                id = (long)command.ExecuteScalar();
            }
            DBGuncelle(guncelSure + " dk", id.ToString());
        }
        if (eklencekurl != sonurl & eklencekurl != "")
        {
            string tarih = DateTime.Now.ToString();
            DBEkle(4, eklencekurl, tarih, Convert.ToString(0) + " dk");
        }
        con.Close();
    }

    private void FirefoxDBAdd()
    {
        //Belirlenen tarayýcý için tarayýcý sekme ismini çekme ve eðer veritabanýnda yoksa ekleme varsa sitede geçirilen süreyi güncelleme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        con.Open();
        string sonurl;
        string sontarih;
        using (SQLiteCommand command = new SQLiteCommand("select site from history WHERE id=(SELECT MAX(id) from history where tarayiciID=3)", con))
        {
            sonurl = (string)command.ExecuteScalar();
        }
        using (SQLiteCommand command = new SQLiteCommand("select tarih from history WHERE id=(SELECT MAX(id) from history where tarayiciID=3)", con))
        {
            sontarih = (string)command.ExecuteScalar();
        }
        string eklencekurl;
        Firefox firefox = new Firefox();
        eklencekurl = firefox.FirefoxUrl();
        TimeSpan sureFark = DateTime.Now.Subtract(Convert.ToDateTime(sontarih));
        if (eklencekurl == sonurl)
        {
            string guncelSure = Convert.ToString(Convert.ToInt32(sureFark.TotalMinutes));
            long id;
            using (SQLiteCommand command = new SQLiteCommand("SELECT MAX(id) from history where tarayiciID=3", con))
            {
                id = (long)command.ExecuteScalar();
            }
            DBGuncelle(guncelSure + " dk", id.ToString());
        }
        if (eklencekurl != sonurl & eklencekurl != "")
        {
            string tarih = DateTime.Now.ToString();
            DBEkle(3, eklencekurl, tarih, Convert.ToString(0) + " dk");
        }
        con.Close();
    }

    public void OperaDBAdd()
    {
        //Belirlenen tarayýcý için tarayýcý sekme ismini çekme ve eðer veritabanýnda yoksa ekleme varsa sitede geçirilen süreyi güncelleme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        con.Open();
        string sonurl;
        string sontarih;
        using (SQLiteCommand command = new SQLiteCommand("select site from history WHERE id=(SELECT MAX(id) from history where tarayiciID=2)", con))
        {
            sonurl = (string)command.ExecuteScalar();
        }
        using (SQLiteCommand command = new SQLiteCommand("select tarih from history WHERE id=(SELECT MAX(id) from history where tarayiciID=2)", con))
        {
            sontarih = (string)command.ExecuteScalar();
        }
        string eklencekurl;
        Opera opera = new Opera();
        eklencekurl = opera.OperaUrl();
        TimeSpan sureFark = DateTime.Now.Subtract(Convert.ToDateTime(sontarih));
        if (eklencekurl == sonurl)
        {
            string guncelSure = Convert.ToString(Convert.ToInt32(sureFark.TotalMinutes));
            long id;
            using (SQLiteCommand command = new SQLiteCommand("SELECT MAX(id) from history where tarayiciID=2", con))
            {
                id = (long)command.ExecuteScalar();
            }
            DBGuncelle(guncelSure + " dk", id.ToString());
        }
        if (eklencekurl != sonurl & eklencekurl != "")
        {
            string tarih = DateTime.Now.ToString();
            DBEkle(2, eklencekurl, tarih, Convert.ToString(0) + " dk");
        }
        con.Close();
    }

    public void ChromeDBAdd()
    {
        //Belirlenen tarayýcý için tarayýcý sekme ismini çekme ve eðer veritabanýnda yoksa ekleme varsa sitede geçirilen süreyi güncelleme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        con.Open();
        string sonurl;
        string sontarih;
        using (SQLiteCommand command = new SQLiteCommand("select site from history WHERE id=(SELECT MAX(id) from history where tarayiciID=1)", con))
        {
            sonurl = (string)command.ExecuteScalar();
        }
        using (SQLiteCommand command = new SQLiteCommand("select tarih from history WHERE id=(SELECT MAX(id) from history where tarayiciID=1)", con))
        {
            sontarih = (string)command.ExecuteScalar();
        }
        Chrome chrome = new Chrome();
        string eklencekurl = (chrome.ChromeUrl());
        TimeSpan sureFark = DateTime.Now.Subtract(Convert.ToDateTime(sontarih));
        if (eklencekurl==sonurl)
        {
           string guncelSure = Convert.ToString(Convert.ToInt32(sureFark.TotalMinutes));
           long id;
           using (SQLiteCommand command = new SQLiteCommand("SELECT MAX(id) from history where tarayiciID=1", con))
           {
               id = (long)command.ExecuteScalar();
           }
           DBGuncelle(guncelSure+" dk",id.ToString());
        }
        if (eklencekurl != sonurl & eklencekurl != "")
        {
            string tarih = DateTime.Now.ToString();
            DBEkle(1, eklencekurl, tarih, Convert.ToString(0)+" dk");
        }
        con.Close();
    }


    private void DBGuncelle(string sure,string id)
    {
        //Tarayýcý metotlarýndan gelen süre güncelleme sorgularýný gerçekleþtirme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        SQLiteCommand cmd = new SQLiteCommand("update history set sure=$sure where id=(SELECT MAX(id) from history where id=$id)", con);
        cmd.Parameters.AddWithValue("$sure", sure);
        cmd.Parameters.AddWithValue("$id", id);
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
    private void DBEkle(int tarayici,string eklencekurl,string tarih, string sure)
    {
        //Tarayýcý metotlarýndan gelen veriyi veritabanýna ekleme sorgularýný gerçekleþtirme
        SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
        SQLiteCommand cmd = new SQLiteCommand("INSERT INTO history (TarayiciID,Site,Tarih,Sure,KullaniciID) VALUES ($tarayici,$eklenecekurl,$tarih,$sure,$kullaniciID)", con);
        cmd.Parameters.AddWithValue("$tarayici", tarayici);
        cmd.Parameters.AddWithValue("$eklenecekurl", eklencekurl);
        cmd.Parameters.AddWithValue("$tarih", tarih);
        cmd.Parameters.AddWithValue("$sure", sure);
        cmd.Parameters.AddWithValue("$kullaniciID", id);
        con.Open();
        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception )
        {

        }
        con.Close();
        //Eðer eklenecek site girilince bilgi gönderilmesi istenen siteyse admin ve çocuðu ekleyen ebeveyn hesabýna mail gönderme
        string[] siteler = sitelerlist.Split("-");
        siteler = siteler.Where(c => c != "").ToArray();
        for (int i = 0; i < siteler.Length; i++)
        {
            if (eklencekurl.ToLower().Contains(siteler[i]))
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                string konustr = "Tracker For Parents Bilgilendirme";
                string icerik = "Çocuðunuz "+kullaniciad+"; " + eklencekurl + " sitesine giriþ yapmýþtýr.";
                smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "12345");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
                mail.To.Add(adminMail);
                mail.Subject = konustr;
                mail.IsBodyHtml = true;
                mail.Body = icerik;
                smtp.Send(mail);
                MailMessage mail2 = new MailMessage();
                mail2.From= new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
                mail2.To.Add(ekleyenMail);
                mail2.Subject = konustr;
                mail2.IsBodyHtml = true;
                mail2.Body = icerik;
                smtp.Send(mail2);
            }
        }
    }

 

    private void Form1_Resize_1(object sender, EventArgs e)
    {
        //Uygulama simge durumuna küçültüðünde sistem tepsisine küçültme
        if (this.WindowState == FormWindowState.Minimized)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(1000);
        }
    }
    private void notifyIcon1_DoubleClick(object sender, EventArgs e)
    {
        //sistem tepsisindeki ikona çift týklandýðýnda uygulamayý normal pencere boyutuna getirme
        this.Show();
        this.WindowState = FormWindowState.Normal;
        notifyIcon1.Visible = false;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        
    }

    private void button1_Click(object sender, EventArgs e)
    {
        //Uygulamadan çýkýþ yapma ve yaparken admin ve çocuðu ekleyen ebeveyn hesabýna bilgilendirme maili gönderilmesi
        SmtpClient smtp = new SmtpClient();
        smtp.Port = 587;
        smtp.Host = "smtp.gmail.com";
        smtp.EnableSsl = true;
        string konustr = "Tracker For Parents Bilgilendirme";
        string icerik = "Çocuðunuz " + kullaniciad + "; uygulamadan çýkýþ yapmýþtýr.";
        smtp.Credentials = new NetworkCredential("trackerforparentsinfo@gmail.com", "12345");
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
        mail.To.Add(adminMail);
        mail.Subject = konustr;
        mail.IsBodyHtml = true;
        mail.Body = icerik;
        smtp.Send(mail);
        MailMessage mail2 = new MailMessage();
        mail2.From = new MailAddress("trackerforparentsinfo@gmail.com", "Tracker For Parents Bilgilendirme");
        mail2.To.Add(ekleyenMail);
        mail2.Subject = konustr;
        mail2.IsBodyHtml = true;
        mail2.Body = icerik;
        smtp.Send(mail2);
        Application.Exit();
    }
}