using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UT.MailSample
{

    public class UIObjectsController : MonoBehaviour
    {
        //To manage timer for activating button when "looked at"
        const float nSecond = 2f;
        float timer = 0;
        bool entered = false;

        public InputField inputField;

        public void PointerEnter()
        {
            entered = true;
        }
        public void PointerExit()
        {
            entered = false;
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

        private void Update()
        {
            if (entered)
            {
                //Increment timer
                timer += Time.deltaTime;

                //Load scene if counter has reached n second
                if (timer > nSecond)
                {
                    //Make sure email is sent before entering gallery scene
                    if (SceneManager.GetActiveScene().name == "0-SceneIntro")
                    {
                        SendEmailFromKeyboardInput();
                        SceneManager.LoadScene("1-SceneVisiteGalerie");
                    }
                }
            }
            else
            {
                timer = 0;
            }
        }

        //public void ChangeScene(string sceneName)
        //{
        //  SceneManager.LoadScene(sceneName);
        //}

        public void Terminer()
        {
            Debug.Log("In ChangeScene: Terminer");
            Application.Quit();
        }
    }
}