using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using XFExpandableListView.Abstractions;

namespace XFExpandableListView.Models
{
    public class ExpandableGroup<T> : ObservableCollection<T>, IExpandableGroup
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
    }
}
