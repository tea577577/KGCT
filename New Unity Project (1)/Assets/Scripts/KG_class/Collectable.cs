using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 可收集寶物＋【產生特效】＋【產生音效】
/// </summary>
public class Collectable : MonoBehaviour
{
    //public ParticleSystem pickEffectP; 
    public GameObject pickE;

    //【產生音效 1】
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //pickEffectP.Play();

        //【產生特效】
        Instantiate(pickE, gameObject.transform.position, Quaternion.identity);

        GoGoGo_14a rubyGO = collision.GetComponent<GoGoGo_14a>();
        print("碰到的東西是：" + rubyGO);
        rubyGO.ChangeHealth(1);

        //【產生音效 2】
        rubyGO.PlaySound(audioClip);

        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Destroy(gameObject);
    }
}
