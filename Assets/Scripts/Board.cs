using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityParty
{
    public class Board : MonoBehaviour
    {
        [HideInInspector]
        public List<BoardSpace> boardSpaces;    // List of spaces controlled by this board.

        // Start is called before the first frame update
        void Start()
        {
            //
        }

        // Update is called once per frame
        void Update()
        {
            //
        }

        public List<BoardSpace> GetBoardSpaces(bool reparent = true)
        {
            if (boardSpaces == null) {
                boardSpaces = new List<BoardSpace>();
            }

            if (reparent) {
                foreach (BoardSpace space in boardSpaces) {
                    space.SetBoard(this);
                }
            }

            return boardSpaces;
        }

        public void AddBoardSpace(BoardSpace space, int index = -1)
        {
            if (boardSpaces == null) {
                boardSpaces = new List<BoardSpace>();
            }

            if (index == -1) {
                boardSpaces.Add(space);
            }
            else {
                boardSpaces.Insert(index, space);
            }

            space.SetBoard(this);
        }
    }
}
