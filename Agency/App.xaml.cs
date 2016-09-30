using System.Threading.Tasks;
using Xamarin.Forms;

namespace Agency
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();
			NavigationPage navPage = new NavigationPage(new LoginPage())
			{
				BarBackgroundColor = Color.FromRgb(246, 132, 132),
				BarTextColor = Color.White
			};

			MainPage = navPage;

			navPage.Pushed += (object sender, NavigationEventArgs e) =>
			{
				if (e.Page.GetType() == typeof(ListViewPage))
				{
					MessagingCenter.Send((ListViewPage)e.Page, "ClearEntryStrings");
				}
			};

			/*navPage.Popped += async (object sender, NavigationEventArgs e) => 
			{
				System.Diagnostics.Debug.WriteLine("Popped sender: " + ((NavigationPage)sender).CurrentPage.GetType());
				System.Diagnostics.Debug.WriteLine("Popped e: " + e.Page.GetType());
				if (e.Page.GetType() == typeof(ListViewPage)) 
				{
					await e.Page.Navigation.PushModalAsync(navPage, true);
					//await e.Page.Navigation.PushAsync(new LoginPage(), true);

					//Task<bool> action = navPage.CurrentPage.DisplayAlert("Wanna go out?", "", "Yes", "Nope");
					//await action.ContinueWith(task => { if (task.Result) navPage.CurrentPage.DisplayAlert("OK", "", "Ok"); });
				}
				if (e.Page.GetType() == typeof(AddItemsPage)) 
				{
					System.Diagnostics.Debug.WriteLine("Popped sender: " + ((NavigationPage)sender).CurrentPage.GetType());
					System.Diagnostics.Debug.WriteLine("Popped e: " + e.Page.GetType());
					await e.Page.Navigation.PushAsync(new ListViewPage());
					e.Page.Navigation.RemovePage(((NavigationPage)sender).CurrentPage);
				}
			};*/

			Global.connectionUri = "https://localhost:44396";
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}