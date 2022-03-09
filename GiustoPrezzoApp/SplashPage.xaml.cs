using System;
using System.Collections.Generic;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Forms;

namespace GiustoPrezzoApp
{

    public partial class SplashPage : ContentPage
    {
        HttpClient client = new HttpClient();
        public Task<List<Categoria>> Categorie = null;
        public List<Categoria> CategorieList = null;
        string urlText = "";

        public SplashPage()
        {
            InitializeComponent();

            Logo.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.GiustoPrezzo.png");
            btn1.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btn1.png");
            btn2.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btn2.png");
            btn3.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btn3.png");
            btn4.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btn4.png");
            btn5.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btn5.png");

            PopolaCategorie();

        }

        public async void PopolaCategorie()
        {
            Categorie = CaricaCategorie();
            CategorieList = await Categorie;
            pkrCategoria.ItemsSource = CategorieList;
            pkrCategoria.ItemDisplayBinding = new Binding("Descrizione");
        }

        public async Task<List<Categoria>> CaricaCategorie()
        {
            //Recupera l'URL della WebApi dal file di configurazione
            //Il file urlApi.txt contiene una sola riga ti testo con l'URL.
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(SplashPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("GiustoPrezzoApp.urlApi.txt");
            string indirizzoWebApi = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                urlText = reader.ReadToEnd();
                indirizzoWebApi = urlText + "/categorie";
            }

            //Azzera gli Header HTTP
            client.DefaultRequestHeaders.Accept.Clear();

            //Configura il client HTTP per accettare risposte in JSON
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //Crea un header User-Agent
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Giusto Prezzo");

            //Invia la richiesta GET avviando un nuovo task. Quando la richiesta è completata,
            //restituisce una stringa.
            //await può essere utilizzato solo all'interno di un metodo async
            //Qui attende il completamento del task
            //var stringTask = await client.GetStringAsync("url_da_utilizzare");
            string stringTask = "";

            try
            {
                stringTask = await client.GetStringAsync(indirizzoWebApi);
            }
            catch (Exception ex)
            {
                App.Current.MainPage = new Errore();
            }

            // Dalla chiamata GET torna (a parte nel caso di parametro uguale a 1) un insieme
            // di articoli, per cui va deserializzato il JSON in una lista. 
            var myDeserializedClass = JsonConvert.DeserializeObject<List<Categoria>>(stringTask);

            return myDeserializedClass;
        }

        void btn_Clicked(System.Object sender, System.EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            Categoria tmpCat = new Categoria();

            int numArt = Convert.ToInt32(btn.ClassId);
            
            if (pkrCategoria.SelectedIndex == -1)
            {
                //Default: tutte le categorie
                tmpCat = CategorieList[0];
            }
            else
            {
                tmpCat = CategorieList[pkrCategoria.SelectedIndex];
            }

            Stato s = new Stato(numArt, tmpCat, urlText);
            App.Current.MainPage = new MainPage(s);
        }
    }
}
