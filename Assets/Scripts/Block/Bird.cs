using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Bird : Block, IFallable {
    public bool IsFalling { get; set; }

    //Components
    [SerializeField] private BlockFaller blockFaller;

    //Events
    [SerializeField] private UnityEvent blocksDestroyedEvent;

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    public void Fall(int targetY) {
        IsFalling = true;
        blockFaller.Fall(transform, targetY, () => {
            IsFalling = false;
            CheckForFall();
        });
    }

    private void CheckForFall()
    {
        if(Col == 0) 
        {
            transform.DOMoveY(10, 1.5f).SetEase(Ease.InBack).OnComplete(() => 
            {
                grid.DestroyBlock(this);
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));
                blocksDestroyedEvent.Invoke();
            });
        }
    }
}
