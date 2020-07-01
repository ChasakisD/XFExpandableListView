using System;
using XFExpandableListView.Abstractions;
using XFExpandableListView.Models;

namespace XFExpandableListViewSample.Models
{
    public class FruitGroup : ExpandableGroup<Fruit>, IExpandableGroup
    {
        public string Title { get; set; }
        public string ShortName { get; set; }

        public FruitGroup(string title, string shortName, bool expanded = true) : base(Guid.NewGuid())
        {
            Title = title;
            ShortName = shortName;
            IsExpanded = expanded;
        }

        public override IExpandableGroup NewInstance()
        {
            return new FruitGroup(Title, ShortName, IsExpanded) { Id = Id };
        }
    }
}
