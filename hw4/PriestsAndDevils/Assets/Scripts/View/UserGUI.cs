using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace {
    public class UserGUI : MonoBehaviour {
        private string guide =  "\n";
        private IUserAction action;
        private GUIStyle textStyle;
        private GUIStyle hintStyle;
        private GUIStyle btnStyle;
        public CharacterController characterCtrl;
        public int status;

	    // Use this for initialization
	    void Start () {
            status = 0;
            action = Director.GetInstance().CurrentSecnController as IUserAction;
        }
	
	    // Update is called once per frame
	    void OnGUI () {
            textStyle = new GUIStyle {
                fontSize = 35,
                alignment = TextAnchor.MiddleCenter,
            };
            hintStyle = new GUIStyle {
                fontSize = 20,
                fontStyle = FontStyle.Normal
            };
            btnStyle = new GUIStyle("button") {
                fontSize = 20
            };
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 150, 100, 50), 
                "牧师和魔鬼", textStyle);
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 120, 100, 50), 
                "金色: 牧师\n银色: 魔鬼\n\n" + guide, hintStyle);
            if (status == 1) {
                // GameOver
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "游戏结束!", textStyle);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "重新开始", btnStyle)) {
                    status = 0;
                    action.Restart();
                }
            } else if (status == 2) {
                // Win
                GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "恭喜！你赢了!", textStyle);
                if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "重新开始", btnStyle)) {
                    status = 0;
                    action.Restart();
                }
            }
	    }

        public void SetCharacterCtrl(CharacterController _characterCtrl) {
            characterCtrl = _characterCtrl;
        }

        void OnMouseDown() {
            if (gameObject.name == "boat") {
                action.MoveBoat();
            } else {
                action.CharacterClicked(characterCtrl);
            }
        }
    }
}
