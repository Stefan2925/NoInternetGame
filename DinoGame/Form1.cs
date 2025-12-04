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
        int DIMENSIUNE=20;
        PictureBox pictureBox;
        int [][] Board;
        Graphics mDesen;
        SolidBrush [] pensula;

       public enum PATRATE{
            GOL=1,
            PLAYER=2,
            OBSTACOL =3,

        }
        public FormDino()
        {
            InitializeComponent();
            mDesen = pictureBox1.CreateGraphics();
          
            pensula = new SolidBrush[3];
            pensula[0] = new SolidBrush(Color.White);
            pensula[1] = new SolidBrush(Color.Red);
            pensula[2] = new SolidBrush(Color.Blue);






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

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    mDesen.FillRectangle(pensula[Board[i][j]], i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
                }
            }
        }
    }
}
