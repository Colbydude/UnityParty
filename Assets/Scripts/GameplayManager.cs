using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public TMPro.TMP_Text CurrentSpacesText;

    // =====================================================
    // Privates
    private MovementActionData m_currentMovementAction;
    private System.Random m_random;

    // Start is called before the first frame update
    void Start()
    {
        m_random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePlayerInputConfirm()
    {
        if (!m_currentMovementAction.IsActive)
        {
            MakeRoll();
        }
    }

    void MakeRoll()
    {
        // Generate random number for the current player to move.
        m_currentMovementAction.SpacesToMove = m_random.Next(1, 11);
        m_currentMovementAction.IsActive = true;
        RefreshCurrentSpacesText();

        CurrentPlayer.MoveToSpace(MoveToSpaceFinished);
    }

    void MoveToSpaceFinished(BoardSpace newSpace)
    {
        // Check if we should decrement the move counter.
        if (!newSpace.Flags.HasFlag(BoardSpace.SpaceFlags.DoesNotDecrementMoveCounter))
        {
            m_currentMovementAction.SpacesToMove--;
            RefreshCurrentSpacesText();
        }

        if (newSpace.Flags.HasFlag(BoardSpace.SpaceFlags.Stoppable))
        {
            // uhhh transfer control to a different controller/manager?
            // would probably have some data in the BoardSpace to point to whatever is taking control.
            // the new manager needs some sort of "Finished" function so we know to continue moving afterwards (if spaces to move left).
        }
        else
        {
            if (m_currentMovementAction.SpacesToMove > 0)
            {
                CurrentPlayer.MoveToSpace(MoveToSpaceFinished);
            }
            else
            {
                m_currentMovementAction.IsActive = false;
            }
        }
    }

    void RefreshCurrentSpacesText()
    {
        // Update text above player.
        CurrentSpacesText.text = m_currentMovementAction.SpacesToMove.ToString();
    }
}
