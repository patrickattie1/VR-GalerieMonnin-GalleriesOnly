using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public List<Tutorial> tutorials = new List<Tutorial>(); //List of all tutorials
    public Text explanationText; //Explanation shown on the screen

    private Text m_text; //Used when loading gallery

    private static TutorialManager instance;
    public  static TutorialManager Instance
    {
        get
        {
            if (instance == null)  //instance variable has not been assigned yet
            {
                instance = GameObject.FindObjectOfType<TutorialManager>(); //No need to manually assign it in the Inspector
            }

            if (instance == null)  //Previous Find did not work
            {
                Debug.Log("There is no TutorialManager");
            }

            return instance;
        }
    }

    private Tutorial currentTutorial;

	void Start ()
    {
        SetNextTutorial(0);
	}

	void Update ()
    {
        if (currentTutorial) //The is actually some tutorial to run
        {
            currentTutorial.CheckIfHappening();
        }
	}

    //Run this method when one tutorial has been completed and run the followind tutorial
    public void Completedtutorial()
    {
        SetNextTutorial(currentTutorial.order + 1);
    }

    //We need to know which tutorial is going to be the next currentTutorial.
    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);
        if (!currentTutorial)
        {
            //Then all the tutorials have been completed
            CompletedAllTutorials();
            return;
        }

        explanationText.text = currentTutorial.explanation;
    }

    //When all tutorials have been completed
    public void CompletedAllTutorials()
    {
        explanationText.text = "Visiteur, vous pouvez maintenant entrer dans la galerie";

        StartCoroutine(LoadAsyncScene());
    }

    //Function to get the next tutorial
    public Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].order == order)
            {
                return tutorials[i];
            } 
        }
        return null;
    }

    IEnumerator LoadAsyncScene()
    {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("1-SceneVisiteGalerie");

            //We decide when we want the new scene to appear
            asyncLoad.allowSceneActivation = false;

            // Wait until the asynchronously scene fully loads
            while (!asyncLoad.isDone)
            {
                //Output current progress (descending)
                int prog = 100 - (int)Mathf.Round(asyncLoad.progress * 100);
                m_text.text = "Immersion dans: " + prog + " %";

                // Check if the load has finished
                if (asyncLoad.progress >= 0.9f)
                {
                    //Activate the Scene
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
    }
}
