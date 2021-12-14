using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 fox_position = transform.position;
        fox_position.x = fox_position.x + 0.01f;
        transform.position = fox_position;
    }
}
