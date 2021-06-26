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

        bool isLinking = true;                  // Is performing a board space link action. @TEMP hard coded to true for now

        private void OnEnable()
        {
            targetBoard = target as Board;
            boardSpaces = targetBoard.GetBoardSpaces();
        }

        private void OnSceneGUI()
        {
            bool needsRepaint = false;

            if (boardSpaces != null) {
                foreach (BoardSpace space in boardSpaces) {
                    // @NOTE: The |= will short circuit the rest of the foreach calls when
                    //        DrawSpaceInScene() returns true.
                    needsRepaint |= DrawSpaceInScene(space);

                    // If the space is linked, draw a line to it.
                    if (space.GetNextSpace() != null) {
                        BoardSpace nextSpace = space.GetNextSpace();

                        Handles.DrawLine(space.GetPosition(), nextSpace.GetPosition());
                    }
                }
            }

            if (needsRepaint) {
                Repaint();
            }
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
                            GUI.color = Color.cyan;
                        }

                        // Individual space row.
                        EditorGUILayout.BeginHorizontal();
                            // Editable XYZ coordinates.
                            EditorGUI.BeginChangeCheck();

                                Vector3 oldPosition = space.GetPosition();
                                Vector3 newPosition = EditorGUILayout.Vector3Field("", oldPosition);

                            if (EditorGUI.EndChangeCheck()) {
                                Undo.RecordObject(targetBoard, "Board Space Moved");
                                space.UpdatePosition(newPosition - oldPosition);
                            }

                            // TEMP select space button.
                            if (GUILayout.Button(EditorGUIUtility.IconContent("d_MoveTool"), GUILayout.Width(25))) {
                                if (selectedBoardSpace == space) {
                                    selectedBoardSpace = null;
                                }
                                else {
                                    selectedBoardSpace = space;
                                }

                                needsRepaint = true;
                            }

                            // Remove space button.
                            if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(25))) {
                                deleteIndex = i;
                                needsRepaint = true;
                            }

                            GUI.color = guiColor;

                        EditorGUILayout.EndHorizontal();
                    }

                    if (deleteIndex > -1) {
                        if (boardSpaces[deleteIndex] == selectedBoardSpace) {
                            selectedBoardSpace = null;
                        }

                        boardSpaces.RemoveAt(deleteIndex);
                    }
                }

                // Add space button.
                if (GUILayout.Button("Add Space")) {
                    Undo.RecordObject(targetBoard, "Board Space Added");

                    BoardSpace space = new BoardSpace();
                    targetBoard.AddBoardSpace(space);
                    selectedBoardSpace = space;

                    needsRepaint = true;
                }

            EditorGUILayout.EndVertical();

            // Repaint the scene (for instance, when a new space is added).
            if (needsRepaint) {
                SceneView.RepaintAll();
            }
        }

        /// <summary>
        /// Draws a selectable BoardSpace in the scene view.
        /// </summary>
        /// <param name="space">The BoardSpace to be drawn.</param>
        private bool DrawSpaceInScene(BoardSpace space)
        {
            if (space == null) {
                return false;
            }

            bool needsRepaint = false;

            // Non-serialized field. Data gets lost during serialize updates?
            space.SetBoard(targetBoard);

            // Draw the selected space as a cyan sphere.
            if (selectedBoardSpace == space) {
                Color c = Handles.color;
                Handles.color = Color.cyan;

                // Process moving the sphere with the position handle.
                EditorGUI.BeginChangeCheck();

                    Vector3 oldPosition = space.GetPosition();
                    Vector3 newPosition = Handles.PositionHandle(oldPosition, Quaternion.identity);

                    float handleSize = HandleUtility.GetHandleSize(newPosition);

                    Handles.SphereHandleCap(-1, newPosition, Quaternion.identity, 0.25f * handleSize, EventType.Repaint);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(targetBoard, "Board Space Moved");
                    space.UpdatePosition(newPosition - oldPosition);
                }

                Handles.color = c;
            }
            // Draw non-selected spaces as a white circle.
            else {
                Vector3 position = space.GetPosition();
                float handleSize = HandleUtility.GetHandleSize(position);

                // Select the space if you click on it in the scene.
                if (Handles.Button(position, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap)) {
                    needsRepaint = true;

                    // Link the selected space if we're in link mode.
                    if (isLinking && selectedBoardSpace != null) {
                        space.LinkSpace(selectedBoardSpace);
                    }

                    selectedBoardSpace = space;
                }
            }

            return needsRepaint;
        }

        /// <summary>
        /// Adds a menu item to create a Board game object in the scene.
        /// </summary>
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
