using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AnagramSharp;
using DataPopulator;
using System.Windows.Controls;

namespace ConsoleApplication1
{
    class Program
    {

        private static void BuildDict()
        {
            HashSet<string> brits = new HashSet<string>();
            using (var reader = new StreamReader(@"bigcorpous.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                  
                    if (!string.IsNullOrEmpty(word))
                    {

                        if (!Regex.IsMatch(word, "[^a-zA-Z]"))
                        {
                            if (word.Length >= 3 || word.Length <=7)
                            {
                                brits.Add(word.ToUpper());
                            }
                        }
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(@"D:\Users\agurha\ankurdict.txt"))
            {
                foreach (var wd in brits)
                {

                    writer.WriteLine(wd.ToUpper());

                }
            }


        }


        private static  AnagramDawg _anagram;
        private static  SharpDawg _dawg;


        private static HashSet<string> finalList;

        [STAThread]
        static void Main(string[] args)
        {

            finalList = new HashSet<string>();

           // BuildDict();
            _dawg = new SharpDawg();
            _anagram = new AnagramDawg(_dawg);

            GetReallyFreqWords();

           // GetBritishWords();
           // ValidateWordList();


           // checkone("bys");

           // BuildWordList();

            //var x =  getSuggestions("bys");

            Console.ReadKey(true);

        }

        private static  void GetReallyFreqWords()
        {
            HashSet<string> brits = new HashSet<string>();
            using (var reader = new StreamReader(@"bnc.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();
                    if (!string.IsNullOrEmpty(word))
                    {
                        if (!Regex.IsMatch(word, "[^a-zA-Z]"))
                        {
                            if (word.Length == 7 )
                            {
                                brits.Add(word.ToUpper());
                            }
                        }
                    }
                }
            }

            var moregood = GetBritishWords();

            foreach (var ff in moregood)
            {
                brits.Add(ff);
            }

            var somemore = GetGoogleNGramWords();

            foreach (var ff in somemore)
            {
                brits.Add(ff);
            }

            var allValidated = ValidateWordList(brits.ToList());

            var shuffled = allValidated.OrderBy(r => Guid.NewGuid());

            using (StreamWriter writer = new StreamWriter(@"C:\wordsketchletters.txt"))
            {
                int count = 0;
                foreach (var wd in shuffled)
                {
                    count++;

                    string rt = string.Format("{0} {1}", wd.ToUpper(), count);

                    writer.WriteLine(rt);

                }
            }

            using (StreamWriter writer = new StreamWriter(@"C:\bestfreqlist.txt"))
            {
                foreach (var wd in finalList)
                {

                    writer.WriteLine(wd.ToUpper());

                }
            }
        }

        private static List<string> GetGoogleNGramWords()
        {
            List<string> checkedList = new List<string>();

            using (var reader = new StreamReader(@"allwords.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                    if (!string.IsNullOrEmpty(word))
                    {
                        if (!String.IsNullOrEmpty(checkone(word)))
                        {
                            checkedList.Add(word);
                        }
                    }
                }
            }
            return checkedList;
        }


        private static List<string> GetBritishWords()
        {
            List<string> brits = new List<string>();
            using (var reader = new StreamReader(@"cen_GB.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                    // string[] splitted = word.Split('/');
                    
                    if (!string.IsNullOrEmpty(word))
                    {
                        string[] splitted = word.Split('\t');

                        string actual = splitted[0].ToUpper().Trim();

                        string ss = splitted[1].Trim();
                        int freq = Int32.Parse(String.IsNullOrEmpty(ss)? "0" : ss);

                        if(!Regex.IsMatch(actual, "[^a-zA-Z]"))
                        {
                            if (actual.Length == 7 && freq >10)
                            {
                                brits.Add(actual);
                            }
                        }
                    }
                }
           }
           return brits;
        }

        private static List<string>  ValidateWordList(List<string> allWords)
        {
            List<string> bigList = new List<string>();
            List<string> smallList = new List<string>();
           
            LoadDict();

            foreach (var word in allWords)
            {
                var results = _anagram.FindForString(word.ToLower());

                var byLength = (from item in results
                                where item.Length > 2
                                orderby item.Length descending
                                select item.ToUpper()).ToList();

                if(GetValidated(byLength))
                {
                    bigList.Add(word);
                }
                else
                {
                    smallList.Add(word);
                }

            }

            return bigList;
        }


        private static bool GetValidated(List<string> masterArray)
        {
            var goodWords = _dict;
            var x = masterArray.Where(r => r.Length == 3).ToList();
            List<string> dictChecked = getSuggestions(masterArray);
            
            if (dictChecked.Count >= 28)
            {
//                int checkCount = dictChecked.Where(r => r.Length == 3).ToList().Count;
//                if(checkCount <12)
//                {
//                    Console.WriteLine("Hello");
//                }
               
                    var frequentOnes = LoadFrequentWords();

                    foreach (var wd in dictChecked)
                    {
                        if (frequentOnes.ContainsKey(wd))
                        {
                            string finalWord = string.Format("{0},{1}", wd.ToUpper(), frequentOnes[wd]);
                            finalList.Add(finalWord);
                            //writer.WriteLine(finalWord);
                        }
                        else
                        {
                            string finalWord = string.Format("{0},{1}", wd.ToUpper(), 0);
                            finalList.Add(finalWord);
                        }
                    }
                
                
                return true;
            }
            else
            {
                return false;
            }

            int threeCount = 0;
            int fourCount = 0;
            int fiveCount = 0;
            int sixCount = 0;

            foreach(var mer in masterArray)
            {
                if(mer.Length ==3)
                {
                    threeCount++;
                }
                if (mer.Length == 4)
                {
                    fourCount++;
                }
                if (mer.Length == 5)
                {
                    fiveCount++;
                }
                if (mer.Length == 6)
                {
                    sixCount++;
                }
               
            }

            //if (masterArray.Count > 30)
            //{

            //   // return true;

                if ((threeCount >= 12) && (fourCount >= 8) && fiveCount >=4 && sixCount>=1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            //}
            //else
            //{
            //    return false;
            //}

        }

        private static List<string> _dict;

        private static void LoadDict()
        {
            if (_dict == null)
            {
                _dict = new List<string>();
                using (var reader = new StreamReader(@"en_GB.txt"))
                {
                    while (reader.Peek() > 0)
                    {
                        string word = reader.ReadLine();

                        if (!string.IsNullOrEmpty(word))
                        {
                            string[] splitted = word.Split(',');
                            _dict.Add(splitted[0].ToUpper());
                        }
                    }

                    // return _freqWords;
                }
            }
        }


        private static Dictionary<string, int> _frequenctDict;

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

            
            var frequentOnes = LoadFrequentWords();

//            var getexcept = distinctList.Except(frequentOnes).OrderByDescending(r => r.Length)
//                                                            .ThenBy(t=>t)
//                                                            .Distinct().Except(allWords.Select(r=>r.ToUpper())).ToList();


            //List<string> checkedList = new List<string>();

            //foreach (var check in getexcept)
            //{
            //    var x = getSuggestions(check);
            //    if(x != "NO")
            //    {
            //        checkedList.Add(check);

            //    }
            //}



           // var checkedList = getSuggestions(getexcept);

            var valids = GetValidWords().Select(r => r.ToUpper()).ToList();
            var checkedList = distinctList.Intersect(valids).ToList();



            using (StreamWriter writer = new StreamWriter(@"D:\Users\agurha\goodwords1.txt"))
            {
                foreach (var wd in checkedList)
                {
                    if (frequentOnes.ContainsKey(wd))
                    {
                        string finalWord = string.Format("{0},{1}", wd.ToUpper(), frequentOnes[wd]);
                        writer.WriteLine(finalWord);
                    }
                    else
                    {
                        string finalWord = string.Format("{0},{1}", wd.ToUpper(), 0);
                        writer.WriteLine(finalWord);
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
                
                 int index = 0;
                 wpfTextBox.Text = ttt;

                Dictionary<int,string> spellingErrors = new Dictionary<int, string>();


                while ((index = wpfTextBox.GetNextSpellingErrorCharacterIndex(index, System.Windows.Documents.LogicalDirection.Forward)) != -1)
                {
	                string currentError = wpfTextBox.Text.Substring(index,
	                wpfTextBox.GetSpellingErrorLength(index));
	                spellingErrors.Add(index, currentError);
	                index += currentError.Length;
                }
                
               

              
               


                int caretIndex = wpfTextBox.CaretIndex;
                SpellingError spellingError;



                spellingError = wpfTextBox.GetSpellingError(caretIndex);



               

                if (spellingError == null)
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

            Dictionary<int, string> spellingErrors = new Dictionary<int, string>();
                
                while ((index = wpfTextBox.GetNextSpellingErrorCharacterIndex(index, System.Windows.Documents.LogicalDirection.Forward)) != -1)
                {
                    string currentError = wpfTextBox.Text.Substring(index,
                    wpfTextBox.GetSpellingErrorLength(index));
                    
                    spellingErrors.Add(index, currentError);
                    index += currentError.Length;
                }


       

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
            //using (DataClasses1DataContext context = new DataClasses1DataContext())
            //{
            //    var wordsDetails = context.Dim_Overs.Select(r => r);

            //    return wordsDetails.Where(r => r.displayed == false).Select(r => r.id).ToArray();
            //}

            using (var reader = new StreamReader(@"britsevenletters.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                    if (!string.IsNullOrEmpty(word))
                    {
                        yield return word;
                    }
                }

               // return _freqWords;
            }
        }

        public static IEnumerable<string> GetValidWords()
        {
            //using (DataClasses1DataContext context = new DataClasses1DataContext())
            //{
            //    var wordsDetails = context.Dim_Overs.Select(r => r);

            //    return wordsDetails.Where(r => r.displayed == false).Select(r => r.id).ToArray();
            //}

            using (var reader = new StreamReader(@"en_GB.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                   // string[] splitted = word.Split('/');

                    if (!string.IsNullOrEmpty(word))
                    {
                        yield return word;
                    }
                }

                // return _freqWords;
            }
        }

       // private static List<string> _freqWords;

        private static Dictionary<string,int> LoadFrequentWords()
        {
          //  Dictionary<string ,int> freq = new Dictionary<string, int>();

            if(_frequenctDict == null)
            {
                _frequenctDict = new Dictionary<string, int>();
                using (var reader = new StreamReader(@"freqfiltered.txt"))
                {
                    while (reader.Peek() > 0)
                    {
                        string word = reader.ReadLine();

                        if (!string.IsNullOrEmpty(word))
                        {
                            string[] splitted = word.Split(',');
                            if (!_frequenctDict.ContainsKey(splitted[0]))
                            {
                                _frequenctDict.Add(splitted[0], Int32.Parse(splitted[1]));
                            }
                        }
                    }
                }

                return _frequenctDict;
            }

            else
            {
                return _frequenctDict;
            }

         
        }

       
    }

    
}
