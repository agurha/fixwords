using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MoreExperiment
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<string> checkedList = new List<string>();

            using (var reader = new StreamReader(@"allwords.txt"))
            {
                while (reader.Peek() > 0)
                {
                    string word = reader.ReadLine();

                    if (!string.IsNullOrEmpty(word))
                    {
                        if(!String.IsNullOrEmpty(checkone(word)))
                        {
                            checkedList.Add(word);
                        }
                    }
                }

                // return _freqWords;
            }
        }


        private static string checkone(string text)
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
    }
}
