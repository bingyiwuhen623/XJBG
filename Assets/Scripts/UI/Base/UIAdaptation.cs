using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdaptation : Singletion<UIAdaptation>
{
    public int ScreenWidth = GameDefine.NORMAL_SCREEN_WIDTH;
    // 游戏内的画面高度均为1080，宽度为计算所得
    public int ScreenHeight => GameDefine.NORMAL_SCREEN_HEIGHT;

    public Vector2 ScreenSize
    {
        get
        {
            return new Vector2(ScreenWidth, ScreenHeight);
        }
    }

    public void Init()
    {
        // 屏幕比例，宽/高
        float screenPro = (float)Screen.width / Screen.height;
        ScreenWidth = (int)(ScreenHeight * screenPro);
    }
}