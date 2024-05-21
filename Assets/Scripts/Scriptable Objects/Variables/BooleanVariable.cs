using UnityEngine;

[CreateAssetMenu(fileName = "Boolean Variable", menuName = "Scriptable Objects/Variables/Boolean Variable")]
public class BooleanVariable : ScriptableObject {
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    [SerializeField] private bool value;
    public bool Value { get { return value; } set { this.value = value; } }

    public void SetValue(bool value) {
        this.value = value;
    }

    public void SetValue(BooleanVariable value) {
        this.value = value.Value;
    }

    public void ToggleValue() {
        value = !value;
    }
}
