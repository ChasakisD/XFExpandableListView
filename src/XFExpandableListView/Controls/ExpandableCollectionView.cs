using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using MvvmHelpers;
using Xamarin.Forms;
using XFExpandableListView.Abstractions;
using XFExpandableListView.EventArgs;
using XFExpandableListView.Utils;

namespace XFExpandableListView.Controls
{
    public class ExpandableCollectionView : CollectionView, IExpandableController
    {
        #region [Bindable Properties]

        /// <summary>
        /// The Bindable Property for All Groups
        /// </summary>
        public static BindableProperty AllGroupsProperty = BindableProperty.Create(
            nameof(AllGroups), typeof(IList), typeof(ExpandableListView), null, propertyChanged: AllGroupsChanged);

        public IList AllGroups
        {
            get => (IList)GetValue(AllGroupsProperty);
            set => SetValue(AllGroupsProperty, value);
        }

        /// <summary>
        /// The Bindable Property for the GroupHeader Command
        /// </summary>
        public static BindableProperty GroupHeaderCommandProperty = BindableProperty.Create(
            nameof(GroupHeaderCommand), typeof(Command), typeof(ExpandableListView));

        public Command GroupHeaderCommand
        {
            get => (Command)GetValue(GroupHeaderCommandProperty);
            set => SetValue(GroupHeaderCommandProperty, value);
        }

        /// <summary>
        /// The Bindable Property for the GroupHeader Command Parameter
        /// </summary>
        public static BindableProperty GroupHeaderCommandParameterProperty = BindableProperty.Create(
            nameof(GroupHeaderCommandParameter), typeof(object), typeof(ExpandableListView));

        public object GroupHeaderCommandParameter
        {
            get => GetValue(GroupHeaderCommandParameterProperty);
            set => SetValue(GroupHeaderCommandParameterProperty, value);
        }

        /// <summary>
        /// The Bindable Property for the CollapsingMode
        /// </summary>
        public static BindableProperty IsCollapsingEnabledProperty = BindableProperty.Create(
            nameof(IsCollapsingEnabled), typeof(bool), typeof(ExpandableListView), true);

        public bool IsCollapsingEnabled
        {
            get => (bool)GetValue(IsCollapsingEnabledProperty);
            set => SetValue(IsCollapsingEnabledProperty, value);
        }

        #endregion

        #region [Events]

        public event EventHandler<ExpandableGroupEventArgs> GroupClicked;
        public event EventHandler<ExpandableGroupEventArgs> GroupExpanded;
        public event EventHandler<ExpandableGroupEventArgs> GroupCollapsed;

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ExpandableCollectionView()
        {
            IsGrouped = true;
        }

        #region [Property Changed]

        static void AllGroupsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ExpandableCollectionView control)) return;

            /* UnSubscribe to the previous events */
            if (oldValue is INotifyCollectionChanged oldItemsSource)
            {
                oldItemsSource.CollectionChanged -= control.GroupsCollectionChanged;
            }

            if (!(newValue is IList items)) return;
            if (!(newValue is INotifyCollectionChanged newItemsSource)) return;

            /* Copy the groups to the ItemsSource */
            control.ItemsSource = new ObservableRangeCollection<IExpandableGroup>(
                items.Cast<IExpandableGroup>().Select(x => x.NewInstance()));

            /* Subscribe to CollectionChanged Event to Update the ItemsSource with any AllGroups updates */
            newItemsSource.CollectionChanged += control.GroupsCollectionChanged;

            /* Expand isExpanded Headers */
            control.UpdateExpandedItems();
        }

        void GroupsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(ItemsSource is ObservableRangeCollection<IExpandableGroup> localItemsSource)) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                    /* Remove the deleted items on the itemsSource */
                    if (e.OldItems != null)
                    {
                        var oldItems = e.OldItems
                            .Cast<IExpandableGroup>()
                            .Select(x => localItemsSource.FirstOrDefault(i => i.Id == x.Id));

                        localItemsSource.RemoveRange(oldItems);
                    }

                    /* Add New Items */
                    if (e.NewItems != null)
                    {
                        var newItems = e.NewItems
                            .Cast<IExpandableGroup>()
                            .Select(x => x.NewInstance())
                            .ToList();

                        localItemsSource.AddRange(newItems);

                        foreach (IExpandableGroup newItem in e.NewItems)
                        {
                            var itemCopy = newItems.FirstOrDefault(
                                x => x.Id == newItem.Id);

                            if (!newItem.IsExpanded) continue;
                            itemCopy.AddRange(newItem);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    /* Create a copy of the item and replace the old one */
                    localItemsSource[e.OldStartingIndex] = ((IExpandableGroup)e.NewItems[e.NewStartingIndex]).NewInstance();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    /* Clear the itemsSource too */
                    localItemsSource.Clear();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region [Expandable Controller Implementation]

        public void OnGroupClicked(IExpandableGroup group) =>
            GroupClicked?.Invoke(this, new ExpandableGroupEventArgs(group));

        public virtual void ToggleGroup(IExpandableGroup group)
        {
            if (group == null) return;

            var allGroupIndex = GetAllGroupPositionById(group.Id);

            ToggleGroup(allGroupIndex);
        }

        public virtual void ToggleGroup(int position)
        {
            if (position < 0 || position > AllGroups.Count) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;

            if (expandableGroup.IsExpanded) Collapse(position);
            else Expand(position);
        }

        public virtual void Expand(int position)
        {
            if (AllGroups.Count < position) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;
            if (!(((IList)ItemsSource)[position] is IExpandableGroup itemsSourceGroup)) return;

            if (expandableGroup.IsExpanded) return;

            expandableGroup.IsExpanded = true;

            /* It's important to do this in main thread */
            itemsSourceGroup.AddRange(expandableGroup);

            /* Invoke Expanded the Event */
            GroupExpanded?.Invoke(this, new ExpandableGroupEventArgs(expandableGroup));
        }

        public virtual void Collapse(int position)
        {
            if (AllGroups.Count < position) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;
            if (!(((IList)ItemsSource)[position] is IExpandableGroup itemsSourceGroup)) return;

            if (!expandableGroup.IsExpanded) return;

            expandableGroup.IsExpanded = false;

            /* It's important to do this in main thread */
            itemsSourceGroup.RemoveRange(expandableGroup);

            /* Invoke Collapsed the Event */
            GroupCollapsed?.Invoke(this, new ExpandableGroupEventArgs(expandableGroup));
        }

        public virtual IExpandableGroup GetAllGroupById(Guid id)
        {
            return AllGroups.Cast<IExpandableGroup>().FirstOrDefault(x => x.Id == id);
        }

        public virtual int GetAllGroupPositionById(Guid id)
        {
            for (var i = 0; i < AllGroups.Count; i++)
            {
                if (!(AllGroups[i] is IExpandableGroup group)) return -1;
                if (group.Id == id) return i;
            }

            return -1;
        }

        #endregion

        #region [Helpers]

        void UpdateExpandedItems()
        {
            if (!(ItemsSource is IList groups)) return;

            var updatedItemsSource = new ObservableRangeCollection<IExpandableGroup>(
                groups.Cast<IExpandableGroup>().Select(x => x.NewInstance()));

            /* Hard operations must not affect the UI Thread */
            for (var i = 0; i < AllGroups.Count; i++)
            {
                var group = (IExpandableGroup)AllGroups[i];
                var itemsSourceGroup = (IExpandableGroup)updatedItemsSource[i];

                if (!group.IsExpanded) continue;

                itemsSourceGroup.AddRange(group);
            }

            ItemsSource = updatedItemsSource;
        }

        #endregion
    }
}
