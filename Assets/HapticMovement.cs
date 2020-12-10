using Bhaptics.Tact.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticMovement : MonoBehaviour
{
    public HapticSource hapticSource;
    public HapticClip[] hapticClips;

    public void PlayHaptic(int index)
    {
        hapticSource.clip = hapticClips[index];
        hapticSource.Play();
    }

}
