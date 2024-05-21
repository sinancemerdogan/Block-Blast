using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private BooleanVariable _movesRunOut;
    [SerializeField] private BooleanVariable _allGoalsCompleted;

    [SerializeField] private UnityEvent _levelWonEvent;
    [SerializeField] private UnityEvent _levelLostEvent;

    public void OnLevelEnd() 
    {
        StartCoroutine(OnLevelEndCoroutine());
    }

    public IEnumerator OnLevelEndCoroutine() 
    {
        yield return new WaitForSeconds(0.5f);

        if (_allGoalsCompleted.Value) 
        {
            _levelWonEvent.Invoke();
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(0);
        }
        else {
            _levelLostEvent.Invoke();
        }
    }
}
