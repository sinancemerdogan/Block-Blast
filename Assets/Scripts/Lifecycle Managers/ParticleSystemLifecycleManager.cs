using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Particle System Lifecycle Manager", menuName = "Scriptable Objects/Managers/Particle System Lifecycle Manager")]
public class ParticleSystemLifecycleManager : ScriptableObject
{
    [SerializeField] private BlockTypeListVariable _randomBlocks;
    [SerializeField] private IntegerVariable _gridWidth;
    [SerializeField] private IntegerVariable _gridHeight;

    private Dictionary<string, ParticleSystemPool> _pools;
    private Transform _parent;

    public void SetParent(Transform parent) {
        _parent = parent;
    }

    public void SetPools() {
        _pools = new Dictionary<string, ParticleSystemPool>();

        if (_randomBlocks == null) {
            Debug.LogWarning(name + " requires list of poolable blocks. No pool will be created for particle systems.");
            return;
        }

        foreach (var blockType in _randomBlocks.Items) {
            ParticleSystemPool pool = CreateParticleSystemPool(blockType.BlockBlastEffect);
            _pools.Add(blockType.BlockBlastEffect.name+"(Clone)", pool);
        }
    }
    private ParticleSystemPool CreateParticleSystemPool(GameObject particleSystem) {
        ParticleSystemPool pool = CreateInstance<ParticleSystemPool>();
        pool.name = particleSystem.name;
        pool.Factory = CreateFactory(particleSystem);
        pool.SetParent(_parent);
        pool.Prewarm((_gridWidth.Value * _gridHeight.Value) / 4);
        return pool;
    }

    private ParticleSystemFactory CreateFactory(GameObject particleSystem) 
    {
        ParticleSystemFactory factory = CreateInstance<ParticleSystemFactory>();
        factory.name = particleSystem.name + " Factory";
        factory.Prefab = particleSystem.GetComponent<ParticleSystem>();
        return factory;
    }

    public ParticleSystem CreateParticleSystem(GameObject particleSystem) {
        ParticleSystem createdParticleSystem;

        if (_pools.TryGetValue(particleSystem.name+"(Clone)", out var pool)) {
            
            createdParticleSystem = pool.Request();
        }
        else 
        {
            createdParticleSystem = Instantiate(particleSystem).GetComponent<ParticleSystem>();
        }
        return createdParticleSystem;
    }

    public void DestroyParticleSystem(ParticleSystem particleSystem) 
    {

        if (_pools.TryGetValue(particleSystem.gameObject.name, out var pool)) {
            pool.Return(particleSystem);
        }
        else {
            Destroy(particleSystem.gameObject);
        }
    }

    public void ResetParticleSystemLifecycleManager() {
        _pools.Clear();
        if (_parent != null) Destroy(_parent.gameObject);
    }

}
