# XFExpandableListView
An Implementation of Expandable ListView Control in Xamarin.Forms.

[![Build status](https://build.appcenter.ms/v0.1/apps/133697af-20ff-41f3-91a4-22c5d99d2efd/branches/master/badge)](https://appcenter.ms) [![NuGet Version](https://buildstats.info/nuget/XFExpandableListView)](https://www.nuget.org/packages/XFExpandableListView)

# Documentation

![Alt Text](https://media.giphy.com/media/1AfbEInjrtTcmVepOM/giphy.gif) |  ![Alt Text](https://media.giphy.com/media/UTHHVrMsLSEI2gw5bc/giphy-downsized.gif)

## Getting Started

Just add the XFExpandableListView nugget to your main standard project and especific plattaform

https://www.nuget.org/packages/XFExpandableListView/


### Example of use
Add ExpandableCollectionView to your page
```
xmlns:controls="clr-namespace:XFExpandableListView.Controls;assembly=XFExpandableListView"
...
    <ContentPage.BindingContext>
        <viewModels:ExpandableListViewPageViewModel /> //ExpandableListView ViewModel
    </ContentPage.BindingContext>
...
 <controls:ExpandableCollectionView 
AllGroups={Binding AnimalAllGroupsCollection}" //Binding to your collection group
ItemSizingStrategy="MeasureAllItems" ItemsLayout="VerticalList" SelectionMode="None">
```
### View model definition
```
 public class ExpandableListViewPageViewModel : NotifyingObject //Inherit from NotifyingObject
    {
        private Command<View> _toggleCommand;  //Toogle group command
        public Command<View> ToggleCommand
        {
            get => _toggleCommand;
            set => Set(ref _toggleCommand, value);
        }

        private ObservableCollection<AnimalGroup> _animalAllGroupsCollection; //Define an observable collection
        public ObservableCollection<AnimalGroup> AnimalAllGroupsCollection    //to save the groups
        {
            get => _animalAllGroupsCollection;
            set => Set(ref _animalAllGroupsCollection, value);
        }

        public ExpandableListViewPageViewModel()
        {
            ToggleCommand = new Command<View>(async view =>  //Initialize Toggle command
            {
                ViewExtensions.CancelAnimations(view);
                await view.RotateTo((int)view.Rotation == 0 ? 180 : 0);
            });
            
            //Initialize Groups
            AnimalAllGroupsCollection = new ObservableCollection<AnimalGroup>{
                new AnimalGroup("Monkeys","M"){
                    new Animal(
                        "Spider Monkey", 
                        "There are 7 known sub species of the Spider Monkey",
                        "spider_monkey.jpg"),...
                },
                new AnimalGroup("Bears","B"){
                    new Animal(
                        "Brown Bear",
                        "The Brown Bear can be found in Alaska, western Canada,and parts of Washington",
                        "charlie_russel_photo_b_150x150.jpg")...
                }
            };
        }
    }
```
## Provide your own class type
```
    public class Animal : NotifyingObject
    {
        ... //Define properties
        public Animal()
        {
        }
    }
```


### Dynamically modifying group collections

 Add items
```
        private void AddFruit(object sender, EventArgs e)
        {
           Fruit Fruit = new Fruit("My fruit", "Description", "Image.png");
            this.ViewModel.FruitslAllGroupsCollection[0].Add(Fruit);
            this.FruitsExpandableCollection.UpdateExpandedItems();//IMPORTANT TO UPDATE COLLECTION VIEW
        }
```
 Remove items
```
        private void ClearMix(object sender, EventArgs e)
        {
            this.ViewModel.FruitslAllGroupsCollection[0].Clear();
            this.FruitsExpandableCollection.UpdateExpandedItems();//IMPORTANT TO UPDATE COLLECTION VIEW
        }
```

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Notes
The images are from:  
http://bearwithus.org  
https://www.monkeyworlds.com  
https://www.halfyourplate.ca/fruits
