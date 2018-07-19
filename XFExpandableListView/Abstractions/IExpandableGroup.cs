using System;
using System.Collections;
using System.Collections.Specialized;

namespace XFExpandableListView.Abstractions
{
    /// <inheritdoc cref="IList" />
    /// <summary>
    /// An Advanced IList Interface with a copy method and and identifier
    /// </summary>
    public interface IExpandableGroup : IList, INotifyCollectionChanged
    {
        Guid Id { get; set; }
        bool IsExpanded { get; set; }

        IExpandableGroup NewInstance();
    }
}
