using UnityEngine;

public class BulletController : PooledBehaviour
{
    public override void Spawn()
    {
        gameObject.SetActive(true);
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}