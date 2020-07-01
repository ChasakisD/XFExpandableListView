using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFExpandableListViewSample.Models;

namespace XFExpandableListViewSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DinamicExpandableListViewPage : ContentPage
    {
        private List<Fruit> FruitsMix = new List<Fruit>();
        public DinamicExpandableListViewPage()
        {
            InitializeComponent();
            FruitsMix = new List<Fruit>(this.ViewModel.FruitslAllGroupsCollection[0]);
            FruitsMix.AddRange(this.ViewModel.FruitslAllGroupsCollection[1]);
        }

        private void AddFruit(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, FruitsMix.Count - 1);
            this.ViewModel.FruitslAllGroupsCollection[2].Add(FruitsMix[randomIndex]);
            this.FruitsExpandableCollection.UpdateExpandedItems();//IMPORTANT IF COMMENTED COLLECTION VIEW NOT UPDATING
        }
        private void ClearMix(object sender, EventArgs e)
        {
            this.ViewModel.FruitslAllGroupsCollection[2].Clear();
            this.FruitsExpandableCollection.UpdateExpandedItems();//IMPORTANT IF COMMENTED COLLECTION VIEW NOT UPDATING
        }
        
    }
}