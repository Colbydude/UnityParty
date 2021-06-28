using System.Collections;
using UnityEngine;

namespace UnityParty
{
    // Definitions for events common to all boards. 
    // Each board should derive from this to add events unique to it
    public class BoardEventDefinitions : MonoBehaviour
    {
        public void BlueSpaceLand()
        {
            Debug.Log("Landed on a blue space");
        }

        public void BranchingPathLand()
        {
            Debug.Log("Landed on branching path");
        }

        public void BankSpaceLand()
        {
            Debug.Log("Landed on a bank space");
        }

        public void BankSpacePass()
        {
            Debug.Log("Passed a bank space");
        }

        public void RedSpaceLand()
        {
            Debug.Log("Landed on a red space");
        }

        public void HappeningSpaceLand()
        {
            Debug.Log("Landed on a happening space");
        }
    }
}