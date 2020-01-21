using System.Collections;
using System.Collections.Specialized;

namespace XFExpandableListView.Utils
{
    public interface IRangeCollectionProxy
    {
        void AddRange(IEnumerable collection,
            NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add);

        void RemoveRange(IEnumerable collection,
            NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove);

        void Replace(object item);

        void ReplaceRange(IEnumerable collection);
    }
}
