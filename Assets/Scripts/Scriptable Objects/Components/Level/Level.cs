using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject 
{
    [SerializeField] public int level_number;
    [SerializeField] public int grid_width;
    [SerializeField] public int grid_height;
    [SerializeField] public int move_count;
    [SerializeField] public List<string> grid;
    [SerializeField] public List<string> goals;

    public int LevelNumber 
    {
        get { return level_number; }
        set { level_number = value; }
    }

    public int GridWidth 
    {
        get { return grid_width; }
        set { grid_width = value; }
    }

    public int GridHeight 
    {
        get { return grid_height; }
        set { grid_height = value; }
    }

    public int MoveCount 
    {
        get { return move_count; }
        set { move_count = value; }
    }

    public List<string> Grid 
    {
        get { return grid; }
        set { grid = value; }
    }

    public List<string> Goals {
        get { return goals; }
        set { goals = value; }
    }

    public Level Copy() {
        Level copiedLevel = CreateInstance<Level>();
        copiedLevel.level_number = level_number;
        copiedLevel.grid_width = grid_width;
        copiedLevel.grid_height = grid_height;
        copiedLevel.move_count = move_count;
        copiedLevel.grid = new List<string>(grid); // Copy the list
        copiedLevel.goals = new List<string>(goals); // Copy the list
        return copiedLevel;
    }
}

