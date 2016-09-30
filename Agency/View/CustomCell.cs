using Xamarin.Forms;

namespace Agency
{
	public class CustomCell : TextCell
	{
		public CustomCell()
		{
			MenuItem deleteAction = new MenuItem { Text = "Delete", IsDestructive = true };
			deleteAction.SetBinding(MenuItem.CommandParameterProperty, "EmployeeID");

			//Subscribe event when Clicked is triggered
			deleteAction.Clicked += async (s, e) => { await ListViewModel.OnDelete(s, e); };
			ContextActions.Add(deleteAction);
		}
	}
}