using XFExpandableListView.Abstractions;

namespace XFExpandableListView.EventArgs
{
    public class ExpandableGroupEventArgs : System.EventArgs
    {
        public IExpandableGroup SelectedGroup { get; set; }

        public ExpandableGroupEventArgs(IExpandableGroup selectedGroup)
        {
            SelectedGroup = selectedGroup;
        }
    }
}
