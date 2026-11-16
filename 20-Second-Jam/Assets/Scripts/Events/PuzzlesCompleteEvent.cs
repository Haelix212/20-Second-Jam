using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Over Event")]
public class PuzzlesCompleteEvent : ScriptableObject
{
    private List<PuzzlesCompleteEventListener> listeners = new List<PuzzlesCompleteEventListener>();

    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(PuzzlesCompleteEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(PuzzlesCompleteEventListener listener)
    {
        listeners.Remove(listener);
    }
}