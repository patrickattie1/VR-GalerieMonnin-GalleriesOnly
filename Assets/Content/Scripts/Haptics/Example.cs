using UnityEngine;

namespace Haptics
{
    /// <summary>
    /// USAGE: a simple sample that will vibrate the controllers when you press space.  
    /// (just remember to assign the 2 controllers)
    /// </summary>
    public class OculusDemo : MonoBehaviour
    {
        [SerializeField]
        OculusHapticsController leftControllerHaptics;

        [SerializeField]
        OculusHapticsController rightControllerHaptics;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                leftControllerHaptics.Vibrate(VibrationForce.Hard);
                rightControllerHaptics.Vibrate(VibrationForce.Light);
            }
        }
    }
}