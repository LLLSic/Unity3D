using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbarIMGUI : MonoBehaviour {
    public Slider slider;
    public float health = 0.0f; //初始血量
    public float curhealth;  //结果血量
    public Rect HealthBar;
    public Rect ButtonUp;
    public Rect ButtonDown;

	// Use this for initialization
	void Start () {
        HealthBar = new Rect(200, 0, 200, 20);  //血条区域
        ButtonUp = new Rect(260, 30, 80, 20);   //加血
        ButtonDown = new Rect(260, 60, 80, 20); //减血
        curhealth = health;
	}

    void OnGUI()
    {
        if (GUI.Button(ButtonUp,"增加血量"))
        {
            curhealth = curhealth + 0.1f > 1.0f ? 1.0f : curhealth + 0.1f;
        }
        if (GUI.Button(ButtonDown, "减少血量"))
        {
            curhealth = curhealth - 0.1f < 0.0f ? 0.0f : curhealth - 0.1f;
        }
        //插值实现平滑变化
        health = Mathf.Lerp(health, curhealth, 0.05f);
        slider.value = health;
        //主要思路：用水平滚动条的宽度作为血条的显示值
        GUI.HorizontalScrollbar(HealthBar, 0.0f, health, 0.0f, 1.0f);
    }
}

