using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorial //Inherits from the class Tutorial
{
    //The keys we need to press to continue (i.e.: press the WASD keys to move 
    //      and all of them have to be pressed to go to the next tutorial)
    public List<string> keys = new List<string>();

    //Each tutorial manages its own Happening stuff (through the overriding
    public override void CheckIfHappening()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.inputString.Contains(keys[i]))
            {
                keys.RemoveAt(i);
                break;
            }
        }

        if (keys.Count == 0)
        {
            TutorialManager.Instance.Completedtutorial();
        }
    }
}
