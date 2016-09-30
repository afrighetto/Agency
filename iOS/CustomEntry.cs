using System;
using Xamarin.Forms;

using CoreAnimation;
using CoreGraphics;
using UIKit;
using Agency.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntry))]
namespace Agency.iOS
{
	public class CustomEntry : global::Xamarin.Forms.Platform.iOS.EntryRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			CALayer borderBottom = new CALayer();
			borderBottom.Frame = new CGRect(x: 0.0f, y: Control.Frame.Size.Height - 1.0f, width: Control.Frame.Size.Width, height: 1.0f);

			borderBottom.BackgroundColor = UIColor.FromRGB(246, 132, 132).CGColor;
			Control.BorderStyle = UITextBorderStyle.None;
			Control.Layer.AddSublayer(borderBottom);
		}
	}
}