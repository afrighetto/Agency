using System;
using SQLite;

namespace Agency
{
	public interface ISQLite
	{
		SQLiteConnection InitConnection();
	}
}

