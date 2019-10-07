@[TOC](目录)
# 基本操作演练【建议做】

## 下载 Fantasy Skybox FREE， 构建自己的游戏场景
使用unity打开asset store，搜索“Fantasy Skybox FREE”并下载，
![ ](https://img-blog.csdnimg.cn/20191007102018710.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
 之后import使用，得到以下页面，点击import。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007102738821.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

创建一个新的Material（菜单 Assets→create→Material），更改Shader为Skybox-6 Sided（右边Inspector栏→shader→skybox→6 sided），按前后（Z）、上下（Y）、左右（X）拖入6个图片（在Fantasy Skybox Free中的Textures文件夹的贴图文件，或者在Inspector栏中点击select按名字进行选择），至此制作完成，便可拖入使用
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007105854198.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)


## 写一个简单的总结，总结游戏对象的使用
当需要在新的一个project使用已经下载过的skybox时，可在asset store中账号管理（右上角的L圆圈）中的My assets中找到。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20191007102554987.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)
至此，我学习到了多种GameObject的添加方式：
 1. 直接在菜单上（scene中）创建添加；
 2. 在代码中通过Instantiate() 函数实例化预制来创建；
 3. 下载现有的游戏对象到自己项目的Asset然后使用。
可以根据具体使用情况选择添加的方式。

此外，我还学习到了GameObject的组件修改方法：

 1. 通过右侧的Inspector栏来进行调节一些基础的组件（如Transform、Filter、Render等）
 2. 通过脚本的方式修改其属性


# 编程实践

## 牧师与魔鬼 动作分离版
【2019新要求】：设计一个裁判类，当游戏达到结束条件时，通知场景控制器游戏结束

### 调整Controller的结构到Action类
首先，重新调整Controller的结构，创建一个Action类，使其在不同控制器中进行实例化以实现各游戏对象移动操作的转移，并将动作的实现放在Action中。即在调用动作时，改为调用Action类中的该动作函数。修改前的代码和修改后的代码如下（主要是对Controller脚本的修改，其余的只需修改调用函数即可，这里就没有贴上来了）
修改前：
```csharp
using UnityEngine;

namespace MyNamespace {
    public enum Location { left, right }

    public class CoastController {
        public Coast coast;

        public CoastController(string _location) {
            coast = new Coast(_location);
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos = coast.positions[GetEmptyIndex()];
            pos.x *= (coast.Location == Location.right ? 1 : -1);
            return pos;
        }

        public void GetOnCoast(CharacterController character) {
            int index = GetEmptyIndex();
            coast.characters[index] = character;
        }

        public void GetOffCoast(string passenger_name) {
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] != null && 
                    coast.characters[i].character.Name == passenger_name) {
                    coast.characters[i] = null;
                }
            }
        }

        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] != null) {                   
                    if (coast.characters[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            coast.characters = new CharacterController[6];
        }
    }

    public class BoatController {
        public Boat boat;

        public BoatController() {
            boat = new Boat();
        }


        public void Move() {
            if (boat.Location == Location.left) {
                boat.mScript.SetDestination(boat.departure);
                boat.Location = Location.right;
            } else {
                boat.mScript.SetDestination(boat.destination);
                boat.Location = Location.left;
            }
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public bool IsEmpty() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    return false;
                }
            }
            return true;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos;
            int emptyIndex = GetEmptyIndex();
            if (boat.Location == Location.right) {
                pos = boat.departures[emptyIndex];
            } else {
                pos = boat.destinations[emptyIndex];
            }
            return pos;
        }

        public void GetOnBoat(CharacterController character) {
            int index = GetEmptyIndex();
            boat.passenger[index] = character;
        }

        public void GetOffBoat(string passenger_name) {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null && 
                    boat.passenger[i].character.Name == passenger_name) {
                    boat.passenger[i] = null;
                }
            }
        }

        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    if (boat.passenger[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            boat.mScript.Reset();
            if (boat.Location == Location.left) {
                Move();
            }
            boat.passenger = new CharacterController[2];
        }
    }

    public class CharacterController {
        readonly UserGUI userGUI;
        public Character character;

        public CharacterController(string _name) {
            character = new Character(_name);
            userGUI = character.Role.AddComponent(typeof(UserGUI)) as UserGUI;
            userGUI.SetCharacterCtrl(this);
        }

        public void SetPosition(Vector3 _pos) {
            character.Role.transform.position = _pos;
        }

        public void MoveTo(Vector3 _pos) {
            character.mScript.SetDestination(_pos);
        }

        public void GetOnBoat(BoatController boatCtrl) {
            character.Coast = null;
            character.Role.transform.parent = boatCtrl.boat._Boat.transform;
            character.IsOnBoat = true;
        }

        public void GetOnCoast(CoastController coast) {
            character.Coast = coast;
            character.Role.transform.parent = null;
            character.IsOnBoat = false;
        }

        public void Reset() {
            character.mScript.Reset();
            character.Coast = (Director.GetInstance().CurrentSecnController as FirstController).rightCoastCtrl;
            GetOnCoast(character.Coast);
            SetPosition(character.Coast.GetEmptyPosition());
            character.Coast.GetOnCoast(this);
        }
    }
}

```

修改后：
```csharp
using UnityEngine;

namespace MyNamespace {
    public enum Location { left, right }

    public class CoastController {
        public Coast coast;

        public Action coastAction = new Action();

        public CoastController(string _location) {
            coast = new Coast(_location);
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos = coast.positions[GetEmptyIndex()];
            pos.x *= (coast.Location == Location.right ? 1 : -1);
            return pos;
        }



        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] != null) {                   
                    if (coast.characters[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            coast.characters = new CharacterController[6];
        }
    }

    public class BoatController {
        public Boat boat;

        public Action boatAction = new Action();

        public BoatController() {
            boat = new Boat();
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public bool IsEmpty() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    return false;
                }
            }
            return true;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos;
            int emptyIndex = GetEmptyIndex();
            if (boat.Location == Location.right) {
                pos = boat.departures[emptyIndex];
            } else {
                pos = boat.destinations[emptyIndex];
            }
            return pos;
        }

        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    if (boat.passenger[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            boat.mScript.Reset();
            if (boat.Location == Location.left) {
                boatAction.boatMove(this);
            }
            boat.passenger = new CharacterController[2];
        }
    }

    public class CharacterController {
        readonly UserGUI userGUI;
        public Character character;

        public Action myAction = new Action();

        public CharacterController(string _name) {
            character = new Character(_name);
            userGUI = character.Role.AddComponent(typeof(UserGUI)) as UserGUI;
            userGUI.SetCharacterCtrl(this);
        }

        public void SetPosition(Vector3 _pos) {
            character.Role.transform.position = _pos;
        }

        public void MoveTo(Vector3 _pos) {
            character.mScript.SetDestination(_pos);
        }


        public void Reset() {
            character.mScript.Reset();
            character.Coast = (Director.GetInstance().CurrentSecnController as FirstController).rightCoastCtrl;
            myAction.GetOnCoast(this, character.Coast);
            SetPosition(character.Coast.GetEmptyPosition());
            //character.Coast.GetOnCoast(this);
        }
    }

    public class Action {
        //船移动
        public void boatMove(BoatController boatCtrl) {
            if (boatCtrl.boat.Location == Location.left) {
                boatCtrl.boat.mScript.SetDestination(boatCtrl.boat.departure);
                boatCtrl.boat.Location = Location.right;
            } else {
                boatCtrl.boat.mScript.SetDestination(boatCtrl.boat.destination);
                boatCtrl.boat.Location = Location.left;
            }
        }

        //上船
        public void GetOnBoat(CharacterController charactrl, BoatController boatCtrl) {
            //船控制器
            int index = boatCtrl.GetEmptyIndex();
            boatCtrl.boat.passenger[index] = charactrl;
            //人物控制器
            charactrl.character.Coast = null;
            charactrl.character.Role.transform.parent = boatCtrl.boat._Boat.transform;
            charactrl.character.IsOnBoat = true;
        }

        //下船
        public void GetOffBoat(string passenger_name, BoatController boatCtrl) {
            for (int i = 0; i < boatCtrl.boat.passenger.Length; ++i) {
                if (boatCtrl.boat.passenger[i] != null && 
                    boatCtrl.boat.passenger[i].character.Name == passenger_name) {
                    boatCtrl.boat.passenger[i] = null;
                }
            }
        }

        //上岸
        public void GetOnCoast(CharacterController charactrl,CoastController coastctrl) {
            //船
            int index = coastctrl.GetEmptyIndex();
            coastctrl.coast.characters[index] = charactrl;
            //人物
            charactrl.character.Coast = coastctrl;
            charactrl.character.Role.transform.parent = null;
            charactrl.character.IsOnBoat = false;
        }
        
        //下岸
        public void GetOffCoast(string passenger_name, CoastController coastctrl) {
            for (int i = 0; i < coastctrl.coast.characters.Length; ++i) {
                if (coastctrl.coast.characters[i] != null && 
                    coastctrl.coast.characters[i].character.Name == passenger_name) {
                    coastctrl.coast.characters[i] = null;
                }
            }
        }
    }
}


```
### 调整FirstController到Judge类
其次，理论上，同样的道理，因为FirstController需要管理的事情太多，所以需要将判断游戏输赢的check函数分离出来，交给单独的一个类管理，但在这里我并没有进行修改（其实修改也很简单，操作同之前的操作即可，只需创建一个新的裁判类Judge，将原代码中的check 函数更改为Judge中的函数即可），原因是我的check函数功能比较集中（没有分checkWin和checkLose而是一整个函数，要是单独出来，新类Judge也只有一个函数的功能，意义不大）。

### 游戏截图
最后出来的游戏截图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/2019100721051580.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0xMTF9TaWNpbHk=,size_16,color_FFFFFF,t_70)

完整代码请看我的Github


