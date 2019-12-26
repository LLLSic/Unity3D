## 作业要求
https://pmlpml.github.io/unity3d-learning/12-AR-and-MR
1. 图片识别与建模
2. 虚拟按键小游戏

## 图片识别与建模
### Vuforia证书配置
在[官网](https://developer.vuforia.com/)注册一个开发者账号，之后进入证书管理界面，在License Manager，选择Get Development Key，填写APP名称和勾选选项。添加一个证书密钥，用于之后unity内部的配置。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226142426710.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
创建成功之后证书列表中就会出现一个新的证书项，点击进入之后就可以进入下面的页面，其中会有一长串密钥，点击这个密钥就会自动复制到剪切板。

### Vuforia数据库配置
在Target Manager页面上起一个名字再点击Add Database
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226130443666.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226130626934.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
点击数据库名称，Add Target，添加图片，用于之后的提取特征与识别。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226140209482.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
添加成功之后targets列表中出现了我们刚刚添加的图片。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226140417816.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
点击Download Database生成一个unity package。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226140758828.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226140905616.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
### 安装vuforia扩展包
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226144907160.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
选择其中的player settings，进入如下界面可以看到XR support Installers下面有一个链接。使用这个链接可以自动跳转到网页，并开始下载与你的unity版本一致的vuforia。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226145057172.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
运行安装程序，将安装目录设置为你的unity所在的安装路径。不要使用默认安装路径，如果安装程序无法在指定路径下找到unity.exe，安装是无法正常进行的。注意，在安装之前必须要关闭当前所有unity项目，不然安装过程也是无法进行的。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226145612454.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
这时我们再次在building setting里面打开player setting，可以看到XR setting下出现了一个新的选项。在如下界面中勾选Vuforia Augmented Reality，完成这一步之后才能进行进一步的配置。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226150451700.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

### 新建项目
新建一个unity项目，导入刚刚生成的数据包，这些导入的数据会在之后用到。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226152011514.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
打开项目，我们可以看到GameObject选项下出现了Vuforia Engine的选项，这时我们导入一个AR Camera。选择其中的VuforiaConfiguration选项，在这个界面中的App License Key位置粘贴上我们前面得到的证书密钥，完成这一步之后我们才能开始使用Vuforia。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191226154216272.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

删除原有的Main camera，选择Gameobject->Vuforia Engine->Camera Image->Camera Image Target，然后在如下设置界面中将Type设置为Predefined，这时系统会自动选择之前已经导入的3D_Course数据库中的dragon图片。引入之前第4步生成的模型package，把Image Target的Database和Image Target都配置好为package的数据库和照片名。


## 虚拟按键小游戏
这里设计的是：通过按键操作简单的任务动作。

### 增加虚拟按钮
点击 ImageTarget ，在其 Inspector 面板中找到 Image Target Behaviour 组件，可以看到 Advanced部件，展开后看到 Add Virtual Button 按钮，点击该按钮即可添加虚拟按钮。
在其下添加了一个plane，大小和位置与按钮相同。
### 控制代码

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VitrualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler
{
    //public GameObject vb;
    public Animator ani;
    public VirtualButtonBehaviour[] vbs;
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (vb.VirtualButtonName == "button1")
        {
            ani.gameObject.transform.Rotate(Vector3.up * 180);
        }
        Debug.Log(vb.VirtualButtonName);
        //throw new System.NotImplementedException();
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        ani.gameObject.transform.Rotate(Vector3.up * 180);
    }

    // Start is called before the first frame update
    void Start()
    {
        vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        //VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
    }

}
```

