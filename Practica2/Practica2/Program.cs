//Denisa Juarranz Berindea

using System.Drawing;
using Coordinates;

namespace Practica2
{
    internal class Tablero
    {
        #region Variables y datos
        enum Casilla { Libre, Muro, Comida, Vitamina, MuroCelda }; // Contenido de las casillas
        Casilla[,] cas; // Matriz de casillas (tablero)

        struct Personaje // Representacion de los personajes (pacman y fantasmas)
        {
            public Coor pos, dir, // Posicion y direccion actual
            ini; // Posicion inicial (para fantasmas)
        }
        Personaje[] pers; // Vector de personajes, 0 es pacman y el resto fantasmas

        // Colores para los personajes
        ConsoleColor[] colors = {ConsoleColor.DarkYellow, ConsoleColor.Red,
        ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.DarkBlue };

        const int lapCarcelFantasmas = 3000; // Retardo para quitar el muro a los fantasmas
        int lapFantasmas; // Tiempo restante para quitar el muro
        int numComida; // Numero de casillas restantes con comida o vitamina

        Random rnd; // Generador de aleatorios

        private bool DEBUG = true; // Para mensajes de depuracion en consola

        #endregion

        static void Main()
        {
            Tablero nivel = new Tablero("levels/level00.dat");
            nivel.Render();
        }

        //Constructora 
        public Tablero(string file)
        {
            StreamReader lect1 = null;

            //Dimensiones del tablero
            int nFils = 0;
            int nCols = 0;

            //Primera Lectura
            //Intentamos leer el archivo (y realizar el conteo de filas y columnas)
            try
            {
                lect1 = new StreamReader(file);

                //Leemos hasta el final del archivo
                while (!lect1.EndOfStream)
                {
                    string[] fila = lect1.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    //Si hay texto, sumamos filas
                    if (fila != null)
                    {
                        nFils++;
                        nCols = fila.Length;
                    }
                }

            }
            //En caso de no haber archivo, o este ser incorrecto, lanza excepcion
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            //En todo caso, cerramos la lectura
            finally
            {
                if (lect1 != null) lect1.Close();
            }

            //Segunda lectura
            //Solo se hará si el archivo no es nulo
            if (lect1 != null)
            {
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
                            //int index = int.Parse(fila[j]) - pers.Length;
                            cas[j, i] = Casilla.Libre;
                            //pers[index].ini = new Coor(0, 0);
                            //pers[index].pos = new Coor(0, 0);
                            //pers[index].dir = new Coor(0, 0);
                            ////Posición inicial
                            //pers[index].ini.X = j;
                            //pers[index].ini.Y = i;
                            ////La posición actual ahora mismo es ini
                            //pers[index].pos.X = j;
                            //pers[index].pos.Y = i;
                            ////La dirección depende del personaje
                            //if ((int.Parse(fila[j]) - pers.Length) < 9) //Enemigos
                            //{
                            //    pers[index].dir.X = 1;
                            //    pers[index].dir.Y = 0;
                            //}
                            //else //Pacman
                            //{
                            //    pers[index].dir.X = 0;
                            //    pers[index].dir.Y = 1;
                            //}
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

        }

        public void Render()
        {
            //Tablero
            RenderTablero();


            //Personajes

        }

        private void RenderTablero()
        {
            //Recorremos el tablero 
            for (int i = 0; i < cas.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < cas.GetLength(1) - 1; j++)
                {
                    if (cas[j, i] == Casilla.Libre)
                    {
                        Console.Write("  ");
                    }
                    else if (cas[j, i] == Casilla.Muro)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("  ");
                    }
                    else if (cas[j, i] == Casilla.Comida)
                    {
                        Console.Write("··");
                    }
                    else if (cas[j, i] == Casilla.Vitamina)
                    {
                        Console.Write("**");
                    }
                    else if (cas[j, i] == Casilla.MuroCelda)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write("  ");
                    }


                }
                Console.WriteLine();
            }
        }



    }
}
