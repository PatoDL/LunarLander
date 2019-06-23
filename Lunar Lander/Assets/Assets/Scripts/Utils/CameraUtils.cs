using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils
{
    public static Bounds OrthographicBoundsForRocket()
    {
        Camera camera = CameraManager.GetActiveCamera();

        float screenwidthLimited = Screen.width;
        float screenheightLimited = Screen.height / 3.3f;
        float screenAspect = screenwidthLimited / screenheightLimited;
        float cameraHeight = camera.orthographicSize * 1.5f;

        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        return bounds;
    }

    public static Bounds OrthographicBounds()
    {
        Camera camera = CameraManager.GetActiveCamera();

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;

        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        return bounds;
    }
}