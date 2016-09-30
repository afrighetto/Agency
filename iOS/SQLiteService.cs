using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Agency.iOS;

[assembly: Dependency(typeof(SQLiteService))]
namespace Agency.iOS
{
	public class SQLiteService : ISQLite
	{
		public SQLiteConnection InitConnection() 
		{
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine(documentsPath, "..", "Library");
			string path = Path.Combine(libraryPath, "sqlite_agency.db3");
			System.Diagnostics.Debug.WriteLine(path);
			return new SQLiteConnection(path);
		}
	}
}

