using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FTWrapper;
using Futu.OpenApi.Pb;

namespace FutuScanner
{
    public class Symbol : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Symbol() { }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public Security Security { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string PinYin { set; get; }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this,
                new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return Name;
        }
    }
    public class Ticker
    {
        public Security Security { get; set; }
        public DateTime Time { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
    }
    public class OHLCBar
    {
        public Security Security { get; set; }
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

    }

    public class OutStanding
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Last { get; set; }
        public double Volume { get; set; }
        public DateTime Time { get; set; }
        public String Remark { get; set; }

    }
    public class Quote : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked != value)
                {
                    _Checked = value;
                    OnPropertyChanged("Checked");
                }
            }
        }
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public Security Security { get; set; }

        private double _last;
        public double Last
        {
            get { return _last; }
            set
            {
                if (_last != value)
                {
                    _last = value;
                    OnPropertyChanged("Last");
                }
            }
        }

        private double _volume;
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (_volume != value)
                {
                    _volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private double _pDayHigh;
        public double DayHigh
        {
            get { return _pDayHigh; }
            set
            {
                if (_pDayHigh != value)
                {
                    _pDayHigh = value;
                    OnPropertyChanged("DayHigh");
                }
            }
        }

        private double _LastClose;
        public double LastClose
        {
            get { return _LastClose; }
            set
            {
                if (_LastClose != value)
                {
                    _LastClose = value;
                    OnPropertyChanged("LastClose");
                }
            }
        }

        private double _LastVolume;
        public double LastVolume
        {
            get { return _LastVolume; }
            set
            {
                if (_LastVolume != value)
                {
                    _LastVolume = value;
                    OnPropertyChanged("LastVolume");
                }
            }
        }

        private double _pLastDayHigh;
        public double LastDayHigh
        {
            get { return _pLastDayHigh; }
            set
            {
                if (_pLastDayHigh != value)
                {
                    _pLastDayHigh = value;
                    OnPropertyChanged("LastDayHigh");
                }
            }
        }


        private DateTime _pLastDate;
        public DateTime LastDate
        {
            get { return _pLastDate; }
            set
            {
                if (_pLastDate != value)
                {
                    _pLastDate = value;
                    OnPropertyChanged("LastDate");
                }
            }
        }

        private DateTime _pLastUpdTs;
        public DateTime LastUpdTs
        {
            get { return _pLastUpdTs; }
            set
            {
                if (_pLastUpdTs != value)
                {
                    _pLastUpdTs = value;
                    OnPropertyChanged("LastUpdTs");
                }
            }
        }

        private string _Fulfilled;
        public string Fulfilled
        {
            get { return _Fulfilled; }
            set
            {
                if (_Fulfilled != value)
                {
                    _Fulfilled = value;
                    OnPropertyChanged("Fulfilled");
                }
            }
        }

        private bool _pIsDirty = false;
        public bool IsDirty
        {
            get { return _pIsDirty; }
            set
            {
                if (_pIsDirty != value)
                {
                    _pIsDirty = value;
                    OnPropertyChanged("IsDirty");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class DataSource
    {
        public string ID { get; set; }
        public string Vendor { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
    }

    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private FTClient client = new FTClient("127.0.0.1", 11111);
        public ObservableCollection<Quote> List { get; set; }
        public ObservableCollection<OutStanding> FilteredResult { get; set; }
        //public HashSet<Symbol> AllSymbols { get; set; }

        private HashSet<Symbol> _AllSymbols;
        public HashSet<Symbol> AllSymbols
        {
            get { return _AllSymbols; }
            set
            {
                if (_AllSymbols != value)
                {
                    _AllSymbols = value;
                    OnPropertyChanged("AllSymbols");
                }
            }
        }
        private List<Symbol> _SymbolSearchResult;
        public List<Symbol> SymbolSearchResult
        {
            get { return _SymbolSearchResult; }
            set
            {
                if (_SymbolSearchResult != value)
                {
                    _SymbolSearchResult = value;
                    OnPropertyChanged("SymbolSearchResult");
                }
            }
        }
        //check if watch list has a header 
        private bool _header;
        public bool header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged("header");
                }
            }
        }

        private Symbol _SelectedSymbol;
        public Symbol SelectedSymbol
        {
            get { return _SelectedSymbol; }
            set
            {
                if (_SelectedSymbol != value)
                {
                    _SelectedSymbol = value;
                    OnPropertyChanged("SelectedSymbol");
                }
            }
        }

        private int _pInterval = 5;
        public int Interval
        {
            get { return _pInterval; }
            set
            {
                if (_pInterval != value)
                {
                    _pInterval = value;
                    OnPropertyChanged("Interval");
                }
            }
        }


        private readonly string[] markets = { "HK", "US" };//, "SH", "SZ", "HK_FUTURE"};
        private readonly string[] stock_types = { "STOCK", "ETF" };//, "IDX", "ETF", "WARRANT", "BOND", "DRVT"};

        private Dictionary<Security, ObservableCollection<OHLCBar>> KLines = new Dictionary<Security, ObservableCollection<OHLCBar>>();
        private string dtFormat = "yyyy-MM-dd HH:mm:ss";
        public VM()
        {            
            List = new ObservableCollection<Quote>();
            FilteredResult = new ObservableCollection<OutStanding>();
            AllSymbols = new HashSet<Symbol>();
            //Selected.Add(new Quote() { Code = "cd1", Time = DateTime.Now });
            List.CollectionChanged += List_CollectionChanged;
            //initialization
            Init();
        }

        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (var item in e.NewItems)
                {
                    Security security = ((Quote)item).Security;
                    if (!KLines.ContainsKey(security))
                    {
                        ObservableCollection<OHLCBar> bars = new ObservableCollection<OHLCBar>();
                        bars.CollectionChanged += Bars_CollectionChanged;
                        KLines.Add(security, bars);
                    }
                }                
            }
        }

        private void Bars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<OHLCBar> bars = (ObservableCollection<OHLCBar>)sender;
            if (bars.Count > 5)
                bars.RemoveAt(0);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        
        public void LookupSymbols(string str)
        {
            var result = AllSymbols.Where(x => x.Code.ToUpper().Contains(str.ToUpper())
                                               || x.PinYin.Contains(str.ToUpper())
                                               || x.Name.ToUpper().Contains(str.ToUpper()));
            SymbolSearchResult = new List<Symbol>(result);
        }

        private List<Security> plates = new List<Security> { new Security { Market = Futu.OpenApi.Pb.QotCommon.QotMarket.QotMarket_HK_Security, Code = "BK1910" } };
        public async Task<bool> Request_all_symbols()
        {
            try
            {
                foreach (var plate in plates)
                {
                    List<Contract> contracts = await client.RequestSymbols(plate);
                    FillInSymbols(contracts);                    
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
                        
        }

        private void FillInSymbols(List<Contract> contracts)
        {
            HashSet<Symbol> tmp = new HashSet<Symbol>();
            foreach (var contract in contracts)
            {
                tmp.Add(new Symbol()
                {
                    Security = contract.Security,
                    Code = contract.Security.Code,
                    Name = contract.Name,
                    PinYin = PinYin.GetSpellCode(contract.Name)
                });
            }

            AllSymbols = tmp;
        }

        public async void Init()
        {
            try
            {
                await client.ConnectAsync();
                await Request_all_symbols();
                client.QotCallback.RequestHistoryKL += QotCallback_RequestHistoryKL;
                client.QotCallback.UpdateTicker += QotCallback_UpdateTicker;
                client.QotCallback.UpdateKL += QotCallback_UpdateKL;
                client.QotCallback.Subscription += QotCallback_Subscription;
            }
            catch (Exception e)
            {
                throw e;    
            }            
        }

        private void QotCallback_UpdateKL(object sender, FTWrapper.Events.UpdateKLEventArgs e)
        {
            //Console.WriteLine("KL update {0}", e.SerialNo);
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.KlListList)
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
                        if (quote != null)
                        {
                            ObservableCollection<OHLCBar> bars = KLines[quote.Security];
                            // put the latest date into queue
                            AddKLToKL(bars, item);

                            OHLCBar bar = bars.LastOrDefault();

                            quote.LastUpdTs = DateTime.Now;
                            quote.Time = d;
                            quote.Last = bar.Close;
                            quote.Volume = bar.Volume;
                            if (quote.DayHigh < bar.High)
                                quote.DayHigh = bar.High;

                            bool cb = false; //close break
                            bool vb = false; // volume break


                            int barNum = 5;
                            if (bar != null && bars.Count == barNum)
                            {
                                if (quote.Last >= GetHighest(bars.Select(x => x.High).ToList().GetRange(0, barNum - 2)))
                                    cb = true;
                                if (bar.Volume >= GetHighest(bars.Select(x => x.Volume).ToList().GetRange(0, barNum - 2)))
                                    vb = true;
                            }

                            if (cb && vb)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    OutStanding tmp = new OutStanding();
                                    tmp.Code = quote.Security.Code;
                                    tmp.Name = quote.Name;
                                    tmp.Last = quote.Last;
                                    tmp.Volume = quote.Volume;
                                    tmp.Time = DateTime.Now;
                                    if (quote.Last > quote.LastDayHigh && quote.LastDayHigh > 0)
                                        tmp.Remark = "Break last dayhigh;";
                                    if (quote.Last > quote.DayHigh && quote.DayHigh > 0)
                                        tmp.Remark = "Break dayhigh;";

                                    if (FilteredResult.Count > 0 &&
                                    tmp.Code == FilteredResult[0].Code &&
                                    tmp.Name == FilteredResult[0].Name)
                                    {
                                        FilteredResult.RemoveAt(0);
                                    }                                    
                                    FilteredResult.Insert(0, tmp);
                                });
                            }
                        }
                    }
                }
            }
        }

        private void QotCallback_Subscription(object sender, FTWrapper.Events.SubscriptionEventArgs e)
        {
            int i = 0;
        }

        private double GetHighest(List<double> queue)
        {
            double result = 0;
            foreach (var item in queue)
            {
                if (item > result)
                    result = item;
            }
            return result;
        }

        private DateTime GetBarTime(DateTime time)
        {
            int day = 0;
            int hour = time.Hour;
            int min = time.Minute;
            if (Interval < 60)
            {
                min = min - min % Interval + (min % Interval > 0 ? Interval : 0);
            }
            else
                min = 0;
            if (min >= 60)
            {
                min = 0;
                hour++;
                if (hour > 23)
                {
                    hour = 0;
                    day++;
                }
            }
            return time.Date.AddDays(day).Add(new TimeSpan(hour, min, 0));
        }

        private void AddTickerToKL(ObservableCollection<OHLCBar> bars, double price, double volume, DateTime time)
        {   
            var bar = bars.FirstOrDefault(x => x.Time == GetBarTime(time));
            if (bar == null)
                bars.Add(new OHLCBar { Time = GetBarTime(time), Open = price, Close = price, High = price, Low = price, Volume = volume });
            else
            {
                if (bar.High < price)
                    bar.High = price;
                if (bar.Low > price)
                    bar.Low = price;
                bar.Close = price;
                bar.Volume += volume;
            }
        }

        private void AddKLToKL(ObservableCollection<OHLCBar> bars, QotCommon.KLine kLine)
        {
            DateTime time = DateTime.ParseExact(kLine.Time, dtFormat, CultureInfo.InvariantCulture);
            var bar = bars.FirstOrDefault(x => x.Time == GetBarTime(time));
            if (bar == null)
                bars.Add(new OHLCBar
                {
                    Time = GetBarTime(time),
                    High = kLine.HighPrice,
                    Low = kLine.LowPrice,
                    Close = kLine.ClosePrice,
                    Open = kLine.OpenPrice,
                    Volume = kLine.Volume
                });
            else
            {
                bar.High = kLine.HighPrice;
                bar.Low = kLine.LowPrice;
                bar.Close = kLine.ClosePrice;
                bar.Open = kLine.OpenPrice;
                bar.Volume = kLine.Volume;
            }
        }

        private void QotCallback_UpdateTicker(object sender, FTWrapper.Events.UpdateTickerEventArgs e)
        {
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.TickerListList)
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
                        if (quote != null)
                        {
                            ObservableCollection<OHLCBar> bars = KLines[quote.Security];
                            // put the latest date into queue
                            AddTickerToKL(bars, item.Price, item.Volume,
                                DateTime.ParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture));

                            OHLCBar bar = bars.LastOrDefault();

                            quote.Time = d;
                            quote.Last = item.Price;
                            quote.Volume = bar.Volume;
                            if (quote.DayHigh < item.Price)
                                quote.DayHigh = item.Price;

                            bool cb = false; //close break
                            bool vb = false; // volume break

                            
                            if (bar != null)
                            {
                                if (quote.Last > GetHighest(bars.Select(x => x.High).ToList()))
                                    cb = true;
                                if (bar.Volume >= GetHighest(bars.Select(x => x.Volume).ToList()))
                                    vb = true;
                            }                            

                            
                            
                        }
                    }
                }
            }
        }

        private void QotCallback_RequestHistoryKL(object sender, FTWrapper.Events.RequestHistoryKLEventArgs e)
        {
            var quote = List.FirstOrDefault(x => (int)x.Security.Market == e.Result.S2C.Security.Market
                            && x.Security.Code == e.Result.S2C.Security.Code);
            if (quote == null) return;
                        
            if (e.Result.RetType == (int)Common.RetType.RetType_Succeed)
            {
                foreach (var item in e.Result.S2C.KlListList.OrderByDescending(x => DateTime.ParseExact(x.Time, dtFormat, CultureInfo.InvariantCulture)))
                {
                    DateTime d;
                    if (DateTime.TryParseExact(item.Time, dtFormat, CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out d))
                    {
                        if (d.Date < DateTime.Now.Date)
                        {
                            quote.LastClose = item.ClosePrice;
                            quote.LastVolume = item.Volume;
                            quote.LastDate = d.Date;
                            quote.LastDayHigh = item.HighPrice;
                            break;
                        }
                        if (d.Date == DateTime.Now.Date)
                        {
                            quote.Last = item.ClosePrice;
                            quote.DayHigh = item.HighPrice;
                        }
                    }
                }
            }
        }

        

        public async void Scan()
        {
            try
            {
                foreach (var item in List)
                {
                    if (item.LastDate != DateTime.Now.Date || item.IsDirty)
                    {

                        ObservableCollection<OHLCBar> bars = KLines[item.Security];
                        // request history data
                        client.RequestHistoryData(item.Security, DateTime.Now.AddDays(-5), DateTime.Now, QotCommon.KLType.KLType_Day);
                        // request latest KLines
                        List<QotCommon.KLine> kLines = await client.GetKL(item.Security, FTUtil.IntToKLType(Interval));
                        var quote = List.FirstOrDefault(x => item.Security == x.Security);                       
                        foreach (var line in kLines)
                        {                            
                            DateTime time = DateTime.ParseExact(line.Time, dtFormat, CultureInfo.InvariantCulture);
                            if (time > GetBarTime(DateTime.Now))
                                break;

                            if (time.Date == DateTime.Now.Date)
                            {
                                bars.Add(new OHLCBar
                                {
                                    Security = item.Security,
                                    Open = line.OpenPrice,
                                    High = line.HighPrice,
                                    Low = line.LowPrice,
                                    Close = line.ClosePrice,
                                    Volume = line.Volume,
                                    Time = GetBarTime(time)
                                });
                                if (quote.DayHigh < line.HighPrice)
                                    quote.DayHigh = line.HighPrice;
                            }
                            
                        }
                        //client.RequestMarketData(item.Security, FTUtil.IntToSubType(Interval));
                        //client.RequestMarketData(item.Security, QotCommon.SubType.SubType_Ticker);
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public void ReadWatchListFromFile(string path)
        {
            var lines = File.ReadAllLines(path);
            char[] charToTrim = { ' ', '\t', '\r', '\n' };
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim(charToTrim) != String.Empty)
                {
                    string code = lines[i].Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                    string market = lines[i].Split(" \t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                    if (i == 0 && header)
                        continue;

                    int j;
                    Security security = new Security();
                    if (Int32.TryParse(market, out j))
                    {
                        security.Market = (QotCommon.QotMarket)j;
                        security.Code = code;
                        List.Add(new Quote() { Code = code, Security = security, Name = AllSymbols.Where(x => x.Code == code).Select(s => s.Name).DefaultIfEmpty("").First() });
                    }                    
                }
            }
        }

        public void saveWList(string path)
        {
            using (StreamWriter writetext = new StreamWriter(path))
            {
                foreach (Quote q in List)
                {
                    writetext.WriteLine(string.Join("\t", new string[] { ((int)q.Security.Market).ToString(), q.Code }));
                }
            }
        }
        public void saveOutput(string path)
        {
            PropertyInfo[] properties = typeof(Quote).GetProperties();
            using (StreamWriter writetext = new StreamWriter(path))
            {
                StringBuilder sb = new StringBuilder();
                // write header
                foreach (PropertyInfo pi in properties)
                {
                    sb.Append(pi.Name).Append(",");
                }
                writetext.WriteLine(sb);
                sb.Clear();
                // write content
                foreach (var q in FilteredResult)
                {
                    foreach (PropertyInfo pi in properties)
                    {
                        sb.Append(pi.GetValue(q)).Append(",");
                    }
                    writetext.WriteLine(sb);
                    sb.Clear();
                }
            }
        }
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //當新增物件到集合時，做一些初始化
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Quote item in e.NewItems)
                {
                    item.PropertyChanged += (x, y) =>
                    {
                        //Do Something
                    };
                }
            }
        }

    }
}
