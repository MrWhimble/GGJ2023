using UnityEngine;

[CreateAssetMenu(fileName = "new WeaponData", menuName = "Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Sprite sprite;
    public Vector2 size;
    public float speed;
    public float deltaTime;
    public float lifeTime;
}