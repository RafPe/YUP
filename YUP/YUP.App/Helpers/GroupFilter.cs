using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YUP.App.Helpers
{
    public class GroupFilter
    {
        private readonly List<Predicate<object>> _filters;

        public GroupFilter()
        {
            _filters = new List<Predicate<object>>();
            Filter = InternalFilter;
        }

        public Predicate<object> Filter { get; private set; }

        private bool InternalFilter(object o)
        {
            return _filters.All(filter => filter(o));
        }

        public void AddFilter(Predicate<object> filter)
        {
            _filters.Add(filter);
        }

        public void RemoveFilter(Predicate<object> filter)
        {
            if (_filters.Contains(filter))
            {
                _filters.Remove(filter);
            }
        }
    }
}
