﻿using Newtonsoft.Json;
using StockExchangeApp.Enums;
using StockExchangeApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StockExchangeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string _searchTerm;
        private string _lastUpdateTime;
        private string _errorText;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private ObservableCollection<Stock> _stocks;

        public ObservableCollection<Stock> Stocks
        {
            get => _stocks;
            set
            {
                _stocks = value;
                OnPropertyChanged(nameof(Stocks));
            }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

        public string LastUpdateTime
        {
            get => _lastUpdateTime;
            set
            {
                _lastUpdateTime = value;
                OnPropertyChanged(nameof(LastUpdateTime));
            }
        }

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged(nameof(ErrorText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeData();
            Stocks = new ObservableCollection<Stock>();
            DataContext = this;
        }

        private async void InitializeData()
        {
            _timer.Interval = TimeSpan.FromSeconds(100);
            _timer.Tick += async (s, e) => await UpdateStocksAsync();
            _timer.Start();

            await UpdateStocksAsync().ConfigureAwait(false);
        }

        private async Task UpdateStocksAsync()
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("https://localhost:7011/api/stock");
                var stockData = JsonConvert.DeserializeObject<List<Stock>>(response);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Stocks.Clear();
                    foreach (var stock in stockData)
                    {
                        Stocks.Add(stock);
                    }
                    LastUpdateTime = $"Last Updated: {DateTime.Now}";
                });
            }
            catch
            {
                ErrorText = $"Error fetching stock data from the server. ({DateTime.Now})";
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is string sortTag)
            {
                SortTypeEnum selectedOption = Enum.Parse<SortTypeEnum>(sortTag);
                var sortedStocks = SortStocks(selectedOption);

                Stocks.Clear();
                foreach (var stock in sortedStocks)
                {
                    Stocks.Add(stock);
                }
            }
        }

        private List<Stock> SortStocks(SortTypeEnum sortOption)
        {
            return sortOption switch
            {
                SortTypeEnum.BySymbol => Stocks.OrderBy(s => s.Symbol).ToList(),
                SortTypeEnum.ByPrice => Stocks.OrderByDescending(s => s.CurrentPrice).ToList(),
                SortTypeEnum.ByName => Stocks.OrderBy(s => s.CompanyName).ToList(),
                SortTypeEnum.ByChangePercentage => Stocks.OrderByDescending(s => s.ChangePercentage).ToList(),
                _ => Stocks.ToList(),
            };
        }

        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStocksAsync();
        }
    }
}