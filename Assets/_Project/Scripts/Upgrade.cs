using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    protected abstract void OnCollisionEnter2D(Collision2D col);
}
