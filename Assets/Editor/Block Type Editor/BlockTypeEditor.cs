using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

[CustomEditor(typeof(BlockType))]
public class BlockTypeEditor : Editor 
{

    SerializedProperty _damageReceiver;
    SerializedProperty _damageDealer;
    SerializedProperty _creator;


    public override VisualElement CreateInspectorGUI() 
{
        var root = new VisualElement();
        VisualTreeAsset UXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Block Type Editor/BlockTypeEditor.uxml");
        UXML.CloneTree(root);
        root.Bind(serializedObject);

        _damageReceiver = serializedObject.FindProperty("damageReceiver");
        _damageDealer = serializedObject.FindProperty("damageDealer");
        _creator = serializedObject.FindProperty("creator");

        PropertyField damageReceiverField = root.Query<PropertyField>("damage-receiver-field");
        PropertyField damageDealerBoolField = root.Query<PropertyField>("damage-dealer-field");
        PropertyField creatorField = root.Query<PropertyField>("creator-field");


        VisualElement damageReceiverSection = root.Query("damage-receiver");
        damageReceiverField.RegisterValueChangeCallback(evt => {
            damageReceiverSection.style.display = _damageReceiver.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
        });

        VisualElement damageDealerSection = root.Query("damage-dealer");
        damageDealerBoolField.RegisterValueChangeCallback(evt => {
            damageDealerSection.style.display = _damageDealer.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
        });

        VisualElement creatorSection = root.Query("creator");
        creatorField.RegisterValueChangeCallback(evt => {
            creatorSection.style.display = _creator.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
        });

        return root;
    }
}
