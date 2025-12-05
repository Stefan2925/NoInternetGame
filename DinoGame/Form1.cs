using DinoGame.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinoGame
{
    public partial class FormDino : Form
    {
        Bitmap canvas;    //Deci la drawBoard am facut niste modificari
        Graphics g;                //cand  ajunge mai sus de nivelul player-ului sa schimbam culoarea astfel incat s-avem pamant/surface
                                   //faza e ca facea un flicker urat de tot si am cautat si cica picture box nu e bun pentru grafice si  sa folosim 
                                   // Bitmap-uri ca ii putem da picturebox=bitmap si e aceeasi chestie far fara flicker


        int DIMENSIUNE = 20;
        PictureBox pictureBox;
        int[][] Board;
        Graphics mDesen;
        SolidBrush[] pensula;
        int dinoX=12;
        int dinoY=10;
        int dinoJump = 3;//asta ma gandeam sa o folosim pentru saritura,
                         //atunci cand apesi space PLAYER-ul sare cu 3 pozitii in sus ca sa evitam obstacolele
        int laCateSecunde = int.MaxValue;  //doar am initializat randomizarea obstacolului
        int TipObstacol;// Sa fie mai multe tipuri de obstacole nu doar unul singur 
        bool isJumping = false;
        bool isUp = false;
        int jumpStep = 0;//numarul de pasi facuti in saritura
        int ground=10;//linia solului
        Random rand = new Random();//pentru obstacole
        private Timer gameTimer;


        public Point[] spike = new Point[3]; //point reprezinta o structura de forma  int  x,int y (adica e facuta de csharp si reprezinta coordonata)
       



        public enum PATRATE
        {
            GOL = 0,
            PLAYER = 1,
            OBSTACOL = 2,
            IARBA=3,
            NOROI=4,

        }
        public FormDino()
        {
            InitializeComponent();

            canvas = new Bitmap(60 * DIMENSIUNE, 20 * DIMENSIUNE);
            g = Graphics.FromImage(canvas);
            pictureBox1.Image = canvas;


            pensula = new SolidBrush[5];
            pensula[0] = new SolidBrush(Color.White);
            pensula[1] = new SolidBrush(Color.Blue);
            pensula[2] = new SolidBrush(Color.Red);
            pensula[3] = new SolidBrush(Color.GreenYellow);
            pensula[4] = new SolidBrush(Color.Brown);

            gameTimer = new Timer();
            gameTimer.Interval = 100;
            gameTimer.Tick += new EventHandler(GameTimer);
            this.KeyPreview = true;
            this.KeyDown += pressSpace;

        }

        private void GameTimer(object sender, EventArgs e)
        {
            UpdateGame();
           
            drawBoard();
        }

       void  GenerareObstacol()
        {
           // if(tipObstacol ==1)
            Board[59][10] = (int)PATRATE.OBSTACOL;
            
        }
        private void pressSpace(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !isJumping)
            {
                isJumping = true;
                isUp = true;
                jumpStep = 0;
            }
        }

        private void UpdateGame()
        {
            if (!isJumping)
            {
                return;
            }
            if (isUp)
            {
                if (dinoY > 0)
                {
                    dinoY--;
                    jumpStep++;

                }
                if (jumpStep >= dinoJump)
                {
                    isUp = false;
                }
            }
            else
            {
                if (dinoY < ground)
                {
                    dinoY++;
                }
                else
                {
                    isJumping = false;
                    jumpStep = 0;
                    isUp = false;
                }
            }
        }
       

        void drawBoard()
        {
            g.Clear(Color.White); //face by default totul alb fara sa se vada delimitarile patratelelor 
                                  //ma gandeam ca am putea folosi un dinasta pentru iarba sau noroi sa fie si pamantul cu textura smooth fara patrate

            for (int i = 0; i < 60; i++)
                Array.Clear(Board[i], 0, 20);   //fara chestia asta nu se mentine background-ul si player-ul da overrun
                                                 //daca vrei sa vezi da-i delete la tot for-ul si apoi dai start si sari odata;
            drawDino(dinoY);
            GenerareObstacol();

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Brush b; //mostenire

                    if (j < 11)
                        b = pensula[Board[i][j]];
                    else if (j == 11)
                        b = pensula[(int)PATRATE.IARBA];
                    else
                        b = pensula[(int)PATRATE.NOROI];
                    if (Board[i][j] == (int)PATRATE.OBSTACOL)
                    {


                      Classes.Spike temp = new Classes.Spike(DIMENSIUNE);

                        g.FillPolygon(b, temp.spike);
                    }
                    else { g.FillRectangle(b, i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1); }
                    
                }
            }

            pictureBox1.Refresh(); 
        }

        void MoveObstacol() //nu are rost ca toata tabla sa se miste spre stanga,obstacolele vor veni din dreapta  cu timer-ul asta si tu trebie doar sa sari peste 
        {                   

        }


        void drawDino(int y)
        {                                                                 //fill Rectangle  nu era necesar
            Board[dinoX][y] = (int)PATRATE.PLAYER; //adineauri desenai dino-ul aici dar nu era nevoie ca oricum in drawboard il deseneaza inca odata
        }

      

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Play_Click(object sender, EventArgs e)
        {

            Board = new int[60][];
            for (int i = 0; i < 60; i++)
            {
                Board[i] = new int[20];
                for (int j = 0; j < 20; j++)
                {
                    Board[i][j] = (int)PATRATE.GOL;
                }
            }
            dinoY = ground;
            gameTimer.Start();
            this.ActiveControl = null;
            drawBoard();

        }


    }
}