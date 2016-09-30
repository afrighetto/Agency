using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;
namespace Agency
{
	public class AddItemsViewModel : BindableBaseNotify
	{
		private AddItemsPage page;
		private INavigation navigation;
		private string message;
		private Employee e;
		private IHUDProgress HUDProgress = DependencyService.Get<IHUDProgress>();

		private string title;
		public string Title
		{
			get { return this.title; }
			set { this.SetProperty(ref this.title, value, "Title"); }
		}

		private string lastName;
		public string LastName 
		{ 
			get { return this.lastName; }
			set { this.SetProperty(ref this.lastName, value, "LastName"); }
		}

		private string firstName;
		public string FirstName
		{
			get { return this.firstName; }
			set { this.SetProperty(ref this.firstName, value, "FirstName"); }
		}

		private DateTime date;
		public DateTime Date
		{
			get { return this.date; }
			set { this.SetProperty(ref this.date, value, "Date"); }
		}

		private string region;
		public string Region
		{
			get { return this.region; }
			set { this.SetProperty(ref this.region, value, "Region"); }
		}

		private string city;
		public string City
		{
			get { return this.city; }
			set { this.SetProperty(ref this.city, value, "City"); }
		}

		private string postalCode;
		public string PostalCode
		{
			get { return this.postalCode; }
			set { this.SetProperty(ref this.postalCode, value, "PostalCode"); }
		}

		private string actionCommandText;
		public string ActionCommandText
		{
			get { return this.actionCommandText; }
			set { this.SetProperty(ref this.actionCommandText, value, "ActionCommandText"); }
		}

		private string titleSection;
		public string TitleSection
		{
			get { return this.titleSection; }
			set { this.SetProperty(ref this.titleSection, value, "TitleSection"); }
		}

		private Command save;
		public ICommand Save
		{
			get
			{
				return save ?? (save = new Command(async () =>
				{
					if (string.IsNullOrWhiteSpace(this.FirstName) || string.IsNullOrWhiteSpace(this.LastName))
					{
						await page.DisplayAlert("Names cannot be set to NULL", "", "Ok");
					}
					else 
					{
						Employee employee = new Employee {
							FirstName = this.FirstName, LastName = this.LastName, BirthDate = this.Date, Region = this.Region, City = this.City, PostalCode = this.PostalCode
						};
						await ((message == "Update") ? UpdateEmployee(employee) : CreateEmployee(employee));
					}
				}));
			}
		}

		public AddItemsViewModel(AddItemsPage page, INavigation navigation, string message, Employee e)
		{
			this.page = page;
			this.navigation = navigation;
			this.message = message;
			this.e = e;

			switch (message) 
			{
				case "Update":
					Title = string.Concat(e.LastName, " ", e.FirstName);
					TitleSection = "Update employee";
					ActionCommandText = "Update employee...";
					this.LastName = e.LastName;
					this.FirstName = e.FirstName;
					this.Date = e.BirthDate;
					this.Region = e.Region;
					this.City = e.City;
					this.PostalCode = e.PostalCode;
					break;
				case "Create":
					Title = "New employee";
					TitleSection = "Create employee";
					ActionCommandText = "Add a new employee...";
					break;
			}
		}

		private async Task CreateEmployee(Employee employee)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpContent content = new StringContent(JsonConvert.SerializeObject(employee), System.Text.Encoding.UTF8, "application/json");
				try
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Global.authorizationToken);
					HttpResponseMessage response = await client.PostAsync(string.Concat(Global.connectionUri, "/api/employee"), content);
					if (response.IsSuccessStatusCode)
					{
						HUDProgress.ShowMessageWithImage("employee_added.png", "Successfully added");
						await Task.Delay(800);
						MessagingCenter.Send(page, "RefreshingListEvent");
						await navigation.PopAsync(true);
					}
					else { HUDProgress.ShowErrorWithStatus("Could not add employee"); }
				}
				catch (Exception) { throw; }
			}
		}

		private async Task UpdateEmployee(Employee employee) 
		{
			using (HttpClient client = new HttpClient())
			{
				HttpContent content = new StringContent(JsonConvert.SerializeObject(employee), System.Text.Encoding.UTF8, "application/json");
				try
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Global.authorizationToken);
					HttpResponseMessage response = await client.PutAsync(string.Concat(Global.connectionUri, "/api/employee/", e.EmployeeID), content);
					if (response.StatusCode == HttpStatusCode.OK)
					{
						HUDProgress.ShowMessageWithImage("employee_updated.png", "Successfully updated");
						await Task.Delay(800);
						MessagingCenter.Send(page, "RefreshingListEvent");
						await navigation.PopAsync(true);
					}
					else if (response.StatusCode == HttpStatusCode.NotModified) 
					{
						HUDProgress.ShowMessageWithImage("unmodified.png", "Nothing changed");
					}
					else { HUDProgress.ShowErrorWithStatus("Could not update employee"); }
				}
				catch (Exception) { throw; }
			}
		}
	}
}