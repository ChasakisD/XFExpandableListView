using System;
using System.Collections;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFExpandableListView.EventArgs;

namespace XFExpandableListView.Abstractions
{
    /// <summary>
    /// The main Controller of the ListView
    /// Use it on custom control with your own risk
    /// </summary>
    public interface IExpandableController
    {
        #region [Properties]

        /// <summary>
        /// Contains the entire list of the items. Do not use ItemsSource
        /// </summary>
        IList AllGroups { get; }

        /// <summary>
        /// If the Collapsing Mode is Enabled
        /// </summary>
        bool IsCollapsingEnabled { get; }

        /// <summary>
        /// The Command that will be executed on the group header click
        /// </summary>
        Command GroupHeaderCommand { get; }

        /// <summary>
        /// The Parameter that will be passed into the Group Header Command
        /// </summary>
        object GroupHeaderCommandParameter { get; }

        #endregion

        #region [Control Methods]

        /// <summary>
        /// The method that will expand the group in the specified
        /// position if it is already collapsed. Ensure that you are
        /// calling the base class while overriding it.
        /// </summary>
        /// <param name="position">The position of the group</param>
        /// <returns></returns>
        void Expand(int position);

        /// <summary>
        /// The method that will collapse the group in the specified
        /// position if it is already expanded. Ensure that you are
        /// calling the base class while overriding it.
        /// </summary>
        /// <param name="position">The position of the group</param>
        /// <returns></returns>
        void Collapse(int position);

        /// <summary>
        /// The method that Collapses/Expands the group in the specified position
        /// This method is called by the ExpandableGroupCell. Ensure that you are
        /// calling the base class while overriding it.
        /// </summary>
        /// <param name="position">The position of the group</param>
        /// <returns></returns>
        void ToggleGroup(int position);

        /// <summary>
        /// The method that Collapses/Expands the selected group
        /// This method is called by the ExpandableGroupCell. Ensure that you are
        /// calling the base class while overriding it.
        /// </summary>
        /// <param name="group">The group to be toggled</param>
        /// <returns></returns>
        void ToggleGroup(IExpandableGroup group);

        /// <summary>
        /// Search and returns the AllGroups' position with the specified id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns></returns>
        int GetAllGroupPositionById(Guid id);

        /// <summary>
        /// Search and returns the AllGroups' group with the specified id
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns></returns>
        IExpandableGroup GetAllGroupById(Guid id);

        /// <summary>
        /// Method that handles the GroupClicked Event
        /// </summary>
        /// <param name="group"></param>
        void OnGroupClicked(IExpandableGroup group);

        #endregion

        #region [Events]

        /// <summary>
        /// The Event that will be invoked when the user clicks on the Group Item
        /// </summary>
        event EventHandler<ExpandableGroupEventArgs> GroupClicked;

        /// <summary>
        /// The Event that will be invoked when a group will be expanded
        /// </summary>
        event EventHandler<ExpandableGroupEventArgs> GroupExpanded;

        /// <summary>
        /// The Event that will be invoked when a group will be collapsed
        /// </summary>
        event EventHandler<ExpandableGroupEventArgs> GroupCollapsed;

        #endregion
    }
}
