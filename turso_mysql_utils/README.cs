/*
	So you have SqlDatabaseClient.cs and SqlDatabaseManager.cs that are the files that you should place them in your Utils folder.
	Then you have MySql.Data.dll, that is the library used by your application to work with mysql (kind of jdbc in java).

	Remember to add MySql.Data.dll in your library references of your project.
	
	To initialize your connection to mysql (at the very start of the program) just:
*/

try
{
	SqlDatabaseManager.Initialize();
}
catch (Exception e)
{
	//smth wrong when connecting, close application... or try again in a loop with for example 5 tries or smth like that...
}

/*
	Once initialized, you can call a Database Cleanup (for example I update the status of players (0=not playing ingame, 1=playing ingame)
*/

private static void PerformDatabaseCleanup()
{
	using (SqlDatabaseClient MySqlClient = SqlDatabaseManager.GetClient())
	{
		MySqlClient.ExecuteNonQuery("UPDATE server_1_players SET online = 0");
	}
}

//NOTE that a parameterized query here is not necessary because we know the value we are updating (0 in this case). ** I mean, is not a variable...

/*
	A query function to make a select should be like this:
*/

public static DataRow yourSelect(int playerID)
{
	try
	{
		using (SqlDatabaseClient MySqlClient = SqlDatabaseManager.GetClient())
		{
			string sql = "SELECT smth FROM table WHERE playerID = @playerID";
			MySqlClient.SetParameter("@playerID", playerID);//a parameter O.o
			return MySqlClient.ExecuteQueryRow(sql);
		}
	}
	catch (Exception e)
	{
		//Your logging shit xD
	}
	return null;
}

/*
	This returns a DataRow reference that you should check if it's null or not (cause if an exception is thrown, it will be null).
	And you should call it like this:
*/

var nameItAsYouWant = yourSelect(ID);
if (nameItAsYouWant != null)
{
	int smth = int.Parse(nameItAsYouWant["smth"].ToString());
	//string: nameItAsYouWant["smth"].ToString();
	//long: long.Parse(nameItAsYouWant["smth"].ToString());
	//byte array: (byte[])nameItAsYouWant["smth"];
}

//You can also make a query of multiple rows by calling...
var queryTable = MySqlClient.ExecuteQueryTable(sql); 
//instead and making a foreach
if(queryTable != null)
{
	foreach (DataRow row in queryTable.Rows)
	{
	}
}

//UPDATE statement queries are easier because you don't have to read the result, so you only need to call 
MySqlClient.ExecuteNonQuery(sql);
//instead and everything should work fine.

//Hope you this helps you...
