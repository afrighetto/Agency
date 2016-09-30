using System;
using UIKit;
using Xamarin.Forms;
using Agency.iOS;

[assembly: ExportRenderer(typeof(DatePicker), typeof(CustomDatePicker))]
namespace Agency.iOS
{
	public class CustomDatePicker : global::Xamarin.Forms.Platform.iOS.DatePickerRenderer 
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);
			Control.BorderStyle = UITextBorderStyle.None;
		}
	}
}

