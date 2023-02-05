using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity tutorial used https://www.youtube.com/watch?v=3UO-1suMbNc
public class BackgroundLoop : MonoBehaviour
{
    public Transform[] backgrounds;
    public float speed;
    public float yReset;

    private void Start()
    {
        backgrounds[0].position = new Vector2(0, 0);
        backgrounds[1].position = new Vector2(0, -yReset);

    }
    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            backgrounds[i].Translate(new Vector2(0, speed * Time.deltaTime));
            if (backgrounds[i].position.y >= yReset)
            {
                backgrounds[i].Translate(new Vector2(0, -yReset * 2));
            }
        }

    }
}
