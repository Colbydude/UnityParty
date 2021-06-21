using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // =====================================================
    // Publics
    public BoardSpace CurrentSpace;
    public int MovesRemaining;
    public float SpeedPerSecond = 2;

    public delegate void MoveToSpaceFinishedDelegate();

    // =====================================================
    // Privates
    private bool m_isMoving;
    private MoveToSpaceFinishedDelegate m_moveToSpaceFinishedCallback;

    // =====================================================
    // Static Privates
    private Transform m_characterTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_characterTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Fire off "moved(?)" event once destination space is reached.
        if (m_isMoving)
        {
            UpdateMoveToSpace();
        }
    }

    void UpdateMoveToSpace()
    {
        // Perform any movement necessary.
        Vector3 ourPosition = m_characterTransform.position;
        Vector3 targetPosition = CurrentSpace.NextSpace.transform.position;
        targetPosition.y = ourPosition.y;
        m_characterTransform.position = Vector3.MoveTowards(ourPosition, targetPosition, Time.deltaTime * SpeedPerSecond);

        if (m_characterTransform.position.Equals(targetPosition))
        {
            // Begin moving to next space.
            MoveToSpaceFinished();
        }
    }

    public void MoveToSpace(MoveToSpaceFinishedDelegate callback)
    {
        m_isMoving = true;
        m_moveToSpaceFinishedCallback = callback;
    }

    void MoveToSpaceFinished()
    {
        // Decrement move counter.
        CurrentSpace = CurrentSpace.NextSpace;
        m_isMoving = false;

        // Fire off signal that we're done or something.
        if (m_moveToSpaceFinishedCallback != null)
        {
            MoveToSpaceFinishedDelegate temp = m_moveToSpaceFinishedCallback;
            m_moveToSpaceFinishedCallback = null;
            temp();
        }
    }
}