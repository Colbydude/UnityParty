using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityParty.Events;
using UnityParty.Helpers;
using UnityParty.UI;

namespace UnityParty
{
    struct MovementActionData
    {
        public int SpacesToMove;
        public bool IsActive;
    }

    public class GameplayManager : MonoBehaviour
    {
        // =====================================================
        // Publics
        public BoardPlayerController CurrentPlayer;
        public UIManager UIManager;

        // =====================================================
        // Privates
        private MovementActionData m_currentMovementAction;
        private System.Random m_random;

        // Start is called before the first frame update
        void Start()
        {
            m_random = new System.Random();

            if (UIManager == null)
            {
                Debug.LogWarning("No UIManager set in the editor. Searching for one in the scene.");

                UIManager = FindObjectOfType<UIManager>();
                if (UIManager == null)
                {
                    Debug.Assert(false, "UIManager not found in the scene. Stop.");
                }
                else
                {
                    Debug.LogWarning("Found [" + UIManager + "]");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HandlePlayerInputConfirm()
        {
            IState playerCurrentState = CurrentPlayer.GetPlayerState();
            if (playerCurrentState.GetType() == typeof(WaitingForRoll))
            {
                MakeRoll();
            }
        }

        public void PathSelected(EventContext e)
        {
            BoardSpace nextSpace = (BoardSpace)e.Parameters["next_space"];
            CurrentPlayer.ChangeState<Walking>();
            CurrentPlayer.MoveToSpace(MoveToSpaceFinished, nextSpace);
        }

        void MakeRoll()
        {
            // Generate random number for the current player to move.
            m_currentMovementAction.SpacesToMove = m_random.Next(1, 11);
            RefreshCurrentSpacesText();

            CurrentPlayer.MoveToSpace(MoveToSpaceFinished, CurrentPlayer.GetCurrentSpace().NextSpace[0]);
        }

        void MoveToSpaceFinished(BoardSpace newSpace)
        {
            // Check if we should decrement the move counter.
            if (!newSpace.Flags.HasFlag(BoardSpace.SpaceFlags.DoesNotDecrementMoveCounter))
            {
                m_currentMovementAction.SpacesToMove--;
                RefreshCurrentSpacesText();
            }

     
            if (m_currentMovementAction.SpacesToMove > 0)
            {
                // call the On Pass Event for the space 
                newSpace.InvokeOnPassEvents();

                // check if we're still walking so that we can move to the next space
                // otherwise the state's been changed by space's events and some action is needed
                if (CurrentPlayer.GetPlayerState().GetType() == typeof(Walking))
                {
                    CurrentPlayer.MoveToSpace(MoveToSpaceFinished, newSpace.NextSpace[0]);
                }
            }
            else
            {
                // call the On Land Events for the space 
                newSpace.InvokeOnLandEvents();

                // End Player turn
                CurrentPlayer.EndTurn();
            }
        }

        void RefreshCurrentSpacesText()
        {
            // Update text above player.
            UIManager.RemainingSpacesText.text = m_currentMovementAction.SpacesToMove.ToString();
        }
    }
}
