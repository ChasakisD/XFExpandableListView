using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFExpandableListView.Abstractions;
using XFExpandableListView.EventArgs;

namespace XFExpandableListView.Controls
{
    /// <inheritdoc cref="ListView" />
    /// <summary>
    /// A Grouping ListView that allows you to expand/collase the header items.
    /// Do NOT use ItemsSource, use AllGroups to bind your list
    /// </summary>
    public class ExpandableListView : ListView, IExpandableController
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
        public ExpandableListView()
        {
            IsGroupingEnabled = true;
        }

        #region [Property Changed]

        static async void AllGroupsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is ExpandableListView control)) return;

            /* UnSubscribe to the previous events */
            if (oldValue is INotifyCollectionChanged oldItemsSource)
            {
                oldItemsSource.CollectionChanged -= control.GroupsCollectionChanged;
            }

            if (!(newValue is IList items)) return;
            if (!(newValue is INotifyCollectionChanged newItemsSource)) return;

            /* Copy the groups to the ItemsSource */
            await Task.Run(() =>
            {
                var itemsSource = new ObservableCollection<IExpandableGroup>();
                foreach (IExpandableGroup item in items)
                {
                    itemsSource.Add(item.NewInstance());
                }

                Device.BeginInvokeOnMainThread(() => { control.ItemsSource = itemsSource; });
            });

            /* Subscribe to CollectionChanged Event to Update the ItemsSource with any AllGroups updates */
            newItemsSource.CollectionChanged += control.GroupsCollectionChanged;

            /* Expand isExpanded Headers */
            await control.UpdateExpandedItems();
        }

        void GroupsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(ItemsSource is IList<IExpandableGroup> localItemsSource)) return;

            Device.BeginInvokeOnMainThread(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Remove:
                        /* Remove the deleted items on the itemsSource */
                        if (e.OldItems != null)
                        {
                            foreach (IExpandableGroup oldItem in e.OldItems)
                            {
                                var deletedItem = localItemsSource.FirstOrDefault(x => x.Id == oldItem.Id);
                                if (deletedItem == null) return;

                                localItemsSource.Remove(deletedItem);
                            }
                        }

                        /* Add New Items */
                        if (e.NewItems != null)
                        {
                            foreach (IExpandableGroup newItem in e.NewItems)
                            {
                                var itemCopy = newItem.NewInstance();
                                localItemsSource.Add(itemCopy);

                                if (!newItem.IsExpanded) continue;
                                foreach (var item in newItem)
                                {
                                    itemCopy.Add(item);
                                }
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
            });
        }

        #endregion

        #region [Expandable Controller Implementation]

        public void OnGroupClicked(IExpandableGroup group) =>
            GroupClicked?.Invoke(this, new ExpandableGroupEventArgs(group));

        public virtual async Task ToggleGroup(IExpandableGroup group)
        {
            var allGroupIndex = await GetAllGroupPositionById(group.Id);

            await ToggleGroup(allGroupIndex);
        }

        public virtual async Task ToggleGroup(int position)
        {
            if (position < 0 || position > AllGroups.Count) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;

            if (expandableGroup.IsExpanded) await Collapse(position);
            else await Expand(position);
        }

        public virtual async Task Expand(int position)
        {
            if (AllGroups.Count < position) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;
            if (!(((IList)ItemsSource)[position] is IExpandableGroup itemsSourceGroup)) return;

            if (expandableGroup.IsExpanded) return;

            expandableGroup.IsExpanded = true;

            /* Hard operations must not affect the UI Thread */
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in expandableGroup)
                    {
                        itemsSourceGroup.Add(item);
                    }
                });
            });

            /* Invoke Expanded the Event */
            GroupExpanded?.Invoke(this, new ExpandableGroupEventArgs(expandableGroup));
        }

        public virtual async Task Collapse(int position)
        {
            if (AllGroups.Count < position) return;
            if (!(AllGroups[position] is IExpandableGroup expandableGroup)) return;
            if (!(((IList)ItemsSource)[position] is IExpandableGroup itemsSourceGroup)) return;

            if (!expandableGroup.IsExpanded) return;

            expandableGroup.IsExpanded = false;

            /* Hard operations must not affect the UI Thread */
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in expandableGroup)
                    {
                        itemsSourceGroup.Remove(item);
                    }
                });
            });

            /* Invoke Collapsed the Event */
            GroupCollapsed?.Invoke(this, new ExpandableGroupEventArgs(expandableGroup));
        }

        public virtual async Task<IExpandableGroup> GetAllGroupById(Guid id)
        {
            /* Hard operations must not affect the UI Thread */
            return await Task.Run(() =>
            {
                return AllGroups.Cast<IExpandableGroup>().FirstOrDefault(x => x.Id == id);
            });
        }

        public virtual async Task<int> GetAllGroupPositionById(Guid id)
        {
            /* Hard operations must not affect the UI Thread */
            return await Task.Run(() =>
            {
                for (var i = 0; i < AllGroups.Count; i++)
                {
                    if (!(AllGroups[i] is IExpandableGroup group)) return -1;
                    if (group.Id == id) return i;
                }

                return -1;
            });
        }

        #endregion

        #region [Helpers]

        async Task UpdateExpandedItems()
        {
            if (!(ItemsSource is IList groups)) return;

            var updatedItemsSource = new ObservableCollection<IExpandableGroup>();
            foreach (IExpandableGroup group in groups)
            {
                updatedItemsSource.Add(group.NewInstance());
            }

            /* Hard operations must not affect the UI Thread */
            await Task.Run(() =>
            {
                for (var i = 0; i < AllGroups.Count; i++)
                {
                    var group = (IExpandableGroup)AllGroups[i];
                    var itemsSourceGroup = (IExpandableGroup)updatedItemsSource[i];
                    if (!group.IsExpanded) continue;

                    foreach (var item in group)
                    {
                        itemsSourceGroup.Add(item);
                    }
                }

                Device.BeginInvokeOnMainThread(() => { ItemsSource = updatedItemsSource; });
            });

            #endregion
        }
    }
}
