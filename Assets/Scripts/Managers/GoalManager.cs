using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalsSO _goalsSO;
    [SerializeField] private BlockTypeListVariable _destroyedBlockTypes;

    [SerializeField] private BooleanVariable _allGoalsCompleted;
    [SerializeField] private UnityEvent _levelEndEvent;

    public void UpdateGoalCounts() 
    {
        List<BlockType> destroyedBlocksTypes = new(_destroyedBlockTypes.Items);

        if (destroyedBlocksTypes.Count == 0) 
        {
            return;
        }

        foreach (GoalSO goal in _goalsSO.Goals.ToList()) 
        {
            if (goal.Count <= 0) { CheckForGoalCompletion(goal); continue; }

            foreach (BlockType type in destroyedBlocksTypes) 
            {
                if (goal.Type == type) 
                {
                    goal.Count--;
                    CheckForGoalCompletion(goal);
                }
                _destroyedBlockTypes.Items.Remove(type);
            }
        }
        CheckForLevelCompletion();
    }

    private void CheckForGoalCompletion(GoalSO goal) 
    {
        if (goal.Count <= 0) 
        {
            goal.IsCompleted = true;
        }
    }

    private void CheckForLevelCompletion() 
    {
        foreach(var goal in _goalsSO.Goals) 
        {
            if(!goal.IsCompleted) 
            {
                return;
            }
        }
        _allGoalsCompleted.SetValue(true);
        _levelEndEvent.Invoke();
    }

    private void OnDisable() 
    {
        _goalsSO.ResetGoals();
        _allGoalsCompleted.SetValue(false);
    }
}
