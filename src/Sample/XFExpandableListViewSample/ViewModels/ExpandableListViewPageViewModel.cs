using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using XFExpandableListView.Abstractions;
using XFExpandableListViewSample.Models;

namespace XFExpandableListViewSample.ViewModels
{
    public class ExpandableListViewPageViewModel : NotifyingObject
    {
        private Command<View> _toggleCommand;
        public Command<View> ToggleCommand
        {
            get => _toggleCommand;
            set => Set(ref _toggleCommand, value);
        }

        private ObservableCollection<AnimalGroup> _animalAllGroupsCollection;
        public ObservableCollection<AnimalGroup> AnimalAllGroupsCollection
        {
            get => _animalAllGroupsCollection;
            set => Set(ref _animalAllGroupsCollection, value);
        }

        public ExpandableListViewPageViewModel()
        {
            ToggleCommand = new Command<View>(async view =>
            {
                ViewExtensions.CancelAnimations(view);
                await view.RotateTo((int)view.Rotation == 0 ? 180 : 0);
            });

            AnimalAllGroupsCollection = new ObservableCollection<AnimalGroup>{
                new AnimalGroup("Monkeys","M"){
                    new Animal(
                        "Spider Monkey", 
                        "There are 7 known sub species of the Spider Monkey",
                        "spider_monkey.jpg"),
                    new Animal(
                        "Squirrel Monkey",
                        "The Common Squirrel Monkey is one that gets its name for looking very similar to the Squirrel",
                        "squirrel_monkey_species.jpg"),
                    new Animal(
                        "Vervet Monkey",
                        "There are 5 known subspecies that have been identified",
                        "vervet_specie.jpg"),
                    new Animal(
                        "Proboscis Monkey",
                        "The Proboscis Monkey is also called the Monyet Belanda Monkey, which means the long nosed Monkey",
                        "proboscis_monkey.jpg")
                },
                new AnimalGroup("Bears","B"){
                    new Animal(
                        "Brown Bear",
                        "The Brown Bear can be found in Alaska, western Canada,and parts of Washington, Montana and Wyoming",
                        "charlie_russel_photo_b_150x150.jpg"),
                    new Animal(
                        "Polar Bear",
                        "Polar bears are among the largest bears in the world. Adult males may reach 800 kilograms (kg) or 1760 pounds (lbs)",
                        "polar_bears_b_roy_dhhaas_150x150.jpg"),
                    new Animal(
                        "Asiatic black bear",
                        "Asiatic black bears have long black fur with a distinct white patch on the chest that is often crescent-shaped",
                        "asiatic_black_bear_b_wild_150x150.jpg")
                }
            };
        }
    }
}
