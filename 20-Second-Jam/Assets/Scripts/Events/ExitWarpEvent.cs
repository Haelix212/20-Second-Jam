using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exit Warp Event")]
public class ExitWarpEvent : ScriptableObject
{
    private List<ExitWarpEventListener> listeners = new List<ExitWarpEventListener>();

    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(ExitWarpEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(ExitWarpEventListener listener)
    {
        listeners.Remove(listener);
    }
}