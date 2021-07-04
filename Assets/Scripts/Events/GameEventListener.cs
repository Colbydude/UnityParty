using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityParty.Events
{
    public class GameEventListener : MonoBehaviour
    {
        // =====================================================
        // Publics

        // Using this struct to simulate dictionary in unity editor
        [Serializable]
        public struct Event
        {
            public GameEvent GameEvent;
            public UnityEvent<EventContext> UnityEvent;
        }

        // This gets converted to dictionary at game start
        public Event[] Events;

        // =====================================================
        // Privates
        private Dictionary<GameEvent, UnityEvent<EventContext>> m_eventDictionary = new Dictionary<GameEvent, UnityEvent<EventContext>>();

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

        public void RaiseEvent(GameEvent gameEvent, EventContext e)
        {
            // Call the corresponding unity event
            m_eventDictionary[gameEvent].Invoke(e);
        }
    }
}
