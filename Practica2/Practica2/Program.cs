//Denisa Juarranz Berindea

using System.Diagnostics;
using System.Drawing;
using Coordinates;
using SetArray;

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

        public Coor[] cs; // Posibles nuevas posiciones de fantasmas

        public Coor[] dirs = new Coor[4]; //Direcciones (1,0), (0,1), (-1, 0), (0, -1) 

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

            nivel.InicializaDirecciones();

            int lap = 200; //Retardo
            char c = ' ';

            while (true)
            {
                //Leemos el input del usuario
                LeeInput(ref c);

                //Procesamos el input
                if (c != ' ' && nivel.CambiaDir(c)) c = ' ';

                //Movemos en base al input
                nivel.MuevePacman();

                //IA Fantasmas

                //Renderizamos 
                nivel.Render();

                // retardo
                System.Threading.Thread.Sleep(lap);
            }
        }

        //Constructora 
        private Tablero(string file)
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
                    if (fila[0] != "")
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
                        else if (fila[j] == "2")
                        {
                            cas[j, i] = Casilla.Comida;
                            numComida++;
                        }
                        else if (fila[j] == "3")
                        {
                            cas[j, i] = Casilla.Vitamina;
                            numComida++;
                        }
                        else if (fila[j] == "4") cas[j, i] = Casilla.MuroCelda;
                        //Personajes
                        else if (int.Parse(fila[j]) > 4)
                        {
                            int index = pers.Length - (int.Parse(fila[j]) - pers.Length) - 1;
                            cas[j, i] = Casilla.Libre;
                            pers[index].ini = new Coor(0, 0);
                            pers[index].pos = new Coor(0, 0);
                            pers[index].dir = new Coor(0, 0);
                            //Posición inicial
                            pers[index].ini.X = j * 2;
                            pers[index].ini.Y = i;
                            //La posición actual ahora mismo es ini
                            pers[index].pos.X = j * 2;
                            pers[index].pos.Y = i;
                            //La dirección depende del personaje
                            if (index > 0) //Enemigos
                            {
                                pers[index].dir.X = 1;
                                pers[index].dir.Y = 0;
                            }
                            else //Pacman
                            {
                                pers[0].dir.X = 0;
                                pers[0].dir.Y = 1;
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

        }

        private void Render()
        {
            Console.Clear();

            //Tablero
            RenderTablero();

            //Personajes
            RenderPersonajes();

            Console.SetCursorPosition(0, cas.GetLength(1));
            Console.WriteLine();

            if (DEBUG) Debug();
        }

        private void RenderTablero()
        {
            //Recorremos el tablero 
            for (int i = 0; i < cas.GetLength(1); i++)
            {
                for (int j = 0; j < cas.GetLength(0); j++)
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("**");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (cas[j, i] == Casilla.MuroCelda)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write("  ");
                    }

                    Console.BackgroundColor = ConsoleColor.Black;

                }
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void RenderPersonajes()
        {
            for (int i = 0; i < pers.Length; i++)
            {
                Console.SetCursorPosition(pers[i].pos.X, pers[i].pos.Y);
                Console.BackgroundColor = colors[i];
                if (i == 0) //PacMan
                {
                    if (pers[0].dir.X == 1 && pers[0].dir.Y == 0) //Derecha
                    {
                        Console.Write(">>");
                    }
                    else if (pers[0].dir.X == -1 && pers[0].dir.Y == 0) //Izquierda
                    {
                        Console.Write("<<");
                    }
                    else if (pers[0].dir.X == 0 && pers[0].dir.Y == -1) //Arriba
                    {
                        Console.Write("^^");
                    }
                    else if (pers[0].dir.X == 0 && pers[0].dir.Y == 1) //Abajo
                    {
                        Console.Write("vv");
                    }
                }
                else //Fantasmas
                {
                    Console.Write("00");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void Debug()
        {
            Coor posUsuario = new Coor();
            for (int i = 0; i < pers.Length; i++)
            {
                posUsuario.X = pers[i].pos.X / 2;
                posUsuario.Y = pers[i].pos.Y;

                Console.ForegroundColor = colors[i];
                if (i == 0) //PacMan
                {
                    Console.WriteLine("PacMan: Dirección " + pers[0].dir.ToString() + " Posición " + posUsuario.ToString());
                }
                else //Fantasmas
                { 
                    Console.WriteLine("Fantasma: Dirección " + pers[i].dir.ToString() + " Posición " + posUsuario.ToString());
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Coor nextPos = new Coor();
            Console.Write(Siguiente(pers[0].pos, pers[0].dir, out nextPos));
            nextPos.X = nextPos.X / 2;
            Console.Write(nextPos.ToString());

            Console.WriteLine();

            Console.WriteLine("Número de comida: " + numComida);
            
        }

        #region PacMan

        private bool Siguiente(Coor pos, Coor dir, out Coor newPos)
        {
            newPos = new Coor();
            //Calculamos la nueva posición, teniendo en mente los bordes de la pantalla
            if (pos.X + dir.X >= 0) newPos.X = (pos.X + 2 * dir.X) % (cas.GetLength(0) * 2);
            else newPos.X = (cas.GetLength(0) * 2) - 1;

            if (pos.Y + dir.Y >= 0) newPos.Y = (pos.Y + dir.Y) % cas.GetLength(1);
            else newPos.Y = cas.GetLength(1) - 1;

            //Si no hay muro, podrá seguir
            if (cas[(newPos.X + 1) / 2, newPos.Y] != Casilla.Muro)
            { 
                return true;
            }
            //Si hay muro, no seguirá
            else return false;
        }

        private void MuevePacman()
        {
            Coor newPos = new Coor();
            //Si se puede mover
            if (Siguiente(pers[0].pos, pers[0].dir, out newPos))
            {
                //Se mueve a esa posición
                pers[0].pos.X = newPos.X;
                pers[0].pos.Y = newPos.Y;

                //Si hay comida
                if (cas[newPos.X / 2, newPos.Y] == Casilla.Comida ||
                    cas[newPos.X / 2, newPos.Y] == Casilla.Vitamina)
                {
                    //Se la come
                    numComida--;
                    //Dejando la casilla libre
                    cas[newPos.X / 2, newPos.Y] = Casilla.Libre;
                }
            }
        }

        private bool CambiaDir(char c)
        {
            Coor newPos = new Coor();
            Coor newDir = new Coor();

            if (c == 'l') //Izquierda
            {
                newDir.X = -1;
                newDir.Y = 0;
            }
            else if (c == 'r') //Derecha
            {
                newDir.X = 1;
                newDir.Y = 0;
            }
            else if (c == 'u') //Arriba
            {
                newDir.X = 0;
                newDir.Y = -1;
            }
            else if (c == 'd') //Abajo
            {
                newDir.X = 0;
                newDir.Y = 1;
            }

            //Si es posible el cambio
            if (Siguiente(pers[0].pos, newDir, out newPos))
            {
                pers[0].dir = newDir;
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void LeeInput(ref char dir)
        {
            if (Console.KeyAvailable)
            {
                string tecla = Console.ReadKey(true).Key.ToString();
                switch (tecla)
                {
                    case "LeftArrow": dir = 'l'; break;
                    case "UpArrow": dir = 'u'; break;
                    case "RightArrow": dir = 'r'; break;
                    case "DownArrow": dir = 'd'; break;
                }
            }
            while (Console.KeyAvailable) Console.ReadKey().Key.ToString();
        }

        #endregion

        #region Fantasmas

        private bool HayFantasma(Coor c)
        {
            bool boo = false;

            //Buscamos algún fantasma en esa posición
            int i = 1;
            while (i < pers.Length && !boo)
            {
                if (pers[i].pos == c)
                {
                    boo = true;
                }
                i++;
            }

            return boo;
        }

        //private int PosiblesDirs(int fant, out SetCoor cs)
        //{
        //    int posibles;

            
        //}

        #endregion


        private void InicializaDirecciones()
        {
            //Derecha
            dirs[0].X = 1;
            dirs[0].Y = 0;
            //Abajo
            dirs[0].X = 0;
            dirs[0].Y = 1;
            //Izquierda
            dirs[0].X = -1;
            dirs[0].Y = 0;
            //Arriba
            dirs[0].X = 0;
            dirs[0].Y = -1;
        }
    }
}
