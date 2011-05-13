using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AnagramSharp;
using DataPopulator;
using System.Windows.Controls;

namespace ConsoleApplication1
{
    class Program
    {
        private static  AnagramDawg _anagram;
        private static  SharpDawg _dawg;

        [STAThread]
        static void Main(string[] args)
        {
            _dawg = new SharpDawg();
            _anagram = new AnagramDawg(_dawg);

            checkone("ess");

            //BuildWordList();

            //var x =  getSuggestions("bys");

            Console.ReadKey(true);

        }

        private static void BuildWordList()
        {
            var allWords = GetWords();
            List<string> bigList = new List<string>();

            //HashSet<string> bigHash = new HashSet<string>();

            foreach (var word in allWords)
            {
                var results = _anagram.FindForString(word);

                var byLength = (from item in results
                                where item.Length > 2
                                orderby item.Length descending
                                select item).ToArray();

                //var frequentWordsExcluded = 

                bigList.AddRange(byLength);

           
                
                //return byLength;
            }

            //ok now we have a big Distinct list
            var distinctList = bigList.Distinct().Select(r => r.ToUpper()).ToList();

            
            var frequentOnes = LoadFrequentWords().ToList();

            var getexcept = distinctList.Except(frequentOnes).OrderByDescending(r => r.Length)
                                                            .ThenBy(t=>t)
                                                            .Distinct().Except(allWords.Select(r=>r.ToUpper())).ToList();


            //List<string> checkedList = new List<string>();

            //foreach (var check in getexcept)
            //{
            //    var x = getSuggestions(check);
            //    if(x != "NO")
            //    {
            //        checkedList.Add(check);

            //    }
            //}

            var checkedList = getSuggestions(getexcept);



            using(StreamWriter writer = new StreamWriter(@"c:\stupidWords5"))
            {
                foreach (var wd in checkedList)
                {
                    if (wd.Length < 7)
                    {
                        writer.WriteLine(wd);
                    }
                }
            }



        }



        private static List<string> getSuggestions(List<string> alltext)
        {
            System.Windows.Controls.TextBox wpfTextBox = new System.Windows.Controls.TextBox();
            wpfTextBox.AcceptsReturn = true;
            wpfTextBox.AcceptsTab = true;
            wpfTextBox.SpellCheck.IsEnabled = true;
            

            List<string> suggestions = new List<string>();

            foreach (var ttt in alltext)
            {
                wpfTextBox.Text = ttt;

                int index = 0;
               


                int caretIndex = wpfTextBox.CaretIndex;
                SpellingError spellingError;

                spellingError = wpfTextBox.GetSpellingError(caretIndex);

                if (spellingError != null)
                {
                    suggestions.Add(ttt);
                }
            }

          

            //while ((index =wpfTextBox.GetNextSpellingErrorCharacterIndex(index, System.Windows.Documents.LogicalDirection.Forward)) !=-1)
            //{
            //    string currentError = wpfTextBox.Text.Substring(index, wpfTextBox.GetSpellingErrorLength(index));
            //    suggestions.Add(currentError);

            //    foreach (string suggestion in wpfTextBox.GetSpellingError(index).Suggestions)
            //    {
            //        suggestions.Add(suggestion);
            //    }
            //})
            return suggestions;
        }

        private static  string checkone(string text)
        {
             System.Windows.Controls.TextBox wpfTextBox = new System.Windows.Controls.TextBox();
            wpfTextBox.AcceptsReturn = true;
            wpfTextBox.AcceptsTab = true;
            wpfTextBox.SpellCheck.IsEnabled = true;
            

            List<string> suggestions = new List<string>();

           
                wpfTextBox.Text = text;

                int index = 0;
               


                int caretIndex = wpfTextBox.CaretIndex;
                SpellingError spellingError;

                spellingError = wpfTextBox.GetSpellingError(caretIndex);

                if (spellingError == null)
                {
                    return text;
                }
                else
                {
                    return null;
                }
            
        }

        public static IEnumerable<string> GetWords()
        {
            using (DataClasses1DataContext context = new DataClasses1DataContext())
            {
                var wordsDetails = context.Dim_Overs.Select(r => r);

                return wordsDetails.Where(r => r.displayed == false).Select(r => r.id).ToArray();
            }
        }

        private static List<string> _freqWords;

        private static IEnumerable<string> LoadFrequentWords()
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
