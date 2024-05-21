using UnityEngine;

public class GoalSO : ScriptableObject
{
    [SerializeField] private BlockType _type;
    [SerializeField] private int _count;
    [SerializeField] private bool _isCompleted;

    public BlockType Type {
        get { return _type; }
        set { _type = value; }
    }

    public int Count {
        get { return _count; }
        set { _count = value; }
    }

    public bool IsCompleted { 
        get { return _isCompleted; } 
        set { _isCompleted = value; }
    }

}
