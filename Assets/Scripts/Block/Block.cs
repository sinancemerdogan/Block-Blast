using UnityEngine;

public abstract class Block : MonoBehaviour 
{
    public int Row { get; set; }
    public int Col { get; set; }

    [SerializeField] private BlockType type;
    [SerializeField] protected Grid grid;

    public BlockType Type {
        get { return type; }
    }

}