using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AnagramSharp
{
    /// <summary>
    /// Finds anagrams.
    /// </summary>
    sealed class AnagramDawg
    {
        /// <summary>
        /// The offset we need to subtract to get from a letter to an integer index.
        /// </summary>
        const int _offset = 97;

        /// <summary>
        /// The bit position of the bit that indicates that a path makes a full word.
        /// </summary>
        const int _fullWord = (1 << 28);

        /// <summary>
        /// The first bitfield from the DAWG. (May need to be changed for different DAWGs.)
        /// </summary>
        const int _firstSuper = 67108863;

        /// <summary>
        /// A directed acyclic word graph.
        /// </summary>
        SharpDawg _dawg;

        /// <summary>
        /// Make a new anagram-finding object. Must have a SharpDawg tree to use.
        /// </summary>
        public AnagramDawg(SharpDawg dawgIn)
        {
            _dawg = dawgIn;
        }

        /// <summary>
        /// Find all anagrams from the string passed in, including any words formed from
        /// a subset of the string.
        /// </summary>
        /// <param name="lettersIn">The string to find anagrams of.</param>
        /// <returns>List containing all of the results.</returns>
        public List<string> FindForString(string lettersIn)
        {
            int length = lettersIn.Length;

            // Make frequency table.
            int[] frequencies = new int[26];
            for (int i = length - 1; i >= 0; --i)
            {
                char letter = lettersIn[i];

                // Lowercase if necessary.
                if (letter >= 'A' && letter <= 'Z')
                {
                    letter = char.ToLower(letter);
                }
                else if (letter > 'z' || letter < 'a')
                {
                    return new List<string>();
                }
                frequencies[letter - _offset]++;
            }

            // Our result lists.
            List<string> result = new List<string>();

            FindRecurse(new char[length], 0, 0, frequencies, result, _firstSuper);
            return result;
        }

        /// <summary>
        /// Internal method that recursively finds anagram matches in the DAWG.
        /// </summary>
        /// <param name="soFar">The string that has been built from the frequencies.</param>
        /// <param name="length">The current length of the buffer of characters.</param>
        /// <param name="currentLine">The line number for both DAWG files.</param>
        /// <param name="frequencyInts">The frequencies of various letters in
        /// the input word.</param>
        /// <param name="resultList">Results built up from the input word.</param>
        /// <param name="superHere">Contains bits set that quickly tell us what
        /// letters we have.</param>
        private void FindRecurse(char[] soFar, int length, int currentLine,
            int[] frequencyInts, List<string> resultList, int superHere)
        {
            // Try to store this path as a result if it has the full word bit set.
            if ((superHere & _fullWord) != 0)
            {
                char[] newArray = new char[length];
                for (int i = length - 1; i >= 0; --i)
                {
                    newArray[i] = soFar[i];
                }
                resultList.Add(new string(newArray));
            }

            // Check all letters.
            int characterOrder = 0;
            for (int charIndex = 0; charIndex < 26; charIndex++)
            {
                // See if word continues to this letter.
                if ((superHere & (1 << charIndex)) != 0)
                {
                    // We can make this letter. Now, do we have it in our word?
                    int freq = frequencyInts[charIndex];
                    if (freq > 0)
                    {
                        // We have this letter in our rack, but have we used it already?
                        char wordChar = (char)(charIndex + _offset);
                        int freqPass = FrequencyInWord(soFar, wordChar, length);

                        // if (remaining > 0)
                        if (freq > freqPass)
                        {
                            // Get the new line for our numbers.
                            int newLine = _dawg.GetNodeAt(currentLine, characterOrder);

                            int newSuper = _dawg.GetSuperAt(newLine);

                            // Make the new string.
                            soFar[length] = wordChar;

                            // Recurse onto the next letter.
                            FindRecurse(soFar, length + 1, newLine, frequencyInts,
                                resultList, newSuper);
                        }
                    }
                    // Increment the current character's order index.
                    characterOrder++;
                }
            }
        }

        /// <summary>
        /// Simple helper method that measures a specified letter's frequency in a string.
        /// </summary>
        /// <param name="wordIn">The string you want to count the letter in.</param>
        /// <param name="letter">The letter you want to count.</param>
        /// <returns>The number of the letter in the string.</returns>
        private static int FrequencyInWord(char[] wordIn, char letter, int length)
        {
            int result = 0;
            for (int i = length - 1; i >= 0; --i)
            {
                char c = wordIn[i];
                if (c == letter)
                {
                    result++;
                }
            }
            return result;
        }
    }
}
