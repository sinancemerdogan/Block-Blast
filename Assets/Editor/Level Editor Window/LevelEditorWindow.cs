using UnityEditor;

public class LevelEditorWindow : EditorWindow {
    private Level levelToEdit;
    private LevelEditor levelEditor;

    public static void ShowWindow(Level level) {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level Editor");
        window.Initialize(level);
    }

    public void Initialize(Level level) {
        levelToEdit = level;
        levelEditor = CreateInstance<LevelEditor>();
        levelEditor.CreateUI(rootVisualElement, levelToEdit);
    }
}
