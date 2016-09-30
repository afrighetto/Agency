namespace Agency
{
	public interface IHUDProgress
	{
		void Show();
		void Show(string message);
		void ShowErrorWithStatus(string message);
		void ShowSuccessWithStatus(string message);
		void ShowMessageWithImage(string path, string message);
		void Dismiss();
	}
}