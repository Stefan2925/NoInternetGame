using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DinoGame
{
    public partial class FormDino : Form
    {
        Bitmap canvas;
        Graphics g;

        const int DIMENSIUNE = 20;
        int[][] Board;
        SolidBrush[] pensula;

        // player
        int dinoX = 12;
        int dinoY = 10;
        int dinoJump = 3;
        bool isJumping = false;
        bool isUp = false;
        int jumpStep = 0;
        int ground = 10;

        // obstacole
        List<Point> obstacles = new List<Point>();
        Random rand = new Random();
        int spawnTimer = 0;
        int spawnInterval = 10;//modificiabil pentru dificultate cu cat e mai mic cu atat se spawneaza mai des

        private Timer gameTimer;

        public enum PATRATE
        {
            GOL = 0,
            PLAYER = 1,
            OBSTACOL = 2,
            IARBA = 3,
            NOROI = 4,
        }

        public FormDino()
        {
            InitializeComponent();

            // Initialize board and drawing surface BEFORE anything uses it.
            InitBoard();

            // Setup bitmap and graphics
            canvas = new Bitmap(60 * DIMENSIUNE, 20 * DIMENSIUNE);
            g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Assign bitmap to picture box (designer should have pictureBox1)
            pictureBox1.Image = canvas;

            // Brushes
            pensula = new SolidBrush[5];
            pensula[0] = new SolidBrush(Color.White);       // GOL
            pensula[1] = new SolidBrush(Color.Blue);        // PLAYER
            pensula[2] = new SolidBrush(Color.Red);         // OBSTACOL
            pensula[3] = new SolidBrush(Color.GreenYellow); // IARBA
            pensula[4] = new SolidBrush(Color.Brown);       // NOROI

            // Timer
            gameTimer = new Timer();
            gameTimer.Interval = 100; // ms
            gameTimer.Tick += GameTimer;

            // Input
            this.KeyPreview = true;
            this.KeyDown += pressSpace;

            // Improve double-buffering (mostly helps form-level painting)
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }

        private void InitBoard()
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
        }

        private void GameTimer(object sender, EventArgs e)
        {
            UpdateGame();
            drawBoard();
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
            // Update jumping physics
            if (isJumping)
            {
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

            UpdateObstacles();
            CheckCollisions();
        }

        void UpdateObstacles()
        {
            spawnTimer++;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0;

                // Random spawn
                if (rand.Next(0, 2) == 0)
                {
                    // spawn at right-most column, on ground
                    obstacles.Add(new Point(59, ground));
                }
            }

            // Move obstacles left, remove if off-screen
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Point p = obstacles[i];
                p.X -= 1;
                if (p.X < 0)
                {
                    obstacles.RemoveAt(i);
                }
                else
                {
                    obstacles[i] = p;
                }
            }
        }

        void CheckCollisions()
        {
            // Basic collision detection: if any obstacle occupies the same grid cell as the player, game over (or restart)
            foreach (var o in obstacles)
            {
                if (o.X == dinoX && o.Y == dinoY)
                {
                    gameTimer.Stop();
                    MessageBox.Show("You hit an obstacle!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        void drawBoard()
        {
            // Draw to bitmap
            g.Clear(Color.White);

            // Clear logical board
            for (int i = 0; i < 60; i++)
                Array.Clear(Board[i], 0, 20);

            // Place dino
            drawDino(dinoY);

            // Place obstacles into logical board
            foreach (var obs in obstacles)
            {
                if (obs.X >= 0 && obs.X < 60 && obs.Y >= 0 && obs.Y < 20)
                {
                    Board[obs.X][obs.Y] = (int)PATRATE.OBSTACOL;
                }
            }

            // Draw grid cells
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Brush b;
                    if (j < 11)
                        b = pensula[Board[i][j]];
                    else if (j == 11)
                        b = pensula[(int)PATRATE.IARBA];
                    else
                        b = pensula[(int)PATRATE.NOROI];

                    if (Board[i][j] == (int)PATRATE.OBSTACOL)
                    {
                        // draw spike polygon at this grid (i, j)
                        var poly = GetSpikePolygon(i, j);
                        g.FillPolygon(b, poly);
                    }
                    else if (Board[i][j] == (int)PATRATE.PLAYER)
                    {
                        // draw player as filled rectangle (can replace with sprite later)
                        g.FillRectangle(b, i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
                    }
                    else
                    {
                        // default cell rectangle
                        g.FillRectangle(b, i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
                    }
                }
            }

            // Present bitmap to picture box
            pictureBox1.Image = canvas;
            pictureBox1.Refresh();
        }

        // Draw the player in the board array
        void drawDino(int y)
        {
            Board[dinoX][y] = (int)PATRATE.PLAYER;
        }

        // Returns a triangle-shaped spike polygon positioned at gridX, gridY in pixel coordinates
        Point[] GetSpikePolygon(int gridX, int gridY)
        {
            int px = gridX * DIMENSIUNE;
            int py = gridY * DIMENSIUNE;

            int inset = 2;
            int left = px + inset;
            int right = px + DIMENSIUNE - inset;
            int bottom = py + DIMENSIUNE - inset;
            int top = py + inset;

            // Simple triangle (pointing up) inside the cell
            Point[] triangle = new Point[3];
            triangle[0] = new Point(left, bottom);
            triangle[1] = new Point(right, bottom);
            triangle[2] = new Point((left + right) / 2, top);

            return triangle;
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Play_Click(object sender, EventArgs e)
        {
            // Re-init board and state
            InitBoard();
            obstacles.Clear();

            dinoY = ground;
            isJumping = false;
            isUp = false;
            jumpStep = 0;

            spawnTimer = 0;

            gameTimer.Start();
            this.ActiveControl = null;
            drawBoard();
        }
    }
}
