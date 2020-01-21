using System;
using System.Collections;
using System.Collections.Specialized;
using XFExpandableListView.Utils;

namespace XFExpandableListView.Abstractions
{
    /// <inheritdoc cref="IList" />
    /// <summary>
    /// An Advanced IList Interface with a copy method and and identifier
    /// </summary>
    public interface IExpandableGroup : IList, INotifyCollectionChanged, IRangeCollectionProxy
    {
        /// <summary>
        /// The Unique Identifier of the List
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Property that indicates if the group is expanded
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Copy Method
        /// </summary>
        /// <returns>The copied instance</returns>
        IExpandableGroup NewInstance();
    }
}
