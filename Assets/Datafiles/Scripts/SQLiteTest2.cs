using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

public class SQLiteTest2 : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        // 1. Create the database
        //Connection string format: URI=file:/path/to/file
        //Persistent = Read Only
        //persistentDataPath in Android: /storage/emulated/0/Android/data/<packagename>/files
        //persistentDataPath in Windows: %userprofile%\AppData\Local\Packages\<productname>\LocalState, in my case: C:/Users/PATRI/AppData/LocalLow/ESIH/SQLiteProjectTraining/My_Database

        //string connectionPath = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
        //Path.Combine will use / or \ depending on the OS used (Windows vs Android)
        string connectionPath = "URI=file:" + Path.Combine(Application.persistentDataPath, "VisitorsData.sqlite");

        //Open connection
        //An SQLiteConnection object is created. This object is used to open a connection to a database. 
        IDbConnection dbConnection = new SqliteConnection(connectionPath);
        //Opens the database connection.
        dbConnection.Open();

        // Create the table
        IDbCommand commandToCreateTable;
        commandToCreateTable = dbConnection.CreateCommand();

        string q_createTable = "CREATE TABLE IF NOT EXISTS VisitorsChoices ( " +
                              "  'visitorName' TEXT NOT NULL, " +
                              "  'artistName' TEXT NOT NULL, " +
                              "  'signText' TEXT NOT NULL " +
                              ");";

        commandToCreateTable.CommandText = q_createTable;
        commandToCreateTable.ExecuteReader();

        //3. Insert records in the table
        IDbCommand commandToInsertValues1 = dbConnection.CreateCommand();
        IDbCommand commandToInsertValues2 = dbConnection.CreateCommand();
        IDbCommand commandToInsertValues3 = dbConnection.CreateCommand();

        //Note that we do not even have to insert the rowid at the beginning of the query. Those will be cumulatively incremented / added to the table.
        //Sets the SQL queries to execute against the database
        commandToInsertValues1.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, signText) VALUES ('Hans TIPPEN', 'Artiste 1', 'Mon Tableau')";
        //We use the ExecuteNonQuery() method if we do not want a result set, for example for DROP, INSERT, or DELETE statements.
        commandToInsertValues1.ExecuteNonQuery();
        commandToInsertValues2.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, signText) VALUES ('Alpha HAITI', 'Artiste 2', 'Bibou Lalo')";
        commandToInsertValues2.ExecuteNonQuery();
        commandToInsertValues3.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, signText) VALUES ('Hila ALLIBERT', 'Artiste 3', 'Bebe Lala')";
        commandToInsertValues3.ExecuteNonQuery();

        // Read and print all values in table
        // Read and print all values in table
        //The SQLiteDataReader is a class used to retrieve data from the database. It is used with the SQLiteCommand class to execute a SELECT statement and then access the returned rows. 
        //It provides fast, forward-only, read-only access to query results. It is the most efficient way to retrieve data from tables.
        IDbCommand commandForReading = dbConnection.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM VisitorsChoices";
        commandForReading.CommandText = query;
        reader = commandForReading.ExecuteReader();

        while (reader.Read())
        {
            //Debug.Log("id: "  + reader[0].ToString());
            Debug.Log("visitorName: " + reader[1].ToString());
            Debug.Log("artistName: " + reader[2]);
            Debug.Log("signText: " + reader[3]);
        }

        // Close connection
        dbConnection.Close();
    }
}