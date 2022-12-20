using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class MyExtensions
    {
        /// <summary>
        /// Chunk an IEnumerable string source by a specific seperator returning a list of string arrays
        /// </summary>
        /// <param name="source">The source string ienumerable instance</param>
        /// <param name="chunker">The chunking string</param>
        /// <returns>List of string arrays</returns>
        public static List<string[]> ChunkBy(this IEnumerable<string> source, string chunker)
        {
            var result = new List<string[]>();
            var vals = new List<string>();
            source.ToList().ForEach(x =>
            {
                if (x.Equals(chunker))
                {
                    result.Add(vals.ToArray());
                    vals = new List<string>();
                }
                else
                {
                    vals.Add(x);
                }
            });
            result.Add(vals.ToArray());
            return result;
        }

        public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
        {
            return input
               .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
               .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
               .Select(s => shouldTrim ? s.Trim() : s)
               .ToList();
        }

        public static List<int> ToListOfTypeInt(this string[] source)
        {
            var result = Array.ConvertAll<string, int>(source, new Converter<string, int>(Convert.ToInt32)).ToList();
            return result;
        }

        public static List<int> Aggregate(this IEnumerable<List<int>> source)
        {
            return source.Select(x => x.Sum()).ToList();
        }

        public static int[,] TrimArray(this int[,] originalArray, int rowToRemove, int columnToRemove)
        {
            int[,] result = new int[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }

    }
}
