using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    private string connectionPath;
    private IDbConnection dbConnection;

    //Artists Scores
    public Text RSText;
    public Text PMText;
    //Visitors Scores
    public Text PAText;
    public Text YAText;

    public Canvas statisticsCanvas;

    private void Awake()
    {
        connectionPath = "URI=file:" + Path.Combine(Application.persistentDataPath, "VisitorsData.sqlite");
    }

    void Start()
    {
        // Create database and open connection
        dbConnection = new SqliteConnection(connectionPath);
        dbConnection.Open();

        //Build the query to create the table
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
    }

    //This method is called when the trigger button is "pushed"
    public void DisplayStatisticsCanvas(bool canvasVisible)
    {
        statisticsCanvas.gameObject.SetActive(canvasVisible); //Make the stats canvas visible/invisible

        if (canvasVisible == true)
        {
            CalculateStatistics();
        }
    }

    void CalculateStatistics()
    {
        GetChoicesCountPerArtistName("Roberto Stephenson");
        GetChoicesCountPerArtistName("Pascale MONNIN");
        
        GetChoicesCountPerVisitorName("Patrick ATTIE");
        GetChoicesCountPerVisitorName("Yann ATTIE");
    }

    //*************Lists of query functions**************

    private void GetChoicesCountPerArtistName(string aName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE artistName = @aName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@aName", aName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();

        if (aName == "Roberto Stephenson")
        {
            RSText.text = result;
        }

        if (aName == "Pascale MONNIN")
        {
            PMText.text = result;
        }
    }

    private void GetChoicesCountPerVisitorName(string vName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE visitorName = @vName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@vName", vName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();

        if (vName == "Patrick ATTIE")
        {
            PAText.text = result;
        }

        if (vName == "Yann ATTIE")
        {
            YAText.text = result;
        }
    }
}
