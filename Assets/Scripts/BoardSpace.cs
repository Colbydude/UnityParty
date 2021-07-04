using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityParty.Events;

namespace UnityParty
{
    public class BoardSpace : MonoBehaviour
    {
        [Flags]
        public enum SpaceFlags
        {
            DoesNotDecrementMoveCounter = 0x01, // Can't end of the space, doesn't decrement movement count.
            Stoppable                   = 0x02, // User stops on the space, control is passed to a different handler(?)
            Flag3                       = 0x04,
            Flag4                       = 0x08,
        };

        // =====================================================
        // Publics
        public SpaceFlags Flags;
        public List<BoardSpace> NextSpace;
        public List<BoardSpace> PreviousSpace;
        public List<GameEvent> OnLandEvent;
        public List<GameEvent> OnPassEvent;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InvokeOnLandEvents()
        {
            foreach (GameEvent e in OnLandEvent)
            { 
                e.Invoke(new EventContext(gameObject));
            }
        }

        public void InvokeOnPassEvents()
        {
            foreach (GameEvent e in OnPassEvent)
            {
                e.Invoke(new EventContext(gameObject));
            }
        }
    }
}
