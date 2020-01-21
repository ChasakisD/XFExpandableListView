using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace XFExpandableListView.Utils
{
    public class ObservableRangeCollection<T> : ObservableCollection<T>, IRangeCollectionProxy
    {
        public ObservableRangeCollection() { }

        public ObservableRangeCollection(List<T> list) : base(list) { }

        public ObservableRangeCollection(IEnumerable<T> collection) : base(collection) { }

        public void AddRange(IEnumerable<T> collection,
            NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add) =>
            AddRangeInternal(collection, mode);

        public void RemoveRange(IEnumerable<T> collection,
            NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Remove) =>
            RemoveRangeInternal(collection, mode);

        public void Replace(T item) =>
            ReplaceRangeInternal(new[] { item });

        public void ReplaceRange(IEnumerable<T> collection) =>
            ReplaceRangeInternal(collection);

        #region IRangeCollectionProxy Impl

        void IRangeCollectionProxy.AddRange(IEnumerable collection, NotifyCollectionChangedAction mode) =>
            AddRange(collection.Cast<T>(), mode);

        void IRangeCollectionProxy.RemoveRange(IEnumerable collection, NotifyCollectionChangedAction mode) =>
            RemoveRange(collection.Cast<T>(), mode);

        void IRangeCollectionProxy.Replace(object item) =>
            Replace((T)item);

        void IRangeCollectionProxy.ReplaceRange(IEnumerable collection) =>
            ReplaceRange(collection.Cast<T>());

        #endregion

        #region Internals

        protected virtual void ClearInternal() => ClearItems();

        protected virtual void AddRangeInternal(IEnumerable<T> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Add)
        {
            if (collection == null) return;

            if (mode != NotifyCollectionChangedAction.Add && mode != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(mode));

            CheckReentrancy();

            if (mode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in collection)
                {
                    Items.Add(item);
                }

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                var startIndex = Count;
                var changedItems = collection is List<T> list ? list : new List<T>(collection);
                foreach (var item in changedItems)
                {
                    Items.Add(item);
                }

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        protected virtual void RemoveRangeInternal(IEnumerable<T> collection, NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Reset)
        {
            if (collection == null) return;

            if (mode != NotifyCollectionChangedAction.Remove && mode != NotifyCollectionChangedAction.Reset)
                throw new ArgumentException("Mode must be either Remove or Reset for RemoveRange.", nameof(mode));

            CheckReentrancy();

            if (mode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in collection)
                {
                    Items.Remove(item);
                }

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return;
            }

            var changedItems = collection is List<T> list ? list : new List<T>(collection);
            for (var i = 0; i < changedItems.Count; i++)
            {
                if (Items.Remove(changedItems[i])) continue;

                changedItems.RemoveAt(i);
                i--;
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems, -1));
        }

        protected virtual void ReplaceRangeInternal(IEnumerable<T> collection)
        {
            if (collection == null) return;

            ClearInternal();
            AddRangeInternal(collection, NotifyCollectionChangedAction.Reset);
        }

        protected virtual void ReplaceRangeLazyInternal(Func<IEnumerable<T>> getCollection)
        {
            if (getCollection == null) return;

            ClearInternal();
            AddRangeInternal(getCollection(), NotifyCollectionChangedAction.Reset);
        }

        #endregion
    }
}
