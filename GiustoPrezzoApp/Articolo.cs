using System;
using System.ComponentModel;

namespace GiustoPrezzoApp
{
	public class Articolo
	{
        public string Descrizione { get; set; }
        public float GiustoPrezzo { get; set; }
        public float PrezzoUno { get; set; }
        public float PrezzoDue { get; set; }
        public string UrlImmagine { get; set; }

        //https://docs.microsoft.com/en-gb/ef/ef6/fundamentals/relationships?redirectedfrom=MSDN

        public override string ToString()
        => $"{Descrizione} {GiustoPrezzo} {PrezzoUno} {PrezzoDue} {UrlImmagine}";
    }
}

