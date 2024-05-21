using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {

    [SerializeField] private VisualTreeAsset m_UXML;
    private Level m_LevelToEdit;
    private VisualElement _root;

    private SerializedObject m_SerializedObject;
    private SerializedProperty _levelNumber;
    private SerializedProperty _moveCount;
    private SerializedProperty _gridWidth;
    private SerializedProperty _gridHeight;
    private SerializedProperty _grid;
    private SerializedProperty _goals;

    // Reference to the loaded SpriteIdMapping asset
    private SpriteIDMapper spriteIDMapper;

    private Sprite selectedSprite;
    private string selectedID;
    private VisualElement selectedElement;

    public override VisualElement CreateInspectorGUI() {
        VisualElement root = new VisualElement();

 
        IMGUIContainer imguiContainer = new IMGUIContainer(() => {
            DrawDefaultInspector();
        });

        // Add the IMGUI section to the root VisualElement
        root.Add(imguiContainer);

        // Create your UIElements section
        Button openEditorButton = new Button();
        openEditorButton.text = "Open Level Editor";
        openEditorButton.clickable.clicked += OpenLevelEditorWindow;
        root.Add(openEditorButton);

        return root;
    }


    private void OpenLevelEditorWindow() {
        LevelEditorWindow.ShowWindow((Level)target);
    }

    public void CreateUI(VisualElement root, Level levelToEdit) {
        root.Clear();

        _root = root;
        m_UXML.CloneTree(_root);
        m_LevelToEdit = levelToEdit;

        m_SerializedObject = new SerializedObject(m_LevelToEdit);

        _levelNumber = m_SerializedObject.FindProperty("level_number");
        _moveCount = m_SerializedObject.FindProperty("move_count");

        _grid = m_SerializedObject.FindProperty("grid");
        _gridWidth = m_SerializedObject.FindProperty("grid_width");
        _gridHeight = m_SerializedObject.FindProperty("grid_height");
        _goals = m_SerializedObject.FindProperty("goals");

        IntegerField _levelNumberField = _root.Query<IntegerField>("levelnumber");
        IntegerField _moveCountField = _root.Query<IntegerField>("movecount");

        _levelNumberField.BindProperty(_levelNumber);
        _moveCountField.BindProperty(_moveCount);

        SliderInt _gridWidthfield = _root.Query<SliderInt>("gridwidth");
        SliderInt _gridHeightfield = _root.Query<SliderInt>("gridheight");

        _gridWidthfield.BindProperty(_gridWidth);
        _gridHeightfield.BindProperty(_gridHeight);

        _gridWidthfield.RegisterValueChangedCallback(evt => DisplayGrid());
        _gridHeightfield.RegisterValueChangedCallback(evt => DisplayGrid());

        LoadSpriteIdMapping();

        DisplayBlocks();
        DisplayGrid();
        DisplayGoals();

    }

    private void DisplayBlocks() {
        VisualElement panel2 = _root.Query<VisualElement>("panel2");
        ScrollView blockListScroller = _root.Query<ScrollView>("blocklistscroller");
        panel2.Add(blockListScroller);


        //List<BlockType> blocks = FindAllBlocks();

        //foreach (BlockType block in blocks) 
        //{
        //    Image spriteElement = new Image();
        //    spriteElement.sprite = block.Sprites.First();
        //    spriteElement.AddToClassList("spriteImage");
        //    spriteElement.RegisterCallback<MouseUpEvent>(OnListSpriteClick);
        //    spriteElement.userData = block.ID;
        //    blockListScroller.Add(spriteElement);

        //}

        int index = 0;
        foreach (var sprite in spriteIDMapper.spritesDict.Values) {
            Image spriteElement = new Image();
            spriteElement.sprite = sprite;
            spriteElement.AddToClassList("spriteImage");
            spriteElement.RegisterCallback<MouseUpEvent>(OnListSpriteClick);
            spriteElement.userData = index;
            blockListScroller.Add(spriteElement);
            index++;
        }
    }

    private List<BlockType> FindAllBlocks() {
        var guids = AssetDatabase.FindAssets("t:BlockType");
        List<BlockType> blocks = new List<BlockType>();

        for (int i = 0; i < guids.Length; i++) {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            blocks.Add(AssetDatabase.LoadAssetAtPath<BlockType>(path));
        }

        return blocks.OrderBy(x => x.name.Split(" ").Last()).ToList();
    }

    void OnListSpriteClick(MouseUpEvent evt) {

        if (selectedElement != null) {
            selectedElement.style.borderTopColor = new StyleColor(new Color(1f, 1f, 1f, 0f));
            selectedElement.style.borderRightColor = new StyleColor(new Color(1f, 1f, 1f, 0f));
            selectedElement.style.borderBottomColor = new StyleColor(new Color(1f, 1f, 1f, 0f));
            selectedElement.style.borderLeftColor = new StyleColor(new Color(1f, 1f, 1f, 0f));
        }

        // Cast the currentTarget to an Image element
        if (evt.currentTarget is not Image clickedSprite) return;

        selectedElement = clickedSprite;
        selectedElement.style.borderTopColor = new StyleColor(new Color(1f, 1f, 1f, 1f));
        selectedElement.style.borderRightColor = new StyleColor(new Color(1f, 1f, 1f, 1f));
        selectedElement.style.borderBottomColor = new StyleColor(new Color(1f, 1f, 1f, 1f));
        selectedElement.style.borderLeftColor = new StyleColor(new Color(1f, 1f, 1f, 1f));


        // Access the sprite of the clicked Image element
        selectedSprite = clickedSprite.sprite;
        //selectedID = (string)clickedSprite.userData;

        // Iterate over the spritesDict to find the matching sprite and its corresponding ID
        foreach (var pair in spriteIDMapper.spritesDict) {
            if (pair.Value == clickedSprite.sprite) {
                selectedID = pair.Key;
                break;
            }
        }
    }
    void OnGridSpriteClick(MouseUpEvent evt) {
        // Cast the currentTarget to an Image element
        if (evt.currentTarget is not Image clickedSprite) return;
        if (selectedSprite == null) return;
        if (selectedID == null) return;

        // Change the sprite of the clicked image to the selected sprite
        clickedSprite.sprite = selectedSprite;

        // Get the index of the clicked grid element
        int clickedIndex = (int)clickedSprite.userData;

        // Update the value of the corresponding grid element to the selected ID
        SerializedProperty clickedElement = _grid.GetArrayElementAtIndex(clickedIndex);
        clickedElement.stringValue = selectedID;

        // Apply modifications to the serialized object
        m_SerializedObject.ApplyModifiedProperties();

    }
    void OnGoalSpriteClick(MouseUpEvent evt) {
        // Cast the currentTarget to an Image element
        if (evt.currentTarget is not Image clickedSprite) return;
        if (selectedSprite == null) return;
        if (selectedID == null) return;

        clickedSprite.sprite = selectedSprite;
        int clickedIndex = (int)clickedSprite.userData;

        // Update the value of the corresponding grid element to the selected ID
        SerializedProperty clickedElement = _goals.GetArrayElementAtIndex(clickedIndex);
        clickedElement.stringValue = selectedID;

        // Apply modifications to the serialized object
        m_SerializedObject.ApplyModifiedProperties();

    }

    private void LoadSpriteIdMapping() {
        string assetPath = "Assets/Scripts/BlockTypeMapper/Sprite ID Mapper.asset";
        spriteIDMapper = AssetDatabase.LoadAssetAtPath<SpriteIDMapper>(assetPath);

        if (spriteIDMapper == null) 
        {
            Debug.LogWarning("No Sprite Id Mapper found.");
        }
    }
    public void DisplayGrid() {
        VisualElement panel1 = _root.Query<VisualElement>("panel1");
        VisualElement gridSection = panel1.Query<VisualElement>("gridsection");
        VisualElement gridLayout = gridSection.Query<VisualElement>("gridlayout");

        gridLayout.Clear();

        ResizeGridIfNeeded();


        for (int y = 0; y < _gridHeight.intValue; y++) {
            VisualElement row = new VisualElement();
            row.AddToClassList("row");
            gridLayout.Add(row);

            for (int x = 0; x < _gridWidth.intValue; x++) {
                int index = y * _gridWidth.intValue + x;

                SerializedProperty gridElement = _grid.GetArrayElementAtIndex(index);
                string id = gridElement.stringValue;

                // Retrieve the sprite associated with the ID
                Sprite sprite;

                spriteIDMapper.spritesDict.TryGetValue(id, out sprite);
                if (sprite == null) {
                    sprite = spriteIDMapper.spritesDict["rand"];
                }

                // Create an image for the sprite
                Image spriteElement = new Image();
                spriteElement.sprite = sprite;

                // Assign the index to the element
                spriteElement.userData = index;

                // Register click event handler for the sprite element
                spriteElement.RegisterCallback<MouseUpEvent>(OnGridSpriteClick);

                VisualElement cell = new VisualElement();
                cell.AddToClassList("cell");
                cell.Add(spriteElement);

                //**********************************************************************
                //gridLayout.RegisterCallback<GeometryChangedEvent>(OnElementResized);



                row.Add(cell);

            }
        }
    }


    private void ResizeGridIfNeeded() {
        // Calculate the new size of the grid
        int newSize = _gridWidth.intValue * _gridHeight.intValue;
        int currentSize = _grid.arraySize;

        if (newSize < currentSize) {
            // If the new size is smaller, remove excess elements from the end of the grid
            for (int i = currentSize - 1; i >= newSize; i--) {
                _grid.DeleteArrayElementAtIndex(i);
            }
        }
        else if (newSize > currentSize) {
            // If the new size is larger, add new elements with value "rand"
            for (int i = currentSize; i < newSize; i++) {
                _grid.InsertArrayElementAtIndex(i);
                SerializedProperty newElement = _grid.GetArrayElementAtIndex(i);
                newElement.stringValue = "rand";
            }
        }

        // Apply changes to the serialized object
        m_SerializedObject.ApplyModifiedProperties();
    }

    private void DisplayGoals() {
        VisualElement panel1 = _root.Query<VisualElement>("panel1");
        VisualElement goalsection = panel1.Query<VisualElement>("goalsection");
        VisualElement goalsGrid = goalsection.Query<VisualElement>("goalsgrid");

        // Clear existing goal elements
        goalsGrid.Clear();

        // Display existing goals
        for (int i = 0; i < _goals.arraySize; i++) {
            SerializedProperty goalElement = _goals.GetArrayElementAtIndex(i);
            string id = goalElement.stringValue;

            // Retrieve the sprite associated with the ID
            Sprite sprite;
            spriteIDMapper.spritesDict.TryGetValue(id, out sprite);
            if (sprite == null) {
                sprite = spriteIDMapper.spritesDict["rand"];
            }

            // Create an image for the sprite
            Image spriteElement = new Image();
            spriteElement.sprite = sprite;

            // Assign the index to the element
            spriteElement.userData = i;

            // Register click event handler for the sprite element
            spriteElement.RegisterCallback<MouseUpEvent>(OnGoalSpriteClick);

            spriteElement.AddToClassList("spriteImage");


            // Create a button to remove the goal
            Button removeButton = new Button();
            removeButton.text = "";
            removeButton.AddToClassList("removebutton");
            int currentIndex = i;
            removeButton.clicked += () => RemoveGoalButtonClicked(currentIndex);

            spriteElement.Add(removeButton);
            goalsGrid.Add(spriteElement);

        }

        // Display "Add Goal" button
        Button addButton = new Button();
        addButton.text = "Add Goal";
        addButton.clicked += AddGoalButtonClicked;
        addButton.style.alignSelf = Align.Center;
        goalsGrid.Add(addButton);

        // Limit the number of goals to 4
        if (_goals.arraySize < 4) {
            addButton.SetEnabled(true);
        }
        else {
            addButton.SetEnabled(false);
        }
    }

    void AddGoalButtonClicked() {
        // Add a new goal to the goals list
        int newIndex = _goals.arraySize;
        _goals.arraySize++;
        SerializedProperty newGoal = _goals.GetArrayElementAtIndex(newIndex);

        // Initialize the new element
        newGoal.stringValue = "rand";

        // Apply changes to the serialized object
        m_SerializedObject.ApplyModifiedProperties();

        // Update the displayed goals
        DisplayGoals();
    }


    void RemoveGoalButtonClicked(int index) {
        // Remove the goal at the specified index from the goals list
        _goals.DeleteArrayElementAtIndex(index);

        // Apply changes to the serialized object
        m_SerializedObject.ApplyModifiedProperties();

        // Update the displayed goals
        DisplayGoals();
    }


    //void OnElementResized(GeometryChangedEvent evt) {
    //    Update element's height based on its width and aspect ratio
    //    if (evt.currentTarget is not VisualElement resizedCell) return;
    //    ResizeElement(resizedCell);
    //}

    //void ResizeElement(VisualElement resizedCell) {
    //    Get the width of the element
    //    float width = resizedCell.layout.width;
    //    float height = resizedCell.layout.height;

    //    float aspectRatio = 71f / 81f;
    //    float currentAspectRatio = width / height;

    //    if (Mathf.Approximately(currentAspectRatio, aspectRatio)) return;

    //    Set the height of the element
    //    resizedCell.style.height = width / aspectRatio;
    //}
}
