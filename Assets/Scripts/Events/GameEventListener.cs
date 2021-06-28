using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityParty.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Serializable]
        public struct Event
        {
            public GameEvent GameEvent;
            public UnityEvent UnityEvent;
        }

        public Event[] Events;

        private Dictionary<GameEvent, UnityEvent> m_eventDictionary = new Dictionary<GameEvent, UnityEvent>();

        public void Start()
        {
            // Fill dictionary from editor values
            foreach (Event e in Events)
            {
                m_eventDictionary.Add(e.GameEvent, e.UnityEvent);
            }
        }

        private void OnEnable()
        {
            foreach (Event e in Events)
            {
                e.GameEvent.Register(this);
            }
        }

        private void OnDisable()
        {
            foreach (Event e in Events)
            {
                e.GameEvent.Unregister(this);
            }
        }

        public void RaiseEvent(GameEvent gameEvent)
        {
            m_eventDictionary[gameEvent].Invoke();
        }
    }
}
