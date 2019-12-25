using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.SQLite;

namespace SQLiteTraining
{
    /// <summary>
    /// This class is attached to the SQLiteTest game object.
    /// Use the DB Browser for SQLite to populate the database.
    /// </summary>
    /// 
    public class DBConnector : MonoBehaviour
    {
        void Start()
        {
            //Opens a new SQLite connection to a database in the directory C:\Users\PATRI\Documents
            //The name of the database is SQLiteDB.db (if the file does not exist, then it will be created)
            SQLiteConnection connection =
                             new SQLiteConnection(@"Data Source=C:\Users\PATRI\Documents\VisitorsData.db;Version=3;");

            connection.Open();

            SQLiteCommand command = connection.CreateCommand();

            //Creates a new table with the name highscores and fields id, name, and score.
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "CREATE TABLE IF NOT EXISTS 'VisitorsSelections' ( " +
                              "  'id' INTEGER PRIMARY KEY, " +
                              "  'visitorName' TEXT NOT NULL, " +
                              "  'artistName' TEXT NOT NULL, " +
                              "  'signText' TEXT NOT NULL " +
                              ");";

            //command.ExecuteNonQuery();
            //connection.Close();
        }
    }
}