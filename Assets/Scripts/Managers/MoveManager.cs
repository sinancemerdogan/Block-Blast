using UnityEngine;
using UnityEngine.Events;

public class MoveManager : MonoBehaviour
{
    [SerializeField] private IntegerVariable _count;
    [SerializeField] private BooleanVariable _movesRunOut;
    [SerializeField] private UnityEvent _levelEndEvent;

    public void DecreaseMoveCount() 
    {
        if(_count.Value <= 0) 
        {
            return;
        }

        _count.ApplyChange(-1);

        if(_count.Value <= 0) 
        {
            _movesRunOut.SetValue(true);
            _levelEndEvent.Invoke();
        }
    }

    private void OnDisable() 
    {
        _movesRunOut.SetValue(false);
        _count.SetValue(0);
    }
}
