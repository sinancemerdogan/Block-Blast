using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private IntegerVariable gridWidth;
    [SerializeField] private IntegerVariable gridHeight;
    [SerializeField] private BlockTypeListVariable initialGrid;

    [SerializeField] private IntegerVariable moveCount;
    [SerializeField] private GoalsSO goalsSO;

    [SerializeField] private BlockTypeMapper _blockTypeMapper;
    [SerializeField] private UnityEvent levelLoaded;
    [SerializeField] private Level currentLevel;

    public void Awake() 
    {
        LoadLevel();
        WriteLevel();
    }

    public void WriteLevel() 
    {
        if (currentLevel == null) 
        {
            Debug.LogWarning("Current Level is not set"); return;
        }

        ResetLevelInfo();

        //Grid size
        gridWidth.SetValue(currentLevel.GridWidth);
        gridHeight.SetValue(currentLevel.GridHeight);

        //Move count
        moveCount.Value = currentLevel.MoveCount;

        //Goal counts
        Dictionary<BlockType, int> goalsDictionary = new Dictionary<BlockType, int>();

        //Level Goals
        List<BlockType> levelGoals = new List<BlockType>();
        foreach (var goal in currentLevel.Goals) 
        {
            if (goal.Equals("")) continue; 
            levelGoals.Add(_blockTypeMapper.GetBlockTypeByID(goal));
            goalsDictionary.Add(_blockTypeMapper.GetBlockTypeByID(goal), 0);
        }

        //Initiai grid
        for (int i = 0; i < gridWidth.Value; i++) {
            for (int j = 0; j < gridHeight.Value; j++) {

                int flatIndex = j * gridWidth.Value + i;
                BlockType currentBlock = _blockTypeMapper.GetBlockTypeByID(currentLevel.Grid[flatIndex]);
                initialGrid.Items.Add(currentBlock);

                //Goal counts
                if (levelGoals.Contains(currentBlock)) {
                    if (goalsDictionary.ContainsKey(currentBlock)) {
                        goalsDictionary[currentBlock]++;
                    }
                }
            }
        }

        //Goals
        foreach (var key in goalsDictionary.Keys) 
        {
            GoalSO goalSO = ScriptableObject.CreateInstance<GoalSO>();
            goalSO.Type = key;
            goalSO.Count = goalsDictionary[key];
            goalSO.IsCompleted = false;
            goalsSO.Goals.Add(goalSO);
        }
        levelLoaded.Invoke();
    }

    public void SetLevel(Level level) 
    {
        currentLevel = level;
    }

    public Level GetLevel() 
    {
        return currentLevel;
    }

    public void LoadLevel() 
    {
        if (!PlayerPrefs.HasKey("Level")) {
            PlayerPrefs.SetInt("Level", 1);
        }
        if (PlayerPrefs.GetInt("Level") >= 5) {
            PlayerPrefs.SetInt("Level", 1);
        }

        Level level = Resources.Load<Level>("Levels/Level " + PlayerPrefs.GetInt("Level"));
        currentLevel = level.Copy();
        currentLevel.name = "Level " + PlayerPrefs.GetInt("Level");
    }

    public void UpdatePlayerLevel() 
    {
        if(PlayerPrefs.HasKey("Level")) 
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
    }

    public void ResetLevelInfo() 
    {
        gridWidth.SetValue(0);
        gridHeight.SetValue(0);
        moveCount.SetValue(0);
        goalsSO.ResetGoals();
        initialGrid.Items.Clear();
    }

    private void OnDisable() 
    {
        ResetLevelInfo();
    }
}
