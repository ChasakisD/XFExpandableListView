using XFExpandableListView.Abstractions;

namespace XFExpandableListView.EventArgs
{
    /// <inheritdoc />
    /// <summary>
    /// Event Args for the ExpandableListView
    /// </summary>
    public class ExpandableGroupEventArgs : System.EventArgs
    {
        /// <summary>
        /// The Selected Group
        /// </summary>
        public IExpandableGroup SelectedGroup { get; set; }

        public ExpandableGroupEventArgs(IExpandableGroup selectedGroup)
        {
            SelectedGroup = selectedGroup;
        }
    }
}
