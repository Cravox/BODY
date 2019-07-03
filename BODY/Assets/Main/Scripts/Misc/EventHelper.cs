using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class EventHelper
{
    public static GameObject currentActive;

    public static void FixEventSystem()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            currentActive = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(currentActive);
    }
}
