using SetArray;
using System.Drawing;

namespace Coordinates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool Exit = false;
            //Console.WriteLine("¿De que tamaño quiere el conjunto?");
            int tam = 5;
            //int.Parse(Console.ReadLine());
            SetCoor coors = new SetCoor(tam);

            while (!Exit)
            {
                //Decides que hacer
                Console.WriteLine("¿Qué quiere hacer?\n" +
                    "[1] Add\n" +
                    "[2] Remove\n" +
                    "[3] Pop\n" +
                    "[4] Size\n" +
                    "[5] isElement\n" +
                    "[6] Exit");

                int decision = int.Parse(Console.ReadLine());
                switch (decision)
                {
                    case 1:
                        try
                        {
                            Coor b = new Coor(3, 4);
                            Coor q = new Coor(1, 3);
                            Coor w = new Coor(2, 5);
                            Coor e = new Coor(3, 7);
                            Console.WriteLine("Añadimos la coordenada " + b.ToString());
                            Console.WriteLine("Añadimos la coordenada " + q.ToString());
                            Console.WriteLine("Añadimos la coordenada " + w.ToString());
                            Console.WriteLine("Añadimos la coordenada " + e.ToString());
                            coors.Add(b);
                            coors.Add(q);
                            coors.Add(w);
                            coors.Add(e);
                        }
                        catch { Console.WriteLine("El conjunto está lleno"); }

                        break;
                    case 2:
                        Coor c = new Coor(3, 4);
                        Console.WriteLine("Eliminamos la coordenada " + c.ToString());
                        coors.Remove(c);
                        break;
                    case 3:
                        //Coor d = new Coor(3, 4);
                        //Console.WriteLine("pop la coordenada " + d.ToString());
                        try
                        {
                            coors.PopElem();
                        }
                        catch { Console.WriteLine("El conjunto está vacío"); }

                        break;
                    case 4:
                        Console.WriteLine("El tamaño es " + coors.Size());
                        break;
                    case 5:
                        Coor f = new Coor(3, 4);
                        Console.WriteLine("comprobamos si está " + f.ToString());
                        Console.WriteLine(coors.IsElementOf(f));
                        break;
                    case 6:
                        Exit = true;
                        break;

                }

                //Despues de la decision escribimos el conjunto 
                Console.WriteLine("El conjunto es: " + coors.ToString());
            }

        }
    }
}