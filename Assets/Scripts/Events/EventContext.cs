using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityParty.Events
{
    public class EventContext
    {
        public GameObject Caller;
        public Dictionary<string, object> Parameters;

        public EventContext(GameObject caller)
        {
            Caller = caller;
        }

        public EventContext(GameObject caller, Dictionary<string, object> parameters)
        {
            Caller = caller;
            Parameters = parameters;
        }
    }
}