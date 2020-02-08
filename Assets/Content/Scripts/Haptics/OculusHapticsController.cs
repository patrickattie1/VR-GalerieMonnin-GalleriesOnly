using UnityEngine;

namespace Haptics
{
    /// <summary>
    /// USAGE: 
    /// To use the haptics code, add the script to your controller object ("Left Hand" or "Right Hand" object). 
    /// Assign the controller mask as L Touch or R Touch (whichever matches the controller it’s attached to)
    /// Then call the Vibrate method when you want it to shake
    /// OTHER NOTES:
    /// The Oculus code is designed to bypass the need for custom audioclips.  
    /// While the clip system is pretty cool and could do quite a bit, it’s not cross platform, and much harder 
    /// to get started with.
    /// </summary>
    public class OculusHapticsController : MonoBehaviour
    {
        [SerializeField]
        OVRInput.Controller controllerMask;

        private OVRHapticsClip clipLight;
        private OVRHapticsClip clipMedium;
        private OVRHapticsClip clipHard;

        private void Start()
        {
            InitializeOVRHaptics();
        }

        private void InitializeOVRHaptics()
        {
            int cnt = 10;
            clipLight = new OVRHapticsClip(cnt);
            clipMedium = new OVRHapticsClip(cnt);
            clipHard = new OVRHapticsClip(cnt);
            for (int i = 0; i < cnt; i++)
            {
                clipLight.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)75;
                clipMedium.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)150;
                clipHard.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)255;
            }

            clipLight = new OVRHapticsClip(clipLight.Samples, clipLight.Samples.Length);
            clipMedium = new OVRHapticsClip(clipMedium.Samples, clipMedium.Samples.Length);
            clipHard = new OVRHapticsClip(clipHard.Samples, clipHard.Samples.Length);
        }

        public void Vibrate(VibrationForce vibrationForce)
        {
            var channel = OVRHaptics.RightChannel;
            if (controllerMask == OVRInput.Controller.LTouch)
                channel = OVRHaptics.LeftChannel;

            switch (vibrationForce)
            {
                case VibrationForce.Light:
                    channel.Preempt(clipLight);
                    break;
                case VibrationForce.Medium:
                    channel.Preempt(clipMedium);
                    break;
                case VibrationForce.Hard:
                    channel.Preempt(clipHard);
                    break;
            }
        }
    }
}