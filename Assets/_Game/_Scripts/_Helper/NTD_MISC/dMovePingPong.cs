using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dMovePingPong : MonoBehaviour
{
    public GameObject ObjectMove;
    [SerializeField] [Range(0, 1)] float speed = 1f;
    [SerializeField] [Range(0, 100)] float range = 1f;

    void Start()
    {
        if(ObjectMove == null)
        {
            ObjectMove = this.gameObject;
        }
    }

    public void Update()
    {
        float yPos = Mathf.PingPong(Time.time * speed, 1) * range;
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
    }
}