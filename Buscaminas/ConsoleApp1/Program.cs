//Denisa Juarranz Berindea

using Coordinates;
using SetArray;
using System.Net.Http.Headers;

namespace Program
{
    //internal class Program
    //{
    //    static string[] ops = { "Quit", "Add", "Remove", "PopElem", "IsElementOf", "Empty", "Size" };
    //    static void Main(string[] args)
    //{
    //    SetCoor s = new SetCoor(6);
    //    s.Add(new Coor(3, 4)); s.Add(new Coor(1, 2)); s.Add(new Coor(2, 5));
    //    bool cont = true;
    //    while (cont)
    //    {
    //        int op = Menu(ops);
    //        if (op == 0) cont = false;
    //        else Test(op, s);
    //    }

    //}

    //static void Test(int op, SetCoor s)
    //{
    //    if (ops[op] == "Add")
    //    {
    //        Console.Write("Elem ");
    //        Coor c = Coor.Parse(Console.ReadLine());
    //        try { s.Add(c); }
    //        catch (Exception e) { Console.WriteLine(e.Message); }
    //    }
    //    else if (ops[op] == "Remove")
    //    {
    //        Console.Write("Elem ");
    //        Coor c = Coor.Parse(Console.ReadLine());
    //        if (s.Remove(c)) Console.Write("Deleted");
    //        else Console.Write("Not found");
    //    }
    //    else if (ops[op] == "PopElem")
    //    {
    //        try
    //        {
    //            Coor c = s.PopElem();
    //            Console.WriteLine($"Extracted {c}");
    //        }
    //        catch (Exception e) { Console.WriteLine(e.Message); }
    //    }
    //    else if (ops[op] == "IsElementOf")
    //    {
    //        Console.Write("Elem ");
    //        Coor c = Coor.Parse(Console.ReadLine());
    //        Console.WriteLine(s.IsElementOf(c));
    //    }
    //    else if (ops[op] == "Empty")
    //    {
    //        Console.WriteLine(s.Empty());
    //    }
    //    else if (ops[op] == "Size")
    //    {
    //        Console.WriteLine(s.Size());
    //    }
    //    Console.WriteLine($"Set: {s}");
    //}

    //static int Menu(string[] ops)
    //{
    //    Console.WriteLine("\nOption: ");
    //    int op;
    //    do
    //    {
    //        for (int i = 0; i < ops.Length; i++)
    //            Console.WriteLine($"{i}. {ops[i]}");
    //        op = int.Parse(Console.ReadLine());
    //    } while (op < 0 || op > ops.Length);
    //    return op;
    //}
    
    public class Program
    {
        const bool DEBUG = true; // para depuración: deja ver posicion de las minas
        static Random rnd = new Random(); // generador de aleatorios para colocar minas
        struct Tablero
        {
            // '1'..'8' indica num de minas alrededor   
            // '·' indica que no hay minas alrededor
            // 'x' marcada como mina, 'o' no destapada, '*' mina destapada
            public char[,] casilla;
            public SetCoor minas;  // Conjunto de (coordenadas de) minas
            public Coor cursor;    // Coordenadas del cursor            
        }


        public static void Main(string[] args)
        {
            // tablero sencillo para depuracion
            Tablero t = Tab();

            Render(t);

            // bucle ppal
            while (true)
            {
                char c = LeeInput();
                ProcesaInput(ref t, c);
                Render(t);
            }
        }

        // tablero de prueba
        static Tablero Tab1()
        {
            Tablero t;
            t.cursor = new Coor(0, 0);
            int fils = 4, cols = 5;
            t.casilla = new char[fils, cols];
            for (int i = 0; i < fils; i++)
                for (int j = 0; j < cols; j++)
                {
                    t.casilla[i, j] = 'o';
                }
            t.minas = new SetCoor(fils * cols / 2);
            t.minas.Add(new Coor(0, 0));
            t.minas.Add(new Coor(0, 2));
            t.minas.Add(new Coor(1, 0));
            t.minas.Add(new Coor(1, 2));
            t.minas.Add(new Coor(2, 0));
            t.minas.Add(new Coor(2, 1));
            return t;
        }

        static Tablero Tab()
        {
            Tablero t;
            t.cursor = new Coor(0, 0);
            int fils = 4, cols = 5;
            t.casilla = new char[fils, cols];
            for (int i = 0; i < fils; i++)
                for (int j = 0; j < cols; j++)
                {
                    t.casilla[i, j] = 'o';
                }
            t.minas = new SetCoor(fils * cols / 2);
            PonMinas(t, 6);
            return t;
        }

        static void Render(Tablero tab)
        {
            Console.Clear();
            for (int i = 0; i < tab.casilla.GetLength(0); i++)
            {
                for (int j = 0; j < tab.casilla.GetLength(1); j++)
                {
                    if (tab.casilla[i, j] == '·')
                    { // no cambiamos color
                    }
                    else if (tab.casilla[i, j] == '1')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (tab.casilla[i, j] == '2')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (tab.casilla[i, j] == '3')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (tab.casilla[i, j] == '4')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (tab.casilla[i, j] == '5')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else if (tab.casilla[i, j] == '6')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (tab.casilla[i, j] == '7')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (tab.casilla[i, j] == '8')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else if (tab.casilla[i, j] == 'x')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (tab.casilla[i, j] == 'o')
                    { // no cambiamos color
                    }
                    else if (tab.casilla[i, j] == '*')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    // Para depuración, marcamos las minas en morado
                    if (DEBUG && tab.minas.IsElementOf(new Coor(i, j)))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    Console.Write($" {tab.casilla[i, j]}");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(tab.cursor.Y * 2 + 1, tab.cursor.X);

        }

        static bool HayMina(Tablero t, int i, int j)
        {
            return t.minas.IsElementOf(new Coor(i, j));
        }

        static int MinasAlrededor(Tablero t, int x, int y)
        {
            int fils = t.casilla.GetLength(0), cols = t.casilla.GetLength(1);
            int cont = 0;
            for (int i = Math.Max(0, x - 1); i <= Math.Min(fils - 1, x + 1); i++)
                for (int j = Math.Max(0, y - 1); j <= Math.Min(cols - 1, y + 1); j++)
                    if (HayMina(t, i, j)) cont++;
            return cont;
        }
        static void DescubreAdyacentes(Tablero t, int x, int y)
        {
            int fils = t.casilla.GetLength(0), cols = t.casilla.GetLength(1);
            SetCoor pendientes = new SetCoor(fils * cols),
                    visitados = new SetCoor(fils * cols);
            pendientes.Add(new Coor(x, y));
            while (!pendientes.Empty())
            {
                Coor c = pendientes.PopElem();
                visitados.Add(c);
                (x, y) = (c.X, c.Y);
                int cont = MinasAlrededor(t, x, y);
                if (cont > 0) t.casilla[x, y] = (char)(((int)'0') + cont);
                else
                { // cont==0
                    t.casilla[x, y] = '·';
                    for (int i = Math.Max(0, x - 1); i <= Math.Min(fils - 1, x + 1); i++)
                        for (int j = Math.Max(0, y - 1); j <= Math.Min(cols - 1, y + 1); j++)
                        {
                            Coor c1 = new Coor(i, j);
                            if (!visitados.IsElementOf(c1)) pendientes.Add(c1);
                        }
                }
            }
        }

        static bool ProcesaInput(ref Tablero t, char c)
        {
            switch (c)
            {
                case 'l':
                    if (t.cursor.Y > 0) t.cursor.Y--;
                    break;
                case 'r':
                    if (t.cursor.Y < t.casilla.GetLength(1) - 1) t.cursor.Y++;
                    break;
                case 'u':
                    if (t.cursor.X > 0) t.cursor.X--;
                    break;
                case 'd':
                    if (t.cursor.X < t.casilla.GetLength(0) - 1) t.cursor.X++;
                    break;
                case 'x':
                    if (t.casilla[t.cursor.X, t.cursor.Y] == 'o')
                        t.casilla[t.cursor.X, t.cursor.Y] = 'x';
                    else if (t.casilla[t.cursor.X, t.cursor.Y] == 'x')
                        t.casilla[t.cursor.X, t.cursor.Y] = 'o';
                    break;
                case 'c':
                    if (HayMina(t, t.cursor.X, t.cursor.Y))
                    {
                        t.casilla[t.cursor.X, t.cursor.Y] = '*';
                        return true;
                    }
                    else if (t.casilla[t.cursor.X, t.cursor.Y] == 'o')
                    {
                        //DescubreAdyacentes(t, t.cursor.X, t.cursor.Y);
                    }
                    break;
            }
            return false;
        }

        static char LeeInput()
        {
            char d = ' ';
            string tecla = Console.ReadKey(true).Key.ToString();
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
                case "Escape":
                    d = 'q';
                    break;
                case "Spacebar":
                    d = 'c';
                    break;
                case "Enter":
                    d = 'x';
                    break;
            }
            while (Console.KeyAvailable)
                Console.ReadKey().Key.ToString();
            return d;
        }

        static void PonMinas (Tablero tab, int nMinas)
        {
            //Colocamos las minas en las posiciones iniciales
            for (int i = 0; i < tab.casilla.GetLength(0) && nMinas > 0; i++)
            {
                for (int j = 0; j < tab.casilla.GetLength(1) && nMinas > 0; j++)
                {
                    tab.minas.Add(new Coor(i, j));
                    nMinas--;
                }
            }

            //Recorremos intercambiando
            for (int i = 0; i < tab.casilla.GetLength(1); i++)
            {
                for (int j = 0; j < tab.casilla.GetLength(0); j++)
                {
                    //En caso de que haya mina
                    if (HayMina(tab, i, j))
                    {
                        //Eliminamos la mina de la posición actual
                        tab.minas.Remove(new Coor(i, j));

                        //Añadimos una mina en una posición aleatoria en la que no haya mina
                        int pos = 0;
                        while (HayMina(tab, pos % tab.casilla.GetLength(1), pos / tab.casilla.GetLength(0)))
                        {
                            pos = rnd.Next(0, tab.casilla.GetLength(0) * tab.casilla.GetLength(1));
                        }
                        tab.minas.Add(new Coor(pos % tab.casilla.GetLength(1), pos / tab.casilla.GetLength(0)));
                    }
                }  
            }

        }


    }
}