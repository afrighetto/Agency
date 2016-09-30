using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SQLite;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Agency
{
	public class ListViewModel : BindableBaseNotify
	{
		private SQLiteConnection database = DependencyService.Get<ISQLite>().InitConnection();
		private ListViewPage page;
		private INavigation navigation;
		private static ObservableCollection<Employee> employeesList;
		public ObservableCollection<Employee> EmployeesList 
		{ 
			get { return employeesList; } 
			set { this.SetProperty(ref employeesList, value, "EmployeesList"); }
		}

		/*private DataTemplate template;
		public DataTemplate Template
		{
			get { return template; }
			set { this.SetProperty(ref template, value, "Template"); }
		}*/

		private bool isRefreshing;
		public bool IsRefreshing 
		{
			get { return isRefreshing; }
			set { this.SetProperty(ref isRefreshing, value, "IsRefreshing"); }
		}

		private Command refreshCommand;
		public ICommand RefreshCommand { get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); } }

		private Command addEmployee;
		public ICommand AddEmployee { get { return addEmployee ?? (addEmployee = new Command(async (object obj) => { await navigation.PushAsync(new AddItemsPage("Create", null)); })); } }

		public ListViewModel(ListViewPage page, INavigation navigation)
		{
			this.page = page;
			this.navigation = navigation;

			Device.BeginInvokeOnMainThread(async () => await LoadEmployees());

			//Listen to event when a new message is sent within ContentPage.
			MessagingCenter.Subscribe<AddItemsPage>(this, "RefreshingListEvent", async (obj) => await LoadEmployees());
		}

		//Need to check whether ObservableCollection's constructor is null.
		private async Task LoadEmployees() 
		{
			List<Employee> list = await GetEmployees();
			if (list == null)
			{
				if (database.Table<Employee>().Any())
				{
					await page.DisplayAlert("Missing connection", "A local copy of employees is stored, though it might be outdated. Wanna use that?", "Yes", "No").ContinueWith(task => 
					{
						if (task.Result) EmployeesList = new ObservableCollection<Employee>(database.Table<Employee>());
						else EmployeesList = new ObservableCollection<Employee>();
					});
				}
				else EmployeesList = new ObservableCollection<Employee>();
			}
			else 
			{
				EmployeesList = new ObservableCollection<Employee>(list);
				database.CreateTable<Employee>(); //Does nothing if it exists already.
				if (!database.Table<Employee>().Any())
					database.InsertAll(list);
				else
					database.UpdateAll(list);
			}
			return;
		}

		private static async Task<List<Employee>> GetEmployees()
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					client.Timeout = TimeSpan.FromMilliseconds(2500);
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Global.authorizationToken);
					HttpResponseMessage response = await client.GetAsync(string.Concat(Global.connectionUri, "/api/employee"));
					if (response.IsSuccessStatusCode)
					{
						string result = await response.Content.ReadAsStringAsync();
						return JsonConvert.DeserializeObject<List<Employee>>(result);
					}
				}
				catch (Exception) { throw; }
				return null;
			}
		}

		private static async Task<HttpResponseMessage> DeleteRequest(int id)
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = null;
				try
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Global.authorizationToken);
					response = await client.DeleteAsync(string.Concat(Global.connectionUri, "/api/employee/", id));
				}
				catch (Exception) { throw; }
				return response;
			}
		}

		private async Task ExecuteRefreshCommand()
		{
			if (IsRefreshing) 
				return; 

			IsRefreshing = true;
			await LoadEmployees();
			IsRefreshing = false;
		}

		public static async Task OnDelete(object sender, EventArgs args)
		{
			MenuItem menuItem = (MenuItem)sender;
			int id = (int)menuItem.CommandParameter;

			HttpResponseMessage response = await DeleteRequest(id);
			if (response.IsSuccessStatusCode)
			{
				Employee e = employeesList.First(item => item.EmployeeID == id);
				employeesList.Remove(e);
			}
		}
	}
}

