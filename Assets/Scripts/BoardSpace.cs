using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public BoardSpace NextSpace;
        public BoardSpace PreviousSpace;
        public SpaceFlags Flags;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
