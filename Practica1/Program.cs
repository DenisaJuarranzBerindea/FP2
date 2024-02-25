//Denisa Juarranz Berindea

using System.Reflection.Metadata;

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
            Coor ori;
            ori.x = -1; //Si no hay rectangulo
            ori.y = 0;
            Coor act;
            act.x = 0;
            act.y = 0;

            ////Testing con nivel 000
            //LeeNivel("puzzles/000.txt", out tablero);

            //Debug(tablero, DEBUG, ori, act);
            //Render(tablero, act, ori);
            //char c = ' ';

            //while (c != 'q')
            //{
            //    c = LeeInput();
            //    ProcesaInput(c, ref tablero, ref act, ref ori);
            //    Render(tablero, act, ori);
            //}

            Console.WriteLine("Inserte un nivel para jugar");
            //Leemos el nivel de teclado
            Console.WriteLine("Nivel: XXX");
            string nivel = Console.ReadLine();
            while (JuegaNivel("puzzles/" + nivel + ".txt"))
            {
                Console.Clear();
                Console.WriteLine("Bien hecho :)");
                Console.WriteLine("Inserte un nivel para jugar");
                //Leemos el nivel de teclado
                Console.WriteLine("Nivel: XXX");
                nivel = Console.ReadLine();
            }

            //Si se aborta la partida (LeeInput() == 'q')
            if (!JuegaNivel("puzzles/" + nivel + ".txt"))
            {
                Console.WriteLine("Gracias por jugar");
            }


        }

        #region Render Nivel

        static void LeeNivel(string file, out Tablero tab)
        {
            //Abrimos flujo de lectura
            StreamReader nivel = new StreamReader(file);

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
            for (int j = 0; j < tab.rects.Length; j++)
            {
                tab.rects[j].lt.x = -1;
                tab.rects[j].lt.y = -1;
                tab.rects[j].rb.x = -1;
                tab.rects[j].rb.x = -1;
            }

            //Inicialmente no hay ningún rectángulo
            tab.numRects = 0;

            //Cerramos flujo de lectura
            nivel.Close();
        }

        static Rect NormalizaRect(Coor c1, Coor c2)
        {
            Rect rect;
            //Daremos por hecho que está normalizado, y luego lo modificaremos si es necesario
            rect.lt = c1;
            rect.rb = c2;

            if (c1.x <= c2.x)
            {
                if (c1.y <= c2.y)
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
                if (c1.y <= c2.y)
                {
                    rect.lt.x = c2.x;
                    rect.lt.y = c1.y;

                    rect.rb.x = c1.x;
                    rect.rb.y = c2.y;
                }
                else
                {
                    rect.lt = c2;
                    rect.rb = c1;
                }
            }

            return rect;
        }

        static void Render(Tablero tab, Coor act, Coor ori)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;

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

            //Rectángulos en curso
            if (ori.x != -1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                RenderRect(NormalizaRect(ori, act));
            }
            Console.ForegroundColor = ConsoleColor.White;

            //Rectángulos ya marcados
            for (int i = 0; i < tab.rects.Length; i++)
            {
                //Evitamos que pinte los rectángulos del array vacío
                if (tab.rects[i].lt.x >= 0 && tab.rects[i].lt.y >= 0)
                {
                    RenderRect(tab.rects[i]);
                }
            }

            //Información de Debug
            Console.SetCursorPosition(0, tab.fils * 2 + 2);
            Debug(tab, DEBUG, ori, act);

            //Cursor en posición act
            Console.SetCursorPosition(act.x + 2, act.y + 1);

        }

        //Método Auxiliar para dibujar rectángulos
        static void RenderRect(Rect r)
        {
            //Horizontal
            for (int i = 0; i <= (r.rb.x - r.lt.x); i += 4)
            {
                Console.SetCursorPosition(r.lt.x + i + 1, r.lt.y);
                Console.Write("---");
                Console.SetCursorPosition(r.lt.x + i + 1, r.rb.y + 2);
                Console.Write("---");
            }

            //Vertical
            for (int i = 0; i <= (r.rb.y - r.lt.y); i += 2)
            {
                Console.SetCursorPosition(r.lt.x, r.lt.y + 1 + i);
                Console.Write("|");
                Console.SetCursorPosition(r.rb.x + 4, r.lt.y + 1 + i);
                Console.Write("|");
            }
        }

        #endregion

        #region Lógica Rectángulos
        static bool Dentro(Coor c, Rect r)
        {
            return c.x >= r.lt.x && c.x <= r.rb.x && c.y >= r.lt.y && c.y <= r.rb.y;
        }

        static bool Intersect(Rect r1, Rect r2)
        {
            bool intersect = false;

            //Recorremos todas las coordenadas del rectángulo 1
            int i = r1.lt.y;
            while (i < r1.rb.y + 1 && !intersect) //filas
            {
                int j = r1.lt.x;
                while (j < r1.rb.x + 1 && !intersect) //columnas
                {
                    Coor coor;
                    coor.x = j;
                    coor.y = i;
                    if (Dentro(coor, r2)) //Comprobamos si esa coordenada está dentro del otro
                    {
                        intersect = true;
                    }

                    j++;
                }

                i++;
            }

            return intersect;
        }

        static void InsertaRect(ref Tablero tab, Coor c1, Coor c2)
        {
            Rect r = NormalizaRect(c1, c2);
            //Comprobamos si el nuevo rectángulo, normalizado, solapa con alguno ya existente
            int i = 0;
            while (i < tab.numRects && !Intersect(r, tab.rects[i]))
            {
                i++;
            }

            //Si no interseca con ninguno, se añade al array de rectángulos
            if (!Intersect(r, tab.rects[i]))
            {
                tab.numRects++;
                tab.rects[tab.numRects - 1] = r;
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
            if (i < tab.rects.Length && Dentro(c, tab.rects[i]))
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
            //Como mínimo será 1
            int area = 1;

            if ((r.rb.x - r.lt.x) == 0 && (r.rb.y - r.lt.y) != 0) { area = (r.rb.y - r.lt.y) + 1; }
            else if ((r.rb.x - r.lt.x) != 0 && (r.rb.y - r.lt.y) == 0) { area = (r.rb.x - r.lt.x) + 1; }
            else if ((r.rb.x - r.lt.x) == 0 && (r.rb.y - r.lt.y) == 0) { area = 1; }
            else if ((r.rb.x - r.lt.x) != 0 && (r.rb.y - r.lt.y) != 0) { area = (r.rb.x - r.lt.x) + 2 * (r.rb.y - r.lt.y)+ 1; }

            return area;
        }

        static bool CheckRect(Rect r, Pilar[] p)
        {
            //Daremos por hecho que inicialmente el rectángulo no es correcto
            bool coincide = false;

            //Busca un pilar dentro del rectángulo
            int pilares = 0; int indice = 0;
            int i = 0;

            r.lt.x = r.lt.x/4;
            r.lt.y = r.lt.y/2;
            r.rb.x = r.rb.x/4;
            r.rb.y = r.rb.y/2;

            while (i < p.Length && pilares < 2)
            {
                //Si está dentro, sumará el número de pilares y almacenaremos su índice
                if (Dentro(p[i].coor, r))
                {
                    indice = i;
                    pilares++;
                }

                i++;
            }

            //Si después de todo, solo hay un pilar
            if (pilares == 1)
            {
                //Comprobamos el área del rectángulo
                if (p[indice].val == AreaRect(r))
                {
                    coincide = true;

                }
            }

            return coincide;
        }

        static bool FinJuego(Tablero tab)
        {
            //Daremos por hecho que inicialmente no se ha terminado
            bool fin = false;

            //Buscamos algún rectángulo que no cumpla con las condiciones
            int i = 0;
            while (i < tab.pils.Length && CheckRect(tab.rects[i], tab.pils))
            {
                i++;
            }

            //Si a llegado al final, es porque todos los rectángulos son correctos
            if (i == tab.pils.Length)
            {
                fin = true;
            }
            //Si algún rectángulo no es correcto, fin se mantiene en false

            return fin;
        }

        static bool JuegaNivel(string file)
        {
            //Daremos por hecho que inicialmente no se ha superado el nivel
            bool superado = false;

            //Inicializamos
            Tablero tablero = new Tablero();
            Coor ori;
            ori.x = -1; //Si no hay rectangulo         
            ori.y = 0;
            Coor act;
            act.x = 0; act.y = 0;

            LeeNivel(file, out tablero);
            Render(tablero, act, ori);
            char c = ' ';

            while (!FinJuego(tablero) && c != 'q')
            {
                c = LeeInput();
                ProcesaInput(c, ref tablero, ref act, ref ori);
                Render(tablero, act, ori);
            }

            if (FinJuego(tablero))
            {
                superado = true;
            }
            else if (LeeInput() == 'q')
            {
                superado = false;
            }

            return superado;
        }
        #endregion

        #region DEBUG
        static void Debug (Tablero tab, bool debug, Coor ori, Coor act)
        {
            if (debug) 
            {
                Console.WriteLine("Act: (" + act.x / 4 + "," + act.y / 2 + ")   Ori: (" + ori.x / 4 + "," + ori.y / 2 + ")");

                //Console.WriteLine("n Rectángulos: " + tab.numRects);

                //Console.WriteLine("Pilares: ");
                //for (int i = 0; i < tab.pils.Length; i++)
                //{
                //    Console.WriteLine("Valor: " + tab.pils[i].val);
                //    Console.WriteLine("Coordenadas: (" + tab.pils[i].coor.x + ", " + tab.pils[i].coor.y + ")");
                //}

                Console.WriteLine("Rects: ");
                for (int i = 0; i < tab.numRects; i++)
                {
                    //Evitamos que pinte los rectángulos del array vacío
                    if (tab.rects[i].lt.x >= 0 && tab.rects[i].lt.y >= 0)
                    {
                        Console.Write("(" + tab.rects[i].lt.x / 4 + ", " + tab.rects[i].lt.y / 2 + ") - (" + tab.rects[i].rb.x / 4 + ", " + tab.rects[i].rb.y / 2 +
                            ")")/* "Correcto: " + CheckRect(tab.rects[i], tab.pils))*/;

                    }
                }
            }
        }
        #endregion

        #region Input

        static void ProcesaInput(char ch, ref Tablero tab, ref Coor act, ref Coor ori)
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
            else if (ch == 'c' && ori.x == -1) //Rectángulos
            {
                if (!EliminaRect(tab, act))
                {
                    ori = act;
                }
            }
            else if (ch == 'c' && ori.x != -1)
            {
                InsertaRect(ref tab, ori, act);
                ori.x = -1;
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
