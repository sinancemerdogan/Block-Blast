using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Particle System Event Channel", menuName = "Scriptable Objects/Event Channels/Particle System Event Channel")]
public class ParticleSystemEventChannel : ScriptableObject 
{
    public UnityAction<GameObject, Vector2> OnParticleSystemRequested;

    public void Raise(GameObject particleSystem, Vector2 position) {
        if (OnParticleSystemRequested != null) {
            OnParticleSystemRequested.Invoke(particleSystem, position);
        }
        else {
            Debug.LogWarning("A particle system was requested, but nobody picked it up.");
        }
    }
}
