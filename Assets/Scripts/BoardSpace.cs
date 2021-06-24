using System;
using UnityEngine;

namespace UnityParty
{
    [Flags]
    public enum SpaceFlags
    {
        DoesNotDecrementMoveCounter = 0x01,     // Can't end of the space, doesn't decrement movement count.
        Stoppable = 0x02,                       // User stops on the space, control is passed to a different handler(?)
        Flag3 = 0x04,
        Flag4 = 0x08,
    }

    [Serializable]
    public class BoardSpace
    {
        public Vector3 position;                // This space's position, relative to the board.
        Board board;                            // The board this space belongs to.

        public void SetBoard(Board board)
        {
            this.board = board;
        }

        public Vector3 GetPosition()
        {
            if (board != null) {
                return board.transform.position + position;
            }

            return position;
        }

        public void UpdatePosition(Vector3 position)
        {
            this.position += position;
        }
    }
}
