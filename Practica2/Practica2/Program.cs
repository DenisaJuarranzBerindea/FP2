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

        static void Main()
        {
            Tablero nivel = new Tablero("levels/level00.dat");
        }

        //Constructora -
        //Excepción archivo nulo
        //Excepción de diferente número de columnas (Con un auxiliar?)
        public Tablero(string file)
        {
            StreamReader lect1 = new StreamReader(file);

            //Dimensiones del tablero
            int nFils = 0;
            int nCols = 0;

            //Primera lectura
            //Leemos hasta el final del archivo
            while (!lect1.EndOfStream) 
            {
                string[] fila = lect1.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                //Si hay texto, sumamos filas
                if (fila != null)
                {
                    nFils++;
                }
                //El numero de columnas es la longitud de cada fila
                nCols = fila.Length;
            }

            lect1.Close();

            //Segunda lectura
            StreamReader lect2 = new StreamReader(file);

            //Seteamos el tamaño del tablero (a lo mejor hay que revertirlas) y el número de personajes
            cas = new Casilla[nCols, nFils];
            pers = new Personaje[5];
            //Recorremos el tablero para rellenarlo
            for (int i = 0; i < nFils; i++)
            {
                string[] fila = lect2.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < nCols; j++)
                {
                    //Tablero
                    if (fila[j] == "0") cas[j, i] = Casilla.Libre;
                    else if (fila[j] == "1") cas[j, i] = Casilla.Muro;
                    else if (fila[j] == "2") cas[j, i] = Casilla.Comida;
                    else if (fila[j] == "3") cas[j, i] = Casilla.Vitamina;
                    else if (fila[j] == "4") cas[j, i] = Casilla.MuroCelda;

                    //Personajes
                    else if (int.Parse(fila[j]) > 4)
                    {
                        pers[int.Parse(fila[j]) - pers.Length].ini = new Coor(0, 0);
                        pers[int.Parse(fila[j]) - pers.Length].pos = new Coor(0, 0);
                        pers[int.Parse(fila[j]) - pers.Length].dir = new Coor(0, 0);
                        //Posición inicial
                        pers[int.Parse(fila[j]) - pers.Length].ini.X = j;
                        pers[int.Parse(fila[j]) - pers.Length].ini.Y = i;
                        //La posición actual ahora mismo es ini
                        pers[int.Parse(fila[j]) - pers.Length].pos = pers[pers.Length - int.Parse(fila[j])].ini;
                        //La dirección depende del personaje
                        if ((int.Parse(fila[j]) - pers.Length) < 9) //Enemigos
                        {
                            pers[int.Parse(fila[j]) - pers.Length].dir.X = 1;
                            pers[int.Parse(fila[j]) - pers.Length].dir.Y = 0;
                        }
                        else //Pacman
                        {
                            pers[int.Parse(fila[j]) - pers.Length].dir.X = 0;
                            pers[int.Parse(fila[j]) - pers.Length].dir.Y = 1;
                        }
                    }
                }
            }

            //Inicializamos la cuenta regresiva
            lapFantasmas = lapCarcelFantasmas; 
            //¿Hay que hacer la cuenta regresiva aquí? ¿No sería en el Main()?

            //Inicializamos el random
            if (DEBUG) rnd = new Random(100);
            else rnd = new Random();

            lect2.Close();
        }


        public void Render()
        {
            //

        }




    }
}
