using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("顯示目前的水平速度")]
    public float speedX; //作為儀表板顯示數值，供設計師看

    [Header("目前的水平數值力道")]
    public float hDirection; //偵測鍵盤左右力道，數值會在 -1 ~ 1 之間

    [Header("水平推力")]
    [Range(0, 150)]
    public float xForce;

    float speedY; //目前垂直速度

    [Header("設定最大水平速度")]
    public float maxSpeedX; //用來限制最大速度用（因為速度過快，會突破兩旁的牆壁 Collider）

    /// <summary> 水平移動 </summary>
    void MovementX()
    {
        hDirection = Input.GetAxis("Horizontal"); //讀取左右按鍵的數值
        rb.AddForce(new Vector2(xForce * hDirection, 0)); //給剛體一個推力，是由 x數值與按鍵加乘組合
    }

    /// <summary> 用來控制最大速度 </summary>
    public void ControlSpeed()
    {
        speedX = rb.velocity.x; //把目前偵測到的玩家水平加速度，加到 speedX 上
        speedY = rb.velocity.y;
        float newSpeedX = Mathf.Clamp(speedX, -maxSpeedX, maxSpeedX); // Mathf.Clamp 用來限制某變數的最大值＆最小值
        rb.velocity = new Vector2(newSpeedX, speedY);
    }

    [Header("垂直向上推力")]
    public float yForce;

    //用來偵測玩家是否按下跳的按鍵
    public bool JumpKey // 這邊做成屬性
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    void TryJump()
    {
        if (IsGround && JumpKey)
        {
            rb.AddForce(Vector2.up * yForce); //Vector2.up 數值為0,1
        }
    }

    [Header("感應地板的距離")]
    [Range(0, 0.5f)]
    public float distance;

    [Header("偵測地板的射線起點")]
    public Transform groundCheck;

    [Header("地面圖層")]
    public LayerMask groundLayer;

    public bool grounded;
    //在玩家的底部射一條很短的射線，如果射線有打到地板圖層的話，代表正踩著地板
    bool IsGround
    {
        get
        {
            Vector2 start = groundCheck.position;
            Vector2 end = new Vector2(start.x, start.y - distance);

            Debug.DrawLine(start, end, Color.blue);
            grounded = Physics2D.Linecast(start, end, groundLayer);
            return grounded;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        MovementX();
        //speedX = rb.velocity.x; //把目前偵測到的玩家水平加速度，加到 speedX 上

        ControlSpeed();
        TryJump();
    }
}
