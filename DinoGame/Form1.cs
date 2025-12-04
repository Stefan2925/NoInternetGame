using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinoGame
{
    public partial class FormDino : Form
    {
        int DIMENSIUNE = 20;
        PictureBox pictureBox;
        int[][] Board;
        Graphics mDesen;
        SolidBrush[] pensula;
        int dinoX=12;
        int dinoY=10;
        int dinoJump = 3;//asta ma gandeam sa o folosim pentru saritura,
                         //atunci cand apesi space PLAYER-ul sare cu 3 pozitii in sus ca sa evitam obstacolele
        bool isJumping = false;
        bool isUp = false;
        int jumpStep = 0;//numarul de pasi facuti in saritura
        int ground=10;//linia solului
        Random rand = new Random();//pentru obstacole
        private Timer gameTimer;

        public enum PATRATE
        {
            GOL = 0,
            PLAYER = 1,
            OBSTACOL = 2,

        }
        public FormDino()
        {
            InitializeComponent();
            mDesen = pictureBox1.CreateGraphics();

            pensula = new SolidBrush[3];
            pensula[0] = new SolidBrush(Color.White);
            pensula[1] = new SolidBrush(Color.Blue);
            pensula[2] = new SolidBrush(Color.Red);

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

        void drawBoard()
        {
            mDesen.Clear(Color.White);

            for (int i = 0; i < 60; i++)
                Array.Clear(Board[i], 0, 20);

            drawDino(dinoY);

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    mDesen.FillRectangle(pensula[Board[i][j]], i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
                }
            }

        }

        void drawDino(int y)
        {
            Board[dinoX][y] = (int)PATRATE.PLAYER;
            mDesen.FillRectangle(pensula[(int)PATRATE.PLAYER], dinoX * DIMENSIUNE + 1, dinoY * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
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
        private void pressSpace(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !isJumping)
            {
                isJumping = true;
                isUp = true;
                jumpStep = 0;
            }
        }


    }
}