using System.Collections;
using UnityEngine;

namespace Assets.Helper.States
{
    public class WaitingForInput : IState
    {
        public void Start()
        {
            Debug.Log("About to roll a 10");
        }

        public void Update()
        {

        }
    }
}