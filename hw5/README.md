@[TOC](目录)
# 作业要求
**编写一个简单的鼠标打飞碟（Hit UFO）游戏**
 - 游戏内容要求：
 1. 游戏有 n 个 round，每个 round 都包括10 次 trial；
 2. 每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
 3. 每个 trial 的飞碟有随机性，总体难度随 round 上升；
 4. 鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
 - 游戏的要求：
 5. 使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
 6. 近可能使用前面 MVC 结构实现人机交互与游戏模型分离
 7. 如果你的使用工厂有疑问，参考：弹药和敌人：减少，重用和再利用
 

# 参考博客
https://blog.csdn.net/jc2474223242/article/details/79975137?tdsourcetag=s_pcqq_aiomsg
 
# 游戏UML图
这里参考了前辈的代码，所以UML类图几乎一样，就直接借用了。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191009205652408.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
# 实现思路
这里主要介绍几个比较大的模块的大概思路，详细说明可见代码的注释部分。

 - GUI页面
首先是用户GUI页面

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private FirstSceneController action;
    private GUIStyle fontstyle1 = new GUIStyle();

    //1
    private GUIStyle textStyle;
    private GUIStyle hintStyle;
    private GUIStyle btnStyle;

    //初始化
    void Start () {
        action = SSDirector.getInstance().currentSceneController as FirstSceneController;
        fontstyle1.fontSize = 20;
    }

    // Update is called once per frame
    private void OnGUI()
    {
        hintStyle = new GUIStyle {
            fontSize = 20,
            fontStyle = FontStyle.Normal
        };

        //标题
        textStyle = new GUIStyle {
            fontSize = 50,
            alignment = TextAnchor.MiddleCenter
        };
        textStyle.normal.textColor = Color.blue;
        GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), "UFO小游戏！", textStyle);

        //button
        btnStyle = new GUIStyle("button") {
            fontSize = 15
        };
        btnStyle.normal.textColor = Color.black;
        if (GUI.Button(new Rect(0, 50, 80, 50), "重新开始", btnStyle))  //前两个参数是位置（左上角），后两个是大小
        {
            action.Restart();
        }
        if (GUI.Button(new Rect(0, 100, 80, 50), " 暂停 ", btnStyle))
        {
            action.Pause();
        }

        //得分
        if (action.flag == 0)
        {
            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(0, 0, 300, 50), "得分: " +
                action.score + ", 回合: " + (Mathf.CeilToInt(FirstSceneController.times / 10) + 1), fontstyle1);
        }
        //结束
        else if (action.flag == 1)
        {
            fontstyle1.normal.textColor = Color.red;
            GUI.Label(new Rect(0, 0, 300, 50), "你的得分是 : " + action.score, fontstyle1);
        }
        //暂停
        else
        {
            fontstyle1.normal.textColor = Color.green;
            GUI.Label(new Rect(0, 0, 300, 50), "得分: " +
                action.score + ", 回合: " + (Mathf.CeilToInt(FirstSceneController.times / 10) + 1), fontstyle1);
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height/2-50, 300, 100), "暂停!", textStyle);
        }
    }
}

```

 - 由之前的两次作业（牧师和魔鬼），我们已经有了一个较为完整的MVC框架，所以这里可以直接套用的之前的框架。但这里比较特别的是，添加了一个**飞碟工厂**，用于减少资源开销 ，详细说明请见注释：

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    //used和free分别表示已经在使用的和未被利用的飞碟对象
    public List<GameObject> used = new List<GameObject>();
    public List<GameObject> free = new List<GameObject>();

	//初始化
	void Start () { }

    public void GenDisk()
    {
        GameObject disk;
        //如果free的列表为空，而我们有需要它，这时才创建新的对象
        if(free.Count == 0)
        {
            disk = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk"), Vector3.zero, Quaternion.identity);
        }
        //不然，就用之前的
        else
        {
            disk = free[0];
            free.RemoveAt(0);
        }
        float x = Random.Range(-10.0f, 10.0f);
        disk.transform.position = new Vector3(x, 0, 0);
        disk.transform.Rotate(new Vector3(x < 0? -x*9 : x*9, 0, 0));
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        disk.transform.GetComponent<Renderer>().material.color = color;
        used.Add(disk);
    }
    public void RecycleDisk(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        free.Add(obj);
    }
}
```


 - 场景控制器，依靠之前所做的MVC架构的分离作用，这里仅需要改Awake,Update,GenGameObject这三个函数和各种变量。 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场景控制器
public class CCActionManager : SSActionManager, ISSActionCallback
{
    //这里将动作变成一个列表，就能够同时控制多个飞碟在游戏中飞行
    public FirstSceneController sceneController;
    public List<CCMoveToAction> seq = new List<CCMoveToAction>();
    public UserClickAction userClickAction;
    public DiskFactory disks;
    
    protected new void Start()
    {
        sceneController = (FirstSceneController)SSDirector.getInstance().currentSceneController;
        sceneController.actionManager = this;
        disks = Singleton<DiskFactory>.Instance;
    }
    protected new void Update()
    {
        if(disks.used.Count > 0)
        {
            GameObject disk = disks.used[0];
            float x = Random.Range(-10, 10);
            CCMoveToAction moveToAction = CCMoveToAction.GetSSAction(new Vector3(x, 12, 0), 3 * (Mathf.CeilToInt(FirstSceneController.times / 10) + 1) * Time.deltaTime);
            seq.Add(moveToAction);
            this.RunAction(disk, moveToAction, this);
            disks.used.RemoveAt(0);
        }
        if (Input.GetMouseButtonDown(0) && sceneController.flag == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitGameObject;
            if (Physics.Raycast(ray, out hitGameObject))
            {
                GameObject gameObject = hitGameObject.collider.gameObject;
                if (gameObject.tag == "disk")
                {
                    foreach(var k in seq)
                    {
                        if (k.gameObject == gameObject)
                            k.transform.position = k.target;
                    }
                    userClickAction = UserClickAction.GetSSAction();
                    this.RunAction(gameObject, userClickAction, this);
                }
            }
        }
        base.Update();
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null)
    {
        disks.RecycleDisk(source.gameObject);
        seq.Remove(source as CCMoveToAction);
        source.destory = true;
        if (FirstSceneController.times >= 30)
            sceneController.flag = 1;
    }
    public void CheckEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null)
    {
    }
    public void Pause()
    {
        if(sceneController.flag == 0)
        {
            foreach (var k in seq)
            {
                k.enable = false;
            }
            sceneController.flag = 2;
        }
        else if(sceneController.flag == 2)
        {
            foreach (var k in seq)
            {
                k.enable = true;
            }
            sceneController.flag = 0;
        }
    }
}

```
# 游戏截图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191009211514426.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

