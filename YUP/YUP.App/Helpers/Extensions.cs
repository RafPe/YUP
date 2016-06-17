using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YUP.App.Helpers
{
    /// <summary>
    /// Extends our observable collection so we could add ranges
    /// </summary>
    public static class ObservableCollectionExtend
    {
        public static void AddRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        public static void UpdateRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
               if(!source.Contains(item)) source.Add(item);
            }
        }
    }

}
