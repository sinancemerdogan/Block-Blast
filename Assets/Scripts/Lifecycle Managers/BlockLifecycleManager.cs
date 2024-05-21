using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Block Lifecycle Manager", menuName = "Scriptable Objects/Managers/Block Lifecycle Manager")]
public class BlockLifecycleManager : ScriptableObject 
 {

    [SerializeField] private BlockTypeListVariable _randomBlocks;
    [SerializeField] private IntegerVariable _gridWidth;
    [SerializeField] private IntegerVariable _gridHeight;

    private Dictionary<BlockType, BlockPool> _pools = new();
    private Transform _parent;


    public void SetParent(Transform parent) {
        _parent = parent;
    }

    public void SetPools() 
    {
        _pools = new Dictionary<BlockType, BlockPool>();

        if (_pools == null) 
        {
            Debug.LogWarning(name + " requires list of poolable blocks. No pool will be created for blocks");
            return;
        }

        foreach (var block in _randomBlocks.Items) {
            BlockPool pool = CreateBlockPool(block);
            _pools.Add(block, pool);
        }
    }

    public void ResetBlockLifecycleManager() {
        _pools.Clear();
        if (_parent != null)  Destroy(_parent.gameObject);
    }

    private BlockFactory CreateFactory(BlockType block) {
        BlockFactory factory = CreateInstance<BlockFactory>();
        factory.name = block.name + " Factory";
        factory.Prefab = block.Prefab;
        return factory;
    }

    private BlockPool CreateBlockPool(BlockType block) {
        BlockPool pool = CreateInstance<BlockPool>();
        pool.name = block.name;
        pool.Factory = CreateFactory(block);
        pool.SetParent(_parent);
        pool.Prewarm((_gridWidth.Value * _gridHeight.Value) / 4);

        return pool;
    }

    public Block CreateBlock(BlockType blockType) {
        Block block;

        if (_pools.TryGetValue(blockType, out var pool)) {
            block = pool.Request();
        }
        else {
            block = Instantiate(blockType.Prefab);
        }
        return block;
    }

    public void DestroyBlock(Block block) {
        if (_pools.TryGetValue(block.Type, out var pool)) {
            pool.Return(block);
        }
        else {
            Destroy(block.gameObject);
        }
    }
}
