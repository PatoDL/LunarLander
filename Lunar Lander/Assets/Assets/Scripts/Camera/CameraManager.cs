using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject zoomedCam;

    static List<GameObject> cameras = new List<GameObject>();
    static GameObject activeCamera;

    void Start()
    {
        mainCam.SetActive(true);
        zoomedCam.SetActive(false);

        cameras.Add(mainCam);
        cameras.Add(zoomedCam);
    }

    static public void SwitchCamera()
    {
        foreach(GameObject c in cameras)
        {
            c.SetActive(!c.activeSelf);
        }
    }

    static public Camera GetActiveCamera()
    {
        foreach(GameObject c in cameras)
        {
            if(c.activeSelf)
            {
                activeCamera = c;
            }
        }
        return activeCamera.GetComponent<Camera>();
    }
}
