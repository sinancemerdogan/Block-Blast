using UnityEngine;
public class Cell : ScriptableObject 
{

    private int row;
    private int col;
    private Block block;

    public int Row { get { return row; } set { row = value; } }
    public int Col { get { return col; } set { col = value; } }
    public Block Block { get { return block; } set { block = value; } }
}