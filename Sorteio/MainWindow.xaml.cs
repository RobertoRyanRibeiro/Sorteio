using Microsoft.Win32;
using Sorteio.Data;
using Sorteio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sorteio
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {

       
        string path = ConfigurationManager.AppSettings["Lista"];
        string music = ConfigurationManager.AppSettings["Music"];
        string load = ConfigurationManager.AppSettings["Load"];

        public MainWindow()
        {
            InitializeComponent();
            //Inserindo Lista
            Loading.Source = new Uri(load,UriKind.RelativeOrAbsolute);
            ListFunc();
        }

        //Listar
        private void ListFunc()
        {   
            NavBar.Children.Clear();
            foreach (var func in DB.Ler(path))
            {
                TextBlock text = new TextBlock();
                text.Text = func.ToString().ToUpper();
                text.FontSize = 15;
                text.FontWeight = FontWeights.UltraLight;
                if (func.Venceu)
                    text.Foreground = new SolidColorBrush(Colors.Green);
                NavBar.Children.Add(text);
            }
        }

        //Random Number
        private int RndNum(List<Funcionario> playable, int max)
        {
            //Random
            var rnd = new Random();
            //Lista de Numeros Validos
            var listaNumeros = new int[max];
            var count = 0;

            foreach (var aux in playable)
            {
                listaNumeros[count] = aux.ID;
                count++;
            }

            int resultado = listaNumeros[rnd.Next(listaNumeros.Count())];
            return resultado;
        }


        //Play Buttom
        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            //Hide the play buttom
            Play.Visibility = Visibility.Hidden;
            Reset.Visibility = Visibility.Hidden;   
            Loading.Visibility = Visibility.Visible;
            Nome.Visibility = Visibility.Collapsed;
            ID.Visibility = Visibility.Hidden;

            //Sound
            MediaPlayer media = new MediaPlayer();
            media.Open(new Uri(music, UriKind.RelativeOrAbsolute));
            media.Play();

            await Task.Delay(23000);
            Loading.Visibility = Visibility.Hidden;
            Play.Visibility = Visibility.Visible;
            Reset.Visibility= Visibility.Visible;
            Nome.Visibility = Visibility.Visible;
            ID.Visibility = Visibility.Visible;



            //Verificando a quantidade de pessoas que sobraram
            var playAble = new List<Funcionario>();
            playAble = DB.Ler(path).FindAll(x => x.Venceu == false);
            //Max
            var max = playAble.Count;
            //Verificando se ainda existe funcionarios para sortear
            if (playAble.Count == 0)
            {
                MessageBox.Show("Todos já foram sorteados.");
                return;
            }
            //Random
            var num = RndNum(playAble, max);
            //Achando o sorteado
            foreach (var func in playAble)
            {
                if (func.ID == num && !func.Venceu)
                {
                    func.Venceu = true;
                    Nome.Text = $"Nome - {func.Nome}";
                    ID.Text = $"ID - {func.ID.ToString()}";
                    DB.Alterar(func, path, func.ID + 1);
                    break;
                }
            }
            ListFunc();
        }

        //Play Reset Settings
        private void Play_Reset(object sender, RoutedEventArgs e)
        {
            foreach (var func in DB.Ler(path))
            {
                DB.Reset(path, func.ID + 1);
            }
            ListFunc();
        }

        //Loop Media
        private void Loading_MediaEnded(object sender, RoutedEventArgs e)
        {
            Loading.Position = TimeSpan.FromMilliseconds(1);
        }
    }
}
