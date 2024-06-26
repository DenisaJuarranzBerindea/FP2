﻿namespace Coordinates
{
    class Coor
    {
        int x, y; // componentes x y de la coordenada

        public Coor(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public int X
        {
            get => x;
            set => x = value;
        }

        public int Y
        {
            get => y;
            set => y = value;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static Coor Parse(string s)
        {
            int ini, fin; // buscamos índices de "(" y de ")"
            (ini, fin) = (s.IndexOf("("), s.IndexOf(")"));
            // nos quedamos con la subcadena entre medias de ambos
            s = s.Substring(ini + 1, fin - ini - 1);
            // dividimos con la ","
            string[] nums = s.Split(",", StringSplitOptions.RemoveEmptyEntries);
            // parseamos los dos enteros y construimos coordenada
            return new Coor(int.Parse(nums[0]), int.Parse(nums[1]));
        }

        // igualdad y desigualdad de coordenadas
        public static bool operator ==(Coor c1, Coor c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coor c1, Coor c2)
        {
            return !(c1 == c2); //c1.x!=c2.x || c1.y!=c2.y;
        }



    }


}