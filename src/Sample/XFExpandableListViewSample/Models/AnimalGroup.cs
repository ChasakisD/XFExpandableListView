using System;
using XFExpandableListView.Abstractions;
using XFExpandableListView.Models;

namespace XFExpandableListViewSample.Models
{
    public class AnimalGroup : ExpandableGroup<Animal>
    {
        public string Title { get; set; }
        public string ShortName { get; set; }

        public AnimalGroup(string title, string shortName, bool expanded = true) : base(Guid.NewGuid())
        {
            Title = title;
            ShortName = shortName;
            IsExpanded = expanded;
        }

        public override IExpandableGroup NewInstance()
        {
            return new AnimalGroup(Title, ShortName, IsExpanded) { Id = Id };
        }
    }
}
