using System;
namespace GiustoPrezzoApp
{
    public class Stato
    {
        public Stato(int numArt, Categoria catGioco)
        {
            CategoriaScelta = catGioco;
            NumeroArticoli = numArt;
        }  

        public Categoria CategoriaScelta { get; set; }
        public int NumeroArticoli { get; set; }
    }
}
