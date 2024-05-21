using UnityEngine;

[CreateAssetMenu(fileName = "Float Variable", menuName = "Scriptable Objects/Variables/Float Variable")]
public class FloatVariable : ScriptableObject {
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    [SerializeField] private float value;
    public float Value { get { return value; } set { this.value = value; } }

    public void SetValue(float value) {
        this.value = value;
    }

    public void SetValue(FloatVariable value) {
        this.value = value.Value;
    }

    public void ApplyChange(float amount) {
        value += amount;
    }

    public void ApplyChange(FloatVariable amount) {
        value += amount.Value;
    }
}
