using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Over Event")]
public class GameOverEvent : ScriptableObject
{
    private List<GameOverEventListener> listeners = new List<GameOverEventListener>();

    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(GameOverEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameOverEventListener listener)
    {
        listeners.Remove(listener);
    }
}