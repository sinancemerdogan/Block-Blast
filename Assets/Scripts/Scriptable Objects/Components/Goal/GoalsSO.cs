using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoalsSO", menuName = "Scriptable Objects/GoalsSO")]
public class GoalsSO : ScriptableObject
{
    [SerializeField] private List<GoalSO> goals = new ();

    public List<GoalSO> Goals 
    {
        get { return goals; }
        set { goals = value; }
    }

    public void ResetGoals() 
    {
        goals.Clear();
    }
}
