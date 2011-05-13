using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataSource dataSource;

        public MainWindow()
        {
            InitializeComponent();

           dataSource = new DataSource();
            this.DataContext = dataSource;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string singleWord = dataSource.AllWords();

            txtBox1.Text = singleWord;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dataSource.BindAnagramsDropDown(txtBox1.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string strSelected = dataSource.SelectedWordsText;

            string[] found = strSelected.Split(',');
            
            List<string> s = found.Select(s1 => s1.Trim().ToUpper()).ToList();

            var freq = this.LoadFrequentWords();

            var notDuplicatedOnes = s.Except(freq).ToList();

            DAL dal = new DAL();
            dal.WriteToOut(txtBox1.Text, notDuplicatedOnes);

            statusBox.Text = "Success";

            dal.UpdateStatus(txtBox1.Text);
        }

        private static List<string> _freqWords;

        private IEnumerable<string> LoadFrequentWords()
        {

            if (_freqWords == null)
            {
                _freqWords = new List<string>();
                using (var reader = new StreamReader(@"freqfiltered.txt"))
                {
                    while (reader.Peek() > 0)
                    {
                        string word = reader.ReadLine();

                        if (!string.IsNullOrEmpty(word))
                        {
                            string[] splitted = word.Split(',');
                            _freqWords.Add(splitted[0]);
                        }
                    }

                    return _freqWords;
                }
            }
            else
            {
                return _freqWords;
            }
        }

       
    }
}
