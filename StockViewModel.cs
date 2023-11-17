using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace stockform
{
    public class StockViewModel : INotifyPropertyChanged
    {
		private System.Timers.Timer _timer;
		private StockModel _model;

        public StockViewModel(StockModel model) 
		{
			_model = model;
			_timer = new System.Timers.Timer(5000);
			_timer.Elapsed += OnTimer;
			_timer.AutoReset = true;
			_timer.Enabled = true;
			_timer.Start();
			_stocks.AddRange(new string[4] {
			"Bybit",
			"Bitget",
			"Binance",
			"Kucoin"
			});

		}

		public event PropertyChangedEventHandler? PropertyChanged;

		private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, e);
			}
		}

		private List<string> _stocks = new List<string>();
		private void NotifyListChanged(object sender, ListChangedEventArgs e) 
		{

			NotifyPropertyChanged(this, new PropertyChangedEventArgs("List"));
		}

		public async void Init()
		{
			if (PricesSource != null)
			{
				PricesSource.Clear();
			}
			else
			{
				PricesSource = new BindingList<Stock>();
            }
			foreach(string stockname in _stocks)
			{
				float rate = await _model.GetRateByStockName(stockname);
				Stock stock = new Stock() {Name= stockname + ":\t" + rate.ToString() };
				PricesSource.Add(stock);
			}

			
		}
		public BindingList<Stock> PricesSource { get; set; }
		private  async void OnTimer(Object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Timer " + e);
            Init();
//            new Task(() => { Init(); }).RunSynchronously();
            NotifyListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 1,1));
        }

    }

}
