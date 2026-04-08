using System.Xml.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Globalization;

namespace News_Site.Models.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private Dictionary<string, decimal> _previousRates;
        private static DateTime _lastBtcFetchTime = DateTime.MinValue;
        private static decimal _cachedBtcValue = 0;
        public CurrencyService()
        {
            _httpClient = new HttpClient();
            _previousRates = LoadPreviousRates();
        }

        public Dictionary<string, string> GetCurrencies()
        {
            var currencies = new Dictionary<string, string>();

            //  (TCMB)
            try
            {
                var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
                XDocument doc = XDocument.Load(url);
                string[] tcmbCodes = { "USD", "EUR", "GBP" };

                foreach (var code in tcmbCodes)
                {
                    var rateStr = doc.Descendants("Currency")
                        .FirstOrDefault(x => (string)x.Attribute("Kod") == code)
                        ?.Element("ForexSelling")?.Value ?? "N/A";

                    if (decimal.TryParse(rateStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    {
                        string arrow = "";
                        decimal lastRate = 0;
                        _previousRates.TryGetValue(code, out lastRate);

                        if (rate > lastRate) arrow = " 🔺";
                        else if (rate < lastRate) arrow = " 🔻";

                        _previousRates[code] = rate;
                        currencies[code] = rate.ToString("N2") + "₺" + arrow;
                    }
                    else
                    {
                        currencies[code] = "HATA";
                    }
                }
            }
            catch
            {
                currencies["USD"] = "HATA";
                currencies["EUR"] = "HATA";
                currencies["GBP"] = "HATA";
            }

            // --- Bitcoin (CoinGecko)
            try
            {
                string arrow = "";
                decimal lastBtc = 0;
                _previousRates.TryGetValue("BTC", out lastBtc);

              
                if ((DateTime.Now - _lastBtcFetchTime).TotalSeconds < 30 && _cachedBtcValue > 0)
                {
                    if (_cachedBtcValue > lastBtc) arrow = " 🔺";
                    else if (_cachedBtcValue < lastBtc) arrow = " 🔻";

                    _previousRates["BTC"] = _cachedBtcValue;
                    currencies["BTC"] = _cachedBtcValue.ToString("N2") + "₺" + arrow;
                }
                else
                {
                    var btcJson = _httpClient.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=try").Result;
                    var btcData = JsonDocument.Parse(btcJson);
                    var btcValue = btcData.RootElement.GetProperty("bitcoin").GetProperty("try").GetDecimal();

                    if (btcValue > lastBtc) arrow = " 🔺";
                    else if (btcValue < lastBtc) arrow = " 🔻";

                    _previousRates["BTC"] = btcValue;
                    _cachedBtcValue = btcValue;
                    _lastBtcFetchTime = DateTime.Now;

                    currencies["BTC"] = btcValue.ToString("N2") + "₺" + arrow;
                }
            }
            catch
            {
                currencies["BTC"] = _cachedBtcValue > 0
                    ? _cachedBtcValue.ToString("N2") + "₺" + " ⚠" 
                    : "HATA";
            }

            // --- Diğer sabit veriler
            currencies["Faiz"] = "45.70";
            currencies["BIST100"] = "10800.34";

            SavePreviousRates(_previousRates);
            return currencies;
        }

        private Dictionary<string, decimal> LoadPreviousRates()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "previousRates.json");

                if (!File.Exists(filePath))
                    return new Dictionary<string, decimal>();

                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, decimal>>(json) ?? new();
            }
            catch
            {
                return new Dictionary<string, decimal>();
            }
        }

        private void SavePreviousRates(Dictionary<string, decimal> rates)
        {
            try
            {
                var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, "previousRates.json");
                var json = JsonSerializer.Serialize(rates);
                File.WriteAllText(filePath, json);
            }
            catch
            {
              
            }
        }
    }
}
