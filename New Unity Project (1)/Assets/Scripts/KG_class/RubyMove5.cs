using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按鍵移動功能＋【恢復血量功能】＋【無敵狀態判定】
/// ＋【判斷是否受傷】＋【走路動畫】＋【面朝方向動畫設定＋方向鍵偵測優化】
/// ＋【發射子彈】＋【UI血條改變】
/// </summary>

public class RubyMove5 : MonoBehaviour
{
    public Rigidbody2D rb; //設定為 private 會較好，避免他人在看此專案時，不知此物件為何;
                           //而 Start() 會有 GetComponent<Rigidbody2D>() 來自動
                           //獲取此物件的 Rigidbody 元件，這將可避免執行遊戲時產生 物件丟失的錯誤
    public float moveSpeed = 5f;

    //【恢復血量功能 1/3】
    public int maxHealth = 5;
    //private int currentHealth;
    public int currentHealth;
    
    public int health
    {
        get
        {
            return currentHealth;
        }
    }

    //【無敵狀態判定 1/2】設置 Ruby 無敵時間
    public float timeInvincible = 2.0f; //無敵時間長度
    public bool IsInvincible;
    public float invincibleTimer; //計時器

    //【走路動畫 1/2：宣告一個動畫變數名稱】
    public Animator rubyAnimator;

    //【面朝方向動畫設定＋方向鍵監聽優化 1/3】
    private Vector2 lookDirection = new Vector2(1, 0);
    //public Vector2 lookDirection; //與上一行同義

    //【發射子彈 1/3】
    public GameObject projectilePrefab;

    



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //【恢復血量功能 2/3】
        currentHealth = maxHealth;
        print("Ruby血量為：" + currentHealth);
        //currentHealth = 3;

        //測試無敵時間長度的流逝和真實世界秒數相當
        //invincibleTimer = timeInvincible; //測試用
        //IsInvincible = true; //測試用

        //int a = GetRubyHealthValue(); //測試用，呼叫有返回值的函數
        //print("Ruby當前的血量為：" + a);

        //【走路動畫 2/3：初始化動畫變數名稱】
        rubyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //玩家輸入偵測
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //【面朝方向動畫設定＋方向鍵偵測優化 2/3】
        Vector2 move = new Vector2(horizontal, vertical);
        // 偵測玩家沒有進行任何方向鍵的移動時，也就是說玩家輸入的某個軸向值不為0，就執行內部的程式
        // Mathf.Approximately(a, b) -> 比較 a & b 兩數值是否相近，是則回傳 true
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x, move.y);
            //lookDirection = move; // 與上一行同義
            lookDirection.Normalize();
        }

        rubyAnimator.SetFloat("Look X", lookDirection.x);
        rubyAnimator.SetFloat("Look Y", lookDirection.y);
        rubyAnimator.SetFloat("Speed", move.magnitude); //將向量長度作為速度使用

        //【面朝方向動畫設定＋方向鍵偵測優化 3/3】
        Vector2 rubyPosition = transform.position;
        //int hInt = (int)Input.GetAxis("Horizontal");
        //rubyPosition.x = rubyPosition.x + moveSpeed * horizontal * Time.deltaTime; //使用 move 後就不用此方法
        //rubyPosition.y = rubyPosition.y + moveSpeed * vertical * Time.deltaTime; //使用 move 後就不用此方法
        // Ruby 位置移動（取代上述的兩行）
        rubyPosition = rubyPosition + moveSpeed * move * Time.deltaTime; 

        //print(Input.GetAxis("Horizontal"));
        //transform.position = rubyPostion; // old way
        rb.MovePosition(rubyPosition); //new way

        //【走路動畫 3/3：給予左右動畫，以及判定是否靜止狀態】
        //效果好一點但會滑步
        //rubyAnimator.SetFloat("moveX", horizontal); //控制左右動畫
        //rubyAnimator.SetFloat("moveY", vertical); //控制上下
        //if (horizontal ==0 && horizontal == 0)
        //{
        //    rubyAnimator.SetTrigger("idle");
        //}
        //rubyAnimator.SetInteger("idleX", (int)Input.GetAxis("Horizontal"));//效果沒有很好


        //【無敵狀態判定 2/2】
        if (IsInvincible)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                IsInvincible = false;
                //invincibleTimer = timeInvincible;
            }
        }

        //【發射子彈 3/3】設定發射行為的按鍵
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
            
        }

    }

    //【恢復血量功能 3/3】
    public void ChangeHealth(int amount)
    {
        //【判斷是否受傷】當傳入的 amount 是負值時，表示 Ruby 受到傷害
        if (amount < 0) //也就是說 當 amount<0 時，表示受到傷害
        {
            if (IsInvincible)
            {
                return;
            }

            //受到傷害
            IsInvincible = true;
            invincibleTimer = timeInvincible;
        }

        //增加血量的方式：currentHealth = currentHealth + amout
        //只是這邊配合 Mathf.Clamp() 方法，來限制避免超過最大血量的數值
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        print(currentHealth + "/" + maxHealth);

        //【UI血條改變】
        UIHealthBar_KG.instance.SetValue(currentHealth / (float)maxHealth);
    }

    //測試用
    //public int GetRubyHealthValue()
    //{
    //    return currentHealth;
    //}

    //【發射子彈 2/3】
    private void Launch()
    {
        // 使每次 Ruby 發射出的子彈（Prefab形式）都「實例化」成場景中的遊戲物件
        // 這個「實例化」動作就好比是我們手動把 #Project 中的物件拖曳到 #Scene 中
        // 但遊戲執行過程中，我們無法這麼做，所以必須透過程式來幫玩家執行這個動作
        // 生成的過程中，必須告知生成的 Prefab、要生成的位置 position、rotation角度
        // Quaternion 表示無角度旋轉
        GameObject projectileOnject = Instantiate(projectilePrefab,
            rb.position, Quaternion.identity);

        // 在 Bullet.cs 中，我們設置了一個 Launch()方法，並透過「受力的方法」來移動
        // 所以這邊需要為此設立一個 Bullet 型態的變量，作為承載此力的施壓容器
        Bullet bullet = projectileOnject.GetComponent<Bullet>();

        // 上面接收完畢後，便可透過自帶的 Launch() 方法來實現「受力的方法」
        // 我們在 Bullet.cs 中定義的 Launch() 方法需要給兩個參數：方向＆力道
        bullet.Launch(lookDirection,300); //300 數值越大，速度越快

        // 發射後，播放動畫
        rubyAnimator.SetTrigger("Launch");
    }

    //public void Hited()
    //{
    //    GameObject
    //}
}
