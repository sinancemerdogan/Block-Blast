using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

public class RocketVertical : Block, ITappable, IFallable, IDamageReceiver, IDamageDealer 
{
    //Properties
    public bool IsFalling { get; set; }
    public int Damage { get; private set; }
    public int Health { get; private set; }

    //Components
    [SerializeField] private BlockFaller blockFaller;

    //Events
    [SerializeField] private UnityEvent blocksDestroyedEvent;
    [SerializeField] private UnityEvent onTapEvent;

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    //Shared Variables
    [SerializeField] private BooleanVariable isInputEnabled;

    //Fields
    private GameObject downSide;
    private GameObject upSide;
    private List<Block> downSideBlocks;
    private List<Block> upSideBlocks;
    private readonly float duration = 0.05f;

    private void Start() 
    {
        downSide = transform.Find("rocket_down").gameObject;
        upSide = transform.Find("rocket_up").gameObject;

        downSideBlocks = new();
        upSideBlocks = new();
    }

    private void OnEnable() 
    {
        Damage = Type.Damage;
        Health = Type.Health;
    }

    public void Fall(int targetY) {
        IsFalling = true;
        blockFaller.Fall(transform, targetY, () => 
        {
            IsFalling = false;
        });
    }

    public void OnTap() 
    {
        isInputEnabled.SetValue(false);

        Health -= 1;

        grid.FindColumnBlocks(this, out downSideBlocks, out upSideBlocks);

        int waitDuration = downSideBlocks.Count > upSideBlocks.Count ? downSideBlocks.Count : upSideBlocks.Count;

        //
        _SFXEventChannel.Raise(Type.BlastSFX);
        //_particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));

        ParticleSystem upTrail = Instantiate(Type.BlockBlastEffect, upSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem downTrail = Instantiate(Type.BlockBlastEffect, downSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        var leftMain = upTrail.main;
        leftMain.startLifetime = (waitDuration + grid.Width) * duration;

        var rightMain = downTrail.main;
        rightMain.startLifetime = (waitDuration + grid.Width) * duration;

        upTrail.transform.SetParent(upSide.transform, true);
        downTrail.transform.SetParent(downSide.transform, true);
        //

        downSide.transform.DOMoveY(Col - downSideBlocks.Count - grid.Width, (downSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);
        upSide.transform.DOMoveY(Col + upSideBlocks.Count + grid.Width, (upSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);

        DealDamage(downSideBlocks, upSideBlocks, Damage, Type);

        StartCoroutine(WaitAndInvoke((waitDuration + grid.Width) * duration));
    }

    private IEnumerator WaitAndInvoke(float duration) {
        yield return new WaitForSeconds(duration);
        onTapEvent.Invoke();
        isInputEnabled.SetValue(true);
        grid.DestroyBlock(this);
        blocksDestroyedEvent.Invoke();
    }

    private void DealDamage(List<Block> downSideBlocks, List<Block> upSideBlocks, int damageAmount, BlockType damageDealerBlockType) {
        StartCoroutine(DealDamageWithDelay(downSideBlocks.OfType<IDamageReceiver>().ToList(), damageAmount, damageDealerBlockType));
        StartCoroutine(DealDamageWithDelay(upSideBlocks.OfType<IDamageReceiver>().ToList(), damageAmount, damageDealerBlockType));
    }

    private IEnumerator DealDamageWithDelay(List<IDamageReceiver> damageReceiverBlocks, int damageAmount, BlockType damageDealerBlockType) {
        foreach (IDamageReceiver damageReceiverBlock in damageReceiverBlocks) {
            damageReceiverBlock.ReceiveDamage(damageAmount, damageDealerBlockType);
            yield return new WaitForSeconds(duration);
        }
    }

    public void DealDamage(int damagedamageAmount) {

    }

    public void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType) {
        if (Health <= 0 || IsFalling) {
            return;
        }

        if (Type.BlockTypesCanDamageThisBlock.Contains(damageDealerBlockType)) {
            Health -= damageAmount;
            if (Health <= 0) 
            {

                grid.FindColumnBlocks(this, out downSideBlocks, out upSideBlocks);
                int waitDuration = downSideBlocks.Count > upSideBlocks.Count ? downSideBlocks.Count : upSideBlocks.Count;

                //
                _SFXEventChannel.Raise(Type.BlastSFX);
                //_particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));

                ParticleSystem upTrail = Instantiate(Type.BlockBlastEffect, upSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                ParticleSystem downTrail = Instantiate(Type.BlockBlastEffect, downSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

                var leftMain = upTrail.main;
                leftMain.startLifetime = (waitDuration + grid.Width) * duration;

                var rightMain = downTrail.main;
                rightMain.startLifetime = (waitDuration + grid.Width) * duration;

                upTrail.transform.SetParent(upSide.transform, true);
                downTrail.transform.SetParent(downSide.transform, true);
                //

                downSide.transform.DOMoveY(Col - downSideBlocks.Count - grid.Width, (downSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);
                upSide.transform.DOMoveY(Col + upSideBlocks.Count + grid.Width, (upSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);

                DealDamage(downSideBlocks, upSideBlocks, Damage, Type);

                StartCoroutine(DelayedDestruction((waitDuration + grid.Width) * duration));
            }
        }
    }

    private IEnumerator DelayedDestruction(float duration) {
        yield return new WaitForSeconds(duration);
        grid.DestroyBlock(this);
        blocksDestroyedEvent.Invoke();
    }

    public void SetConnectedBlock(List<Block> connectedBlocks) {

    }
}
