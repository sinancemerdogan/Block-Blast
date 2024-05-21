using UnityEngine;

public class Vase : Block, IFallable, IDamageReceiver 
{
    //Properties
    public bool IsFalling { get; set; }
    public int Health { get; private set; }

    //Components
    [SerializeField] private BlockFaller blockFaller;

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    private void Start() 
    {
        Health = Type.Health;
    }

    public void Fall(int targetY) 
    {
        IsFalling = true;
        blockFaller.Fall(transform, targetY, () => 
        {
            IsFalling = false;
        });
    }

    public void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType) 
    {
        if(Health <= 0) 
        {
            return;
        }

        if (Type.BlockTypesCanDamageThisBlock.Contains(damageDealerBlockType)) 
        {
            Health -= damageAmount;

            if(Health == 1) 
            {
                GetComponent<SpriteRenderer>().sprite = Type.Sprites[1];
                
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row,Col));
              
                
            }
            if (Health <= 0) 
            {
                grid.DestroyBlock(this);
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));
            }
        }
    }
}