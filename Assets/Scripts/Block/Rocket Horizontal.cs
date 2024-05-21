using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RocketHorizontal : Block, ITappable, IFallable, IDamageReceiver, IDamageDealer 
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
    private GameObject leftSide;
    private GameObject rightSide;
    private List<Block> leftSideBlocks;
    private List<Block> rightSideBlocks;
    private readonly float duration = 0.05f;

    private void Start() 
    {
        leftSide = transform.Find("rocket_left").gameObject;
        rightSide = transform.Find("rocket_right").gameObject;

        leftSideBlocks = new();
        rightSideBlocks = new();
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

        grid.FindRowBlocks(this, out leftSideBlocks, out rightSideBlocks);
        int waitDuration = leftSideBlocks.Count > rightSideBlocks.Count ? leftSideBlocks.Count : rightSideBlocks.Count;

        //
        _SFXEventChannel.Raise(Type.BlastSFX);
        //_particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));

        ParticleSystem leftTrail = Instantiate(Type.BlockBlastEffect, leftSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem rightTrail = Instantiate(Type.BlockBlastEffect, leftSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        var leftMain = leftTrail.main;
        leftMain.startLifetime = (waitDuration + grid.Width) * duration;

        var rightMain = rightTrail.main;
        rightMain.startLifetime = (waitDuration + grid.Width) * duration;

        leftTrail.transform.SetParent(leftSide.transform, true);
        rightTrail.transform.SetParent(rightSide.transform, true);
        //


        leftSide.transform.DOMoveX(Row - leftSideBlocks.Count - grid.Width, (leftSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);
        rightSide.transform.DOMoveX(Row + rightSideBlocks.Count + grid.Width, (rightSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);

        DealDamage(leftSideBlocks, rightSideBlocks, Damage, Type);

        
        StartCoroutine(WaitAndInvoke((waitDuration + grid.Width) * duration));
    }

    private IEnumerator WaitAndInvoke(float duration) 
    {
        yield return new WaitForSeconds(duration);
        onTapEvent.Invoke();
        isInputEnabled.SetValue(true);
        grid.DestroyBlock(this);
        blocksDestroyedEvent.Invoke();
    }

    private void DealDamage(List<Block> leftSideBlocks, List<Block> rightSideBlocks, int damageAmount, BlockType damageDealerBlockType) 
    {
        StartCoroutine(DealDamageWithDelay(leftSideBlocks.OfType<IDamageReceiver>().ToList(), damageAmount, damageDealerBlockType));
        StartCoroutine(DealDamageWithDelay(rightSideBlocks.OfType<IDamageReceiver>().ToList(), damageAmount, damageDealerBlockType));
    }

    private IEnumerator DealDamageWithDelay(List<IDamageReceiver> damageReceiverBlocks, int damageAmount, BlockType damageDealerBlockType) 
    {
        foreach (IDamageReceiver damageReceiverBlock in damageReceiverBlocks) 
        {
            damageReceiverBlock.ReceiveDamage(damageAmount, damageDealerBlockType);
            yield return new WaitForSeconds(duration);
        }
    }

    public void DealDamage(int damagedamageAmount) {

    }

    public void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType) 
    {
        if (Health <= 0 || IsFalling) 
        {
            return;
        }

        if (Type.BlockTypesCanDamageThisBlock.Contains(damageDealerBlockType)) 
        {
            Health -= damageAmount;
            if (Health <= 0) 
            {
                grid.FindRowBlocks(this, out leftSideBlocks, out rightSideBlocks);
                int waitDuration = leftSideBlocks.Count > rightSideBlocks.Count ? leftSideBlocks.Count : rightSideBlocks.Count;

                //
                _SFXEventChannel.Raise(Type.BlastSFX);
                //_particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));

                ParticleSystem leftTrail = Instantiate(Type.BlockBlastEffect, leftSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                ParticleSystem rightTrail = Instantiate(Type.BlockBlastEffect, rightSide.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

                var leftMain = leftTrail.main;
                leftMain.startLifetime = (waitDuration + grid.Width) * duration;

                var rightMain = rightTrail.main;
                rightMain.startLifetime = (waitDuration + grid.Width) * duration;

                leftTrail.transform.SetParent(leftSide.transform, true);
                rightTrail.transform.SetParent(rightSide.transform, true);
                //

                leftSide.transform.DOMoveX(Row - leftSideBlocks.Count - grid.Width, (leftSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);
                rightSide.transform.DOMoveX(Row + rightSideBlocks.Count + grid.Width, (rightSideBlocks.Count + grid.Width) * duration).SetEase(Ease.Linear);

                DealDamage(leftSideBlocks, rightSideBlocks, Damage, Type);

                StartCoroutine(DelayedDestruction((waitDuration + grid.Width) * duration));
            }
        }
    }

    private IEnumerator DelayedDestruction(float duration) {
        yield return new WaitForSeconds(duration);
        grid.DestroyBlock(this);
        blocksDestroyedEvent.Invoke();
    }

    public void SetConnectedBlock(List<Block> connectedBlocks) 
    {
        
    }
}
