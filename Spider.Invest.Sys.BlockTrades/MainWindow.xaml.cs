using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Spider.Invest.Sys.BlockTrades.Facade;
using Spider.Invest.Sys.BlockTrades.Facade.Data;
using Spider.Invest.Sys.BlockTrades.Facade.Entities;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Logic;
using Tweetinvi.Logic.Model.Parameters;

namespace Spider.Invest.Sys.BlockTrades
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BlockTradeTweetHarvester _blockTradeHarvester = BlockTradeTweetHarvester.Instance;
        private string _selectedSymbol = string.Empty;
        private bool _isLoading = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var harvester = Spider.Invest.Sys.BlockTrades.Facade.BlockTradeTweetHarvester.Instance;


            _isLoading = true;

            LoadFavSymbols();

            try
            {

                LoadSettings();
            }
            catch (Exception ex1)
            {

                MessageBox.Show("Error: " + ex1.ToString(), "Error Occured", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            _isLoading = false;

            LoadData();
        }



        private void LoadData()
        {
            if (_isLoading || string.IsNullOrEmpty(_selectedSymbol))
            {
                ShowEmptyData();
            }
            else
            {
                ShowReadData();
            }


        }

        private void ShowReadData()
        {
            
        }

        private void ShowEmptyData()
        {
            DataGridBlockTrades.Items.Clear();


            ImageChart.Stretch = Stretch.Uniform;
            
            var uriSource = new Uri(@"/Spider.Invest.Sys.BlockTrades;component/Images/img_not_found.jpg", UriKind.Relative);
            //imgChart.Source = new BitmapImage(uriSource);


            ImageChart.Source = new BitmapImage(uriSource);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.FromDate = DateTimeFrom.SelectedDate.Value;
            Properties.Settings.Default.ToDate = DateTimeTo.SelectedDate.Value;
            Properties.Settings.Default.Symbol = TextBoxSymbol.Text.Trim();
            Properties.Settings.Default.FavSymbol = (ComboBoxFavSymbols.SelectedItem).ToString();
            Properties.Settings.Default.Save();
        }


        private void LoadFavSymbols()
        {
            ComboBoxFavSymbols.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));

            List<string> favSymbols = new List<string>();

            favSymbols.Add("SPY");
            favSymbols.Add("DIA");
            favSymbols.Add("QQQ");
            favSymbols.Add("IWM");
            favSymbols.Add("EEM");
            favSymbols.Add("GDX");

            favSymbols.Add("FB");
            favSymbols.Add("AAPL");

            favSymbols.Sort();


            foreach (var favSymbol in favSymbols)
            {
                ComboBoxFavSymbols.Items.Add(favSymbol);
            }
        }

        private void LoadSettings()
        {
            DateTimeFrom.SelectedDate = Properties.Settings.Default.FromDate;
            DateTimeTo.SelectedDate = Properties.Settings.Default.ToDate;
            TextBoxSymbol.Text = Properties.Settings.Default.Symbol.Trim();

            string favSymbol = Properties.Settings.Default.FavSymbol;
            if (!string.IsNullOrWhiteSpace(favSymbol))
            {
                ComboBoxFavSymbols.SelectedValue = favSymbol;
                _selectedSymbol = favSymbol;
            }


            if (!string.IsNullOrEmpty(Properties.Settings.Default.Symbol))
                _selectedSymbol = Properties.Settings.Default.Symbol.Trim();

        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
           _blockTradeHarvester.Harvest();

            string symbol = TextBoxSymbol.Text;
            
            if (string.IsNullOrWhiteSpace(symbol))
            symbol= ComboBoxFavSymbols.SelectedItem.ToString();


            DateTime from = DateTimeFrom.SelectedDate.Value.Date;
            DateTime to = DateTimeTo.SelectedDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var entries = new TweetProcessor().GetBlockTradeSummaries(symbol, from, to);
            DataGridBlockTrades.Items.Clear();

            foreach (var blockTradeSummary in entries)
            {

                DataGridBlockTrades.Items.Add(blockTradeSummary);
            }

            var datedEntries = new TweetProcessor().GetBlockTradeSummariesByDate(symbol, from, to);

            DataGridBlockTradesByDate.Items.Clear();

            foreach (var blockTradeSummary in datedEntries)
            {

                DataGridBlockTradesByDate.Items.Add(blockTradeSummary);
            }

        }

       
    }
}
