﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;

namespace GMGalleriesOnly
{

    public class Statistics : MonoBehaviour
    {
        private string connectionPath;
        private IDbConnection dbConnection;

        //Artists Scores
        public Text rsText;
        public Text pmText;
        //Visitors Scores
        public Text paText;
        public Text yaText;

        //We need the vrRig object to make sure the canvas "looks" at it when displayed
        public GameObject vrRig;

        public Canvas statsCanvas;

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
            statsCanvas.gameObject.SetActive(canvasVisible); //Make the stats canvas visible/invisible

            //Make sure the stats canvas is always directed towards the VRRig for easy reading
            statsCanvas.transform.LookAt(vrRig.transform.position);
            statsCanvas.transform.Rotate(Vector3.up, 180);

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

            if (vName == "Patrick ATTIE")
            {
                paText.text = result;
            }

            if (vName == "Yann ATTIE")
            {
                yaText.text = result;
            }
        }
    }
}