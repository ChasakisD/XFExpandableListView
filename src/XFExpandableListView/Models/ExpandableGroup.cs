﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    }
}
