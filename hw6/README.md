 
@[TOC](目录)
# 作业要求
**改进飞碟（Hit UFO）游戏**
  - 按 *adapter模式* 设计图修改飞碟游戏
  - 使它同时支持物理运动与运动学（变换）运动
 
# 参考博客
https://blog.csdn.net/JC2474223242/article/details/80065909

# 实验UML图
这里参考了前辈的思想，所以UML类图几乎一样，就直接借用了。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191018134830950.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
将组合在FirstSceneController里面的动作管理器改为了动作管理器接口，然后两个适配器（CCActionManager和PhysicsActionManager）继承这个接口。

# 具体实现
- 首先给物体加上感谢属性：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191018204501475.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

-  **CCActionManager：**修改Update为PlayDisk，只需修改函数名即可。

```csharp
protected new void PlayDisk()  //此处修改Update为PlayDisk，只需修改函数名即可。
```
-  **FirstSceneController:** 实现动作管理器版和物理引擎版之间切换

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneController : MonoBehaviour, IUserAction, ISceneController{
    //修改CCActionManager为IActionManager
    public IActionManager actionManager;

    public GameObject disk;
    protected DiskFactory df;
    public int flag = 0;
    private float interval = 3;
    public int score = 0;
    public static int times = 0;

    private void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.setFPS(60);
        director.currentSceneController = this;
        this.gameObject.AddComponent<DiskFactory>();
        /*原来的代码为this.gameObject.AddComponent<CCActionManager>();*/
        this.gameObject.AddComponent<PhysicsActionManager>();
        this.gameObject.AddComponent<UserGUI>();
        df = Singleton<DiskFactory>.Instance;
    }
    private void Start()
    {
    }
    public void GenGameObjects ()
    {
    }
    public void Restart()
    {
        SceneManager.LoadScene("1");
    }
    public void Pause ()
    {
        actionManager.Pause();
    }
    public void Update()
    {
        if (times < 30 && flag == 0)
        {
            if (interval <= 0)
            {
                interval = Random.Range(3, 5);
                times++;
                df.GenDisk();
            }
            interval -= Time.deltaTime;
        }
        //这一句是添加的代码
        actionManager.PlayDisk();
    }
}
```
- **实现PhysicsEmitAction：** 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEmitAction : SSAction {
    public Vector3 speed;

    public static PhysicsEmitAction GetSSAction()
    {
        PhysicsEmitAction action = CreateInstance<PhysicsEmitAction>();
        return action;
    }
    public override void Start()
    {
    }
    public override void Update()
    {
        if (transform.position.y < -10 || transform.position.x <= -20 || transform.position.x >= 20)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Vector3.down;
            callback.SSActionEvent(this);
        }
    }
}
```
- **实现适配器PhysicsActionManager：**

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsActionManager : SSActionManager, ISSActionCallback, IActionManager {
    public FirstSceneController sceneController;
    public List<PhysicsEmitAction> seq = new List<PhysicsEmitAction>();
    public UserClickAction userClickAction;
    public DiskFactory disks;

    protected void Start()
    {
        sceneController = (FirstSceneController)SSDirector.getInstance().currentSceneController;
        sceneController.actionManager = this;
        disks = Singleton<DiskFactory>.Instance;
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null)
    {
        disks.RecycleDisk(source.gameObject);
        seq.Remove(source as PhysicsEmitAction);
        source.destory = true;
        if (FirstSceneController.times >= 30)
            sceneController.flag = 1;
    }
    public void CheckEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed, int intParam = 0, string strParam = null, Object objParam = null)
    {
    }
    public void Pause()
    {
        if (sceneController.flag == 0)
        {
            foreach (var k in seq)
            {
                k.speed = k.transform.GetComponent<Rigidbody>().velocity;
                k.transform.GetComponent<Rigidbody>().isKinematic = true;
            }
            sceneController.flag = 2;
        }
        else if (sceneController.flag == 2)
        {
            foreach (var k in seq)
            {
                k.transform.GetComponent<Rigidbody>().isKinematic = false;
                k.transform.GetComponent<Rigidbody>().velocity = k.speed;
            }
            sceneController.flag = 0;
        }
    }
    public void PlayDisk()
    {
        if (disks.used.Count > 0)
        {
            GameObject disk = disks.used[0];
            float x = Random.Range(-5, 5);
            disk.GetComponent<Rigidbody>().isKinematic = false;
            disk.GetComponent<Rigidbody>().velocity = new Vector3(x, 8 * (Mathf.CeilToInt(FirstSceneController.times / 10) + 1), 6);
            disk.GetComponent<Rigidbody>().AddForce(new Vector3(0,8.8f, 0),ForceMode.Force);
            PhysicsEmitAction physicsEmitAction = PhysicsEmitAction.GetSSAction();
            seq.Add(physicsEmitAction);
            this.RunAction(disk, physicsEmitAction, this);
            disks.used.RemoveAt(0);
        }
        if (Input.GetMouseButtonDown(0) && sceneController.flag == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitGameObject;
            if (Physics.Raycast(ray, out hitGameObject))
            {
                GameObject gameObject = hitGameObject.collider.gameObject;
                Debug.Log(gameObject.tag);
                if (gameObject.tag == "disk")
                {
                    gameObject.transform.position=new Vector3(100,100,100);
                    userClickAction = UserClickAction.GetSSAction();
                    this.RunAction(gameObject, userClickAction, this);
                }
            }
        }
        base.Update();
    }
}
```

# 游戏截图
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191018204607897.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)


