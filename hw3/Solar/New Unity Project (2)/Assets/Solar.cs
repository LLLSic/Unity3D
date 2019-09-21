using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solar : MonoBehaviour
{
    //定义public的transform变量，用于之后把各行星与此transform类进行对应
    public Transform Sun;  //太阳
    public Transform Mercury;  //水星
    public Transform Venus;  //金星
    public Transform Earth;  //地球
    public Transform Mars;  //火星
    public Transform Jupiter;  //木星
    public Transform Saturn;  //土星
    public Transform Uranus;  //天王星
    public Transform Neptune;  //海王星

    // Start is called before the first frame update
    void Start()
    {
        //在Start函数里面对各行星的位置进行初始化(大致参照实际远近)
        Sun.position = new Vector3(0, 0, 0);
        Mercury.position = new Vector3(2, 0, 0);
        Venus.position = new Vector3(-3, 0, 0);
        Earth.position = new Vector3(4, 0, 0);
        Mars.position = new Vector3(-5, 0, 0);
        Jupiter.position = new Vector3(6, 0, 0);
        Saturn.position = new Vector3(-7, 0, 0);
        Uranus.position = new Vector3(8, 0, 0);
        Neptune.position = new Vector3(-9, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //RotateAround函数表示公转
        //Rotate函数表示自转
        Mercury.RotateAround(Sun.position, new Vector3(0, 3, 1), 15 * Time.deltaTime);
        Mercury.Rotate(new Vector3(0, 5, 1) * 5 * Time.deltaTime);

        Venus.RotateAround(Sun.position, new Vector3(0, 2, 1), 13 * Time.deltaTime);
        Venus.Rotate(new Vector3(0, 2, 1) * Time.deltaTime);

        Earth.RotateAround(Sun.position, Vector3.up, 12 * Time.deltaTime);
        Earth.Rotate(Vector3.up * 30 * Time.deltaTime);

        Mars.RotateAround(Sun.position, new Vector3(0, 13, 5), 11 * Time.deltaTime);
        Mars.Rotate(new Vector3(0, 12, 5) * 40 * Time.deltaTime);

        Jupiter.RotateAround(Sun.position, new Vector3(0, 8, 3), 10 * Time.deltaTime);
        Jupiter.Rotate(new Vector3(0, 10, 3) * 30 * Time.deltaTime);

        Saturn.RotateAround(Sun.position, new Vector3(0, 2, 1), 9 * Time.deltaTime);
        Saturn.Rotate(new Vector3(0, 3, 1) * 20 * Time.deltaTime);

        Uranus.RotateAround(Sun.position, new Vector3(0, 9, 1), 7 * Time.deltaTime);
        Uranus.Rotate(new Vector3(0, 10, 1) * 20 * Time.deltaTime);

        Neptune.RotateAround(Sun.position, new Vector3(0, 7, 1), 5 * Time.deltaTime);
        Neptune.Rotate(new Vector3(0, 8, 1) * 30 * Time.deltaTime);

    }
}

