using Population;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace Population
{
    public partial class MainWindow : Window
    {
        private readonly List<Allampolgar> lakossag;
        const int feladatokSzama = 5;

        public MainWindow()
        {
            InitializeComponent();
            lakossag = new List<Allampolgar>();

            using var sr = new StreamReader(@"..\..\..\SRC\population.txt");
            _ = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                lakossag.Add(new Allampolgar(sr.ReadLine()));
            }

            for (int i = 1; i <= feladatokSzama; i++)
            {
                feladatComboBox.Items.Add($"{i}.");
            }

            DataContext = this;
            MegoldasTeljes.ItemsSource = lakossag;
        }

        private void FeladatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MegoldasMondatos.Content = null;
            MegoldasLista.ItemsSource = null;
            MegoldasTeljes.ItemsSource = null;

            var methodName = $"Feladat{feladatComboBox.SelectedIndex + 1}";
            var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(this, null);
        }

        private void Feladat1()
        {
            MegoldasMondatos.Content = lakossag.Where(l => l.CsaladiAllapot == "egyedülálló").Max(l => l.TeljesJovedelem());
        }

        private void Feladat2()
        {
            Random r = new Random();
            var nincsEgeszegBiztositasa = lakossag.Where(l => !l.Egeszsegbiztositas).ToList();

            HashSet<string> _5randomEgeszsegbiztositasNelkuliLakos = new();

            while (!(_5randomEgeszsegbiztositasNelkuliLakos.Count() == 5))
            {
                var randomIndex = nincsEgeszegBiztositasa[r.Next(nincsEgeszegBiztositasa.Count())];
                _5randomEgeszsegbiztositasNelkuliLakos.Add(randomIndex.Megjelenit(false));
            }
            MegoldasLista.ItemsSource = _5randomEgeszsegbiztositasNelkuliLakos;
        }

        private void Feladat3()
        {
            MegoldasTeljes.ItemsSource = lakossag.Where(l => l.CsaladiAllapot == "házas" && l.Nemzetiseg == "skót" && l.TeaFogyasztasNaponta > 5);
        }

        private void Feladat4()
        {
            MegoldasMondatos.Content = lakossag.GroupBy(l => l.PolitikaiNezet).ToDictionary(l => l.Key, l => l.Count()).OrderBy(l => l.Value).Select(l => $"Legkevesebben vallott politikai nézet: {l.Key}, lélekszám: {l.Value}").First();
        }

        private void Feladat5()
        {
            var atlagosFogyasztas = lakossag.Average(x => x.FishAndChipsFogyasztasEvente);
            MegoldasLista.ItemsSource = lakossag.GroupBy(l => l.Megye).ToDictionary(l => l.Key, l => l.Count(x => x.FishAndChipsFogyasztasEvente > atlagosFogyasztas)).OrderByDescending(l => l.Value).Select(l => $"Megyenév: {l.Key}, érték: {l.Value}");
        }

    }
}
