using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraMovement : MonoBehaviour
{
    public GameObject rocket;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(rocket.transform.position.x,rocket.transform.position.y,transform.position.z);
        Debug.Log("xd");
    }
}
