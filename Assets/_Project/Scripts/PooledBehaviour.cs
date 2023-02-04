using UnityEngine;

public abstract class PooledBehaviour : MonoBehaviour
{
    public abstract void Spawn();
    public abstract void Die();
}