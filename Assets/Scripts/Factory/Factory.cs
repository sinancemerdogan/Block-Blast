using UnityEngine;

public abstract class Factory<T> : ScriptableObject, IFactory<T> 
{
    protected T _prefab;
    public T Prefab {  get { return _prefab; } set { _prefab = value; } }
    public abstract T Create();
}