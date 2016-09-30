using System;
using Agency.iOS;
using Xamarin.Forms;
using UIKit;

[assembly: ExportRenderer(typeof(TextCell), typeof(CustomTextCell))]
namespace Agency.iOS
{
	public class CustomTextCell : global::Xamarin.Forms.Platform.iOS.TextCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			UITableViewCell cell = base.GetCell(item, reusableCell, tv);
			switch (item.StyleId) 
			{
				case "None":
					cell.Accessory = UITableViewCellAccessory.None; break;
				default:
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator; break;
			}
			return cell;
		}
	}
}
