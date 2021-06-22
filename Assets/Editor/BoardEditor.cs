using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityParty.Editor
{
    [CustomEditor(typeof(Board))]
    public class BoardEditor : UnityEditor.Editor
    {
        Board targetBoard;                      // The board being edited in the scene.
        List<BoardSpace> boardSpaces;           // The board's spaces.

        BoardSpace selectedBoardSpace = null;   // The selected space we're editing.

        private void OnEnable()
        {
            targetBoard = target as Board;
            boardSpaces = targetBoard.GetBoardSpaces();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            bool needsRepaint = false;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Spaces");

                if (boardSpaces != null) {
                    int deleteIndex = -1;

                    for (int i = 0; i < boardSpaces.Count; i++) {
                        Color guiColor = GUI.color;
                        BoardSpace space = boardSpaces[i];

                        if (space == selectedBoardSpace) {
                            GUI.color = Color.blue;
                        }

                        EditorGUILayout.BeginHorizontal();

                            EditorGUILayout.Vector3Field("", space.GetPosition());

                            if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(25))) {
                                deleteIndex = i;
                                needsRepaint = true;
                            }

                        EditorGUILayout.EndHorizontal();
                    }

                    if (deleteIndex > -1) {
                        if (boardSpaces[deleteIndex] == selectedBoardSpace) {
                            selectedBoardSpace = null;
                        }

                        boardSpaces.RemoveAt(deleteIndex);
                    }
                }

                if (GUILayout.Button("Add Space"))
                {
                    Undo.RecordObject(targetBoard, "Board Space Added");

                    BoardSpace space = new BoardSpace();
                    targetBoard.AddBoardSpace(space);

                    Debug.Log(boardSpaces.Count);

                    needsRepaint = true;
                }

            EditorGUILayout.EndVertical();

            // Repaint the scene (for instance, when a new space is added).
            if (needsRepaint) {
                SceneView.RepaintAll();
            }
        }

        // Menu item for creating a board.
        [MenuItem("GameObject/UnityParty/Create Board")]
        public static void CreateBoardManager()
        {
            GameObject board = new GameObject("Board");
            board.AddComponent<Board>();

            // Select it.
            Selection.activeGameObject = board;
        }
    }
}
