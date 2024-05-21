using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Grid", menuName = "Scriptable Objects/Components/Grid")]
public class Grid : ScriptableObject 
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif


    [SerializeField] private IntegerVariable width;
    [SerializeField] private IntegerVariable height;
    [SerializeField] private BlockTypeListVariable initialGrid;
    [SerializeField] private BlockLifecycleManager _blockLifecycleManager;

    //
    [SerializeField] private BlockTypeListVariable _destroyedBlockTypes;
    //

    private List<Column> columns = new();
    private List<Column> modifiedColumns = new();

    private Transform _gridParent;

    //
    private DFSConnectedBlocksFinder connectedBlocksFinder;
    private BorderBlockFinder borderBlockFinder;
    private NxNAreaBlockFinder nxNAreaBlockFinder;
    //


    public int Width { get { return width.Value; } }
    public int Height { get { return height.Value; } }
    public List<Column> Columns { get { return columns; } set { columns = value; } }
    public List<Column> ModifiedColumns { get { return modifiedColumns; } }
    public BlockTypeListVariable InitialGrid { get { return initialGrid; }}


    public void Initialize() 
    {
        //
        connectedBlocksFinder = new DFSConnectedBlocksFinder();
        borderBlockFinder = new BorderBlockFinder();
        nxNAreaBlockFinder = new NxNAreaBlockFinder();
        //


        if(_blockLifecycleManager == null) 
        {
            Debug.LogError("Block Lifecycle Manager is null.");
        }

        _blockLifecycleManager.SetParent(new GameObject("Block Pools").transform);
        _blockLifecycleManager.SetPools();
    }

    public void SetParent(Transform parent) 
    {
        _gridParent = parent;
    }

    public Block CreateBlock(int row, int col, Vector2 position, BlockType blockType) {
        Block block = _blockLifecycleManager.CreateBlock(blockType);
        block.transform.position = position; block.Row = row; block.Col = col; block.transform.parent = _gridParent; Columns[row].Cells[col].Block = block;
        return block;
    }

    public void DestroyBlock(Block block) 
    {
        Column column = Columns[block.Row];
        AddModifiedColumn(column);
        _destroyedBlockTypes.Items.Add(block.Type);
        _blockLifecycleManager.DestroyBlock(block);
        column.Cells[block.Col].Block = null;
    }

    public void AddModifiedColumn(Column column)
    {
        if (!ModifiedColumns.Contains(column)) 
        {
            ModifiedColumns.Add(column);
        }
    }

    public List<Block> FindConnectedBlocks(int x, int y, BlockType targetBlockType) 
    {
        return connectedBlocksFinder.FindConnectedBlocks(x, y, this, targetBlockType);
    }

    public List<Block> FindBorderBlocks(List<Block> connectedBlocks) 
    {
        return borderBlockFinder.FindBorderBlocks(connectedBlocks, this);
    }

    public List<Block> FindNxNAreaBlocks(Block block, int n) 
    {
        return nxNAreaBlockFinder.FindNxNAreaBlocks(block, this, n);
    }

    public void ResetGridData() 
    {
        ClearGrid();
        columns.Clear();
        modifiedColumns.Clear();
        _blockLifecycleManager.ResetBlockLifecycleManager();
        if(_gridParent != null) Destroy(_gridParent.gameObject);

    }

    private void ClearGrid() {
        foreach (Column column in columns) {
            if (column.Cells == null) {
                continue;
            }
            foreach (Cell cell in column.Cells) {
                if (cell.Block != null && cell.Block.gameObject != null) {
                    Destroy(cell.Block.gameObject);
                }
            }
        }
    }


    public void FindRowBlocks(Block block, out List<Block> leftSideBlocks, out List<Block> rightSideBlocks) 
    {
        leftSideBlocks = new List<Block>();
        rightSideBlocks = new List<Block>();

        int row = block.Row;
        int col = block.Col;

        for(int i = row - 1; i >= 0; i--) 
        {
            leftSideBlocks.Add(columns[i].Cells[col].Block);
        }

        for (int i = row + 1; i < Width; i++) {
            rightSideBlocks.Add(columns[i].Cells[col].Block);
        }
    }

    public void FindColumnBlocks(Block block, out List<Block> downSideBlocks, out List<Block> upSideBlocks) {
        downSideBlocks = new List<Block>();
        upSideBlocks = new List<Block>();

        int row = block.Row;
        int col = block.Col;

        for (int i = col - 1; i >= 0; i--) {
            downSideBlocks.Add(columns[row].Cells[i].Block);
        }

        for (int i = col + 1; i < Height; i++) {
            upSideBlocks.Add(columns[row].Cells[i].Block);
        }
    }



    //[SerializeField] private BlockTypeVectorPairListVariable _destroyedBlocks;
    //[SerializeField] private AudioClipListVariable _blockSFXQueue;
}