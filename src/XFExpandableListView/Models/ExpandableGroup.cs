using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using MvvmHelpers;
using XFExpandableListView.Abstractions;
using XFExpandableListView.Utils;

namespace XFExpandableListView.Models
{
    /// <inheritdoc cref="ObservableCollection{T}" />
    /// <summary>
    /// Base Class for the Group Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpandableGroup<T> : ObservableRangeCollection<T>, IExpandableGroup
    {
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public Guid Id { get; set; }

        public ExpandableGroup(Guid id)
        {
            Id = id;
        }

        public virtual IExpandableGroup NewInstance()
        {
            return new ExpandableGroup<T>(Id);
        }

        public void AddRange(IEnumerable collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add)
        {
            base.AddRange((IEnumerable<T>)collection, mode);
        }

        public void RemoveRange(IEnumerable collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove)
        {
            base.RemoveRange((IEnumerable<T>)collection, NotifyCollectionChangedAction.Reset);
        }

        public void Replace(object item)
        {
            base.Replace((T)item);
        }

        public void ReplaceRange(IEnumerable collection)
        {
            base.ReplaceRange((IEnumerable<T>)collection);
        }
    }
}
