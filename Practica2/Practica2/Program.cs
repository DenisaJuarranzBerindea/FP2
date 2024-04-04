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
        //Excepción de diferente número de columnas (Con un auxiliar?)
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
                string[] fila = nivel.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                //Si hay texto, sumamos filas
                if (fila != null)
                {
                    nFils++;
                }
                //El numero de columnas es la longitud de cada fila
                nCols = fila.Length;
            }

            //Segunda lectura
            //Seteamos el tamaño del tablero (a lo mejor hay que revertirlas) y el número de personajes
            cas = new Casilla[nCols, nFils];
            pers = new Personaje[5];
            //Recorremos el tablero para rellenarlo
            for (int i = 0; i < nFils; i++)
            {
                string[] fila = nivel.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < nCols; j++)
                {
                    //Tablero
                    if (fila[j] == "0") cas[j, i] = Casilla.Libre;
                    else if (fila[j] == "1") cas[j, i] = Casilla.Muro;
                    else if (fila[j] == "2") cas[j, i] = Casilla.Comida;
                    else if (fila[j] == "3") cas[j, i] = Casilla.Vitamina;
                    else if (fila[j] == "4") cas[j, i] = Casilla.MuroCelda;

                    //Personajes
                    else if (int.Parse(fila[j]) >= 5 && int.Parse(fila[j]) <= 8)
                    else if (fila[j] == "9") pers[0].ini.X = j;
                }
            }

            //Inicializamos la cuenta regresiva
            lapFantasmas = lapCarcelFantasmas;


            nivel.Close();
        }




        static void Main()
        {

        }
    }
}
