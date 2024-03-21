using Coordinates;
namespace SetArray;

class SetCoor
{

    // atributos de la clase   
    Coor[] coors; // array con coordenadas
    int oc;        // núm de componentes ocupadas del array = primera pos libre

    public SetCoor(int tam = 10) // constructora
    {
        coors = new Coor[tam];      //Creamos el array con el tamaño correspondiente 
        oc = 0;
    }

    // Privado. Busca elto en el array y devuelve su posición (-1 si no está)
    private int SearchElem(Coor c)
    {
        int i = 0;
        while (i < oc && coors[i] != c) { i++; }

        if (i == oc) { i = -1; }        //Si la coordenada no está en el array se devuelve -1
        return i;
    }

    public bool Add(Coor c) // añadir elto al conjunto
    {
        bool add = true;
        if (SearchElem(c) != -1) { add = false; }   //Si la coordenada está no se hace nada
        else
        {
            if (oc == coors.Length) { throw new Exception("El conjunto está lleno"); }
            else
            {
                coors[oc] = c;
                oc++;
            }
        }

        return add;
    }

    public bool Remove(Coor c) // eliminar elto del cto
    {
        bool remove = true;
        int id = SearchElem(c); //Buscamos el elemento
        if (id == -1) { remove = false; }  //Si el elemento no está devolvemos false
        else
        {
            //Eliminamos la coordenada guardando la última
            oc--;
            coors[id] = coors[oc];
        }
        return remove;
    }

    public Coor PopElem() // extracción de un elto (cualquiera) del cto
    {
        //Comprobamos si el conjunto está vacío
        if (Size() == 0) throw new Exception("El conjunto es vacío");
        else
        {
            oc--;                //La borramos simplemente colocando encima la oc por lo tanto cuando se añada una nueva lo sobreescribirá
            return coors[oc];    //Devolvemos la coordenada
        }
    }

    public int Size() { return oc; }  // numero de eltos del conjunto

    public bool IsElementOf(Coor c) // pertenencia 
    {
        bool i = true;
        if (SearchElem(c) == -1) { i = false; }
        return i;
    }

    public override string ToString() // conversión a string
    {
        //Pasamos los números a string
        string cadena = " ";
        for (int i = 0; i < oc; i++)
        {
            cadena += coors[i].ToString();
        }
        return cadena;
    }

}
