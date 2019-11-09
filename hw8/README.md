@[TOC](目录)
# 作业要求
 - 简单粒子制作

	按参考资源要求，制作一个粒子系统，参考资源
使用 3.3 节介绍，用代码控制使之在不同场景下效果不一样

# 参考博客
https://www.cnblogs.com/CaomaoUnity3d/p/5983730.html
和
https://blog.csdn.net/qq_36312878/article/details/80492125
其中，后一篇博客将粒子系统的各个参数均罗列了出来，这里就不照搬了。

# 实现过程
## 生成ParticleSystem
创建一个空项目，添加ParticleSystem的组件，挂上材料，调节参数。

整体粒子包含：
1. 中间光的模拟（midLight）
2. 光晕的模拟（halo）
3. 周围星光的模拟（shining）

### 中间光的模拟（midLight）
（因为我个人比较喜欢蓝色，这里就用的蓝色的光啦）
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191109195043900.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

### 光晕的模拟（halo）
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191109195220488.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
### 周围星光的模拟（shining）
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191109195322903.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
三者都是创建在空对象的下面，因为是粒子的整体部分，所以把它当做粒子的父类节点。
中间部分的粒子不会移动，所以Speed设置为0，粒子的Shape可以为Box或者Sphere，因为主要目的是让光晕填充完这个粒子（显的饱满）。

## 代码控制
### 控制功能
模拟下矿洞，或者调入深渊中，头上的太阳模拟
### 中间部分的控制
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class midLightChange : MonoBehaviour {

    ParticleSystem exhaust;
    float size = 2f;

    // Use this for initialization
    void Start()
    {
        exhaust = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        size = size * 0.999f;
        var main = exhaust.main;
        main.startSize = size;
    }

}

```
### 光晕控制

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class haloChange : MonoBehaviour {
    ParticleSystem exhaust;
    float size = 5f;

    // Use this for initialization
    void Start()
    {
        exhaust = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        size = size * 0.999f;
        var main = exhaust.main;
        main.startSize = size;
    }
}

```
