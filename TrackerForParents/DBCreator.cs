using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace TrackerForParents
{
    internal class DBCreator
    {
        public void Calistir()
        {
            veriSetiOlustur();
        }
        static void veriSetiOlustur()
        {
                //veritabanındaki tabloları oluşturacak sorgular
                string[] sorgular =
                {
                    "CREATE TABLE \"History\" (\r\n\t\"ID\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"TarayiciID\"\tINTEGER,\r\n\t\"Site\"\tTEXT,\r\n\t\"Tarih\"\tTEXT,\r\n\t\"Sure\"\tINTEGER,\r\n\t\"KullaniciID\"\tINTEGER,\r\n\tPRIMARY KEY(\"ID\" AUTOINCREMENT)\r\n);",
                    "CREATE TABLE \"Kullanicilar\" (\r\n\t\"ID\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"kullaniciAd\"\tTEXT NOT NULL,\r\n\t\"kullaniciSifre\"\tTEXT NOT NULL,\r\n\t\"tYetkiID\"\tINTEGER,\r\n\t\"addedBy\"\tINTEGER,\r\n\t\"mail\"\tTEXT,\r\n\t\"siteler\"\tTEXT,\r\n\t\"wantsmail\"\tINTEGER,\r\n\tPRIMARY KEY(\"ID\" AUTOINCREMENT)\r\n);"
                    ,"CREATE TABLE \"Yetkiler\" (\r\n\t\"ID\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"YetkiAd\"\tTEXT,\r\n\tPRIMARY KEY(\"ID\" AUTOINCREMENT)\r\n);","CREATE TABLE \"Tarayicilar\" (\r\n\t\"ID\"\tINTEGER NOT NULL UNIQUE,\r\n\t\"TarayiciAd\"\tTEXT,\r\n\tPRIMARY KEY(\"ID\" AUTOINCREMENT)\r\n);"
                    ,"INSERT INTO Tarayicilar VALUES (1,\"Chrome\"),(2,\"Opera\"),(3,\"Firefox\"),(4,\"Microsoft Edge\")","INSERT INTO Yetkiler VALUES (1,\"Admin\"),(2,\"Kullanıcı\")"
                };

                query(sorgular);
        }

        static void query(string[] sorgular)
        {
            //sorgu metinlerini işleme metotu
            string dbdosya = "Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3";
            SQLiteConnection con = new SQLiteConnection(dbdosya);
            con.Open();
            foreach (string sorgu in sorgular)
            {
                SQLiteCommand cmd = new SQLiteCommand(sorgu, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }
}

