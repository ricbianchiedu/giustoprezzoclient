using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GiustoPrezzoApp
{
    public partial class MainPage : ContentPage
    {
        int nArt = 1;
        string url = "";
        int indovinati = 0;
        int prossimoArticolo = 0;
        bool bottoneCreato = false;
        Random ordinePrezzi = new Random();
        Button btnAvanti = new Button();

        HttpClient client = new HttpClient();
        public Task<List<Articolo>> Articoli = null;
        public List<Articolo> ArticoliList = null;

        public MainPage( Stato s )
        {
            InitializeComponent();

            P1.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btnP1.png");
            P2.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btnP2.png");
            P3.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btnP3.png");
            PF.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.btnAvanti.png");

            url = s.urlApi;
            Articoli = CaricaArticoli(s);
            nArt = s.NumeroArticoli;
            lblCategoria.Text = lblCategoria.Text + s.CategoriaScelta.Descrizione;
            
            Gioco();

        }

        public void Gioco()
        {
            if (PF.IsVisible == true)
                PF.IsVisible = false;

            VisualizzaBottoniPrezzo(true);

            if (prossimoArticolo < nArt)
            {
                VisualizzaArticolo(Articoli);
            }
            else
            {
                PF.IsVisible = false;
                PannelloEsito.BackgroundColor = Color.Red;
                Messaggio.Text = "\nGame Over!" + "\nPrezzi indovinati:\n" + indovinati + " su " + nArt;

                VisualizzaBottoniPrezzo(false);

                DescrizioneArticolo.Text = "";
                Immagine.IsVisible = false;
                ImageButton btnRestart = new ImageButton();
                btnRestart.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.GiocaAncora.png");
                btnRestart.WidthRequest = 80;
                Pannello.Children.Add(btnRestart);
                btnRestart.Clicked += btnRestart_OnClicked;
            }
        }

        private void CheckPrezzo(object sender, EventArgs e)
        {
            ImageButton B = sender as ImageButton;
            string prezzo = "";

            switch (B.ClassId)
            {
                case "P1":
                    prezzo = lblP1.Text;
                    break;
                case "P2":
                    prezzo = lblP2.Text;
                    break;
                case "P3":
                    prezzo = lblP3.Text;
                    break;
                default:
                    prezzo = "0.00";
                    break;
            }

            VisualizzaBottoniPrezzo(false);

            int result = String.Compare(prezzo, FormattaPrezzo(ArticoliList[prossimoArticolo].GiustoPrezzo));
            if (result == 0)
            {
                PannelloEsito.BackgroundColor = Color.Green;
                indovinati++;
                Messaggio.Text = "\nEsatto! " + indovinati + " su " + nArt;
            }
            else
            {
                PannelloEsito.BackgroundColor = Color.Red;
                Messaggio.Text = "\nSbagliato... " + indovinati + " su " + nArt;
            }

            if (!bottoneCreato)
            {
                CreaBottoneAvanti();
            }
            else
            {
                if (prossimoArticolo < nArt)
                    btnAvanti.IsVisible = true;
            } 
        }

        public async void VisualizzaArticolo(Task<List<Articolo>> Articoli)
        {
            int sequenzaPrezzi = ordinePrezzi.Next(1, 4);
            ArticoliList = await Articoli;

            try
            {
                Immagine.Source = ImageSource.FromUri(new Uri(ArticoliList[prossimoArticolo].UrlImmagine));
            }
            catch (Exception ex)
            {
                Immagine.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.Errore.png");
            }

            DescrizioneArticolo.Text = ArticoliList[prossimoArticolo].Descrizione;

            switch (sequenzaPrezzi)
            {
                case 1:
                    lblP1.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].GiustoPrezzo);
                    lblP2.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoUno);
                    lblP3.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoDue);
                    break;
                case 2:
                    lblP1.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoDue);
                    lblP2.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].GiustoPrezzo);
                    lblP3.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoUno);
                    break;
                case 3:
                    lblP1.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoUno);
                    lblP2.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoDue);
                    lblP3.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].GiustoPrezzo);
                    break;
                default:
                    lblP1.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoUno);
                    lblP2.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].PrezzoDue);
                    lblP3.Text = FormattaPrezzo(ArticoliList[prossimoArticolo].GiustoPrezzo);
                    break;
            }

            return;
        }

        private string FormattaPrezzo(float prezzo)
        {
            string tmpPrezzo = prezzo.ToString();
            string[] partiPrezzo = null;
            //Workaround per diverso comportamento deserializzazione che in
            //alcuni casi restituisce un separatore decimale rappresentato da
            //un . e in altri casi da una ,
            if (tmpPrezzo.IndexOf('.') >= 0)
                partiPrezzo = tmpPrezzo.Split('.');
            else
                partiPrezzo = tmpPrezzo.Split(',');

            if (partiPrezzo.Length < 2)
            {
                tmpPrezzo = tmpPrezzo + ".00 €";
            }
            else
            {
                switch (partiPrezzo[1].Length)
                {
                    case 1:
                        //Un solo decimale
                        partiPrezzo[1] = "." + partiPrezzo[1] + "0 €";
                        break;
                    case 2:
                        //Due decimali
                        partiPrezzo[1] = "." + partiPrezzo[1] + " €";
                        break;

                    default:
                        break;
                }
                tmpPrezzo = partiPrezzo[0] + partiPrezzo[1];
            }

            return tmpPrezzo;
        }

        public async Task<List<Articolo>> CaricaArticoli(Stato stato)
        {
            string indirizzoApiConParametri = "";
            string indirizzoApi = url;
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
            //var stringTask = await client.GetStringAsync("https://giustoprezzo.azurewebsites.net/" + nArt.ToString());
            if (stato.CategoriaScelta.CategoriaWebID == 1)
            {
                indirizzoApiConParametri = indirizzoApi + "/" + stato.NumeroArticoli.ToString();
            }   
            else
            {
                indirizzoApiConParametri = indirizzoApi + "/" + stato.NumeroArticoli.ToString() + "/" + stato.CategoriaScelta.CategoriaWebID.ToString();
            }

            string stringTask = "";

            try
            {
                stringTask = await client.GetStringAsync(indirizzoApiConParametri);
            }
            catch (Exception ex)
            {
                App.Current.MainPage = new Errore();
            }

            // Dalla chiamata GET torna (a parte nel caso di parametro uguale a 1) un insieme
            // di articoli, per cui va deserializzato il JSON in una lista. 
            var myDeserializedClass = JsonConvert.DeserializeObject<List<Articolo>>(stringTask);
            int numOggetti = myDeserializedClass.Count;
            if (numOggetti < stato.NumeroArticoli)
            {
                nArt = numOggetti;
                await DisplayAlert("Attenzione", "Nella categoria prescelta sono disponibili solo " + numOggetti + " oggetti.", "OK");
            }

            return (List<Articolo>)myDeserializedClass;
        }

        private void Avanti(object sender, EventArgs e)
        {
            PannelloEsito.BackgroundColor = Color.FromRgb(33, 150, 243);
            Messaggio.Text = "\nIl Giusto Prezzo!";
            prossimoArticolo++;
            Gioco();
        }

        private void CreaBottoneAvanti()
        {
            PF.IsVisible = true;
        }

        private void btnRestart_OnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SplashPage();
        }

        public void VisualizzaBottoniPrezzo(bool onoff)
        {
            PrezzoUno.IsVisible = onoff;
            PrezzoDue.IsVisible = onoff;
            PrezzoTre.IsVisible = onoff;
        }
    }
}