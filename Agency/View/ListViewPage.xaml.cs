using System;
using Xamarin.Forms;

namespace Agency
{
	public partial class ListViewPage : ContentPage
	{
		public ListViewPage()
		{
			InitializeComponent();
			BindingContext = new ListViewModel(this, this.Navigation);

			listView.ItemTemplate = new DataTemplate(typeof(CustomCell));
			listView.ItemTemplate.SetBinding(CustomCell.TextProperty, "FirstLastName");
			listView.ItemTemplate.SetBinding(CustomCell.DetailProperty, "City");

			Content = listView;
			NavigationPage.SetBackButtonTitle(this, "Back");

			listView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				await Navigation.PushAsync(new AddItemsPage("Update", (Employee)e.Item));
				((ListView)sender).SelectedItem = null;
			};
		}
	}
}