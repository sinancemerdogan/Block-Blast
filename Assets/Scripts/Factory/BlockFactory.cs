public class BlockFactory : Factory<Block> {
   
    public override Block Create() 
    {
        return Instantiate(_prefab);
    }
}
