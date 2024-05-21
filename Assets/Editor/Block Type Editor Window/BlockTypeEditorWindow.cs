using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class BlockEditorWindow : EditorWindow 
{
    [MenuItem("Tools/Block Editor Window")]

    public static void ShowWindow() 
    {
        var window = GetWindow<BlockEditorWindow>();
        window.titleContent = new GUIContent("Block Editor");
        window.minSize = new Vector2(600,800);
    }

    private void OnEnable() 
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Block Type Editor Window/BlockTypeEditorWindow.uxml");
        TemplateContainer treeAsset = original.CloneTree();
        rootVisualElement.Add(treeAsset);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/Block Type Editor Window/BlockTypeEditorWindowStyles.uss");
        rootVisualElement.styleSheets.Add(styleSheet);

        CreateBlockListView();
    }

    private void CreateBlockListView() 
    {
        List<BlockType> blocks = FindAllBlocks();

        ListView blockList = rootVisualElement.Query<ListView>("block-list");
        blockList.makeItem = () => new Label();
        blockList.bindItem = (element, i) => (element as Label).text = blocks[i].name;
        
        blockList.itemsSource = blocks;
        blockList.selectionType = SelectionType.Single;

        blockList.selectionChanged += (enumerable) => 
        {
            foreach (Object iterator in enumerable) 
            {
                Box blockInfoContainer = rootVisualElement.Query<Box>("block-info");
                blockInfoContainer.Clear();

                // Instantiate the custom editor for BlockType
                BlockType block = iterator as BlockType;
                BlockTypeEditor blockEditor = Editor.CreateEditor(block) as BlockTypeEditor;

                // Add the custom editor to the block-info section
                blockInfoContainer.Add(blockEditor.CreateInspectorGUI());

                LoadBlockImage(block.Sprites[0].texture);
            }
        };

        blockList.Rebuild();
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


    private void LoadBlockImage(Texture texture) 
    {
        Image blockImagePreview = rootVisualElement.Query<Image>("preview");
        blockImagePreview.style.width = 142;
        blockImagePreview.style.height = 162;
        blockImagePreview.image = texture;
    }
}

