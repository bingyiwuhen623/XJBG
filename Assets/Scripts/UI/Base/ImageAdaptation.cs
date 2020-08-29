using UnityEngine;
using UnityEngine.UI;

public class ImageAdaptation : MonoBehaviour
{
    // 图片宽度，非Image以填写的为准
    [SerializeField] float spriteWidth;
    // 图片高度，非Image以填写的为准
    [SerializeField] float spriteHeight;
    RectTransform rect;
    public void Start()
    {
        rect = GetComponent<RectTransform>();
        if (rect == null)
        {
            return;
        }
        SetRectSize();
    }
    /// <summary>
    /// 设置图片大小
    /// </summary>
    private void SetRectSize()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            if (image.sprite != null)
            {
                spriteWidth = image.sprite.rect.width;
                spriteHeight = image.sprite.rect.height;
            }
            // Image调整rect的sizeDelta
            SetSizeDelta();
        }
        else
        {
            // 非Image调整rect的scale
            SetSizeScale();
        }
    }

    /// <summary>
    /// 调整rect的sizeDelta
    /// </summary>
    private void SetSizeDelta()
    {
        Vector2 endSize = Vector2.one * ChangeValue();
        endSize.x *= spriteWidth;
        endSize.y *= spriteHeight;
        rect.sizeDelta = endSize;
    }

    /// <summary>
    /// 调整rect的scale
    /// </summary>
    private void SetSizeScale()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * ChangeValue();
        transform.localScale = endScale;
    }

    private float ChangeValue()
    {
        float spritePro = spriteWidth / spriteHeight;
        Vector2 screenSize = UIAdaptation.Me.ScreenSize;
        // 屏幕宽高比
        float screenPro = screenSize.x / screenSize.y;
        float changeValue = 1;
        // 图比屏幕宽
        if (spritePro > screenPro)
        {
            // 以高度为标准计算缩放
            changeValue = screenSize.y / GameDefine.NORMAL_SCREEN_HEIGHT;
        }
        // 图比屏幕高
        else if (spritePro < screenPro)
        {
            // 以宽度为标准计算缩放
            changeValue = screenSize.x / GameDefine.NORMAL_SCREEN_WIDTH;
        }
        else
        {
            changeValue = 1;
        }
        return changeValue;
    }
}