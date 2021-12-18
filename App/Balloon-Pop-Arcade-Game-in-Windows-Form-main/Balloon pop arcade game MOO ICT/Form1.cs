using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Balloon_pop_arcade_game_MOO_ICT
{
   
    public partial class Form1 : Form
    {
        public static string name;
        public static int score;
        int speed;
        
        Random rand = new Random();
        bool gameOver;

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {

            txtScore.Text = "Score: " + score;

            if (gameOver == true)
            {
                gameTimer.Stop();
                txtScore.Text = "Score: " + score + " Game over, press enter to restart!";
                button1.Visible = true;
                button2.Visible = true;
                textBox1.Visible = true;
            }

            foreach(Control x in this.Controls)
            {

                if (x is PictureBox)
                {
                    x.Top -= speed;

                    if (x.Top < -100)
                    {
                        x.Top = rand.Next(700, 1000);
                        x.Left = rand.Next(5, 500);
                    }

                    if ((string)x.Tag == "balloon")
                    {

                        if (x.Top < -50)
                        {
                            gameOver = true;
                        }

                        if (bomb.Bounds.IntersectsWith(x.Bounds))
                        {
                            x.Top = rand.Next(700, 1000);
                            x.Left = rand.Next(5, 500);
                        }
                    }
                }

            }

            if (score > 5)
            {
                speed = 8;
            }
            if (score >= 15 && score < 25)
            {
                speed = 12;
            }
            if (score >= 25)
            {
                speed = 16;
            }
        }

        private void PopBalloon(object sender, EventArgs e)
        {
            if (gameOver == false)
            {
                var balloon = (PictureBox)sender;

                balloon.Top = rand.Next(750, 1000);
                balloon.Left = rand.Next(5, 500);

                score += 1;
            }
        }

        private void GoBoom(object sender, EventArgs e)
        {
            if (gameOver == false)
            {
                bomb.Image = Properties.Resources.boom;
                gameOver = true;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            speed = 5;
            score = 0;
            gameOver = false;

            bomb.Image = Properties.Resources.bomb;

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    x.Top = rand.Next(750,  1000);
                    x.Left = rand.Next(5, 500);
                }
            }
            gameTimer.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RestartGame();
            button1.Visible = false;
            button2.Visible = false;
            textBox1.Visible = false;
        }
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=sajt;password=sajt");

        private void button2_Click_1(object sender, EventArgs e)
        {
            name = textBox1.Text;
            string insertQuery = "INSERT INTO database1.table1() VALUES('" + name + "','" + score + "')";
            connection.Open();
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
              try
              {
                  if (command.ExecuteNonQuery() == 1)
                  {
                      MessageBox.Show("Data Inserted");
                  }
                  else
                  {
                      MessageBox.Show("Data Not Inserted");
                  }
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }
            connection.Close();
        }
    }
}
