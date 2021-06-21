using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public BoardSpace CurrentSpace;
    public int MovesRemaining;
    public float SpeedPerSecond = 2;

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
        if (MovesRemaining > 0)
        {
            MoveToSpace();
        }
    }

    void MoveToSpace()
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

    void MoveToSpaceFinished()
    {
        // Decrement move counter.
        MovesRemaining--;
        CurrentSpace = CurrentSpace.NextSpace;
    }
}