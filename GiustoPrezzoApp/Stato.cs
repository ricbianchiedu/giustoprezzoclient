using System;
namespace GiustoPrezzoApp
{
    public class Stato
    {
        public Stato(int numArt, Categoria catGioco, string url)
        {
            CategoriaScelta = catGioco;
            NumeroArticoli = numArt;
            urlApi = url;
        }  

        public Categoria CategoriaScelta { get; set; }
        public int NumeroArticoli { get; set; }
        public string urlApi { get; set; }
    }
}
