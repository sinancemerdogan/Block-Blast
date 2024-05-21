using UnityEngine;

public class ParticleSystemFactory : Factory<ParticleSystem> 
{
    public override ParticleSystem Create() 
    {
        return Instantiate(_prefab).GetComponent<ParticleSystem>();
    }
}
