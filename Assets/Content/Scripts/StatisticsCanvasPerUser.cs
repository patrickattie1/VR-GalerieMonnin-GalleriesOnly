using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

public class StatisticsCanvasPerUser : MonoBehaviour
{
    private string connectionPath;
    private IDbConnection dbConnection;

    //Artists Scores
    public Text rsText;
    public Text pmText;
    //Visitors Scores
    public Text currentUserText;

    //We need the vrRig object to make sure the canvas "looks" at it when displayed
    public GameObject vrRig;

    public Canvas statisticsPerUserCanvas;

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
        statisticsPerUserCanvas.gameObject.SetActive(canvasVisible); //Make the stats canvas visible/invisible

        //Make sure the stats canvas is always directed towards the VRRig for easy reading
        statisticsPerUserCanvas.transform.LookAt(vrRig.transform.position);
        statisticsPerUserCanvas.transform.Rotate(Vector3.up, 180);

        if (canvasVisible == true)
        {
            CalculateForCurrentVisitorStatistics();
        }
    }

    void CalculateForCurrentVisitorStatistics()
    {
        //Get current visitor from XRGrabAndRelease.cs script's static variable
        string visitor = XRGrabAndRelease.visitor;

        GetChoicesCountPerArtistName("Roberto Stephenson", visitor);
        GetChoicesCountPerArtistName("Pascale MONNIN", visitor);

        GetChoicesCountPerVisitorName(visitor);
    }

    //*************Lists of query functions**************

    private void GetChoicesCountPerArtistName(string aName, string vName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE artistName = @aName AND visitorName = @vName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@aName", aName));
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@vName", vName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();

        if (aName == "Roberto Stephenson")
        {
            rsText.text = result;
        }

        if (aName == "Pascale MONNIN")
        {
            pmText.text = result;
        }
    }

    private void GetChoicesCountPerVisitorName(string vName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (*) FROM VisitorsChoices WHERE visitorName = @vName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@vName", vName));

        var result = commandToCreateQuery.ExecuteScalar().ToString();

        //Disply the total for the current visitor
        currentUserText.text = result;
    }
}
