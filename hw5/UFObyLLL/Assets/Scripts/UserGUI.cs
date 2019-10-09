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
