using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_0 : MonoBehaviour
{
    public Transform Hero;
    public Transform[] Layer_Objects;
    public float[] Layer_Speed;


    private Vector2 direction = new Vector2(0, 0);
    private Vector3 tempPosition;

    public void Start()
    {
        tempPosition = Hero.position;
    }

    void FixedUpdate()
    {
        if (Hero.position != tempPosition)
        {
            direction = (Hero.position - tempPosition);
            tempPosition = Hero.position;
        }
        else
        {
            direction = Vector2.zero;
        }

        for (int i = 0; i < Layer_Objects.Length; i++)
        {
            Vector3 movement = new Vector3(
                Layer_Speed[i] * direction.x,
                Layer_Speed[i] * direction.y,
                0);
            movement *= Time.deltaTime;
            Layer_Objects[i].Translate(movement);
        }
    }
}
