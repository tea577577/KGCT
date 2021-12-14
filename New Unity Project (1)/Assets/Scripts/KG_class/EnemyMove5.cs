using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵人程式：KG自己練習。敵人移動＋折返方向(時間倒數)＋【碰撞檢測】碰撞主角使之受傷
/// ＋【動畫左右移動】＋上下＋【改為混合樹控制上下左右移動】
/// ＋【敵人碰到子彈行為】＋【關閉粒子系統】
/// </summary>
public class EnemyMove5 : MonoBehaviour
{
    //移動控制用
    public int speed = 5;
    private Rigidbody2D rb; //設定為 private，避免他人在看此專案時，不知此物件為何
    public bool isVertical;
    public int direction = 1;

    //時間控制用

    public float walkTime = 3;
    private float timer;

    //宣告放置動畫的變數
    public Animator enemyAnimator;


    //【敵人碰到子彈行為 1/2】
    // 機器人是否故障
    private bool broken = true; //初始設定為故障，所以具危險性，會亂走(需要維修)

    //【關閉粒子系統 1/2】
    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = walkTime;

        //初始化：取得動畫控制器
        enemyAnimator = GetComponent<Animator>();
        //enemyAnimator.SetFloat("moveX", direction); //朝向左邊，補空閑時間
        //enemyAnimator.SetBool("Vertical", isVertical);//上下移動動畫

        //【改為混合樹控制上下左右移動 1/3】，上面的原本的 Animator 需調整
        // 垂直軸向控制
        if (isVertical)
        {
            enemyAnimator.SetFloat("MoveX", 0);
            enemyAnimator.SetFloat("MoveY", direction);
        }
        else //水平軸向動畫控制
        {
            enemyAnimator.SetFloat("MoveX", direction);
            enemyAnimator.SetFloat("MoveY", 0);
        }
        //(但因有重複，所以使用封裝方法)，取代上面的程式碼
        PlayMoveAnimation();

    }

    // Update is called once per frame
    void Update()
    {
        //【敵人碰到子彈行為 2/3】
        if (!broken) //表示沒有損壞，意謂著被修好，就原地跳舞，不再移動
        {
            // 已修好，不再移動
            return; //當執行到 return 時，會跳出目前的所處方法，
                    //如 Update()，reutrn以下的程式碼都不會執行
        }


        //時間處理
        timer -= Time.deltaTime;
        //print("敵人移動，時間倒數:" + timer);
        if (timer <= 0)
        {
            direction = -direction;

            //朝向左邊
            //enemyAnimator.SetFloat("moveX",direction);
            //enemyAnimator.SetBool("Vertical", isVertical);

            //【改為混合樹控制上下左右移動 2/3】（和上面是一樣的，會放到這邊是因為倒數 3 秒後，也希望有變化）
            // 垂直軸向控制 
            //if (isVertical)
            //{
            //    enemyAnimator.SetFloat("MoveX", 0);
            //    enemyAnimator.SetFloat("MoveY", direction);
            //}
            //else //水平軸向動畫控制
            //{
            //    enemyAnimator.SetFloat("MoveX", direction);
            //    enemyAnimator.SetFloat("MoveY", 0);
            //}
            
            PlayMoveAnimation(); //(但因有重複，所以使用封裝方法)，取代上面的程式碼
            timer = walkTime;
        }

        //移動處理
        Vector2 enemyPosition = transform.position;

        if (isVertical)
        {
            enemyPosition.y = enemyPosition.y + speed * Time.deltaTime * direction;
        }
        else
        {
            enemyPosition.x = enemyPosition.x + speed * Time.deltaTime * direction;
        }
     
        rb.MovePosition(enemyPosition);
    }

    //【碰撞檢測】
    private void OnCollisionEnter2D(Collision2D collision) //使用碰撞行為:敵人身上的 Collider Is Trigger 不勾
    //private void OnTriggerEnter2D(Collider2D collision) //使用接觸行為:敵人身上的 Collider Is Trigger 打勾
    {
        RubyMove5 rubyMove5 = collision.gameObject.GetComponent<RubyMove5>();
        if (rubyMove5 != null)
        {
            print("碰到敵人，扣血！");
            rubyMove5.ChangeHealth(-1);
        }
    }

    // 【改為混合樹控制上下左右移動 3/3】 因為上面的相同程式碼重複了兩個地方，因此這邊做方法封裝
    private void PlayMoveAnimation()
    {
        // 垂直軸向動畫控制
        if (isVertical)
        {
            enemyAnimator.SetFloat("MoveX", 0);
            enemyAnimator.SetFloat("MoveY", direction);
        }
        else //水平軸向動畫控制
        {
            enemyAnimator.SetFloat("MoveX", direction);
            enemyAnimator.SetFloat("MoveY", 0);
        }
    }

    //【敵人碰到子彈行為 3/3】：修復機器人的方法
    public void Fix()
    {
        broken = false; //機器人「非」損壞
        rb.simulated = false; //剛體的.simulated 方法若取消(false)
                              //表示不再與任何物件進行碰撞互動檢測

        //【敵人碰到子彈行為】動畫控制
        enemyAnimator.SetTrigger("Fixed");

        //【關閉粒子系統 2/2】
        smokeEffect.Stop();
        //Destroy(smokeEffect); //此方式也可以，但粒子會瞬間消失，不帶感
    }
}
