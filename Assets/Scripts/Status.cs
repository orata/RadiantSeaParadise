using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour
{
    public int status;

    void Awake()
    {
        if (this.gameObject.name == "Rock(Clone)")
        {
            status = 0;
        }
        if (this.gameObject.name == "Scissors(Clone)")
        {
            status = 1;
        }
        if (this.gameObject.name == "Paper(Clone)")
        {
            status = 2;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
