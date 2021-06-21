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
    public CharacterController CurrentPlayer;

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
        bool isMakeRollKeyDown = Input.GetKeyDown(KeyCode.Space);
        if (isMakeRollKeyDown && !m_currentMovementAction.IsActive)
        {
            MakeRoll();
        }
    }

    void MakeRoll()
    {
        // Generate random number for the current player to move.
        m_currentMovementAction.SpacesToMove = m_random.Next(1, 10);
        m_currentMovementAction.IsActive = true;

        CurrentPlayer.MoveToSpace(MoveToSpaceFinished);
    }

    void MoveToSpaceFinished()
    {
        m_currentMovementAction.SpacesToMove--;
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
