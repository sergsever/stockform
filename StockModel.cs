using Binance.Net.Clients;
using Binance.Net.Clients.GeneralApi;
using Binance.Net.Enums;
using Bitget.Net.Clients;
using Bitget.Net.Clients.SpotApi;
using Bitget.Net.Interfaces.Clients;
using Bitget.Net.Objects;
using Bybit.Net.Clients;
using Bybit.Net.Interfaces.Clients;
using CryptoExchange.Net.Interfaces.CommonClients;
using Kucoin.Net.Clients;
using Kucoin.Net.Clients.SpotApi;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stockform
{
    public class Stock
    {
        public string? Name { get; set; }
        public float Price { get; set; }
    }

    public class StockModel
    {
        public StockModel()
        {
			bool value = false;
            WebSocks = Boolean.TryParse(ConfigurationManager.AppSettings["WebSocks"], out value);
			if (WebSocks == true)
			{
				WebSocks = value;
			}
			else
				WebSocks = false;
        }

		private bool WebSocks { get; set; } = false;
		public async Task<float> GetRateByStockName(string name)
		{
			Debug.WriteLine("stock: " + name);
			switch (name)
			{
				case "Bybit":
					return await GetBybitRate();
				case "Bitget":
					return await GetBitgetRate();
				case "Binance":
					return await GetBinanceRate();
				case "Kucoin":
					return await GetKucoinRate();
				default:
					throw new NotImplementedException();
			}
		}


		public async Task<float> GetBybitRate()
        {
            float rate = 0;
            string? key = ConfigurationManager.AppSettings["BybitApiKey"];
            string? secret = ConfigurationManager.AppSettings["BybitSKey"];
                        
            var client = new BybitRestClient(options =>
            {
                options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(key, secret);

            });
            var price = await client.SpotApiV3.ExchangeData.GetTickersAsync();  // .SetApiCredentials InverseFuturesApi.ExchangeData.GetTradeHistoryAsync("BTCUSDT"); //.UsdPerpetualApi.Account.GetPositionAsync("BTCUSDT");
			rate = await GetPriceAsync((ISpotClient)client.SpotApiV3);
			return rate;
        }

		private async Task<float> GetPriceAsync(ISpotClient client)
		{
			float price = 0;
			var tickers = await client.GetTickersAsync();
			price = (float)tickers.Data.Where(e => e.Symbol == "BTCUSDT" | e.Symbol == "BTCUSDT_UMCBL" | e.Symbol == "BTC-USDT").First().LastPrice;
			return price;

		}

    
        public async Task<float> GetBitgetRate()
        {
            float rate = 0;
            string key = ConfigurationManager.AppSettings["BitgetApiKey"];
            string secret = ConfigurationManager.AppSettings["BitgetSKey"];
            string phrase = ConfigurationManager.AppSettings["BitgetP"];

             var client = new BitgetRestClient(options =>
             {
                options.ApiCredentials = new BitgetApiCredentials(key, secret, phrase);
             });
			rate = await GetPriceAsync((ISpotClient)client.SpotApi); 
            return rate;
        }

        public async Task<float> GetBinanceRate()
        {
            float rate = 0;

            var client = new BinanceRestClient(Options =>
            {
				/* Ключи для piblic API в документации. */
                Options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A", "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j");

            });


            rate = await GetPriceAsync((ISpotClient)client.SpotApi); 
			return rate;
        }

        public async Task<float> GetKucoinRate()
        {
            float rate = 0;
            var client = new KucoinRestClient(options =>
            {
                string key = ConfigurationManager.AppSettings["kucoinKey"];
                string secret = ConfigurationManager.AppSettings["kucoinS"];
                options.ApiCredentials = new Kucoin.Net.Objects.KucoinApiCredentials(key, secret, "233334");
            });
			//var price = await client.SpotApi.ExchangeData.GetFiatPricesAsync(); // GetFiatPricesAsync("BTCUSDT"); 
			rate = await GetPriceAsync((ISpotClient)client.SpotApi); //(float)price.Data["BTC"];
            return rate;
        }
    }
    }
    

