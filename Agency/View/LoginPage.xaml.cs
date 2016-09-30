using System.Threading.Tasks;
using Xamarin.Forms;

namespace Agency
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			BindingContext = new LoginViewModel(this.Navigation);

			NavigationPage.SetBackButtonTitle(this, "Logout");
		}
	}
}