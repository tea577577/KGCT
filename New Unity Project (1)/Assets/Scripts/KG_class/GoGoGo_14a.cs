using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 使用混合樹動畫機制的 ruby 移動程式＋Week13【血量控制】＋【發射子彈】
/// GoGoGoGo_14a 學生練習版
/// ＋Week14【新增音效】
/// </summary>
public class GoGoGo_14a : MonoBehaviour
{
    private Vector2 lookDirection; //定義看的方向
    private Vector2 rubyPosition;  //定義位置
    private Vector2 rubyMove;      //定義移動量

    public Animator rubyAnimator; //定義乘載 ruby 的動畫控制器變數箱子
    public Rigidbody2D rb; //定義剛體（移動用）

    public float speed = 3;

    //【血量控制 1/4】
    [Header("最高血量")]        //在檢查器內的輔助顯示
    public int maxHealth = 5;  //定義最高血量值

    [Header("當前血量"), Range(0, 5)] //在檢查器內的輔助顯示+可調動
    //private int currentHelath;     //定義當前血量（不顯示）    
    public int currentHelath;        //定義當前血量（顯示在檢查器）

    //【發射子彈 1】
    public GameObject projectilePrefab;

    public AudioSource audioSource;

    public AudioClip playHit;
    public AudioClip ThrowCog;

    // Start is called before the first frame update
    void Start()
    {
        rubyAnimator = GetComponent<Animator>(); //遊戲啟動取得 動畫控制器元件
        rb = GetComponent<Rigidbody2D>(); //遊戲啟動取得 剛體元件

        //【血量控制 2/4】
        currentHelath = maxHealth;
        print("Ruby當前血量為：" + currentHelath);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rubyPosition = transform.position; //把目前物件的位置給予ruby

        float horizontal = Input.GetAxis("Horizontal"); //擷取左右按鍵的數值
        float vertical = Input.GetAxis("Vertical");     //擷取上下按鍵的數值
        //print("Horizontal is: " + horizontal); //檢查用（顯示按鍵數值）
        //print("Vertical is: " + vertical);     //檢查用（顯示按鍵數值）

        rubyMove = new Vector2(horizontal, vertical); //把按鍵的數值給予 rubyMove

        //當按鍵輸入不為0時
        if (!Mathf.Approximately(rubyMove.x, 0) || !Mathf.Approximately(rubyMove.y, 0))
        {
            lookDirection = rubyMove;  //當玩家按下移動按鍵時(不為0)，則給予 ruby 方位 
            lookDirection.Normalize(); //標準化，使按鍵數值更快接近數值：1
        }

        //【控制混合樹的動畫】
        rubyAnimator.SetFloat("Look X", lookDirection.x); //給予朝向的數值
        rubyAnimator.SetFloat("Look Y", lookDirection.y); //給予朝向的數值
        rubyAnimator.SetFloat("Speed", rubyMove.magnitude); //把rubyMove的移動向量給予 Speed

        //移動 ruby 位置
        rubyPosition = rubyPosition + speed * rubyMove * Time.deltaTime;
        rb.MovePosition(rubyPosition); //使用剛體進行移動


        // 【血量控制 4/4】當血量為0時，重新遊戲關卡（讀取關卡）
        if (currentHelath == 0)
        {
            Application.LoadLevel("class_enemyGo");

        }

        //【發射子彈 3/3】設定發射行為的按鍵
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
            print("fire!發射了！");
            PlaySound(ThrowCog);
        }

    }

    //【血量控制 3/4】
    public void ChangeHealth(int amout)
    {
        if (amout < 0)
        {
            PlaySound(playHit);
        }

        

        currentHelath = Mathf.Clamp(currentHelath + amout, 0, maxHealth); //加血機制-2 改良版
        print("Ruby 當前血量為：" + currentHelath); //檢查是否有加血

    }

    //【發射子彈 2/3】
    private void Launch() //使用 private ，因只有此程式專用
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
        bullet.Launch(lookDirection, 300); //300 數值越大，速度越快

        // 發射後，播放動畫
        rubyAnimator.SetTrigger("Launch");


    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    
}
