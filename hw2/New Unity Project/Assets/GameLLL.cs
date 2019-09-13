using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLLL : MonoBehaviour
{
    //设置变量
    int turn = 1;  //用1和-1来表示回合的轮流
    int[,] state = new int[3, 3]; //用于表示棋盘在该处的状态，0为空，1为O，2为X

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //设置GUI
    //其中函数
    void OnGUI()
    {
        //设置颜色
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(30 / 256f, 93f / 256f, 124 / 256f);

        int result = Check();  //用于检查游戏结果

        //开始游戏
        if (GUI.Button(new Rect(Screen.width / 2 - 45, Screen.height / 2 + 130, 100, 50), "      开始游戏", style))
        {
            Reset();
        }
        //O方胜利
        if (result == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "O方胜利!", style);
        }
        //X方胜利
        else if (result == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "X方胜利!", style);
        }
        //平局
        else if (result == 3)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, Screen.height / 2 + 75, 100, 50), "  平局!", style);
        }

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                //O
                if (state[i, j] == 1)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "O");
                }
                //X  
                if (state[i, j] == 2)
                {
                    GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), "X");
                }
                //空
                if (GUI.Button(new Rect(Screen.width / 2 - 75 + 50 * i, Screen.height / 2 - 130 + 50 * j, 50, 50), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1)
                            state[i, j] = 1;
                        else
                            state[i, j] = 2;
                        turn *= -1;
                    }
                }
            }
        }
    }

    //重置函数Reset：
    void Reset()
    {
        turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                state[i, j] = 0;
            }
        }
    }

    //检查结果函数
    int Check()
    {
        //横向连成三个
        for (int i = 0; i < 3; ++i)
        {
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 0] == state[i, 2])
            {
                return state[i, 0];
            }
        }
        //竖向连成三个
        for (int i = 0; i < 3; ++i)
        {
            if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])
            {
                return state[0, i];
            }
        }
        //斜着连成三个-->必定包含正中间的那一个
        if (state[1, 1] != 0 && ((state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2]) ||
            (state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0])))
        {
            return state[1, 1];
        }

        // 平局-->全都有下（即不等于0），否则游戏继续
        int tied = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] == 0)
                {
                    tied = 0;
                }
            }
        }
        if (tied == 1)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }
}
