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

