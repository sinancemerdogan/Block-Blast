using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : Pool<ParticleSystem> 
{
    private ParticleSystemFactory _factory;
    public override IFactory<ParticleSystem> Factory { get { return _factory; } set { _factory = value as ParticleSystemFactory; } }

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
    public void SetParent(Transform t) {
        _parent = t;
        PoolRoot.SetParent(_parent);
    }

    public override ParticleSystem Request() {
        ParticleSystem particleSystem = base.Request();
        particleSystem.gameObject.SetActive(true);
        return particleSystem;
    }

    public override void Return(ParticleSystem particleSystem) {
        particleSystem.transform.SetParent(PoolRoot);
        particleSystem.gameObject.SetActive(false);
        base.Return(particleSystem);
    }

    public override void Prewarm(int num) {
        if (HasBeenPrewarmed) {
            Debug.LogWarning($"Pool {name} has already been prewarmed.");
            return;
        }

        for (int i = 0; i < num; i++) {
            ParticleSystem createdParticleSystem = Create();
            createdParticleSystem.transform.SetParent(PoolRoot);
            createdParticleSystem.gameObject.SetActive(false);
            Available.Push(createdParticleSystem);
        }
        HasBeenPrewarmed = true;
    }
}
