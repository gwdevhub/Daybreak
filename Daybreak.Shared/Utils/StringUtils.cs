using System.Extensions;
using System.Text.RegularExpressions;

namespace Daybreak.Shared.Utils;

public static partial class StringUtils
{
    private const int SearchSensitivity = 2;
    private static readonly Regex SplitIntoWordsRegex = WordRegex();

    public static int DamerauLevenshteinDistance(string s, string t)
    {
        var (height, width) = (s.Length + 1, t.Length + 1);

        var matrix = new int[height, width];

        for (var j = 0; j < height; j++) { matrix[j, 0] = j; };
        for (var i = 0; i < width; i++) { matrix[0, i] = i; };

        for (var j = 1; j < height; j++)
        {
            for (var i = 1; i < width; i++)
            {
                var cost = s[j - 1] == t[i - 1] ? 0 : 1;
                var insertion = matrix[j, i - 1] + 1;
                var deletion = matrix[j - 1, i] + 1;
                var substitution = matrix[j - 1, i - 1] + cost;

                var distance = Math.Min(insertion, Math.Min(deletion, substitution));

                if (j > 1 && i > 1 && s[j - 1] == t[i - 2] && s[j - 2] == t[i - 1])
                {
                    distance = Math.Min(distance, matrix[j - 2, i - 2] + cost);
                }

                matrix[j, i] = distance;
            }
        }

        return matrix[height - 1, width - 1];
    }

    /// <summary>
    /// Returns true if stringToSearch is somewhat close to searchString.
    /// </summary>
    /// <param name="stringToSearch"></param>
    /// <param name="searchString"></param>
    /// <returns>True if strings match.</returns>
    public static bool MatchesSearchString(string stringToSearch, string searchString)
    {
        return MatchSearchStringScore(stringToSearch, searchString) < SearchSensitivity;
    }

    /// <summary>
    /// Returns the match string score
    /// </summary>
    /// <param name="stringToSearch"></param>
    /// <param name="searchString"></param>
    /// <returns>True if strings match.</returns>
    public static int MatchSearchStringScore(string stringToSearch, string searchString)
    {
        if (searchString.IsNullOrWhiteSpace() ||
            searchString.Length < SearchSensitivity)
        {
            return 0;
        }

        if (stringToSearch.IsNullOrWhiteSpace() ||
            stringToSearch.Length < SearchSensitivity)
        {
            return 0;
        }

        // Return true if either the distance between the entire text and the searchstring is small enough, or if any of the words are close to the search string.
        return Math.Min(
            DamerauLevenshteinDistance(stringToSearch.ToLower()[..Math.Min(stringToSearch.Length, searchString.Length)], searchString.ToLower()),
            SplitIntoWordsRegex.Split(stringToSearch.ToLower())
                .Select(word => DamerauLevenshteinDistance(word.ToLower()[..Math.Min(word.Length, searchString.Length)], searchString.ToLower())).Min());
    }

    [GeneratedRegex(@"\W+", RegexOptions.Compiled)]
    private static partial Regex WordRegex();
}
