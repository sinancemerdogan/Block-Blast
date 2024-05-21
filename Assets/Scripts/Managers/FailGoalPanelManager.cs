using UnityEngine;

public class FailGoalPanelManager : MonoBehaviour
{
    private Transform _goals;
    [SerializeField] GoalsSO _goalsSO;
    [SerializeField] private GoalIndicator _goalIndicatorPrefab;


    void Start() {
        _goals = transform.Find("Goals");
        CreateGoalIndicators();
    }

    private void CreateGoalIndicators() {
        foreach (GoalSO goalSO in _goalsSO.Goals) 
        {
            GoalIndicator goalIndicator = Instantiate(_goalIndicatorPrefab);
            goalIndicator.SetGoalIndicator(goalSO);
            goalIndicator.transform.SetParent(_goals, false);

            if (!goalSO.IsCompleted) 
            {
                goalIndicator.SetGoalFailed();
            }
        }
    }
}
