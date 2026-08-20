namespace WordFinder
{
    /// <summary>
    /// English was used only because the test was sent in english.
    /// Searches for words in a character matrix (64x64). Matches are found in horizontal and vertical directions, 
    /// and the results are ordered by frequency of occurrence in the input stream.
    /// </summary>
    public class WordFinder
    {
        private const int MaxDimension = 64;

        /// <summary>
        /// Collects every substring from the matrix rows and columns for quick lookup during the Find operation.
        /// </summary>
        private readonly HashSet<string> _matrixSubstrings;
        public WordFinder(IEnumerable<string> matrix)
        {
            ArgumentNullException.ThrowIfNull(matrix, nameof(matrix));
            var rows = matrix.ToList();
            if(rows.Count == 0)
            {
                throw new ArgumentException("Matrix cannot be empty.", nameof(matrix));
            }
            if (rows.Count > MaxDimension) 
            { 
                throw new ArgumentException($"Matrix cannot have more than {MaxDimension} rows.", nameof(matrix));
            }

            int columnCount = rows[0].Length;

            if (columnCount == 0 || columnCount > MaxDimension) 
            {
                throw new ArgumentException($"Matrix must be between 1 and {MaxDimension}.", nameof(matrix));
            }
            if(rows.Any(row => row.Length != columnCount))
            {
                throw new ArgumentException("All rows in the matrix must have the same length.", nameof(matrix));
            }

            _matrixSubstrings = BuildSubstringIndex(rows, columnCount);
        }
        /// <summary>
        /// Builds a HashSet containing all possible substrings from the rows and columns of the matrix for quick lookup.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columnCount"></param>
        private static HashSet<string> BuildSubstringIndex(List<string> rows, int columnCount)
        {
            var substrings = new HashSet<string>();
            foreach (var row in rows) 
            {
                AddAllSubstrings(row, substrings);
            }
            for (int col = 0; col < columnCount; col++) { 
                var columnChars = new char[rows.Count];
                for(int row = 0; row < rows.Count; row++)
                {
                    columnChars[row] = rows[row][col];
                }
                AddAllSubstrings(new string(columnChars), substrings);
            }
            return substrings;
        }
        /// <summary>
        /// Adds all possible substrings of <paramref name="line"/> to the <paramref name="destination"/> HashSet.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="destination"></param>
        private static void AddAllSubstrings(string line, HashSet<string> destination)
        {
            for (int start = 0; start < line.Length; start++)
            {
                for (int length = 1; length <= line.Length - start; length++)
                {
                    destination.Add(line.Substring(start, length));
                }
            }
        }
        /// <summary>
        /// Returns the top 10 most repeated words from <paramref name="wordstream"/> in the matrix.
        /// </summary>
        /// <param name="wordstream"></param>
        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            ArgumentNullException.ThrowIfNull(wordstream, nameof(wordstream));
            var matchCounts = new Dictionary<string, int>();
            foreach (var word in wordstream) {
                // Skip null or empty words and those not found in the matrix.
                if (string.IsNullOrEmpty(word)|| !_matrixSubstrings.Contains(word))
                {
                    continue;
                }
                matchCounts.TryGetValue(word, out int currentCount);
                matchCounts[word] = currentCount + 1;
            }
            // Order by frequency descending and take the top 10 matches.
            return matchCounts.OrderByDescending(kvp => kvp.Value).Take(10).Select(kvp => kvp.Key);
        }
    }
}