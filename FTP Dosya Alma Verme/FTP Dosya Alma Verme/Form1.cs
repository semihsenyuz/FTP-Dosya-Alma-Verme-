using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace FTP_Dosya_Alma_Verme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /*void DosyaIndir()
        {
            try
            {
                progressBar1.Value += 5;
                //Web istemcisi çıkartıyoruz.
                WebClient istemci = new WebClient();

                //(Gerekiyorsa)FTP giriş bilgilerimizi ayarlıyoruz.
                istemci.Credentials = new NetworkCredential(textEditKullaniciAd.Text, textEditParola.Text);
                progressBar1.Value += 15;
                //İstediğimiz data'yı bir bayt dizisine yüklüyoruz.
                byte[] veriDosya = istemci.DownloadData(textEditDosyaLink.Text);
                progressBar1.Value += 15;
                //Bayt dizisini yazacağımız bir FileStream oluşturuyoruz.
                FileStream dosya = File.Create(textEditKayitYeri.Text);
                progressBar1.Value += 15;
                //Bayt dizinimizdeki tüm veriyi dosyamıza yazdırıyoruz.
                dosya.Write(veriDosya, 0, veriDosya.Length);
                progressBar1.Value += 50;
                //Dosyayı diğer işlemlerin dosyaya ulaşabilmesi için kapatıyoruz.
                dosya.Close();
                MessageBox.Show("İndirme tamamlandı. Dosya " + textEditKayitYeri.Text + " adresine kaydedildi.");
                progressBar1.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                progressBar1.Value = 0;
            }
        }*/

        void DosyaYukle()
        {
            try
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    progressBar2.Value = 0;
                    FileInfo filei = new FileInfo(textEditYuklenecekDosya.Text + "\\" + listBox2.Items[i]);
                    string adres = textEditYuklenecekLink.Text + listBox1.Items[i];
                    string path = adres;
                    progressBar2.Value += 5;
                    FtpWebRequest FTP;
                    FTP = (FtpWebRequest)FtpWebRequest.Create(path);
                    progressBar2.Value += 5;
                    FTP.Credentials = new NetworkCredential(textEditKullaniciAd.Text, textEditParola.Text);
                    progressBar2.Value += 10;
                    FTP.KeepAlive = false;
                    FTP.Method = WebRequestMethods.Ftp.UploadFile;
                    progressBar2.Value += 5;
                    FTP.UseBinary = true;
                    FTP.ContentLength = filei.Length;
                    progressBar2.Value += 10;
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    progressBar2.Value += 5;
                    int contentLen;
                    FileStream FS = filei.OpenRead();
                    progressBar2.Value += 10;
                    try
                    {
                        Stream strm = FTP.GetRequestStream(); contentLen = FS.Read(buff, 0, buffLength); while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen); contentLen = FS.Read(buff, 0, buffLength);
                        }
                        progressBar2.Value += 50;
                        strm.Close();
                        FS.Close();
                    }
                    catch
                    {
                        progressBar2.Value = 0;
                    }
                }

            }
            catch
            {
                progressBar2.Value = 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Properties.Settings.Default.Sunucu = textEditSunucu.Text;
                Properties.Settings.Default.KullaniciAd = textEditKullaniciAd.Text;
                Properties.Settings.Default.Parola = textEditParola.Text;
                Properties.Settings.Default.CheckDurum = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Sunucu = null;
                Properties.Settings.Default.KullaniciAd = null;
                Properties.Settings.Default.Parola = null;
                Properties.Settings.Default.CheckDurum = false;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            klasorOlustur();
            DosyaYukle();
        }
        int index = 0;
        int index2 = 0;

        void klasorOlustur()
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;
            try
            {
                for (int i = 0; i < listBox5.Items.Count; i++)
                {
                    try
                    {
                        reqFTP = (FtpWebRequest)FtpWebRequest.Create(textEditYuklenecekLink.Text + listBox5.Items[i]);
                        progressBar2.Value += 25;
                        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                        reqFTP.UseBinary = true;
                        progressBar2.Value += 25;
                        reqFTP.Credentials = new NetworkCredential(textEditKullaniciAd.Text, textEditParola.Text);
                        FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                        ftpStream = response.GetResponseStream();
                        progressBar2.Value += 25;
                        ftpStream.Close();
                        response.Close();
                        progressBar2.Value += 25;
                    }
                    catch
                    {
                        progressBar2.Value = 0;
                    }
                    progressBar2.Value = 0;
                }
            }
            catch
            {
            }
        }
        void tarihSorgu()
        {
            listBox1.Items.Clear(); // clear kısmı
            listBox2.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();

            string kaynak = textEditYuklenecekDosya.Text;
            var dosyalar = new DirectoryInfo(kaynak).GetFiles("*.*");

            bool test = true;
            foreach (FileInfo dosya in dosyalar)
            {
                if (test)
                {
                    for (int i = 0; i < textEditYuklenecekDosya.Text.Length; i++)
                    {
                        if (dosya.FullName[i].ToString() == "\\")
                        {
                            index = i + 1;
                        }
                    }
                    for (int i = 0; i <= textEditYuklenecekDosya.Text.Length; i++)
                    {
                        if (dosya.FullName[i].ToString() == "\\")
                        {
                            index2 = i + 1;
                        }
                    }
                }
                test = false;
                listBox1.Items.Add(dosya.FullName.Substring(index).Replace("\\", "/"));
                listBox2.Items.Add(dosya.FullName.Substring(index2));
                listBox5.Items.Add(dosya.DirectoryName.Substring(index).Replace("\\", "/"));
                listBox4.Items.Add(dosya.LastAccessTime);
            }
            var klasorler = new DirectoryInfo(kaynak).GetDirectories("*");
            foreach (DirectoryInfo klasor in klasorler)
            {
                var dosyalar2 = new DirectoryInfo(klasor.FullName).GetFiles("*.*");
                foreach (FileInfo dosya in dosyalar2)
                {
                    listBox1.Items.Add(dosya.FullName.Substring(index).Replace("\\", "/"));
                    listBox2.Items.Add(dosya.FullName.Substring(index2));
                    listBox5.Items.Add(dosya.DirectoryName.Substring(index).Replace("\\", "/"));
                    listBox4.Items.Add(dosya.LastAccessTime);
                }
                var klasorler2 = new DirectoryInfo(klasor.FullName).GetDirectories("*");
                foreach (DirectoryInfo klasor2 in klasorler2)
                {
                    var dosyalar3 = new DirectoryInfo(klasor2.FullName).GetFiles("*.*");
                    foreach (FileInfo dosya1 in dosyalar3)
                    {
                        listBox1.Items.Add(dosya1.FullName.Substring(index).Replace("\\", "/"));
                        listBox2.Items.Add(dosya1.FullName.Substring(index2));
                        listBox5.Items.Add(dosya1.DirectoryName.Substring(index).Replace("\\", "/"));
                        listBox4.Items.Add(dosya1.LastAccessTime);
                    }
                }

            }
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                try
                {
                    if (Convert.ToDateTime(listBox3.Items[i].ToString()) != Convert.ToDateTime(listBox4.Items[i].ToString()))
                    {
                        progressBar2.Value = 0;
                        FileInfo filei = new FileInfo(textEditYuklenecekDosya.Text + "\\" + listBox2.Items[i]);
                        string adres = textEditYuklenecekLink.Text + listBox1.Items[i];
                        string path = adres;
                        progressBar2.Value += 5;
                        FtpWebRequest FTP;
                        FTP = (FtpWebRequest)FtpWebRequest.Create(path);
                        FTP.Credentials = new NetworkCredential(textEditKullaniciAd.Text, textEditParola.Text);
                        progressBar2.Value += 15;
                        FTP.KeepAlive = false;
                        FTP.Method = WebRequestMethods.Ftp.UploadFile;
                        FTP.UseBinary = true;
                        FTP.ContentLength = filei.Length;
                        progressBar2.Value += 15;
                        int buffLength = 2048;
                        byte[] buff = new byte[buffLength];
                        int contentLen;
                        FileStream FS = filei.OpenRead();
                        progressBar2.Value += 15;
                        try
                        {
                            Stream strm = FTP.GetRequestStream(); contentLen = FS.Read(buff, 0, buffLength); while (contentLen != 0)
                            {
                                strm.Write(buff, 0, contentLen); contentLen = FS.Read(buff, 0, buffLength);
                            }
                            progressBar2.Value += 50;
                            strm.Close();
                            FS.Close();
                        }
                        catch
                        {
                            progressBar2.Value = 0;
                        }
                    }
                }
                catch
                {
                    progressBar2.Value = 0;
                }
            }

        }
        int dosyaSayisi = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textEditYuklenecekDosya.Text = folderBrowserDialog1.SelectedPath;
                string kaynak = textEditYuklenecekDosya.Text;
                var dosyalar = new DirectoryInfo(kaynak).GetFiles("*.*");

                bool test = true;
                foreach (FileInfo dosya in dosyalar)
                {
                    if (test)
                    {
                        for (int i = 0; i < textEditYuklenecekDosya.Text.Length; i++)
                        {
                            if (dosya.FullName[i].ToString() == "\\")
                            {
                                index = i + 1;
                            }
                        }
                        for (int i = 0; i <= textEditYuklenecekDosya.Text.Length; i++)
                        {
                            if (dosya.FullName[i].ToString() == "\\")
                            {
                                index2 = i + 1;
                            }
                        }
                    }
                    test = false;
                    listBox1.Items.Add(dosya.FullName.Substring(index).Replace("\\", "/"));
                    listBox2.Items.Add(dosya.FullName.Substring(index2));
                    listBox5.Items.Add(dosya.DirectoryName.Substring(index).Replace("\\", "/"));
                    listBox3.Items.Add(dosya.LastAccessTime);
                }
                var klasorler = new DirectoryInfo(kaynak).GetDirectories("*");
                foreach (DirectoryInfo klasor in klasorler)
                {
                    var dosyalar2 = new DirectoryInfo(klasor.FullName).GetFiles("*.*");
                    foreach (FileInfo dosya in dosyalar2)
                    {
                        listBox1.Items.Add(dosya.FullName.Substring(index).Replace("\\", "/"));
                        listBox2.Items.Add(dosya.FullName.Substring(index2));
                        listBox5.Items.Add(dosya.DirectoryName.Substring(index).Replace("\\", "/"));
                        listBox3.Items.Add(dosya.LastAccessTime);

                    }
                    var klasorler2 = new DirectoryInfo(klasor.FullName).GetDirectories("*");
                    foreach (DirectoryInfo klasor2 in klasorler2)
                    {
                        var dosyalar3 = new DirectoryInfo(klasor2.FullName).GetFiles("*.*");
                        foreach (FileInfo dosya1 in dosyalar3)
                        {
                            listBox1.Items.Add(dosya1.FullName.Substring(index).Replace("\\", "/"));
                            listBox2.Items.Add(dosya1.FullName.Substring(index2));
                            listBox5.Items.Add(dosya1.DirectoryName.Substring(index).Replace("\\", "/"));
                            listBox3.Items.Add(dosya1.LastAccessTime);
                        }
                    }

                }
                tarihSorgu();
                dosyaSayisi = listBox1.Items.Count;
                klasorSayisi = listBox5.Items.Count;
            }
            string[] arr = new string[listBox5.Items.Count];
            listBox5.Items.CopyTo(arr, 0);

            var arr2 = arr.Distinct();

            listBox5.Items.Clear();
            foreach (string s in arr2)
            {
                listBox5.Items.Add(s);
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textEditYuklenecekDosya.Text = openFileDialog1.FileName;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textEditSunucu.Text = Properties.Settings.Default.Sunucu;
            textEditKullaniciAd.Text = Properties.Settings.Default.KullaniciAd;
            textEditParola.Text = Properties.Settings.Default.Parola;
            checkBox1.Checked = Properties.Settings.Default.CheckDurum;
            textEditYuklenecekLink.Text = "ftp://" + textEditSunucu.Text + "/";
            textEditDosyaLink.Text = "ftp://" + textEditSunucu.Text + "/";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textEditKayitYeri.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Properties.Settings.Default.Sunucu = textEditSunucu.Text;
                Properties.Settings.Default.KullaniciAd = textEditKullaniciAd.Text;
                Properties.Settings.Default.Parola = textEditParola.Text;
                Properties.Settings.Default.CheckDurum = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Sunucu = null;
                Properties.Settings.Default.KullaniciAd = null;
                Properties.Settings.Default.Parola = null;
                Properties.Settings.Default.CheckDurum = false;
                Properties.Settings.Default.Save();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox1.SelectedIndex;
            listBox3.SelectedIndex = a;
            listBox2.SelectedIndex = a;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox2.SelectedIndex;
            listBox1.SelectedIndex = a;
            listBox3.SelectedIndex = a;
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = listBox3.SelectedIndex;
            listBox1.SelectedIndex = a;
            listBox2.SelectedIndex = a;
        }
        int klasorSayisi = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] arr = new string[listBox5.Items.Count];
            listBox5.Items.CopyTo(arr, 0);

            var arr2 = arr.Distinct();

            listBox5.Items.Clear();
            foreach (string s in arr2)
            {
                listBox5.Items.Add(s);
            }
            if (int.Parse(label12.Text) >= 0)
            {
                timer1.Interval = 1000;
                timer1.Enabled = true;
                label12.Text = (int.Parse(label12.Text) - 1).ToString();
            }
            if (int.Parse(label12.Text) == 0)
            {
                if (dosyaSayisi != listBox1.Items.Count)
                {
                    if (klasorSayisi != listBox5.Items.Count)
                    {
                        klasorOlustur();
                        klasorSayisi = listBox5.Items.Count;
                    }
                    DosyaYukle();
                    dosyaSayisi = listBox1.Items.Count;
                }
                label12.Text = trackBar1.Value.ToString();
                tarihSorgu();
                listBox3.Items.Clear();
                foreach (var item in listBox4.Items)
                {
                    listBox3.Items.Add(item);
                }
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label12.Text = trackBar1.Value.ToString();
        }
    }
}