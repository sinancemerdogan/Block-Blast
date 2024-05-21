using UnityEngine;

[CreateAssetMenu(fileName = "Integer Variable", menuName = "Scriptable Objects/Variables/Integer Variable")]
public class IntegerVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    [SerializeField] private int value;
    public int Value { get { return value; } set { this.value = value; } }

    public void SetValue(int value) {
        this.value = value;
    }

    public void SetValue(IntegerVariable value) {
        this.value = value.Value;
    }

    public void ApplyChange(int amount) {
        value += amount;
    }

    public void ApplyChange(IntegerVariable amount) {
        value += amount.Value;
    }
}
