
## 作业要求
 - 分别使用 IMGUI 和 UGUI 实现
 - 使用 UGUI，血条是游戏对象的一个子元素，任何时候需要面对主摄像机
 - 分析两种实现的优缺点
 - 给出预制的使用方法

## 参考博客
 https://blog.csdn.net/JC2474223242/article/details/80584980
 
## IMGUI实现
直接代码实现：

```csharp
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

```

新建一个Emtpy对象，将该脚本拖到该对象运行即可。

## UGUI实现
首先资源导入，然后给人物加上血条即可。
（这里步骤跟着参考博客走即可）
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191123213430678.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
之后在ethen上挂上下面脚本：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    public Canvas canvas;
    public GameObject healthPrefab;

    public float healthPanelOffset = 2f;
    public GameObject healthPanel;
    private Slider healthSlider;

    // Use this for initialization
    void Start () {
        healthPanel = Instantiate(healthPrefab) as GameObject;
        healthPanel.transform.SetParent(canvas.transform, false);
        healthSlider = healthPanel.GetComponentInChildren<Slider>();
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }

    // Update is called once per frame
    void Update () {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
    }
}
```


## 两种实现的优缺点
### IMGUI
 - 优点：
符合游戏编程的传统
开发简单，仅需几行代码

 - 缺点：
效率低下难以调试


### UGUI
 - 优点：
效率更高更好调试
UGUI直接定位到所要的位置即可，不像IMGUI需要放置在游戏对象下面，作为游戏对象的一个子对象

 - 缺点：
效率低下难以调试
上手较困难

## 预制的使用方法
将预制体拖入场景→导入资源→用预制体生成Ethan→将Canvas预制体拖入到Ethan对象，成为其子对象→将Canvas的子对象Slider拖入HeatlthBarIMGUI对象的healthbarIMGUI.cs组件中的Slider属性
运行后点击增/减血按钮即可


