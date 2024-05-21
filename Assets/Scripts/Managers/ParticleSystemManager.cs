using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class ParticleSystemManager : MonoBehaviour
{
    [SerializeField] private ParticleSystemLifecycleManager _particleSystemLifecycleManager;
    [SerializeField] private ParticleSystemEventChannel _particleSystemEventChannel;

    void Start()
    {
        if(_particleSystemLifecycleManager == null) 
        {
            Debug.LogWarning("Particle System Lifecycle Manager is null.");
            Debug.LogWarning("Disabling " + name);
            enabled = false;
        }

        _particleSystemLifecycleManager.SetParent(new GameObject("Particle System Pools").transform);
        _particleSystemLifecycleManager.SetPools();
    }

    private void OnEnable() {
        _particleSystemEventChannel.OnParticleSystemRequested += PlayParticleSystem;
    }
    private void OnDisable() {
        _particleSystemEventChannel.OnParticleSystemRequested -= PlayParticleSystem;
    }


    public void PlayParticleSystem(GameObject particleSystem, Vector2 position) {
        StartCoroutine(PlayAndDestroyParticleSystem(position, particleSystem));
    }

    public ParticleSystem CreateParticleSystem(Vector2 position, GameObject particleSystem) 
    {
        ParticleSystem createdParticleSystem = _particleSystemLifecycleManager.CreateParticleSystem(particleSystem);
        createdParticleSystem.transform.position = position;
        createdParticleSystem.transform.parent = transform;
        return createdParticleSystem;
    }

    public void DestroyParticleSystem(ParticleSystem particleSystem) 
    {
        _particleSystemLifecycleManager.DestroyParticleSystem(particleSystem);
    }
    IEnumerator PlayAndDestroyParticleSystem(Vector2 position, GameObject particleSystem) 
    {
        ParticleSystem particle = CreateParticleSystem(position, particleSystem);
        particle.Play();

        yield return new WaitForSeconds(particle.main.duration);

        DestroyParticleSystem(particle);
    }

    //[SerializeField] private BlockTypeVectorPairListVariable _particleSystemQueue;

    //public void PlayParticleSystems() 
    //{
    //    if(_particleSystemQueue == null) 
    //    {
    //        Debug.LogWarning("Particle System Queue is null.");
    //        Debug.LogWarning("No Particle System will be played.");
    //        return;
    //    }

    //    List<BlockTypeVectorPair> particleSystemQueue = new(_particleSystemQueue.Items);

    //    foreach (var block in particleSystemQueue) 
    //    {
    //        StartCoroutine(PlayAndDestroyParticleSystem(block.position, block.type.BlockBlastEffect));
    //        _particleSystemQueue.Items.Remove(block);
    //    }
    //    particleSystemQueue.Clear();
    //}
}
