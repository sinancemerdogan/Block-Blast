using UnityEngine;

public class Stone : Block, IDamageReceiver 
{
    //Properties
    public int Health { get; private set; }

    //Event Channels
    [SerializeField] private AudioEventChannel _SFXEventChannel;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    private void Start() 
    {
        Health = Type.Health;
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
            if(Health <= 0) 
            {
                grid.DestroyBlock(this);
                _SFXEventChannel.Raise(Type.BlastSFX);
                _particleSystemEventChannel.Raise(Type.BlockBlastEffect, new Vector2(Row, Col));
            }
        }
    }
}