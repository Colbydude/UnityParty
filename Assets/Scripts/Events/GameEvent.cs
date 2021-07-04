using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityParty.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 1)]

    public class GameEvent : ScriptableObject
    {
        // =====================================================
        // Privates 
        private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

        public void Invoke(EventContext e)
        {
            foreach (GameEventListener listener in listeners)
            {
                listener.RaiseEvent(this, e);
            }
        }

        public void Register(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void Unregister(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
