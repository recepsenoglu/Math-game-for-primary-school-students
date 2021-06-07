using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Matematik_oyunu                          // Written by Recep Oğuzhan Şenoğlu
                                                  // Recep Oğuzhan Şenoğlu tarafından yazılmıştır
{
    public partial class Form1 : Form
    {       
        public Form1()
        {
            // Cmd'den gelen parametleri almak için yazdığımız kod satırı
            string[] args = Environment.GetCommandLineArgs();
            InitializeComponent();

            if (args.Length > 2)
            {
                if (args[2] != "all")
                {
                    level3 = int.Parse(args[2]);
                    level = level3;
                }
                else if(args[2] == "all")
                {
                    level3 = 1;
                    level = level3;
                    godmode = true;                  
                }
                else
                {
                    throw new Exception("Invalid Parameter");
                }
            }
            else
            {
                //throw new Exception("Invalid Parameter");
            }
        }        
        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxes[0] = txt_answer1;
            textBoxes[1] = txt_answer2;
            textBoxes[2] = txt_answer3;
            textBoxes[3] = txt_answer4;
            textBoxes[4] = txt_answer5;
            buttons[0] = btn_pass1;
            buttons[1] = btn_pass2;
            buttons[2] = btn_pass3;
            buttons[3] = btn_pass4;
            buttons[4] = btn_pass5;
            labels[0] = lbl_question1;
            labels[1] = lbl_question2;
            labels[2] = lbl_question3;
            labels[3] = lbl_question4;
            labels[4] = lbl_question5;
            /*
            lbl_question1.Text = CreateQuestions(level, 0);
            lbl_question2.Text = CreateQuestions(level, 1);
            lbl_question3.Text = CreateQuestions(level, 2);
            lbl_question4.Text = CreateQuestions(level, 3);
            lbl_question5.Text = CreateQuestions(level, 4);
            */
            lbl_time.Text = saniye.ToString();
            if (godmode)
            {
                btn_god.Visible = true;
                nickname = "the god";
                pnl_start.Visible = false;
                lbl_level.Text = "Level: " + level.ToString();
                timer1.Start();
                lbl_question1.Text = CreateQuestions(level, 0);
                lbl_question2.Text = CreateQuestions(level, 1);
                lbl_question3.Text = CreateQuestions(level, 2);
                lbl_question4.Text = CreateQuestions(level, 3);
                lbl_question5.Text = CreateQuestions(level, 4);
            }
        }

        // Gerekli dizi ve değişkenleri yarattığımız kısım
        TextBox[] textBoxes = new TextBox[5];
        Button[] buttons = new Button[5];
        Label[] labels = new Label[5];
        bool godmode = false;
        string dir = Directory.GetCurrentDirectory();
        string[] islemler = {"+","-","*","/"};

        string soru;
        int cevap;
        bool include = false;
        int[] cevaplar = new int[5];
        string[] pass_sorular = new string[5];
        int[] pass_cevaplar = new int[5];
        int i = 0;
        string rnd0;
        int rnd1;
        int rnd2;
        string nickname = "rec";
        
        int block = 1;
        static int level3 = 0;
        int level = level3;
        int saniye1 = 90;
        int saniye = 90;

        int toplamdoğrusayısı = 0;
        int doğrusorusayısı = 0;
        int[] leveldoğrusayısı = new int[5];
        string[] levelyıldızları = new string[5];
        int cevaplanmışsorusayısı = 0;
        int pass_number = 0;
        Random random = new Random();
        private string CreateQuestions(int level1,int sorunumarası) // Soru yaratma kısmı
        {
            // Seviyeye göre soruların zorluklarını artırıyoruz
            if (level1 == 1 || level1 == 2)
            {
                rnd0 = islemler[random.Next(0, 2)];
                rnd1 = random.Next(5, 15);
                rnd2 = random.Next(2, 10);
            }
            else if (level1 == 3 || level1 == 4)
            {
                rnd0 = islemler[random.Next(0, 3)];
                rnd1 = random.Next(5, 20);
                rnd2 = random.Next(2, 15);
            }
            else if (level1 == 5)
            {
                while (true)
                {
                    rnd0 = islemler[random.Next(2, 4)];
                    rnd1 = random.Next(5, 200);
                    rnd2 = random.Next(2, 50);
                    if(rnd1%rnd2 == 0 || rnd2 % rnd1 == 0)
                    {
                        break;
                    }
                }
            }
            if (rnd2 > rnd1)
            {
                int temp = rnd1;
                rnd1 = rnd2;
                rnd2 = temp;
            }
            soru = rnd1.ToString() + rnd0 + rnd2.ToString();

            if(rnd0 == "+") cevap = rnd1 + rnd2;
            else if (rnd0 == "-") cevap = rnd1 - rnd2;
            else if (rnd0 == "*") cevap = rnd1 * rnd2;
            else if (rnd0 == "/") cevap = rnd1 / rnd2;

            cevaplar[sorunumarası] = cevap; // Cevapları bir diziye atıyoruz
            return soru;
        }
        private void Check(int txtboxnumber) // Cevabı kontrol etme metodu
        {
            if (textBoxes[txtboxnumber].Text == cevaplar[txtboxnumber].ToString())
            {
                // Cevap doğru
                doğrusorusayısı++;
                textBoxes[txtboxnumber].BackColor = Color.LimeGreen;
            }
            else
            {
                // Cevap yanlış
                textBoxes[txtboxnumber].BackColor = Color.Red;
            }
            if (txtboxnumber != 4)
            {
                this.ActiveControl = textBoxes[txtboxnumber + 1];
                textBoxes[txtboxnumber+1].Enabled = true;
            }
            buttons[txtboxnumber].Enabled = false;
            textBoxes[txtboxnumber].Enabled = false;
            
            cevaplanmışsorusayısı++;
            if (cevaplanmışsorusayısı >= 5)
            {
                btn_next.Visible = true;
                btn_next.Focus();
            }
            for(int i=0;i<5;i++)
            {
                buttons[i].Visible = false;
            }
            if (txtboxnumber != 4 && block != 5) buttons[txtboxnumber + 1].Visible = true;
        }   
        private void Pass(int btnnumber) // Pas tuşu
        {
            pass_number++;
            if(pass_number == 1)
            {
                textBoxes[btnnumber].BackColor = Color.DimGray;
                textBoxes[btnnumber].Enabled = false;

                pass_sorular[i] = labels[btnnumber].Text;
                pass_cevaplar[i] = cevaplar[btnnumber];
                i++;
            }           
            else if (pass_number >= 2) // Eğer 1den fazla pas demişsek yanlış kabul edilir
            {
                textBoxes[btnnumber].BackColor = Color.Red;
            }
            if (btnnumber != 4)
            {
                this.ActiveControl = textBoxes[btnnumber + 1];
                textBoxes[btnnumber+1].Enabled = true;
            }
            buttons[btnnumber].Enabled = false;
            textBoxes[btnnumber].Enabled = false;
            cevaplanmışsorusayısı++;
            for (int i = 0; i < 5; i++)
            {
                buttons[i].Visible = false;
            }
            if (cevaplanmışsorusayısı >= 5)
            {
                btn_next.Visible = true;
            }
            if (btnnumber != 4) buttons[btnnumber + 1].Visible = true;
        }
        private void btn_next_Click(object sender, EventArgs e)
        {
            // Duruma göre sonraki bloğa veya sonraki seviyeye geçmeyi sağlar
            block++;
            cevaplanmışsorusayısı = 0;
            btn_next.Visible = false;
            if (block == 5) // Block 4den sonra pas bloğunu açar
            {
                for (int i = 0; i < 5; i++)
                {
                    labels[i].Text = pass_sorular[i];
                    cevaplar[i] = pass_cevaplar[i];
                    if (labels[i].Text == "")
                    {
                        cevaplanmışsorusayısı++;
                        buttons[i].Visible = false;
                        textBoxes[i].Visible = false;
                        btn_next.Focus();
                    }
                }
                if (cevaplanmışsorusayısı == 5)
                {
                    btn_next.Visible = true;
                    btn_next_Click(btn_next, new EventArgs());
                }
                lbl_block.Text = "Block:\npass\nqeustions";
                btn_next.Text = "Next Level";
            }
            else if (block == 6 || block == 7) // Pas bloğundan sonra level artırır
            {
                if (doğrusorusayısı < 11) // Eğer 11den az soru bilmiş isek oyun biter
                {
                    pnl_over.Visible = true;
                    lbl_over.Text = "You have lost...";
                    lbl_over2.Text = "Game Over!";
                }
                else // Eğer 11 veya daha fazla soru bilmiş ise level artırır
                {
                    // Seviyedeki doğru sayısına göre yıldız veriyoruz
                    leveldoğrusayısı[level - 1] = doğrusorusayısı;
                    if (doğrusorusayısı >= 11 && doğrusorusayısı <= 15) levelyıldızları[level - 1] = "★";
                    else if (doğrusorusayısı >= 16 && doğrusorusayısı <= 118) levelyıldızları[level - 1] = "★★";
                    else if (doğrusorusayısı >= 19 && doğrusorusayısı <= 20) levelyıldızları[level - 1] = "★★★";
                    toplamdoğrusayısı += doğrusorusayısı;
                    doğrusorusayısı = 0;
                    level++;
                    if (level != 6) FileSave(nickname, (level)); // Checkpoint noktası oluşturuyoruz
                                    // Programı kapattıktan sonra aynı kkulanıcı adı ile girersek kaldığımız seviyeden devam ediyoruz
                    saniye1 += 10;
                    saniye = saniye1;
                    block = 1;
                    i = 0;
                    pass_sorular = new string[5];
                    pass_cevaplar = new int[5];
                    lbl_level.Text = "Level: " + level;
                    btn_next.Text = "Next Block";
                    buttons[0].Visible = true;
                    for (int i = 0; i < 5; i++)
                    {
                        textBoxes[i].Visible = true;
                    }
                    if(level == 6)
                    {
                        // Oyun bitti. Skorları dosyaya kaydediyoruz
                        File_operations(levelyıldızları,leveldoğrusayısı,toplamdoğrusayısı,nickname);
                        pnl_over.Visible = true;
                        timer1.Stop();
                        lbl_over.Text = "You have won";
                        lbl_over2.Text = "Congratulations!!";
                        lbl_achievement.Text = nickname + "'s Achievement: \nTotal number of correct answers: "
                    + toplamdoğrusayısı + "\nLevel score and stars:";
                        for (int i = 0; i < 5; i++)
                        {
                            lbl_achievement2.Text += "level " + (i + 1) + "'s Score: " + leveldoğrusayısı[i] + "  " + levelyıldızları[i] + "\n";
                        }
                    }
                }
            }
            if (block != 5)
            {
                // Her blok için soru yarat
                lbl_question1.Text = CreateQuestions(level, 0);
                lbl_question2.Text = CreateQuestions(level, 1);
                lbl_question3.Text = CreateQuestions(level, 2);
                lbl_question4.Text = CreateQuestions(level, 3);
                lbl_question5.Text = CreateQuestions(level, 4);
                lbl_block.Text = "Block: " + block;
            }
            for (int i = 0; i < 5; i++)
            {
                buttons[i].Visible = false;
                buttons[i].Enabled = true;
                textBoxes[i].Enabled = false;
                textBoxes[i].BackColor = Color.White;
                textBoxes[i].Clear();
            }
            textBoxes[0].Enabled = true;
            if (block != 5) buttons[0].Visible = true;
            pass_number = 0;
            textBoxes[0].Focus();
        }     
        // Dosya skor kayıt kısmı
        private void File_operations(string[] levelyıldızları1,int[] leveldoğrusayısı1,int toplamdoğrusayısı1,string nick)
        {
            string file_path = dir + "\\Scores.txt";
            if (File.Exists(file_path))
            {
                StreamWriter SW = File.AppendText(file_path);
                SW.WriteLine(nick + "'s Achievement: \nTotal number of correct answers: " 
                    + toplamdoğrusayısı1 + "\nLevel score and stars:");
                for(int i=0;i<5;i++)
                {
                    SW.WriteLine("level " + (i + 1) + "'s Score: " + leveldoğrusayısı1[i] + "  " + levelyıldızları1[i]);
                }
                SW.WriteLine("\n");
                SW.Close();
            }
            else
            {
                FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(nick + "'s achievement: \nTotal number of correct answers: "
                    + toplamdoğrusayısı1 + "\nLevel score and stars:\n");
                for (int i = 0; i < 5; i++)
                {
                    sw.WriteLine("level " + (i + 1) + "'s Score: " + leveldoğrusayısı1[i] + "  " + levelyıldızları1[i]);
                }
                sw.WriteLine("\n");
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }       
        private void FileSave(string nickname1,int level1)
        {
            // Dosya checkpoint kısmı
            string file_path = dir + "\\Checkpoints.txt";
            if (File.Exists(file_path))
            {
                StringBuilder newFile = new StringBuilder();
                string temp = "";
                string[] file = File.ReadAllLines(file_path);
                foreach (string line in file)
                {
                    if (line.Contains(nickname1))
                    {
                        include = true;
                        temp = line.Replace(
                line, (nickname1 + ":" + level1.ToString()));
                        newFile.Append(temp +
                "\r\n");
                        continue;
                    }
                    newFile.Append(line +
                "\r\n");
                }
                File.WriteAllText(file_path, newFile.ToString());

                if (include==false)
                {                   
                    StreamWriter SW = File.AppendText(file_path);
                    SW.WriteLine(nickname1 + ":" + level1);
                    SW.Close();
                }                             
            }
            else
            {
                FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(nickname1 + ":" + level1.ToString());
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        // Checkpoint okuma kısmı
        private string FileRead(string nick01)
        {
            string file_path = dir + "\\Checkpoints.txt";
            if (File.Exists(file_path))
            {
                FileStream fs = new FileStream(file_path, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);
                string word = sw.ReadLine();
                while (word != null)
                {
                    if (word.Contains(nick01))
                    {
                        string return_value;
                        return_value = word[word.Length-1].ToString();
                        return return_value;                  
                    }
                    word = sw.ReadLine();
                }
                sw.Close();
                fs.Close();
                if (level == 0) return "1";
                else return level3.ToString();
            }
            else
            {
                if (level == 0) return "1";
                else return level3.ToString();
            }
        }       
        private void timer1_Tick(object sender, EventArgs e) // Süre metodu
        {
            saniye--;
            lbl_time.Text = saniye.ToString() + "s";
            if(saniye <0)
            {
                timer1.Stop();
                pnl_over.Visible = true;
                lbl_over.Text = "You have lost...";
                lbl_over2.Text = "Game Over!";
                lbl_achievement.Text = "Time out";
            }
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            // Start butonuna basınca rastgele sorular yaratılır. Ve isim eğer kayıtlı ise kaldığı seviyeden başlar
            nickname = txt_nickname.Text;
            pnl_start.Visible = false;
            if (level == 0) level = int.Parse(FileRead(nickname));
            else level = level3;
            lbl_level.Text = "Level: " + level.ToString();
            timer1.Start();
            lbl_question1.Text = CreateQuestions(level, 0);
            lbl_question2.Text = CreateQuestions(level, 1);
            lbl_question3.Text = CreateQuestions(level, 2);
            lbl_question4.Text = CreateQuestions(level, 3);
            lbl_question5.Text = CreateQuestions(level, 4);
        }
        private void txt_nickname_TextChanged(object sender, EventArgs e)
        {
            int txt_length = txt_nickname.TextLength;
            if (txt_length == 0) btn_start.Enabled = false;
            else btn_start.Enabled = true;
        }
        private void txt_nickname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btn_start.Enabled != false)
            {
                btn_start_Click(btn_start, new EventArgs());
            }
        }
        private void txt_answer1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_answer1.Text != "")
            {
                Check(0);
            }
        }
        private void txt_answer2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_answer2.Text != "")
            {
                Check(1);
            }
        }
        private void txt_answer3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_answer3.Text != "")
            {
                Check(2);
            }
        }
        private void txt_answer4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_answer4.Text != "")
            {
                Check(3);
            }
        }
        private void txt_answer5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txt_answer5.Text != "")
            {
                Check(4);
            }
        }

        private void btn_pass1_Click(object sender, EventArgs e)
        {
            Pass(0);
        }

        private void btn_pass2_Click(object sender, EventArgs e)
        {
            Pass(1);
        }

        private void btn_pass3_Click(object sender, EventArgs e)
        {
            Pass(2);
        }

        private void btn_pass4_Click(object sender, EventArgs e)
        {
            Pass(3);
        }

        private void btn_pass5_Click(object sender, EventArgs e)
        {
            Pass(4);
        }
        private void btn_god_Click(object sender, EventArgs e)
        {
            block = 5;
            btn_next_Click(btn_next, new EventArgs());
        }
    }
}
// Written by Recep Oğuzhan Şenoğlu
// Recep Oğuzhan Şenoğlu tarafından yazılmıştır