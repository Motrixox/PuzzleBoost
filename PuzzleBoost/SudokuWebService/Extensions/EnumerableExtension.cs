namespace SudokuWebService.Extensions
{
    static class EnumerableExtension
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rand)
        {
            return source.OrderBy(x => rand.Next());
        }
    }
}
