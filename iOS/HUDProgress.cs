using System;
using BigTed;
using UIKit;
using Xamarin.Forms;
using System.Threading.Tasks;
using Agency.iOS;

[assembly: Dependency(typeof(HUDProgress))]
namespace Agency.iOS
{
	public class HUDProgress : IHUDProgress
	{
		public void Show() { BTProgressHUD.Show(); }
		public void Show(string message) { BTProgressHUD.Show(message); }
		public void ShowErrorWithStatus(string message)
		{
			BTProgressHUD.ShowImage(UIImage.FromFile("notdone.png"), message);
		}
		public void ShowSuccessWithStatus(string message) 
		{
			BTProgressHUD.ShowImage(UIImage.FromFile("done.png"), message);
		}
		public void ShowMessageWithImage(string path, string message)
		{
			BTProgressHUD.ShowImage(UIImage.FromFile(path), message);
		}
		public void Dismiss() { BTProgressHUD.Dismiss(); }
	}
}