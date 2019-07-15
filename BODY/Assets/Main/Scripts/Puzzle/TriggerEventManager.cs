using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventManager : MonoBehaviour
{
    public List<TriggerEvent> triggerEvents = new List<TriggerEvent>();

    // Update is called once per frame
    void Update()
    {
        foreach(TriggerEvent t in triggerEvents)
        {
            List<bool> triggerVals = new List<bool>();

            foreach(TriggerObject o in t.triggers)
            {
                triggerVals.Add(o.triggered);
            }

            t.conditionsMet = (!triggerVals.Contains(false));

            foreach (TriggerContainer c in t.containers)
            {
                c.gotActive = t.conditionsMet;
            }
        }
    }
}

[System.Serializable]
public class TriggerEvent
{
    public string name;
    public List<TriggerObject> triggers;
    public List<TriggerContainer> containers;
    public bool conditionsMet;
}