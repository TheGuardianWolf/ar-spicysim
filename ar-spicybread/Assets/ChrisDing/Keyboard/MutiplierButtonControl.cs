using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutiplierButtonControl : MonoBehaviour
{
    public Button buttonDecimal;
    public Button button1;
    public Button button10;
    public Button button100;
    public Button button1K;
    public Button button10K;
    public Button button100K;
    public Button button1M;
    public Button button10M;
    public Button button100M;

    // Use this for initialization
    void Start()
    {
        //this.transform.position = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z) * 2.5f;

        //Quaternion qr = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        //Vector3 er = transform.eulerAngles;
        //transform.rotation = Quaternion.Euler(er.x, qr.eulerAngles.y ,er.z);
        //new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 2.5f);


        buttonDecimal.onClick.AddListener(buttonDecimalListener);
        button1.onClick.AddListener(button1Listener);
        button10.onClick.AddListener(button10Listener);
        button100.onClick.AddListener(button100Listener);
        button1K.onClick.AddListener(button1KListener);
        button10K.onClick.AddListener(button10KListener);
        button100K.onClick.AddListener(button100KListener);
        button1M.onClick.AddListener(button1MListener);
        button10M.onClick.AddListener(button10MListener);
        button100M.onClick.AddListener(button100MListener);
    }

    void buttonDecimalListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", (double) 0.1);
        Destroy(this.gameObject);
    }

    void button1Listener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 1);
        Destroy(this.gameObject);
    }
    void button10Listener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 10);
        Destroy(this.gameObject);
    }
    void button100Listener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 100);
        Destroy(this.gameObject);
    }
    void button1KListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 1000);
        Destroy(this.gameObject);
    }
    void button10KListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 10000);
        Destroy(this.gameObject);
    }
    void button100KListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 100000);
        Destroy(this.gameObject);
    }
    void button1MListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 1000000);
        Destroy(this.gameObject);
    }
    void button10MListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 10000000);
        Destroy(this.gameObject);
    }
    void button100MListener()
    {
        this.transform.root.BroadcastMessage("multiplierValue", 100000000);
        Destroy(this.gameObject);
    }
}
