using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
    public override VisualElement CreateInspectorGUI() {

        VisualElement root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        // Create a button to open the LevelEditorWindow
        Button openEditorButton = new Button();
        openEditorButton.text = "Open Level Editor";
        openEditorButton.clickable.clicked += OpenLevelEditorWindow;
        root.Add(openEditorButton);

        Button applyChanges = new Button();
        applyChanges.text = "Apply Changes";
        applyChanges.clickable.clicked += ApplyChanges;
        root.Add(applyChanges);

        return root;
    }

    private void OpenLevelEditorWindow() {
        // Retrieve the currentLevel property
        SerializedProperty currentLevelProperty = serializedObject.FindProperty("currentLevel");

        // Ensure it's not null and its type is Level
        if (currentLevelProperty != null && currentLevelProperty.objectReferenceValue is Level) {
            Level currentLevel = (Level)currentLevelProperty.objectReferenceValue;

            // Pass the currentLevel to the LevelEditorWindow
            LevelEditorWindow.ShowWindow(currentLevel);
        }
        else {
            Debug.LogWarning("Current Level is not set");
        }
    }
    private void ApplyChanges() 
    {
        LevelManager levelManager = (LevelManager)target;
        levelManager.WriteLevel();
    }
}
