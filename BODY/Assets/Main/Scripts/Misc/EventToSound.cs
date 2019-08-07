using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventToSound : MonoBehaviour
{
    public void EventSound(int type)
    {
        SoundController.Play(gameObject, (SoundController.Sounds)type, 128, 0.15f);
    }
}
