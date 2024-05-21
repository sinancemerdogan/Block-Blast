using DG.Tweening;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Faller", menuName = "Scriptable Objects/Block Faller")]
public class BlockFaller : ScriptableObject 
{

    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve curve;

    public void Fall(Transform transform, int targetY, Action onCompleteCallback) 
    {
        Tween fallTweeen = transform.DOMoveY(targetY, duration)
                .SetEase(curve).OnComplete(() => { onCompleteCallback?.Invoke(); });

        //fallTweeen.OnUpdate(() =>
        //{
        //    if (fallTweeen.position >= duration * 0.75f) {
        //        onCompleteCallback?.Invoke();
        //    }
        //});
    }
}
