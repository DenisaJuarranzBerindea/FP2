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

        static void Main()
        {
            Tablero tablero = new Tablero();
            Coor ori = new Coor();
            ori.x = -1; //Si no hay rectangulo
            Coor act = new Coor();

            ////Leemos el nivel de teclado
            //Console.WriteLine("Nivel: XXX");
            //string nivel = Console.ReadLine();
            //Console.WriteLine();
            //LeeNivel("puzzles/" + nivel + ".txt", out tablero);

            LeeNivel("puzzles/000.txt", out tablero);

            Debug(tablero, DEBUG, ori, act);

            Render(tablero, act, ori);

            while(true)
            {
                ProcesaInput(LeeInput(), tablero, ref act, ori);
                Render(tablero, act, ori);
            }
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

            //Pilares, poniendo el número en el centro de la casilla correspondiente.
            for (int i = 0; i < tab.pils.Length; i++)
            {
                Console.SetCursorPosition(2 + tab.pils[i].coor.x * 4, 1 + tab.pils[i].coor.y * 2);
                Console.Write(tab.pils[i].val);
            }

            //Rectángulos ya marcados
            for (int i = 0; i < tab.rects.Length; i++)
            {
                RenderRect(tab.rects[i]);
            }

            //Rectángulos en curso
            Rect actual = new Rect();
            actual = NormalizaRect(act, ori);
            Console.ForegroundColor = ConsoleColor.Green;
            RenderRect(actual);

            Console.ForegroundColor = ConsoleColor.White;
            //Información de Debug
            Console.SetCursorPosition(0, tab.fils * 2 + 2);
            Debug(tab, DEBUG, ori, act);

            //Cursor en posición act
            Console.SetCursorPosition(act.x + 2, act.y + 1);

        }

        //Método Auxiliar para dibujar rectángulos
        static void RenderRect(Rect r)
        {
            for(int i = 0; i < (r.rb.x - r.lt.x); i++)
            {
                if ((r.rb.y - r.lt.y) % 2 != 0) //Horizontal
                {
                    Console.WriteLine(" ---");
                }
                else //Vertical
                {
                    Console.WriteLine("|");
                }
            }
        }

        #endregion

        #region Lógica Rectángulos
        static bool Dentro(Coor c, Rect r)
        {
            return (c.x >= r.lt.x || c.x <= r.rb.x) && (c.y >= r.lt.y || c.y <= r.rb.y);
        }

        static bool Intersect(Rect r1, Rect r2)
        {
            return (Dentro(r1.lt, r2) && Dentro(r1.rb, r2) && Dentro(r2.lt, r1) && Dentro(r2.rb, r1));
        }

        static void InsertaRect(Tablero tab, Coor c1, Coor c2)
        {
            //Comprobamos si el nuevo rectángulo, normalizado, solapa con alguno ya existente
            for (int i = 0; i < tab.rects.Length; i++)
            {
                //Si no interseca con ninguno, se añade al array de rectángulos
                if (!Intersect(NormalizaRect(c1, c2), tab.rects[i]))
                {
                    tab.rects[tab.numRects] = NormalizaRect(c1, c2);
                    tab.numRects++;
                }
            }
        }

        static bool EliminaRect(Tablero tab, Coor c)
        {
            //Daremos por hecho que al inicio esa coordenada no está en ningún rectángulo
            bool encontrada = false;
            //Buscamos la coordenada entre los rectángulos del tablero
            int i = 0;
            while (i < tab.rects.Length && !Dentro(c, tab.rects[i])) 
            {
                i++;
            }

            if (i == tab.rects.Length)
            {
                encontrada = false;
            }

            //Falta eliminar el rectángulo
            if (Dentro(c, tab.rects[i]))
            {
                //Elimina el rectángulo
                for (int j = i; j < tab.rects.Length - 1; j++)
                {
                    Rect aux = tab.rects[j + 1];
                    tab.rects[j] = aux;
                }
                tab.numRects--;
                //Devuelve true
                encontrada = true;
            }

            return encontrada;
        }
        #endregion

        #region Lógica Victoria

        static int AreaRect(Rect r)
        {
            return (r.rb.x - r.lt.x) * (r.rb.y - r.lt.y);
        }

        #endregion

        #region DEBUG
        static void Debug (Tablero tab, bool debug, Coor ori, Coor act)
        {
            if (debug) 
            {
                Console.WriteLine("Act: (" + act.x / 4 + "," + act.y / 2 + ")   Ori: (" + ori.x + "," + ori.y + ")");

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
                    Console.WriteLine("Coordenadas: ( (" + tab.rects[i].lt.x + ", " + tab.rects[i].lt.y + ") - (" + tab.rects[i].rb.x + ", " + tab.rects[i].rb.y + ") )");
                }

                

            }

        }
        #endregion

        #region Input

        static void ProcesaInput(char ch, Tablero tab, ref Coor act, Coor ori)
        {
            if (ch == 'l' && act.x > 2) //Izquierda
            {
                act.x -= 4;
            }
            else if (ch == 'r' && act.x < (tab.cols * 4) - 4) //Derecha
            {
                act.x += 4;
            }
            else if (ch == 'u' && act.y > 1) //Arriba
            {
                act.y -= 2;
            }
            else if (ch == 'd' && act.y < (tab.fils * 2) - 2) //Abajo
            {
                act.y += 2;
            }
            else if (ch == 'c')
            {
                SeleccionaOperacion(tab, act, ref ori);
            }
        }

        //Método aux que define si hay que eliminar un rectángulo o añadirlo
        static void SeleccionaOperacion(Tablero tab, Coor act, ref Coor ori)
        {
            //Comprobaremos si la coordenada seleccionada está dentro de alguno de los rectángulos ya existentes.
            for (int i = 0; i < tab.rects.Length; i++)
            {
                //Si la encuentra es que hay que borrar ese rectángulo
                if (!EliminaRect(tab, act))
                {
                    //Y si no, es que no hay rectángulo, y lo inserta.
                    InsertaRect(tab, act, ori);
                    ori = act;
                }
            }
        }

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

        #endregion
    }
}
