using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Agency
{
	public class LoginViewModel : BindableBaseNotify
	{
		private INavigation navigation;
		private IHUDProgress HUDProgress = DependencyService.Get<IHUDProgress>();

		private string email;
		public string Email
		{
			get { return this.email; }
			set { this.SetProperty(ref this.email, value, "Email"); }
		}

		private string password;
		public string Password
		{
			get { return this.password; }
			set { this.SetProperty(ref this.password, value, "Password"); }
		}

		private bool flag = true;
		private Command authenticationCommand;
		public ICommand AuthenticationCommand
		{
			get
			{
				return authenticationCommand ?? (authenticationCommand = new Command(async () =>
				{
					if (!string.IsNullOrWhiteSpace(this.Email) && !string.IsNullOrWhiteSpace(this.Password)) 
					{
						//Prevent from multiple 'Login' clicks
						if (flag) await Authenticate();
					}
			    }));
			}
		}

		public LoginViewModel(INavigation navigation)
		{
			this.navigation = navigation;
			MessagingCenter.Subscribe<ListViewPage>(this, "ClearEntryStrings", (obj) => { this.Email = string.Empty; this.Password = string.Empty; });
		}

		private async Task<string> GetToken()
		{
			using (HttpClient client = new HttpClient())
			{
				client.Timeout = TimeSpan.FromMilliseconds(4000);
				HttpContent content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("username", this.Email),
					new KeyValuePair<string, string>("password", this.Password),
					new KeyValuePair<string, string>("grant_type", "password")
				});
				try
				{
					HttpResponseMessage response = await client.PostAsync(string.Concat(Global.connectionUri, "/Token"), content);
					if (response.IsSuccessStatusCode)
					{
						string result = await response.Content.ReadAsStringAsync(); //Returns serialized object.
						dynamic token = JsonConvert.DeserializeObject(result);
						return token["access_token"].ToString();
					}
					return response.StatusCode.ToString();
				}
				catch (Exception e)
				{
					if (e is WebException || e is TaskCanceledException) return null; 
					throw;
				}
			}
		}

		private async Task Authenticate()
		{
			flag = false;
			HUDProgress.Show("Loading...");
			using (HttpClient client = new HttpClient())
			{
				Global.authorizationToken = await GetToken(); //Do not block the execution.
				string token = Global.authorizationToken;

				if (token == null) { HUDProgress.ShowErrorWithStatus("Timeout: server not found"); flag = true; return; }
				if (token == "BadRequest") { HUDProgress.ShowErrorWithStatus("Invalid authentication"); flag = true; return; }

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				try
				{
					HttpResponseMessage response = await client.GetAsync(string.Concat(Global.connectionUri, "/api/values/1"));
					if (response.IsSuccessStatusCode)
					{
						string result = await response.Content.ReadAsStringAsync();
						HUDProgress.ShowSuccessWithStatus(string.Format("Welcome {0}.", JsonConvert.DeserializeObject(result)));

						await Task.Delay(800);
						await navigation.PushAsync(new ListViewPage());
					}
				}
				catch (Exception) { throw; }
			}
			flag = true;
		}
	}
}