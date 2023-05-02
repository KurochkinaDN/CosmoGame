using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cosmo_Game
{
    public partial class CosmoGame : Form
    {
        bool goLeft, goRight, goDown, goUp, gameOver;
        string facing = "up";
        int rocketHealth = 100;
        int speed = 10;
        int ammo = 10;
        int ufoSpeed = 3;
        Random randNum = new Random();
        List<PictureBox> ufoList=new List<PictureBox>();
        int score;

        public CosmoGame()
        {
            InitializeComponent();
            RestartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (rocketHealth > 1)
            {
                HealthBar.Value = rocketHealth;
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtKillers.Text = "Kills: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp == true && player.Top > 45)
            {
                player.Top -= speed;
            }
            if (goDown == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;

                    }
                }

                if (x is PictureBox && (string)x.Tag == "ufo")
                {

                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        rocketHealth -= 1;
                    }

                    if (x.Left > player.Left)
                    {
                        x.Left -= ufoSpeed;
                        ((PictureBox)x).Image = Properties.Resources.uleft;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += ufoSpeed;
                        ((PictureBox)x).Image = Properties.Resources.uright;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= ufoSpeed;
                        ((PictureBox)x).Image = Properties.Resources.uup;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += ufoSpeed;
                        ((PictureBox)x).Image = Properties.Resources.udown;
                    }

                }

                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "ufo")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            ufoList.Remove(((PictureBox)x));
                            MakeUfo();
                        }
                    }
                }
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);


                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }

        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }
        private void DropAmmo()
        {

            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();



        }
        private void MakeUfo()
        {
            PictureBox ufo = new PictureBox();
            ufo.Tag = "ufo";
            ufo.Image = Properties.Resources.UFO;
            ufo.Left = randNum.Next(0, 900);
            ufo.Top = randNum.Next(0, 800);
            ufo.SizeMode = PictureBoxSizeMode.AutoSize;
            ufoList.Add(ufo);
            this.Controls.Add(ufo);
            player.BringToFront();
        }

        private void RestartGame()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.UFO;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();
        }
    }
}
