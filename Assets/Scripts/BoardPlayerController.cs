using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlayerController : MonoBehaviour
{
    // =====================================================
    // Publics
    public BoardSpace CurrentSpace;
    public float SpeedPerSecond = 10;

    public delegate void MoveToSpaceFinishedDelegate(BoardSpace newSpace);

    // =====================================================
    // Privates
    private bool m_isMoving;
    private MoveToSpaceFinishedDelegate m_moveToSpaceFinishedCallback;
    private bool m_isRotating;
    private Quaternion targetDir;

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
        // Continue rotating player until we are finished
        if (m_isRotating)
        {
            UpdateRotatePlayer();
        }

        // Move to the next space if we are currently moving.
        if (m_isMoving)
        {
            UpdateMoveToSpace();
        }
    }

    void UpdateMoveToSpace()
    {
        // Perform any movement necessary.
        Vector3 ourPosition = m_characterTransform.position;
        Vector3 targetPosition = CurrentSpace.transform.position;
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
        // Move to the space we've selected next (if selectable).
        CurrentSpace = CurrentSpace.NextSpace;

        //Character rotates towards the target space
        RotatePlayer(CurrentSpace.transform.position);

        m_isMoving = true;
        m_moveToSpaceFinishedCallback = callback;
    }

    void MoveToSpaceFinished()
    {
        // Decrement move counter.
        m_isMoving = false;

        // Fire off signal that we're done or something.
        if (m_moveToSpaceFinishedCallback != null)
        {
            MoveToSpaceFinishedDelegate temp = m_moveToSpaceFinishedCallback;
            m_moveToSpaceFinishedCallback = null;
            temp(CurrentSpace);
        }
    }


    void UpdateRotatePlayer()
    {
        // Perform rotation over time
        Quaternion currDir = m_characterTransform.rotation;
        m_characterTransform.rotation = Quaternion.RotateTowards(currDir, targetDir, Time.deltaTime * 500.0f);

        if (m_characterTransform.rotation == targetDir)
        {
            // Rotation complete
            RotatePlayerFinished();
        }
    }

    public void RotatePlayer(Vector3 target)
    {
        m_isRotating = true;

        // Place the target and the character position at the same height before finding the target direction
        target.y = m_characterTransform.position.y;
        FindTargetDirection(target);
    }

    void RotatePlayerFinished()
    {
        // Finished rotating
        m_isRotating = false;
    }

    void FindTargetDirection(Vector3 target)
    {
        // Find angle from forward direction to target direction (current space)
        Vector3 forwardVec = m_characterTransform.forward;
        forwardVec.y = target.y;
        Vector3 targetSpace = target - m_characterTransform.position;
        targetSpace = Vector3.Normalize(targetSpace);
        float angle = Vector3.Angle(forwardVec, targetSpace);
 
        // Check the direction of the angle via cross product
        Vector3 cross = Vector3.Cross(forwardVec, targetSpace);
        if (cross.y < 0) angle = -angle;

        // Store target angle
        targetDir = m_characterTransform.rotation;
        targetDir.eulerAngles = (targetDir.eulerAngles += new Vector3(0.0f, angle, 0.0f));
    }

}