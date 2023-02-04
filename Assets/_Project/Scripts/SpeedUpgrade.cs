using UnityEngine;


public class SpeedUpgrade : Upgrade
{
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        col.rigidbody.GetComponent<PlayerController>().UpgradeSpeed();
        gameObject.SetActive(false);
    }
}