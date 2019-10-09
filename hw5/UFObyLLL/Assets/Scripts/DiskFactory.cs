using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    //used和free分别表示已经在使用的和未被利用的飞碟对象
    public List<GameObject> used = new List<GameObject>();
    public List<GameObject> free = new List<GameObject>();

	//初始化
	void Start () { }

    public void GenDisk()
    {
        GameObject disk;
        //如果free的列表为空，而我们有需要它，这时才创建新的对象
        if(free.Count == 0)
        {
            disk = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Disk"), Vector3.zero, Quaternion.identity);
        }
        //不然，就用之前的
        else
        {
            disk = free[0];
            free.RemoveAt(0);
        }
        float x = Random.Range(-10.0f, 10.0f);
        disk.transform.position = new Vector3(x, 0, 0);
        disk.transform.Rotate(new Vector3(x < 0? -x*9 : x*9, 0, 0));
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        disk.transform.GetComponent<Renderer>().material.color = color;
        used.Add(disk);
    }
    public void RecycleDisk(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        free.Add(obj);
    }
}