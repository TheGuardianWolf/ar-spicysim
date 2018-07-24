using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButtonControl : MonoBehaviour {

    public Button button0;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    private int value;

    // Use this for initialization
    void Start () {
        //this.transform.position = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x , 0.0f, Camera.main.transform.forward.z * 3.0f);

        //Vector3 desired_normal_y = transform.position - Camera.main.transform.position;
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        //this.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 2.5f);
        button0.onClick.AddListener(button0Listener);
        button1.onClick.AddListener(button1Listener);
        button2.onClick.AddListener(button2Listener);
        button3.onClick.AddListener(button3Listener);
        button4.onClick.AddListener(button4Listener);
        button5.onClick.AddListener(button5Listener);
        button6.onClick.AddListener(button6Listener);
        button7.onClick.AddListener(button7Listener);
        button8.onClick.AddListener(button8Listener);
        button9.onClick.AddListener(button9Listener);
    }

    void button0Listener()
    {
        value = 0;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button1Listener()
    {
        value = 1;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button2Listener()
    {
        value = 2;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button3Listener()
    {
        value = 3;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button4Listener()
    {
        value = 4;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button5Listener()
    {
        value = 5;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button6Listener()
    {
        value = 6;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button7Listener()
    {
        value = 7;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button8Listener()
    {
        value = 8;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
    void button9Listener()
    {
        value = 9;
        this.transform.root.BroadcastMessage("baseValue", value);
        Destroy(this.gameObject);
    }
}
