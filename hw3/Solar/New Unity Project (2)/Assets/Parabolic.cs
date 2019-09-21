using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabolic : MonoBehaviour
{
    //初始化x方向速度为4(v = 4)，y为0,默认重力加速度为10
    private float xspeed = 4;
    private float yspeed = 0;
    private float g = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //此时y方向的速度(v = a * t)
        yspeed += g * Time.deltaTime;
        //用一个Vector3表示增加的position（s = v * t）
        Vector3 v = new Vector3(xspeed * Time.deltaTime, -yspeed * Time.deltaTime, 0);
        this.transform.Translate(v);
    }
}
