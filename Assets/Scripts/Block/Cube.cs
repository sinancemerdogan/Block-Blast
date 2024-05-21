using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;


public class Cube : Block, ITappable, IFallable, IDamageDealer, IDamageReceiver 
{
    //Properties
    public bool IsFalling { get; set; }
    public int Damage { get; private set; }
    public int Health { get; private set; }
    public List<Block> ConnectedBlocks { get; set; }

    //Fields
    private List<Block> borderBlocks;
    
    //Events
    [SerializeField] private UnityEvent blocksDestroyedEvent;
    [SerializeField] private UnityEvent onTapEvent;

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    //Components
    [SerializeField] private BlockFaller blockFaller;

    //Shared Variables
    [SerializeField] private BooleanVariable isInputEnabled;

    //
    [SerializeField] private AudioClip creationSFX;
    [SerializeField] private GameObject creationParticles;
    //
    
    private void OnEnable() 
    {
        Damage = Type.Damage;
        Health = Type.Health;

        gameObject.GetComponent<SpriteRenderer>().sprite = Type.Sprites[0];
        transform.localScale = Vector3.one;

        ConnectedBlocks = new();
        borderBlocks = new();
    }
    public void Fall(int targetY) {
        gameObject.GetComponent<SpriteRenderer>().sprite = Type.Sprites[0];
        IsFalling = true;
        blockFaller.Fall(transform, targetY, () => {
            IsFalling = false;
            UpdateConnectedBlocks();
        });
    }

    public void OnTap() 
     {
        if (IsFalling) return;
        if (ConnectedBlocks.Count < 2) return;
        BlastConnectedBlocks(ConnectedBlocks);
    }

    private void BlastConnectedBlocks(List<Block> connectedBlocks) 
    {
        borderBlocks = grid.FindBorderBlocks(ConnectedBlocks);

        if (connectedBlocks.Count >= Type.CreationThresholds[0]) {
            CreationAnimation(connectedBlocks);
        }
        else 
        {
            BlastAnimation(connectedBlocks);
        }
    }

    private void CreationAnimation(List<Block> connectedBlocks) 
    {
        isInputEnabled.Value = false;

        foreach (Block block in connectedBlocks) 
         {

            block.transform.DOMove(new Vector3(Row, Col), 0.4f).SetEase(Ease.InBack).OnComplete(() => 
            {
                grid.DestroyBlock(block);

                if (block == connectedBlocks[^1]) 
                {
                    
                    Block block = grid.CreateBlock(Row, Col, new Vector2(Row, Col), Type.BlockTypesThisBlockCanCreate[0]);
                    
                    _SFXEventChannel.OnAudioRequested(creationSFX);
                    _particleSystemEventChannel.Raise(creationParticles, new Vector2(Row, Col));

                    block.transform.DORotate(new Vector3(0f, 0f, 360f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear);

                    DealDamage(Damage);
                    isInputEnabled.Value = true;
                    onTapEvent.Invoke();
                    blocksDestroyedEvent.Invoke();
                }
            });
        }
    }
    private void BlastAnimation(List<Block> connectedBlocks) 
    {
        foreach (Block block in connectedBlocks) 
        {
            if (connectedBlocks.Count < 5) 
            {
                block.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.Unset).OnComplete(() => 
                {
                    grid.DestroyBlock(block);
                    _SFXEventChannel.Raise(Type.BlastSFX);
                    _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(block.Row, block.Col));

                    if (block == connectedBlocks[^1]) 
                    {
                        DealDamage(Damage);
                        onTapEvent.Invoke();
                        blocksDestroyedEvent.Invoke();
                    }
                });
            }
        }
    }

    public void DealDamage(int damageAmount) 
    {
        foreach (Block block in borderBlocks) 
        {
            if (block is IDamageReceiver damageReceiverBlock) {
                damageReceiverBlock.ReceiveDamage(damageAmount, Type);
            }
        }
        borderBlocks.Clear();
    }

    public void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType) {

        if (Health <= 0 || IsFalling) {
            return;
        }

        if (Type.BlockTypesCanDamageThisBlock.Contains(damageDealerBlockType)) {
            Health -= damageAmount;
            if (Health <= 0) 
            {
                grid.DestroyBlock(this);
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));
            }
        }
    }

    public void SetConnectedBlock(List<Block> connectedBlocks) {
        ConnectedBlocks = connectedBlocks;
        SetSprite();
    }

    public void SetSprite() {
        int threshold = ConnectedBlocks.Count;
        bool thresholdMet = false;
        for (int i = Type.CreationThresholds.Count - 1; i >= 0; i--) 
        {
            if (threshold >= Type.CreationThresholds[i]) {
                gameObject.GetComponent<SpriteRenderer>().sprite = Type.Sprites[i + 1];
                thresholdMet = true;
                break;
            }
        }
        if (!thresholdMet) {
            gameObject.GetComponent<SpriteRenderer>().sprite = Type.Sprites[0];
        }
    }

    public void UpdateConnectedBlocks() {
        ConnectedBlocks = grid.FindConnectedBlocks(Row, Col, Type);
        foreach (Block block in ConnectedBlocks) {
            if (block is ITappable tappable) {
                tappable.SetConnectedBlock(ConnectedBlocks);
            }
        }
        SetSprite();
    }
}
