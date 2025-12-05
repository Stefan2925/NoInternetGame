using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DinoGame.Classes
{
    internal class Spike
    {
        int x, y;
        public Point[] spike;
      public   Spike(int coord_x,int coord_y,int DIMENSIUNE)
        {
            x = coord_x;
            y = coord_y;
            spike = new Point[3];
        
            spike[0] = new Point(x*DIMENSIUNE + DIMENSIUNE / 2, y*DIMENSIUNE);        
            spike[1] = new Point(x*DIMENSIUNE, y*DIMENSIUNE + DIMENSIUNE+1); 
            spike[2] = new Point(x*DIMENSIUNE + DIMENSIUNE, y*DIMENSIUNE + DIMENSIUNE+1);
            //Logica la chestia asta e sub clasa SpikeMare 
        }

    }
    internal class SpikeMare : Spike 
    {
     
        public Rectangle Patrat;

        public SpikeMare(int coord_x, int coord_y, int DIMENSIUNE) : base(coord_x, coord_y-1, DIMENSIUNE)
        {

            Patrat = new Rectangle(coord_x * DIMENSIUNE + 1, (coord_y) * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
          
           
        }
    }


}
//practic fillPolygon va lua patratul nostru de 20 pe 20 si il va transforma intr-o matrice de 400 de pixeli
//apoi 3 puncte vor fi pozitionate pe aceasta matrice si la final se vor unii,tot ceea ce este  in interiorul acestei forme geometrice 
//determinate de unirea celor trei puncte va fi umplut cu culoarea aleasa
// astfel la inceput toate punctele incep din stanga sus a matricii
//deci la spike[0] 59*Dimensiune e locatia patratului a matricii de 400 pixeli pe matricea  noastra de 60x20*400(picture box)
// iar +DIMENSIUNE/2 inseamna ca muta primul punct la mijlocul matricii in partea de sus ,deoarece Y nu adauga cu nimic
//la spike[1]  muta al doilea punct in coltul din stanga jos ,lasa x neschimbat 59 *DIMENSIUNE,dar la y pe langa  10*DIM ii mai baga 
//inca 20 pixeli ca sa fie in stanga jos
//la spike [2] la fel ca la spie[1] doar ca in dreapta jos a matricii
//la final le uneste si coloreaza si ai spike-ul;




//am incercat si spike mare dar mi se pare prea greu de implementat
/*
  internal class SpikeMare : Spike 
    {
        public Point[] jumatate_stanga;
        public Point[] jumatate_dreapta;
        public Rectangle Patrat;

        public SpikeMare(int coord_x, int coord_y, int DIMENSIUNE) : base(coord_x, coord_y-1, DIMENSIUNE)
        {

            jumatate_dreapta = new Point[3];
            jumatate_dreapta[0] = new Point((coord_x +1) * DIMENSIUNE, coord_y * DIMENSIUNE);
            jumatate_dreapta[1] = new Point((coord_x +1) * DIMENSIUNE + DIMENSIUNE / 2, coord_y * DIMENSIUNE + DIMENSIUNE);
            jumatate_dreapta[2] = new Point((coord_x +1) * DIMENSIUNE, coord_y * DIMENSIUNE + DIMENSIUNE);

           
            jumatate_stanga = new Point[3];
            jumatate_stanga[0] = new Point((coord_x -1)* DIMENSIUNE + DIMENSIUNE+1, coord_y * DIMENSIUNE);
            jumatate_stanga[1] = new Point((coord_x -1) * DIMENSIUNE + DIMENSIUNE / 2, coord_y * DIMENSIUNE + DIMENSIUNE);
            jumatate_stanga[2] = new Point((coord_x -1) * DIMENSIUNE + DIMENSIUNE+1, coord_y * DIMENSIUNE + DIMENSIUNE);

            Patrat = new Rectangle(coord_x * DIMENSIUNE + 1, coord_y * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1);
          
           
        }
    }

 */
// SI IN  DrawBoard:

/*
    void  GenerareObstacol()
        {
            TipObstacol = 2;//rand.Next()%2+1;
            switch (TipObstacol)
            {
                case 1: Board[59][11] = (int)PATRATE.OBSTACOL; break;
                case 2: Board[58][10] = (int)PATRATE.OBSTACOL; break;
            }
        }

////////////////////////////////////////////////////////////////////////////////



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
                        switch (TipObstacol)
                        {
                            case 1:
                                Classes.Spike temp = new Classes.Spike(i, j, DIMENSIUNE);
                                g.FillPolygon(b, temp.spike);
                                break;

                            case 2:
                              
                                Classes.SpikeMare tempMare = new Classes.SpikeMare(i, j, DIMENSIUNE);

                                g.FillRectangle(b, tempMare.Patrat);
                                g.FillPolygon(b, tempMare.jumatate_stanga);
                                g.FillPolygon(b, tempMare.jumatate_dreapta);
                                g.FillPolygon(b, tempMare.spike); ++i;
                                break;
                        }


                    }
                    else { g.FillRectangle(b, i * DIMENSIUNE + 1, j * DIMENSIUNE + 1, DIMENSIUNE - 1, DIMENSIUNE - 1); }
                    
                }
            }

            pictureBox1.Refresh(); 
        }
  */

//Poti  da copy si vezi 