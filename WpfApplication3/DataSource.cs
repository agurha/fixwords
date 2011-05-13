using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using AnagramSharp;
using DataPopulator;

namespace WpfApplication3
{
    public class DataSource : INotifyPropertyChanged
    {
        private readonly AnagramDawg _anagram;
        private readonly SharpDawg _dawg;

      

        public DataSource()
        {
            _dawg = new SharpDawg();
            _anagram = new AnagramDawg(_dawg);

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        private ObservableCollection<string> _wordsGeneratedByAlgo = new ObservableCollection<string>();

        public ObservableCollection<string> WordsGeneratedByAlgo
        {
            get
            {
                //_wordsGeneratedByAlgo = GetAnagramsForWord("");
                return _wordsGeneratedByAlgo;
            }
            set { _wordsGeneratedByAlgo = value;
            
            }
        }

        public string AllWords()
        {
            DAL dal = new DAL();
            var allWords = dal.GetWords();

            return allWords.FirstOrDefault();

        }

        private IEnumerable<string> GetAnagramsForWord(string word)
        {
            var results = _anagram.FindForString(word);

            var byLength = (from item in results
								 where item.Length > 2
								 orderby item.Length descending
								 select item).ToArray();

            //var frequentWordsExcluded = 

            return byLength;

        }

     

        private string _selectedWord = "The";
        public string SelectedWord
        {
            get { return _selectedWord; }
            set
            {
                _selectedWord = value;
                OnPropertyChanged("SelectedWord");
            }
        }

        private ObservableCollection<string> _selectedWords;
        public ObservableCollection<string> SelectedWords
        {
            get
            {
                if (_selectedWords == null)
                {
                    _selectedWords = new ObservableCollection<string>();
                    SelectedWordsText = WriteSelectedWordsString(_selectedWords);
                    _selectedWords.CollectionChanged +=
                        (s, e) =>
                        {
                            SelectedWordsText = WriteSelectedWordsString(_selectedWords);
                            OnPropertyChanged("SelectedWords");
                        };
                }
                return _selectedWords;
            }
            set
            {
                _selectedWords = value;
            }
        }

        public string SelectedWordsText
        {
            get { return _selectedWordsText; }
            set
            {
                _selectedWordsText = value;
                OnPropertyChanged("SelectedWordsText");
            }
        } 
        
        string _selectedWordsText;


        public static string WriteSelectedWordsString(IList<string> list)
        {
            if (list.Count == 0)
                return String.Empty;

            StringBuilder builder = new StringBuilder(list[0]);

            for (int i = 1; i < list.Count; i++)
            {
                builder.Append(", ");
                builder.Append(list[i]);
            }

            return builder.ToString();
        }

        internal void BindAnagramsDropDown(string word)
        {
            var results = _anagram.FindForString(word);

            var byLength = (from item in results
                            where item.Length > 2
                            orderby item.Length descending
                            select item).ToArray();

            _wordsGeneratedByAlgo = new ObservableCollection<string>(byLength);
            OnPropertyChanged("WordsGeneratedByAlgo");
        }
    }
}
