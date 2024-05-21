using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TNT : Block, IFallable, ITappable, IDamageDealer, IDamageReceiver 
{
    //Properties
    public bool IsFalling { get; set; }
    public int Damage { get; private set; }
    public int Health { get; private set; }
    public List<Block> ConnectedBlocks { get; set; }

    //Fields
    private List<Block> NxNAdjacentBlocks;

    //Events
    [SerializeField] private UnityEvent blocksDestroyedEvent;
    [SerializeField] private UnityEvent onClickEvent;

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    //Components
    [SerializeField] private BlockFaller blockFaller;

    //Shared Variables
    [SerializeField] private BooleanVariable isInputEnabled;

    private void OnEnable() 
    {
        Health = Type.Health;
        Damage = Type.Damage;

        NxNAdjacentBlocks = new();
        ConnectedBlocks = new();
    }
    public void Fall(int targetY) {
        IsFalling = true;
        blockFaller.Fall(transform, targetY, () => 
        {
            IsFalling = false;
            UpdateConnectedBlocks();
        });
    }

    public void OnTap() 
    {
        if (IsFalling) return;
        if (ConnectedBlocks.Count < 1) return;
        Explode();
    }

    private void Explode() 
    {
        if (ConnectedBlocks.Count > 1) ComboExplosion();
        else DefaultExplosion();
    }

    private void DefaultExplosion() 
    {
        NxNAdjacentBlocks = grid.FindNxNAreaBlocks(this, 5);
        grid.DestroyBlock(this);
        _SFXEventChannel.Raise(Type.BlastSFX);
        _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));
        NxNAdjacentBlocks.Remove(this);
        DealDamage(Damage);
        isInputEnabled.Value = true;
        onClickEvent.Invoke();
        blocksDestroyedEvent.Invoke();
    }

    private void ComboExplosion() 
    {
        isInputEnabled.Value = false;
        NxNAdjacentBlocks = grid.FindNxNAreaBlocks(this, 7);
        ConnectedBlocks.Remove(this);
        NxNAdjacentBlocks.Remove(this);

        foreach (Block block in ConnectedBlocks) {
            NxNAdjacentBlocks.Remove(block);
            block.transform.DOMove(new Vector3(Row, Col), 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                grid.DestroyBlock(block);
                NxNAdjacentBlocks.Remove(block);

                if (block == ConnectedBlocks[^1]) {
                    GameObject particleSystemObject = gameObject.transform.Find("CandleFlame").gameObject;

                    // Scale animation
                    transform.DOScale(new Vector3(3, 3, 3), 2f)
                        .SetEase(Ease.InOutQuint)

                        // Shake animation
                        .OnStart(() => {
                            particleSystemObject.transform.DOScale(new Vector3(3, 3, 3), 1.5f).SetEase(Ease.InOutQuint);
                            transform.DOShakePosition(1.9f, 0.2f, 10, 90, false, true);
                        })
                        .OnComplete(() => 
                        {
                            grid.DestroyBlock(this);
                            _SFXEventChannel.Raise(Type.BlastSFX);
                            _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));

                            DealDamage(Damage);
                            isInputEnabled.Value = true;
                            onClickEvent.Invoke();
                            blocksDestroyedEvent.Invoke();
                        });
                }
            });
        }
    }


    public void DealDamage(int damageAmount) {
        foreach (Block block in NxNAdjacentBlocks) {
            if(block is IDamageReceiver damageReceiverBlock) {
                damageReceiverBlock.ReceiveDamage(damageAmount, Type);
            }
        }
        NxNAdjacentBlocks.Clear();
    }

    public void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType) 
    {
        if (Health <= 0) 
        {
            return;
        }

        if (Type.BlockTypesCanDamageThisBlock.Contains(damageDealerBlockType)) 
        {
            Health -= damageAmount;

            if (Health <= 0) 
            {

                grid.DestroyBlock(this);
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row,Col));

                NxNAdjacentBlocks = grid.FindNxNAreaBlocks(this, 5);
                DealDamage(Damage);
            }
        }
    }

    public void SetConnectedBlock(List<Block> connectedBlocks) {
        ConnectedBlocks = connectedBlocks;
    }

    public void UpdateConnectedBlocks() {
        ConnectedBlocks = grid.FindConnectedBlocks(Row, Col, Type);
        foreach (Block block in ConnectedBlocks) {
            if (block is ITappable tappable) {
                tappable.SetConnectedBlock(ConnectedBlocks);
            }
        }
    }
}