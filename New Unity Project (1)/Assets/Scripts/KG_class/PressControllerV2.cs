using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressControllerV2 : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb = this.GetComponent<Rigidbody2D>(); 效果和前者一樣
        //rb = gameObject.GetComponent<Rigidbody2D>(); 效果和前者一樣

    }


    void FixedUpdate()
    {
        Vector2 rubyMove = new Vector2();
        rubyMove = transform.position;
        rubyMove.x += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        rubyMove.y += Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //第一種方法 - 基本上和第二種方法效果一樣
        //transform.position = rubyMove;
        //print("transform.position is " + transform.position);
        //第二種方法 - 搭配 Update() 仍會抖動 ；搭配 FixedUpate() 會有擠壓效果
        //rb.transform.position = rubyMove;
        //print("rb.transform.position is " + rb.transform.position);
        //第三種方法 + 搭配 FixedUpdate() 可解決幀數不足的問題
        rb.MovePosition(rubyMove);

    }
}
