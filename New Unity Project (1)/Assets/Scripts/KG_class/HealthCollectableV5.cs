using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 檢測碰撞＋Ruby加血(數值1)＋檢查是否玩家角色接觸＋檢查玩家角色是否滿血狀態
/// </summary>
public class HealthCollectableV5 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyMove5 RubyMove5 = collision.GetComponent<RubyMove5>();

        //檢查接觸物件若RubyMove2物件「並非為空」（表示接觸的是Ruby本身）
        if (RubyMove5 != null)
        {
            if (RubyMove5.health < RubyMove5.maxHealth)
            {
                print("碰撞前 Ruby 的血量為：" + RubyMove5.health);
                print("碰到了！碰到的東西是：" + RubyMove5);
                RubyMove5.ChangeHealth(1);
                Destroy(gameObject);
            }
            else
            {
                print("現在Ruby是滿血狀態");
            }
        }
        else
        {
            print("我是敵人");
        }

        //transform.position = Vector2.zero;
        //上面為測試用，若上述滿足 null 條件則執行的程式碼，當由敵人碰撞時（草莓會回到(0,0)的位置上）
    }

    
}
