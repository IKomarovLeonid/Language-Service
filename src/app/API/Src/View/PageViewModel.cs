using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.View
{
    public class PageViewModel<TModel>
    {
        public ICollection<TModel> Items { get; private init; } = new Collection<TModel>();

        public int Count { get; private init; }

        public static PageViewModel<TModel> New(ICollection<TModel> items) => new PageViewModel<TModel>() { Items = items, Count = items.Count };
    }
}
