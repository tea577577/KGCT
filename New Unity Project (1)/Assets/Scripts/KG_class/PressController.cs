using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressController : MonoBehaviour
{
    public float moveSpeed = 10;

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector2 rubyMove = new Vector2();
        rubyMove = transform.position;
        rubyMove.x += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        rubyMove.y += Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position = rubyMove;


    }
}
