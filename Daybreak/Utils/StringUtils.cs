using System;

namespace Utils
{
    public static class StringUtils
    {
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
                    var cost = (s[j - 1] == t[i - 1]) ? 0 : 1;
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
    }
}
