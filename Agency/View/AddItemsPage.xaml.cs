using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Agency
{
	public partial class AddItemsPage : ContentPage
	{
		public AddItemsPage(string message, Employee e)
		{
			InitializeComponent();
			BindingContext = new AddItemsViewModel(this, this.Navigation, message, e);
		}
	}
}

