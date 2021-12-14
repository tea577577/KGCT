using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //宣告一個「以 Vector2 為"單位/類型"」的變數
        Vector2 foxpositionVector2 = transform.position;
        print("foxposition-Vector2 is: " + foxpositionVector2);
        foxpositionVector2.x = 5;
        foxpositionVector2.y = 2;

        //宣告一個「以 Transform 為"單位/類型"」的變數
        Transform foxpositionTransform = transform;
        print("foxposition-Transform is: " + foxpositionTransform);
        print("foxposition-Transform.position is: " + foxpositionTransform.position);
        print("foxposition-Transform.position.x is: " + foxpositionTransform.position.x);
        

        print("New foxposition-Vector2 is: " + foxpositionVector2);


    }
}
