using System;

using System.Collections.Generic;

namespace GiustoPrezzoApp
{
    public class Categoria
    {
        public int CategoriaID { get; set; }

        public string Descrizione { get; set; }

        public override string ToString() => $"{CategoriaID}\t{Descrizione}";
    }
}
