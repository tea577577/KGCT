using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵人程式：課堂解說用，功能和EnemyMove5 一樣
/// 【移動控制】+【折返方向】＋【動畫混合樹】
/// 【碰撞玩家】+【敵人碰到子彈行為】+【關閉煙霧特效】
/// Week14【移動音效】-學生練習版
/// </summary>
public class EnemyGo_14a : MonoBehaviour
{
    //【移動控制 1/3】
    public int speed = 5;
    private Rigidbody2D rb; //設定為 private，避免他人在看此專案時，不知此物件為何
    public bool isVertical;
    public int direction = 1; //用來控制方向用，配和【折返方向】

    //【折返方向 1/4】使用時間機制達到折返
    public float walkTime = 3;
    private float timer; //設計一個倒數計時器

    //【動畫混合樹 1/4】
    public Animator enemyAnimator;

    //【敵人碰到子彈行為 1/3】
    public bool broken = true; //初始設定為故障，所以具危險性，會亂走(需要維修)

    //【關閉煙霧特效 1/2】：使用關閉特效機制
    public ParticleSystem smokeEffect;

    public AudioSource audioSource;
    public AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        //【移動控制 2/3】使用剛體移動，遊戲啟動初始取得剛體並存於 rb 剛體變數中
        rb = GetComponent<Rigidbody2D>();

        //【折返方向 2/4】遊戲啟動，timer 獲得 walkTime 的時間
        timer = walkTime;

        //【動畫混合樹 2/4】
        enemyAnimator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //【敵人碰到子彈行為 2/3】
        if (!broken) //表示沒有損壞，意謂著被修好，就原地跳舞，不再移動
        {
            return; // 已修好，不再移動。//當執行到 return 時，會跳出目前的所處方法，
                    // 如 Update()，reutrn 以下的程式將不再執行
        }

        //【折返方向 3/4】倒數計時
        timer = timer - Time.deltaTime;

        //【折返方向 4/4】當計時器歸零時，方向反轉走
        if (timer <= 0)
        {
            direction = -direction;
            timer = walkTime; //計時器再度取得原本設定的倒數時間（行走時間）
        }

        //【移動控制 3/3】
        Vector2 enemyPosition = transform.position; //將目前物件所在位置傳給 enemyPositon

        if (isVertical)
        {
            enemyPosition.y = enemyPosition.y + speed * Time.deltaTime * direction;
        }
        else
        {
            enemyPosition.x = enemyPosition.x + speed * Time.deltaTime * direction;
        }

        rb.MovePosition(enemyPosition);

        //【動畫混合樹 4/4】
        PlayMoveAnimation();

    }

    //【動畫混合樹 3/4】
    //因為此新建方法，只有在這裡使用，所以使用 private 即可
    private void PlayMoveAnimation()
    {
        if (isVertical) //垂直軸向動畫設置
        {
            enemyAnimator.SetFloat("MoveX", 0);
            enemyAnimator.SetFloat("MoveY", direction);
        }
        else //水平軸向動畫設置
        {
            enemyAnimator.SetFloat("MoveX", direction);
            enemyAnimator.SetFloat("MoveY", 0);
        }
    }

    //【碰撞玩家 1/1】
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GoGoGo_14a gogogo_14 = collision.gameObject.GetComponent<GoGoGo_14a>();
        if (gogogo_14 != null) //檢查碰撞 Ruby
        {
            print("碰到敵人了，扣血(-1)");
            gogogo_14.ChangeHealth(-1);
        }
    }

    //【敵人碰到子彈行為 3/3】：機器人被修復的方法
    public void Fix() //一定得用 public 因為要讓子彈的程式碼調用這個方法
    {
        broken = false; //機器人「非」損壞
        rb.simulated = false; //剛體的.simulated 方法若取消(false)
                              //表示不再與任何物件進行碰撞互動檢測

        //【敵人碰到子彈行為】動畫控制
        enemyAnimator.SetTrigger("Fixed");

        //【關閉煙霧特效 2/2】
        smokeEffect.Stop();
        //Destroy(smokeEffect); //此方式也可以，但粒子會瞬間消失，沒有 fu

        audioSource.PlayOneShot(hitSound);
    }
    
}
