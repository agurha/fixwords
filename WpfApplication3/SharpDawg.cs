using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AnagramSharp
{
    /// <summary>
    /// Builds a DAWG representation in memory from the two DAWG files.
    /// </summary>
    sealed class SharpDawg
    {
        /// <summary>
        /// Number of lines in DAWG files. Must be changed for each different DAWG used.
        /// </summary>
        const int _customLineCount = 52930;

        /// <summary>
        /// Number of nodes in the words file.
        /// </summary>
        const int _customNodeCount = 121438;

        /// <summary>
        /// The file name where the bitfields are stored.
        /// </summary>
        const string _dictSuper = "new-super.txt";

        /// <summary>
        /// The file where the arrays of line numbers are stored.
        /// </summary>
        const string _dictWords = "new-words.txt";

        /// <summary>
        /// The "super" bitfields for the tree.
        /// </summary>
        int[] _supers = new int[_customLineCount];

        /// <summary>
        /// An array of ints that tell us where the 'blocks' of the node arrays
        /// are located in the single flat array.
        /// </summary>
        int[] _where = new int[_customLineCount];

        /// <summary>
        /// The nodes, which are just pointers to line numbers.
        /// With the line numbers, we can get to other bitfields.
        /// </summary>
        ushort[] _total = new ushort[_customNodeCount];

        /// <summary>
        /// Get a number from the bitfields array data.
        /// </summary>
        /// <param name="line">The line number you want to access.</param>
        /// <returns>The bitfield in integer form.</returns>
        public int GetSuperAt(int line)
        {
            return _supers[line];
        }

        /// <summary>
        /// Get a node from the words file.
        /// </summary>
        /// <param name="line">Look at the nodes on this line in the words file.</param>
        /// <param name="position">Get the node at this position from left to right
        /// in the words file.</param>
        /// <returns>The value of the node, which is also a line number.</returns>
        public int GetNodeAt(int line, int position)
        {
            return _total[_where[line] + position];
        }

        /// <summary>
        /// Create a new DAWG object that stores a tree of words in a compact format.
        /// </summary>
        public SharpDawg()
        {
            // Read in the bitfields file.
            int lineCountA = 0;
            using (StreamReader reader = new StreamReader(_dictSuper))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int number = SafeParse(line);
                    _supers[lineCountA] = number;
                    lineCountA++;
                }
            }
            // Read in the line number arrays file.
            int totalNodes = 0;
            int lineCountB = 0;
            using (StreamReader reader = new StreamReader(_dictWords))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _where[lineCountB] = totalNodes;
                    ParseLine(line, ref totalNodes);
                    lineCountB++;
                }
            }
        }

        /// <summary>
        /// Parse a line of integers separated by spaces. Store them in the _total array.
        /// </summary>
        private void ParseLine(string line, ref int totalNodes)
        {
            int currentNumber = 0;
            for (int i = 0; i < line.Length; i++)
            {
                char let = line[i];
                if (let == ' ')
                {
                    // save and store our number on a space.
                    _total[totalNodes] = (ushort)currentNumber;
                    totalNodes++;
                    // reset the current number.
                    currentNumber = 0;
                }
                else if (char.IsDigit(let))
                {
                    currentNumber = 10 * currentNumber + (let - 48);
                }
            }
            // save and store the last number.
            _total[totalNodes] = (ushort)currentNumber;
            totalNodes++;
        }

        /// <summary>
        /// Parse a single numeric string into an int.
        /// </summary>
        private static int SafeParse(string value)
        {
            int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                result = 10 * result + (value[i] - 48);
            }
            return result;
        }

#if OLD_PARSE
        /// <summary>
        /// Convert a series of characters to a number using pointers.
        /// </summary>
        /// <param name="value">String that must contain only digits.</param>
        /// <returns>Integer represented by the input string.</returns>
        unsafe static int ParseUnsafe(string value)
        {
            int result = 0;
            fixed (char* valuePointer = value)
            {
                char* str = valuePointer;
                while (*str != '\0')
                {
                    result = 10 * result + (*str - 48); // Convert ASCII.
                    str++;
                }
            }
            return result;
        }
#endif

    }
}