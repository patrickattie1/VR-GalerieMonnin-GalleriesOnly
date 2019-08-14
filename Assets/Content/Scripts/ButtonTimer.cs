using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UT.MailSample
{
    // Use this next line of code if you are not using the EventTrigger component attached to the button but
    //     rather manage it in code using OnPointerenter and OnPointerExit methods
    //public class ButtonTimer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    public class ButtonTimer : MonoBehaviour
    {
        //To manage timer for activating button when "looked at"
        float nSecond = 2f;
        float timer = 0;
        bool entered = false;

        public Text m_text;

        public InputField inputField;

        public void EnterButton()
        {
            entered = true;
        }

        public void ExitButton()
        {
            entered = false;
        }

        public void ClickButton()
        {
            StartCoroutine(LoadAsyncScene());
        }

        private void Start()
        {
           //m_text.enabled = false;
        }

        void Update()
        {
            if (entered)
            {
                //Increment timer
                timer += Time.deltaTime;

                //Load scene if button has been gazed more than n second
                if (timer > nSecond)
                {
                    StartCoroutine(LoadAsyncScene());
                }
            }
            else
            {
                timer = 0;
            }
        }

        IEnumerator LoadAsyncScene()
        {
            //Make sure email is sent before entering gallery scene
            if (SceneManager.GetActiveScene().name.ToString() == "0-SceneIntro 1")
            {
                //////SendEmailFromKeyboardInput();

                //m_text.enabled = true;

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("1-SceneVisiteGalerie");

                //We decide when we want the new scene to appear
                asyncLoad.allowSceneActivation = false;

                // Wait until the asynchronously scene fully loads
                while (!asyncLoad.isDone)
                {
                    //Output current progress (descending)
                    int prog = 100 - (int) Mathf.Round(asyncLoad.progress * 100);
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

        public string GetInputFromVRKeyboard()
        {
            if (inputField != null)
            {
                Debug.Log("InputField Text is: " + inputField.text);
                return inputField.text.ToString();
            }
            else
            {
                Debug.Log("inputField is null");
                return null;
            }
        }

        public void SendEmailFromKeyboardInput()
        {
            //Recuperate the text from user
            string text = GetInputFromVRKeyboard();

            //Email the text to the gallery owner
            GetComponent<UTMailSample1>().Body.text = "La Gallerie Virtuelle Roberto STEPHENSON a été visitée par: " + inputField.text.ToString();
            GetComponent<UTMailSample1>().Send();
        }

        public void Terminer()
        {
            Debug.Log("In ChangeScene: Terminer");
            Application.Quit();
        }
    }
}