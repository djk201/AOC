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
            return result;
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
    }
}
