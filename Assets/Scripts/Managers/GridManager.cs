using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour 
{
    [SerializeField] private BlockTypeListVariable _randomBlocks;
    [SerializeField] private Grid grid;

    public void SetUpGrid() 
    {
        grid.ResetGridData();
        grid.SetParent(new GameObject("Grid").transform);
        grid.Initialize();
        PopulateGrid();
        SetConnectedBlocks();
    }

    private void PopulateGrid() 
    {
        int gridWidth = grid.Width;
        int gridHeight = grid.Height;

        grid.Columns = new List<Column>(gridWidth);
        for (int i = 0; i < gridWidth; i++) 
        {
            List<Cell> colData = new(gridHeight);
            for (int j = 0; j < gridHeight; j++) 
            {
                Cell cellData = ScriptableObject.CreateInstance<Cell>(); cellData.Row = i; cellData.Col = j;
                colData.Add(cellData);    
            }
            Column columnData = ScriptableObject.CreateInstance<Column>(); columnData.ColumnIndex = i; columnData.Cells = colData; 
            grid.Columns.Add(columnData);
        }

        for (int i = 0; i < gridWidth; i++) 
        {
            for (int j = 0; j < gridHeight; j++) 
            {
                int flatIndex = i * gridHeight + j;
                grid.CreateBlock(i, j, new Vector2(i, j), grid.InitialGrid.Items[flatIndex]);
            }
        }
    }

    public void RearrangeColumns() 
    {
        foreach (Column column in grid.ModifiedColumns) 
        {
            for (int i = 0; i < column.Cells.Count; i++) 
            {
                if (column.Cells[i].Block == null) 
                {
                    int j = i + 1;
                    while (j < column.Cells.Count && column.Cells[j].Block == null) 
                    {
                        j++;
                    }
                    if (j < column.Cells.Count) {
                        if (column.Cells[j].Block is IFallable fallableBlock && column.Cells[j].Block != null) 
                        {
                            fallableBlock.Fall(i);
                            column.Cells[j].Block.Col = i;
                            column.Cells[i].Block = column.Cells[j].Block;
                            column.Cells[j].Block = null;
                        }
                    }
                }
            }
        }
        FillColumns();
    }

    public void FillColumns() {
        foreach (Column column in grid.ModifiedColumns) 
        {
            for (int i = FindModifiablePortionIndex(column); i < column.Cells.Count; i++) 
            {
                if (column.Cells[i].Block == null) 
                 {
                    int randomNumber = Random.Range(0, _randomBlocks.Items.Count);
                    Block block = grid.CreateBlock(column.ColumnIndex, i, new Vector2(column.ColumnIndex, grid.Height + i), _randomBlocks.Items[randomNumber]);
                    if (block is IFallable fallableBlock) {
                        fallableBlock.Fall(i);
                    }
                }
            }
        }
        SetConnectedBlocks();
        grid.ModifiedColumns.Clear();
    }

    private int FindModifiablePortionIndex(Column column) 
    {
        int index = 0;
        for (int i = column.Cells.Count - 1; i >= 0; i--) 
        {
            if (column.Cells[i].Block != null && column.Cells[i].Block is not IFallable) 
            {
                if (i + 1 <= column.Cells.Count) 
                {
                    index = i;
                    break;
                }
            }
        }
        return index;
    }

    private void SetConnectedBlocks() {
        List<Block> processedBlocks = new();

        for (int i = 0; i < grid.Width; i++) {
            for (int j = 0; j < grid.Height; j++) {

                Block block = grid.Columns[i].Cells[j].Block;

                if (block is ITappable && !processedBlocks.Contains(block)) {
                    List<Block> connectedBlocks = grid.FindConnectedBlocks(block.Row, block.Col, block.Type);

                    foreach (Block connectedBlock in connectedBlocks) {
                        processedBlocks.Add(connectedBlock);
                        if (connectedBlock is ITappable tappableBlock2) {
                            tappableBlock2.SetConnectedBlock(connectedBlocks);
                        }
                    }
                }
            }
        }
    }

    private void OnDisable() {
        grid.ResetGridData();
    }

    //private void DebugGrid() {
    //    for (int i = 0; i < width; i++) {
    //        for (int j = 0; j < height; j++) {
    //            Column column = Grid.Columns[i];
    //            Cell cell = column.Cells[j];

    //            if (cell.Block != null) {
    //                Debug.Log($"Block at ({i}, {j})");
    //            }
    //            else {
    //                Debug.Log($"Empty cell at ({i}, {j})");
    //            }
    //        }
    //    }
    //}

}






