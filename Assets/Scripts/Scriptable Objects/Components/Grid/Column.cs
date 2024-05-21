using System.Collections.Generic;
using UnityEngine;

public class Column : ScriptableObject {

    private int columnIndex;

    private List<Cell> cells = new();
    public int ColumnIndex { get { return columnIndex; } set { columnIndex = value; } }
    public List<Cell> Cells { get { return cells; } set { cells = value; } }
}