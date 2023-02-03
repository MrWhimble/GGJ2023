using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        Vector2 inputDir = PlayerInput.GetMoveDirection(transform.position);
        
        transform.Translate(inputDir * speed * Time.deltaTime);
    }
    
}
