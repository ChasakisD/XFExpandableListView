using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XFExpandableListView.Abstractions;
using XFExpandableListViewSample.Models;

namespace XFExpandableListViewSample.ViewModels
{
    public class FruitsListViewPageViewModel : NotifyingObject
    {
        private Command<Label> _toggleCommand;
        public Command<Label> ToggleCommand
        {
            get => _toggleCommand;
            set => Set(ref _toggleCommand, value);
        }

        private ObservableCollection<FruitGroup> _fruitsAllGroupsCollection;
        public ObservableCollection<FruitGroup> FruitslAllGroupsCollection
        {
            get => _fruitsAllGroupsCollection;
            set => Set(ref _fruitsAllGroupsCollection, value);
        }

        public FruitsListViewPageViewModel()
        {
            ToggleCommand = new Command<Label>(async view =>
            {
                ViewExtensions.CancelAnimations(view);
                await view.RotateTo((int)view.Rotation == 0 ? 180 : 0);
            });

            FruitslAllGroupsCollection = new ObservableCollection<FruitGroup>{
                new FruitGroup("Sweet","S",false){
                    new Fruit(
                        "Apple",
                        @"An apple is an edible fruit produced by an apple tree (Malus domestica).\n Apple trees are cultivated worldwide and are the most widely grown species in the genus Malus. The tree originated in Central Asia, where its wild ancestor, Malus sieversii, is still found today.\n Apples have been grown for thousands of years in Asia and Europe and were brought to North America by European colonists.\n Apples have religious and mythological significance in many cultures, including Norse, Greek and European Christian tradition.",
                        "apple_small.gif"),
                    new Fruit(
                        "Banana",
                        "A banana is an elongated, edible fruit – botanically a berry – produced by several kinds of large herbaceous flowering plants in the genus Musa. In some countries, bananas used for cooking may be called \"plantains\", distinguishing them from dessert bananas.",
                        "banana_small.gif"),
                    new Fruit(
                        "Cherry",
                        "A cherry is the fruit of many plants of the genus Prunus, and is a fleshy drupe (stone fruit).",
                        "cherries_small.jpg"),
                    new Fruit(
                        "Watermelon",
                        "Watermelon (Citrullus lanatus) is a plant species in the family Cucurbitaceae, a vine-like flowering plant originally domesticated in West Africa.\n It is a highly cultivated fruit worldwide, having more than 1000 varieties.",
                        "watermelon_small.gif")
                },
                new FruitGroup("Sour","O",false){
                    new Fruit(
                        "Lemon",
                        "The lemon, Citrus limon, is a species of small evergreen tree in the flowering plant family Rutaceae, native to South Asia, primarily North eastern India.\n Its fruits are round in shape.",
                        "lemon_small.gif"),
                    new Fruit(
                        "Mulberry",
                        "Morus, a genus of flowering plants in the family Moraceae, consists of diverse species of deciduous trees commonly known as mulberries, growing wild and under cultivation in many temperate world regions.",
                        "mulberries_small.gif"),
                    new Fruit(
                        "Raspberries",
                        "World production of raspberries in 2018 was 870,209 tonnes, led by Russia with 19% of the world total.",
                        "raspberries_small.gif")
                },
                new FruitGroup("Mix here","M",true)
            };
        }
    }
}
