using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

/// <summary>
/// This script uses a parmeterized query to add records in a table
/// </summary>
public class Test1 : MonoBehaviour
{
    //For the SQLite needs - Public Variables
    public  string visitor = "Yann ATTIE";
    //public GameObject photoDescription;
    private string artist, product, price;

    //For the SQLite needs - Private Variables
    private string connectionPath;
    private IDbConnection dbConnection;
    private string description;

    private void Awake()
    {
        connectionPath = "URI=file:" + Path.Combine(Application.persistentDataPath, "VisitorsData.sqlite");
    }

    private void Start()
    {
        //Create database and open connection
        dbConnection = new SqliteConnection(connectionPath);
        dbConnection.Open();

        //Build the query to create the table
        string q_createTable = @"CREATE TABLE IF NOT EXISTS VisitorsChoices ( " +
                              "  Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                              "  visitorName  TEXT NOT NULL, " +
                              "  artistName   TEXT NOT NULL, " +
                              "  productName  TEXT NOT NULL, " +
                              "  productPrice TEXT NOT NULL " +
                              ");";

        //Instantiate a new command object that will run an SQL instruction
        IDbCommand commandToCreateTable;
        commandToCreateTable = dbConnection.CreateCommand();

        //Assign the command and execute it
        commandToCreateTable.CommandText = q_createTable;
        commandToCreateTable.ExecuteReader();

        CalculateStatistics();
    }

       // Update is called once per frame
    void Update() //Occurs 90 times/second in VR
    {
        ////Get the description's text for insertion in the SQLite database
        //description = photoDescription.GetComponentInChildren<TextMesh>().text;

        //artist  = photoDescription.transform.Find("ArtistName").GetComponent<TextMesh>().text;
        //product = photoDescription.transform.Find("ProductName").GetComponent<TextMesh>().text;
        //price   = photoDescription.transform.Find("ProductPrice").GetComponent<TextMesh>().text;

        //////Insert description (text) of selected photo to the database
        //IDbCommand commandToInsertValues = dbConnection.CreateCommand();

        //commandToInsertValues.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, productName, productPrice) VALUES (@param1, @param2, @param3, @param4)";

        //commandToInsertValues.Parameters.Add(new SqliteParameter("@param1", visitor));
        //commandToInsertValues.Parameters.Add(new SqliteParameter("@param2", artist));
        //commandToInsertValues.Parameters.Add(new SqliteParameter("@param3", product));
        //commandToInsertValues.Parameters.Add(new SqliteParameter("@param4", price));

        //commandToInsertValues.ExecuteNonQuery();

    }


    //*************Lists of query functions**************

    void CalculateStatistics()
    {
        GetChoicesCountPerArtistName("Roberto Stephenson");
        GetChoicesCountPerArtistName("Vizal Babo");
        GetChoicesCountPerVisitorName("Patrick ATTIE");
        GetChoicesCountPerVisitorName("Yann ATTIE");
    }

    private void GetChoicesCountPerArtistName(string aName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE artistName = @aName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@aName", aName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();
        Debug.Log("Number of times a product from artist " + aName + " has been selected " + result);  
    }

    private void GetChoicesCountPerVisitorName(string vName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE visitorName = @vName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@vName", vName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();
        Debug.Log("Number of products chosen by visitor " + vName + " is " + result);
    }
}
