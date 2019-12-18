using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //To access the UI elements objects

public class TargetController : MonoBehaviour
{
    private int m_numHits;
    public Text uiText; //Reference to my UI Text to show fireball hit count

    //This method is run when the PlayerCube enters in collision with the TargetCube
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PaintBall")
        {
            m_numHits++;
            uiText.text = "Hits = " + m_numHits;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        uiText.text = "Hits = " + 0; //set the default starting text
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
