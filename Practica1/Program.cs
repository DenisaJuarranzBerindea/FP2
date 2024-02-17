//Denisa Juarranz Berindea

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace shikaku
{
    class Program
    {
        const bool DEBUG = true;

        struct Coor //Coordenadas en el tablero 
        { 
            public int x, y;
        }

        struct Pilar //Pilar en el tablero
        { 
            public Coor coor;   // posición en el tablero
            public int val;     // valor
        } 

        struct Rect //Rectángulo determinado por dos esquinas
        {  
            public Coor lt, rb; // left-top, right-bottom
        }  

        struct Tablero //Tamaño, pilares, rectángulos marcados
        { 
            public int fils, cols;  // num fils y cols del tablero   
            public Pilar[] pils;    // array de pilares
            public Rect[] rects;    // array de rectángulos
            public int numRects;    // num de rectángulos definidos = prim pos libre en rect
        }

        static void Main()
        {
            Tablero tablero = new Tablero();
            Coor ori = new Coor();
            //ori.x = -1; //Si no hay rectangulo
            Coor act = new Coor();

            ////Leemos el nivel de teclado
            //Console.WriteLine("Nivel: XXX");
            //string nivel = Console.ReadLine();
            //Console.WriteLine();
            //LeeNivel("puzzles/" + nivel + ".txt", out tablero);

            LeeNivel("puzzles/000.txt", out tablero);

            Debug(tablero, DEBUG);

            Render(tablero, act, ori);
        }

        #region Render Nivel

        static void LeeNivel (string file, out Tablero tab)
        {
            //Abrimos flujo de lectura
            StreamReader nivel = new StreamReader (file);

            //Leemos la dimensión del tablero
            tab.fils = int.Parse(nivel.ReadLine());
            tab.cols = int.Parse(nivel.ReadLine());

            //Crearemos un array auxiliar de tamaño máximo, que posteriormente, redimensionaremos para convertirlo en tab.pils 
            Pilar[] pilarAux = new Pilar[tab.fils * tab.cols];
            //Variable para redimensionar
            int tamanoPils = 0;

            //Leemos el resto de filas
            int i = 0; //Fila en la que nos encontramos
            while (!nivel.EndOfStream && i < tab.fils)
            {
                //Leemos línea a línea
                string[] fila = nivel.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                //Recorremos la fila, buscando pilares
                for (int j = 0; j < fila.Length; j++) 
                {
                    //Si hay pilares, aumentaremos el tamaño del array final y rellenamos el auxiliar
                    if (fila[j] != "-")
                    {
                        //Valor
                        pilarAux[tamanoPils].val = int.Parse(fila[j]);
                        //Coordenadas
                        pilarAux[tamanoPils].coor.x = j % tab.cols;
                        pilarAux[tamanoPils].coor.y = i;

                        tamanoPils++;
                    }
                }
                i++;
            }

            //Ahora, rellenamos pils en su tamaño real
            tab.pils = new Pilar[tamanoPils];
            for (int j = 0; j < tab.pils.Length; j++) 
            {
                tab.pils[j] = pilarAux[j];
            }

            //Creamos el array de rectángulos
            tab.rects = new Rect[tab.fils * tab.cols];

            //Inicialmente no hay ningún rectángulo
            tab.numRects = 0;

            //Cerramos flujo de lectura
            nivel.Close();
        }

        static Rect NormalizaRect(Coor c1, Coor c2)
        {
            Rect rect = new Rect();

            if (c1.x < c2.x)
            { 
                if (c1.y > c2.y) 
                {
                    rect.lt = c1;
                    rect.rb = c2;
                }
                else
                {
                    rect.lt.x = c1.x;
                    rect.lt.y = c2.y;

                    rect.rb.x = c2.x;
                    rect.rb.y = c1.y;
                }

            }
            else
            {
                if (c2.y > c1.y)
                {
                    rect.lt = c2;
                    rect.rb = c1;
                }
                else
                {
                    rect.lt.x = c1.x;
                    rect.lt.y = c2.y;

                    rect.rb.x = c2.x;
                    rect.rb.y = c1.y;
                }
            }

            return rect;
        }
        #endregion

        static void Render(Tablero tab, Coor act, Coor ori)
        {
            Console.Clear();

            //Tablero Vacío
            for (int i = 0; i < tab.fils + 1; i++) 
            { 
                for (int j = 0; j < tab.cols + 1; j++) 
                {
                    Console.Write("+   ");
                }
            
                Console.WriteLine();
                Console.WriteLine();
            }

            ////Pilares, poniendo el número en el centro de la casilla correspondiente.
            //Console.SetCursorPosition(2, 1);
            //for (int i = 0; i < tab.fils * 4; i++)
            //{
            //    for (int j = 0; j < tab.cols * 4; j++)
            //    {
            //        Console.SetCursorPosition(2 + j, 1 + i);

            //    }
            //}





            Console.WriteLine();
            Console.WriteLine();
            Debug(tab, DEBUG);

            //Console.SetCursorPosition(act.x, act.y);

        }



        #region DEBUG
        static void Debug (Tablero tab, bool debug)
        {
            if (debug) 
            {
                Console.WriteLine("n Rectángulos: " + tab.numRects);

                //Console.WriteLine("Pilares: ");
                //for (int i = 0; i < tab.pils.Length; i++)
                //{
                //    Console.WriteLine("Valor: " + tab.pils[i].val);
                //    Console.WriteLine("Coordenadas: (" + tab.pils[i].coor.x + ", " + tab.pils[i].coor.y + ")");
                //}

                Console.WriteLine("Rectángulos: ");
                for (int i = 0; i < tab.numRects; i++)
                {
                    Console.WriteLine("Coordenadas: (" + tab.rects[i].lt + ", " + tab.rects[i].rb + ")");
                }


            }

        }
        #endregion

        static char LeeInput()
        {
            char d = ' ';
            while (d == ' ')
            {
                if (Console.KeyAvailable)
                {
                    string tecla = Console.ReadKey().Key.ToString();
                    switch (tecla)
                    {
                        case "LeftArrow":
                            d = 'l';
                            break;
                        case "UpArrow":
                            d = 'u';
                            break;
                        case "RightArrow":
                            d = 'r';
                            break;
                        case "DownArrow":
                            d = 'd';
                            break;
                        case "Spacebar":
                            d = 'c';
                            break;
                        case "Escape":
                        case "Q":
                            d = 'q';
                            break;
                    }
                }
            }
            return d;
        }
    }
}
