using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 給予黃色子彈「受力」的方法＋【使物件消失】＋【碰撞敵人並改變狀態】
/// 【未擊中目標的子彈自動銷毀】＋【擊中特效】
/// </summary>
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    public float bulletDis;

    //【擊中特效 1/2】
    public ParticleSystem hitEffect;

    // 當有使用 實例化生成函式 Instantiate() 就需要使用 Awake()
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletDis = transform.position.magnitude;
    }

    // 設定Launch(給予力的方向＆力的大小)
    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    // 子彈的碰撞檢測
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("當前子彈碰撞到：" + collision.gameObject);

        //【碰撞敵人並改變狀態 1/1】呼叫 EnemyGo 這支程式裡面的 Fix() 方法函式
        EnemyGo_14a enemyGo_14 = collision.gameObject.GetComponent<EnemyGo_14a>();
        if (enemyGo_14 != null)
        {
            enemyGo_14.Fix();

            //【擊中特效 2/2】
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }


        //【使物件消失 1/1】
        Destroy(gameObject); //子彈消失
        //Destroy(collision.gameObject); //碰撞到的物件消失
    }

    //【未擊中目標的子彈自動銷毀 1/1】以距離來計算
    void Update()
    {
        if (transform.position.magnitude > 100)
        {
            Destroy(gameObject);
        }
    }

}
