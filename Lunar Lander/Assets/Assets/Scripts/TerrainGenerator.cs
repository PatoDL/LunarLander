using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    float maxVertexAmount = 1000f;
    Bounds bounds;
    float seed;

    public static float minY;

    void Start()
    {
        bounds = CameraUtils.OrthographicBounds();

        minY = bounds.max.y;

        lineRenderer = GetComponent<LineRenderer>();

        List<Vector2> pointsPositions = new List<Vector2>(); 

        float offsetY = 0.2f;

        seed = Random.Range(0f, 9999f);

        lineRenderer.positionCount = (int)maxVertexAmount;

        EdgeCollider2D col = gameObject.GetComponent<EdgeCollider2D>();
        col.points = new Vector2[(int)maxVertexAmount];

        float timer = 0;

        for (float x = bounds.min.x, i = 0f; i < maxVertexAmount; x += bounds.max.x / (maxVertexAmount - 1), i++)
        {
            timer++;
            float y = ((Mathf.PerlinNoise(x, seed)) + offsetY) * bounds.max.y / 2;
            int timeLimit = 200;
            int waitTime = 20;
            if (timer == timeLimit-1)
                waitTime = Random.Range(20, 50);
            if (timer > timeLimit && timer <= timeLimit + waitTime && i < maxVertexAmount - 5)
            {
                y = lineRenderer.GetPosition((int)i - 1).y;

                if(timer==timeLimit+waitTime)
                    timer = 0;
            }
            if (y < minY)
                minY = y;
            lineRenderer.SetPosition((int)i, new Vector3(x, y));
            pointsPositions.Add(lineRenderer.GetPosition((int)i));
        }

        col.points = pointsPositions.ToArray();
    }
}
