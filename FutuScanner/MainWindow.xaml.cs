using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FutuScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VM vm = new VM();
        public double ScalingFactor
        {
            get { return (double)GetValue(ScalingFactorProperty); }
            set { SetValue(ScalingFactorProperty, value); }
        }
        public static readonly DependencyProperty ScalingFactorProperty =
            DependencyProperty.Register("ScalingFactor", typeof(double), typeof(Window));

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.vm;
            ScalingFactor = 1;
        }

        private DataGrid gridControl = null;
        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            gridControl = (DataGrid)this.FindName("output");
            ObservableCollection<Quote> src = (ObservableCollection<Quote>)gridControl.ItemsSource;
        }

        private void OpenList(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                vm.ReadWatchListFromFile(openFileDialog.FileName);
        }

        private void SaveList(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                vm.saveWList(saveFileDialog.FileName);
        }

        private void SaveOutput(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                vm.saveOutput(saveFileDialog.FileName);
        }
        private void Init(object sender, RoutedEventArgs e)
        {
            vm.Init();
        }
        private void Scan(object sender, RoutedEventArgs e)
        {
            vm.Scan();
        }
        public void OnAddSymbol(object sender, RoutedEventArgs e)
        {
            vm.List.Insert(0, new Quote() { Code = vm.SelectedSymbol.Code, Name = vm.SelectedSymbol.Name, Security = vm.SelectedSymbol.Security });
        }

        public void OnDeleteSymbol(object sender, RoutedEventArgs e)
        {
            //vm.List.Remove(x => x.Checked);
            DataGrid gc = (DataGrid)this.FindName("watchListGrid");
            vm.List.Remove(x => gc.SelectedItems.Contains(x));
        }

        private void Cb_OnKeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            //TextBox cb = (TextBox) sender;
            /*
            if ((e.Key >= Key.A && e.Key <= Key.Z)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                string key = KeyEventUtility.GetCharFromKey(e.Key).ToString().Trim();
                if (!string.IsNullOrEmpty(key) && string.IsNullOrEmpty(cb.Text))
                    cb.Text = key;
            }*/

            if ((e.Key >= Key.A && e.Key <= Key.Z)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || e.Key == Key.Decimal
                || e.Key == Key.Delete || e.Key == Key.Back)
            {
                vm.SelectedSymbol = null;
                
                //string ss = cb.Text + key.Substring(key.Length - 1);
                if (cb.Text != String.Empty)
                {
                    vm.LookupSymbols(cb.Text);
                    //if (!cb.IsDropDownOpen && cb.Text.Length > 0)
                    //    cb.IsDropDownOpen = true;
                    if (!cb.IsDropDownOpen && cb.Text.Length > 0)
                        cb.IsDropDownOpen = true;
                }
            }

            if ((e.Key >= Key.A && e.Key <= Key.Z)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                var textBox = (cb.Template.FindName("PART_EditableTextBox", cb) as TextBox);
                if (textBox != null && textBox.Text.Length > 0)
                {
                    textBox.SelectionStart = textBox.Text.Length;
                    textBox.SelectionLength = 0;
                }
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs args)
        {
            base.OnPreviewMouseWheel(args);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                ScalingFactor += ((args.Delta > 0) ? 0.1 : -0.1);
            }
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs args)
        {
            base.OnPreviewMouseDown(args);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (args.MiddleButton == MouseButtonState.Pressed)
                {
                    //RestoreScalingFactor(uiScaleSlider, args);
                }
            }
        }

        void RestoreScalingFactor(object sender, MouseButtonEventArgs args)
        {
            ((Slider)sender).Value = 1.0;
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedSymbol == null && vm.SymbolSearchResult != null && vm.SymbolSearchResult.Count > 0)
                vm.SelectedSymbol = vm.SymbolSearchResult[0];
        }

        
    }

    #region Ticker
    public class Clocking : INotifyPropertyChanged
    {
        public Clocking()
        {
            Timer timer = new Timer();
            timer.Interval = 1000; // 1 second updates
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        public string Now
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Now"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    #endregion
}
