//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
////Add the following Libraries
//using System;       //to use Actions
//using System.Linq;  //to use ToArray
//using UnityEngine.Windows.Speech;   //To use voice recognition

//namespace Voice
//{
//    public class VoiceTest : MonoBehaviour
//    {
//        //A dictionary will act like a list of 2 associated variables one is a key and the second is its value like a real dictionary
//        //has a word and its definition. In our case we will use a string for our key (word) and a Action as our value (definition).
//        //This allows use to invoke something from a key and it will find the associated action for us.
//        private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();

//        private KeywordRecognizer keywordRecognizer;
//        // Use this for initialization
//        void Start()
//        {
//            keywordActions.Add("Your command phrase", YourFunctionHere);   //Adding a phrase/word to be recognized into our dictionary with an associated action a function.
//                                                                           //You can use the above to add as many commands as you like

//            keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());   //Create an array of phrases to be recognized 
//            keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;   //Everytime a phrase is recognized run the OnKeywordsRecognized function
//            keywordRecognizer.Start();  //Intialize the recognizer system
//        }

//        private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
//        {
//            keywordActions[args.text].Invoke(); //Invoke a function associated with that key
//        }

//        //Create some function to run on command
//        void YourFunctionHere()
//        {
//            Debug.Log("You made something happen via a voice command!");
//        }
//    }
//}