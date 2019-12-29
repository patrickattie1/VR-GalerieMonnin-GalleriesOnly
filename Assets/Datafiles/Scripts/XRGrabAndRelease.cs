using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

//Tell what hands are touching and use right click to grab object.
public class XRGrabAndRelease : MonoBehaviour
{
    private GameObject m_CollidingObject;
    private GameObject m_HeldObject;
    public  GameObject m_Cart;

    //For the SQLite needs
    public string visitor = "Patrick ATTIE";
    //private string photoDescription;
    private string artist, product, price;

    private string connectionPath;
    private IDbConnection dbConnection;

    //This is a reference to the Animator component attached to the VRHand object in the Hierarchy
    public Animator animator;

    //XR Specific variables
    //Name of the grip object that we setup in the Project Settings Inputs list (setup in the corresponding slot in the Inspector)
    public string gripInputName;
    //Used to determine if we are already holding the object. Prevent the Update Loop to run the code 90x/sec.
    private bool gripHeld;

    private void Awake()
    {
        connectionPath = "URI=file:" + Path.Combine(Application.persistentDataPath, "VisitorsData.sqlite"); ;
    }

    private void Start()
    {
        //Create database and open connection
        dbConnection = new SqliteConnection(connectionPath);
        dbConnection.Open();

        //Build the query to create the table
        string q_createTable = "CREATE TABLE IF NOT EXISTS VisitorsChoices ( " +
                              "  id int primary key, " +
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

    private void OnTriggerStay(Collider other) //Other object has to have a RigidBody
    {
        if (other.GetComponent<Rigidbody>())
        {
            m_CollidingObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_CollidingObject = null;
    }

    // Update is called once per frame
    void Update() //Occurs 90 times/second in VR. But needed only when we pull the trigger
    {

        //If other input type, replace Mouse1 with whatever....
        //This if statement is activated only when we press the grip button.
        if (Input.GetAxis(gripInputName) > 0.5f && gripHeld == false) //Grab
        {
            animator.SetBool("Closed Hand", true); //Changes the parameter in the Animator

            if (m_CollidingObject != null)
            {
                Grab();
            }
            //Prevents from running this code(e.g. m_animator.setBool("Close Hand", true) and Grab()) every frame(e.g. 90x per second)
            gripHeld = true;
        }
        else if (Input.GetAxis(gripInputName) < 0.5f && gripHeld == true) //Release
        {
            animator.SetBool("Closed Hand", false); //Changes the parameter in the Animator

            if (m_HeldObject != null)
            {
                Release();
            }
            gripHeld = false;
        }
    }

    //When we pick up the cube it becomes a child of our hand.
    void Grab()  //Grab colliding object (= Make it a child of the parent hand)
    {
        m_HeldObject = m_CollidingObject; //Referencing the touched object
        m_CollidingObject.GetComponent<Rigidbody>().isKinematic = true; //Making the touched object kinematic so it does not fall
        m_CollidingObject.transform.SetParent(transform);//Make it a child of our hand so that it moves with our hand
    }

    void Release() //Release colliding object
    {
        //Get the description's text for insertion in the SQLite database
        //photoDescription = m_CollidingObject.GetComponentInChildren<TextMesh>().text;
        //Note: Find does NOT persform a recursive descend down a transform hierarchy.
        artist  = m_CollidingObject.transform.Find("ArtistName").GetComponent<TextMesh>().text;
        product = m_CollidingObject.transform.Find("ProductName").GetComponent<TextMesh>().text;
        price   = m_CollidingObject.transform.Find("ProductPrice").GetComponent<TextMesh>().text;


        m_HeldObject.transform.SetParent(null);
       // m_HeldObject.GetComponent<Rigidbody>().isKinematic = false; //To make sure the sign remains where it was placed in the cart
        m_HeldObject = null;

        ////Fill up the cart with the grabbed signs game objects
        m_CollidingObject.transform.SetParent(m_Cart.transform); //Make the grabbed sign a child of the Cart game object so that it moves with the cart


        //Insert description (text) of selected photo to the database
        IDbCommand commandToInsertValues = dbConnection.CreateCommand();

        //commandToInsertValues.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, signText) VALUES (@visitor, @artist, @photoDescription)";
        // DO NOT USE: commandToInsertValues.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, signText) VALUES (@visitor, @artist, @photoDescription)";
        //   as this line of code will not work.
        //USE INSTEAD:
        commandToInsertValues.CommandText = "INSERT INTO VisitorsChoices (visitorName, artistName, productName, productPrice) " +
                                                    "VALUES (@param1, @param2, @param3, @param4)";

        ///commandToInsertValues.CommandType = CommandType.Text;
        commandToInsertValues.Parameters.Add(new SqliteParameter("@param1", visitor));
        commandToInsertValues.Parameters.Add(new SqliteParameter("@param2", artist));
        commandToInsertValues.Parameters.Add(new SqliteParameter("@param3", product));
        commandToInsertValues.Parameters.Add(new SqliteParameter("@param4", price));

        commandToInsertValues.ExecuteNonQuery();
    }

    void CalculateStatistics()
    {
        GetChoicesCountPerArtistName("Roberto Stephenson");
        GetChoicesCountPerArtistName("Vizal Babo");
        GetChoicesCountPerVisitorName("Patrick ATTIE");
        GetChoicesCountPerVisitorName("Yann ATTIE");
    }

    //*************Lists of query functions**************

    private void GetChoicesCountPerArtistName(string aName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (id) FROM VisitorsChoices WHERE artistName = @aName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@aName", aName));
        IDataReader reader;
        reader = commandToCreateQuery.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("id: "  + reader[0].ToString());
            Debug.Log("The artist " + aName + "has been selected " + reader[1].ToString() + "times.");
        }
    }

    private void GetChoicesCountPerVisitorName(string vName)
    {
        IDbCommand commandToCreateQuery = dbConnection.CreateCommand();
        commandToCreateQuery.CommandText = "SELECT COUNT (id) FROM VisitorsChoices WHERE artistName = @vName";
        commandToCreateQuery.Parameters.Add(new SqliteParameter("@vName", vName));
        IDataReader reader;
        reader = commandToCreateQuery.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("id: " + reader[0].ToString());
            Debug.Log("The visitor " + vName + "has made " + reader[1].ToString() + "choices.");
        }
    }
}