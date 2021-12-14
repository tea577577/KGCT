using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyMoveKeyDown : MonoBehaviour
{
    float moveSpeed = 0.5f;
    Vector2 rubyPosition;

    // Start is called before the first frame update
    void Start()
    {

        //rubyPosition = GetComponent < > ();
    }

    // Update is called once per frame
    void Update()
    {
        rubyPosition = transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            print("按下右鍵");
            //rubyPosition.x += moveSpeed;
            //transform.position = rubyPosition;
            //gameObject.transform.position = new Vector3(rubyPosition.x+=moveSpeed,0, 0);
            gameObject.transform.position += new Vector3(moveSpeed, 0, 0); //以上寫法皆可
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("按下左鍵");
            //rubyPosition.x -= moveSpeed;
            //transform.position = rubyPosition;
            //gameObject.transform.position = new Vector3(rubyPosition.x -= moveSpeed, 0, 0);
            gameObject.transform.position -= new Vector3(moveSpeed, 0, 0);
        }
    }
}
