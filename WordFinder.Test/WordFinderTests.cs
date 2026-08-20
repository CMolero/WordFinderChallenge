using WordFinder;

namespace WordFinder.Test
{
    public class WordFinderTests
    {
        private static readonly string[] BriefExampleMatrix =
         {
            "chill",
            "oxxxx",
            "lxxxx",
            "dxxxx",
            "wind!",
        };

        [Fact]
        public void Find_MatchesTestExample()
        {
            var finder = new WordFinder(BriefExampleMatrix);

            var result = finder.Find(new[] { "cold", "wind", "chill", "cars" }).ToList();

            Assert.Contains("cold", result);
            Assert.Contains("wind", result);
            Assert.Contains("chill", result);
            Assert.DoesNotContain("cars", result);
        }
        [Fact]
        public void Find_HorizontalWord_IsFound()
        {
            var matrix = new[]
            {
            "catdog",
            "xxxxxx",
            "xxxxxx",
        };

            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "cat" });

            Assert.Contains("cat", result);
        }

        [Fact]
        public void Find_VerticalWord_IsFound()
        {
            // Column 0 spells "cat"
            var matrix = new[]
            {
            "cxx",
            "axx",
            "txx",
        };

            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "cat" });

            Assert.Contains("cat", result);
        }

        [Fact]
        public void Find_WordOnlyDiagonal_IsNotFound()
        {
            // "cat" reads diagonally here (c, a, t on the diagonal) but never horizontally
            // or vertically.
            var matrix = new[]
            {
            "cxx",
            "xax",
            "xxt",
        };

            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "cat" });

            Assert.DoesNotContain("cat", result);
        }

        [Fact]
        public void Find_WordNotInMatrix_IsExcludedFromResults()
        {
            var matrix = new[] { "abcd", "efgh" };
            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "zzzz" });

            Assert.Empty(result);
        }

        [Fact]
        public void Find_WordRepeatedInStream_AppearsOnlyOnceInResults()
        {
            var matrix = new[] { "catdog", "xxxxxx" };
            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "cat", "cat", "cat", "cat" }).ToList();

            Assert.Single(result);
            Assert.Equal("cat", result[0]);
        }

        [Fact]
        public void Find_EmptyWordStream_ReturnsEmptyResult()
        {
            var matrix = new[] { "abcd" };
            var finder = new WordFinder(matrix);

            var result = finder.Find(Array.Empty<string>());

            Assert.Empty(result);
        }

        [Fact]
        public void Find_NoStreamWordsMatchMatrix_ReturnsEmptyResult()
        {
            var matrix = new[] { "abcd" };
            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "zzz", "yyy" });

            Assert.Empty(result);
        }

        [Fact]
        public void Find_IsCaseSensitive_ByDesign()
        {
            // Assumption: matching is case-sensitive. "CAT" should not match
            // a matrix that only contains lowercase "cat".
            var matrix = new[] { "catdog" };
            var finder = new WordFinder(matrix);

            var result = finder.Find(new[] { "CAT" });

            Assert.Empty(result);
        }

        [Fact]
        public void Find_NullWordStream_ThrowsArgumentNullException()
        {
            var finder = new WordFinder(new[] { "abcd" });

            Assert.Throws<ArgumentNullException>(() => finder.Find(null!).ToList());
        }

        [Fact]
        public void Constructor_NullMatrix_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WordFinder(null!));
        }

        [Fact]
        public void Constructor_EmptyMatrix_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new WordFinder(Array.Empty<string>()));
        }

        [Fact]
        public void Constructor_RowsOfDifferentLengths_ThrowsArgumentException()
        {
            var matrix = new[] { "abcd", "ab" };

            Assert.Throws<ArgumentException>(() => new WordFinder(matrix));
        }

        [Fact]
        public void Constructor_MoreThan64Rows_ThrowsArgumentException()
        {
            var matrix = Enumerable.Repeat("a", 65).ToArray();

            Assert.Throws<ArgumentException>(() => new WordFinder(matrix));
        }

        [Fact]
        public void Constructor_RowLongerThan64Characters_ThrowsArgumentException()
        {
            var matrix = new[] { new string('a', 65) };

            Assert.Throws<ArgumentException>(() => new WordFinder(matrix));
        }
    }
}
