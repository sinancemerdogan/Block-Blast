using UnityEngine;

public class BlockPool : Pool<Block> 
{
    private BlockFactory _factory;
    
    public override IFactory<Block> Factory { get { return _factory; } set { _factory = value as BlockFactory; } }

    private Transform _poolRoot;
    private Transform PoolRoot {
        get {
            if (_poolRoot == null) {
                _poolRoot = new GameObject(name + " Pool").transform;
            }
            return _poolRoot;
        }
    }

    private Transform _parent;

    public void SetParent(Transform t) 
    {
        _parent = t;
        PoolRoot.SetParent(_parent);
    }

    public override Block Request() {
        Block block = base.Request();
        block.gameObject.SetActive(true);
        return block;
    }

    public override void Return(Block block) {
        block.transform.SetParent(PoolRoot);
        block.gameObject.SetActive(false);
        base.Return(block);
    }

    public override void Prewarm(int num) {
        if (HasBeenPrewarmed) {
            Debug.LogWarning($"Pool {name} has already been prewarmed.");
            return;
        }

        for (int i = 0; i < num; i++) {
            Block createdBlock = Create();
            createdBlock.transform.SetParent(PoolRoot);
            createdBlock.gameObject.SetActive(false);
            Available.Push(createdBlock);
        }
        HasBeenPrewarmed = true;
    }
}
