using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

namespace Hjp.Fair_Die
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Throws DieThrows;
        ObservableCollection<Result> Results = new ObservableCollection<Result>();

        public MainWindow()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Results;
        }

        

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == true)
            {
                using (var rs = File.OpenText(openFileDialog.FileName)) {
                    DieThrows = new Throws();
                    string line;
                    while ((line = rs.ReadLine()) != null)
                    {
                        int t = int.Parse(line);
                        DieThrows.Add(t);
                    }
                }
                foreach (var item in DieThrows.RankedValues)
                {
                    Results.Add(new Result() { Rank = item.Rank, Value = item.Value, Count = item.Count });
                }

            }
        }

        private void SimulationRun_Click(object sender, RoutedEventArgs e)
        {
            int runs = 10000;
            progressBar.Value = 0;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += RunSimulation;
            worker.ProgressChanged += RunSimulationProgressChanged;
            worker.RunWorkerCompleted += RunSimulationCompleted;
            worker.RunWorkerAsync(runs);
            
        }

        private void RunSimulationCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Value = 100;
            MessageBox.Show("Done");
        }

        private void RunSimulationProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                Result[] currentResults = (Result[])e.UserState;
                for(int i = 0; i < currentResults.Length; i++)
                {
                    Results[i].Min = currentResults[i].Min;
                    Results[i].Pct05 = currentResults[i].Pct05;
                    Results[i].Pct50 = currentResults[i].Pct50;
                    Results[i].Pct95 = currentResults[i].Pct95;
                    Results[i].Max   = currentResults[i].Max;
                }
            }
        }

        private void RunSimulation(object sender, DoWorkEventArgs e)
        {

            int runs = (int)e.Argument;

            List<int>[] rankResults = new List<int>[DieThrows.max];
            for (int i = 0; i < rankResults.Length; i++)
            {
                rankResults[i] = new List<int>();
            }

            Random rng = new Random();
            for (int i = 0; i < runs; i++)
            {
                Console.WriteLine(i);
                int progressPercentage = i * 100 / runs;
                var throws = new Throws() { Rng = rng, max = DieThrows.max };
                for (int j = 0; j < DieThrows.Count; j++)
                {
                    throws.AddRandom();
                }
                var rankedValues = throws.RankedValues;
                var results = new Result[rankedValues.Length];
                for (int j = 0; j < results.Length; j++)
                {
                    results[j] = new Result();
                }
                foreach (var item in rankedValues)
                {
                    var rr = rankResults[item.Rank];
                    rr.Add(item.Count);
                    rr.Sort();
                    var n = rr.Count();
                    results[item.Rank].Min = rr[0];
                    results[item.Rank].Pct05 = rr[(n - 1) * 5 / 100];
                    results[item.Rank].Pct50 = rr[(n - 1) * 50 / 100];
                    results[item.Rank].Pct95 = rr[(n - 1) * 95 / 100];
                    results[item.Rank].Max = rr[n - 1];

                }
                (sender as BackgroundWorker).ReportProgress(progressPercentage, results);
                System.Threading.Thread.Sleep(10);

            }
         
        }
    }

    public class Result
        : INotifyPropertyChanged
    {
        #region Properties
        int _Rank;
        public int Rank {
            get
            {
                return _Rank;
            }
            set
            {
                if (value != _Rank)
                {
                    _Rank = value;
                    NotifyPropertyChanged("Rank");
                } 
            }
        }

        int _Value;
        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        int _Count;
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                if (value != _Count)
                {
                    _Count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        int _Min;
        public int Min
        {
            get
            {
                return _Min;
            }
            set
            {
                if (value != _Min)
                {
                    _Min = value;
                    NotifyPropertyChanged("Min");
                }
            }
        }

        int _Pct05;
        public int Pct05
        {
            get
            {
                return _Pct05;
            }
            set
            {
                if (value != _Pct05)
                {
                    _Pct05 = value;
                    NotifyPropertyChanged("Pct05");
                }
            }
        }

        int _Pct50;
        public int Pct50
        {
            get
            {
                return _Pct50;
            }
            set
            {
                if (value != _Pct50)
                {
                    _Pct50 = value;
                    NotifyPropertyChanged("Pct50");
                }
            }
        }

        int _Pct95;
        public int Pct95
        {
            get
            {
                return _Pct95;
            }
            set
            {
                if (value != _Pct95)
                {
                    _Pct95 = value;
                    NotifyPropertyChanged("Pct95");
                }
            }
        }

        int _Max;
        public int Max
        {
            get
            {
                return _Max;
            }
            set
            {
                if (value != _Max)
                {
                    _Max = value;
                    NotifyPropertyChanged("Max");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
