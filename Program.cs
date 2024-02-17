//Denisa Juarranz Berindea

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

        //Estado de jueg tiene 2 Coor - act (actual) y ori (origen). Si no hay rectángulo seleccionado, ori.x = -1
        static void Main()
        {
            //Para lee nivel, tenemos que solicitar antes de teclado al usuario que nos diga el nivel...
            Tablero tablero = new Tablero();

            Console.Write("Nivel: XXX");
            int nivel = int.Parse(Console.ReadLine());

            LeeNivel("puzzles/" + nivel + ".txt", out tablero);
            Debug(tablero, DEBUG);

        }

        #region Render Nivel

        static void LeeNivel (string file, out Tablero tab)
        {
            //Abrimos flujo de lectura
            StreamReader nivel = new StreamReader (file);

            //Leemos la dimensión del tablero
            tab.fils = int.Parse(nivel.ReadLine());
            tab.cols = int.Parse(nivel.ReadLine());

            //Crearemos un array auxiliar, de tamaño máximo, que posteriormente, redimensionaremos para convertirlo en tab.pils 
            Pilar[] pilars = new Pilar[tab.fils * tab.cols];
            //Variable para redimensionar
            int tamanoPils = 0;

            //Leemos el resto de filas, dando por hecho que el archivo es correcto
            while (!nivel.EndOfStream) 
            {
                //Como ya sabemos el tamaño del tablero, no será un problema. 
                string[] pilares = nivel.ReadLine().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                //Ahora, solo nos quedaremos con los valores numéricos, donde si que haya un pilar
                for (int i = 0; i < pilares.Length; i++)
                {
                    if (pilares[i] != "-")
                    {
                        tamanoPils++;

                        //Recorremos los pilares del tablero y lo rellenamos
                        for (int j = 0; j < pilars.Length; j++)
                        {
                            //Valor
                            pilars[j].val = int.Parse(pilares[i]);
                            //Coordenadas
                            pilars[j].coor.y = i / tab.fils;
                            pilars[j].coor.x = i % tab.cols;
                        }
                    }
                }
            }

            //Ahora, rellenamos pils en su tamaño real
            tab.pils = new Pilar[tamanoPils];
            for (int i = 0; i < tab.pils.Length; i++) 
            {
                tab.pils[i] = pilars[i];
            }

            //Dimensionamos el array de rectángulos
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





        #region DEBUG
        static void Debug (Tablero tab, bool debug)
        {
            if (debug) 
            {
                Console.WriteLine("Columnas: " + tab.cols);
                Console.WriteLine("Filas: " + tab.fils);
                Console.WriteLine("n Rectángulos: " + tab.numRects);

                Console.WriteLine("Pilares: ");
                for (int i = 0; i < tab.pils.Length; i++)
                {
                    Console.WriteLine("Valor: " + tab.pils[i].val);
                    Console.WriteLine("Coordenadas: (" + tab.pils[i].coor.x + ", " + tab.pils[i].coor.y + ")");
                }

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
