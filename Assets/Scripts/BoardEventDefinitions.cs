using System.Collections;
using UnityEngine;
using UnityParty.Events;

namespace UnityParty
{
    // Definitions for events common to all boards. 
    // Each board should derive from this to add events unique to it
    public class BoardEventDefinitions : MonoBehaviour
    {
        private GameplayManager m_manager;

        public void BlueSpaceLand(EventContext e)
        {
            Debug.Log("Landed on a blue space");
        }

        public void BranchingPathPass(EventContext e)
        {
            Debug.Log("Landed on branching path");
            m_manager.CurrentPlayer.ChangeState<WaitingForInput>();
        }

        public void BankSpaceLand(EventContext e)
        {
            Debug.Log("Landed on a bank space");
        }

        public void BankSpacePass(EventContext e)
        {
            Debug.Log("Passed a bank space");
        }

        public void RedSpaceLand(EventContext e)
        {
            Debug.Log("Landed on a red space");
        }

        public void HappeningSpaceLand(EventContext e)
        {
            Debug.Log("Landed on a happening space");
        }

        public void Start()
        {
            m_manager = GetComponent<GameplayManager>();
        }
    }
}