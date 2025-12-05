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
       
        public Point[] spike;
      public   Spike(int DIMENSIUNE)
        {
            spike = new Point[3];
        
            spike[0] = new Point(59*DIMENSIUNE + DIMENSIUNE / 2, 10*DIMENSIUNE);        
            spike[1] = new Point(59*DIMENSIUNE, 10*DIMENSIUNE + DIMENSIUNE); 
            spike[2] = new Point(59*DIMENSIUNE + DIMENSIUNE, 10*DIMENSIUNE + DIMENSIUNE); //Logica la chestia asta e sub clasa asta
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