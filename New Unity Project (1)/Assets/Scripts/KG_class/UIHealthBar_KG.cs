using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 控制 UI 血條＋【單一實例】
/// </summary>
public class UIHealthBar_KG : MonoBehaviour
{
    public Image mask;
    float originalSize;

    //【單一實例】:整個專案只有此名稱唯一，方便外部隨時調用此方法
    public static UIHealthBar_KG instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;        
    }

    //設置當前 UI 血條顯示值
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            originalSize * fillPercent);
    }
}
