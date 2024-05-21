public interface IDamageReceiver 
{
    int Health { get; }
    void ReceiveDamage(int damageAmount, BlockType damageDealerBlockType);
}
