//Denisa Juarranz Berindea

using System.Drawing;
using Coordinates;

namespace Practica2
{
    internal class Tablero
    {
        // contenido de las casillas
        enum Casilla { Libre, Muro, Comida, Vitamina, MuroCelda };

        // matriz de casillas (tablero)
        Casilla[,] cas;

        // representacion de los personajes (pacman y fantasmas)
        struct Personaje
        {
            public Coor pos, dir, // posicion y direccion actual
            ini; // posicion inicial (para fantasmas)
        }

        // vector de personajes, 0 es pacman y el resto fantasmas
        Personaje[] pers;

        // colores para los personajes
        ConsoleColor[] colors = {ConsoleColor.DarkYellow, ConsoleColor.Red,
        ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.DarkBlue };

        const int lapCarcelFantasmas = 3000; // retardo para quitar el muro a los fantasmas
        int lapFantasmas; // tiempo restante para quitar el muro
        int numComida; // numero de casillas restantes con comida o vitamina

        Random rnd; // generador de aleatorios
                    
        private bool DEBUG = true; // flag para mensajes de depuracion en consola

        //Constructora - Excepción archivo nulo
        //Excepción de diferente número de columnas
        public Tablero(string file)
        {
            StreamReader nivel = new StreamReader(file);

            //Dimensiones del tablero
            int nFils = 0;
            int nCols = 0;

            //Primera lectura
            //Leemos hasta el final del archivo
            while (!nivel.EndOfStream) 
            {
                string fila = nivel.ReadLine().Split(' ', StringSplitOptions options = System.StringSplitOptions.None) ;
                //Si hay texto, sumamos filas
                if (fila != "")
                {
                    nFils++;
                }
                //El numero de columnas es a longitud de cada fila
                nCols = fila.Length;
            }

            //Segunda lectura
            //Seteamos el tamaño del tablero (a lo mejor hay que revertirlas)
            cas = new Casilla[nCols, nFils];
            //Recorremos el tablero para rellenarlo
            for (int i = 0; i < nFils; i++)
            {
                for (int j = 0; j < nCols; j++)
                {

                }
            }


            nivel.Close();
        }




        static void Main()
        {

        }
    }
}
