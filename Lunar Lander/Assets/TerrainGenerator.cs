﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    float maxVertexAmount = 1000f;
    Bounds bounds;
    float seed;

    void Start()
    {
        bounds = CameraUtils.OrthographicBounds();
        lineRenderer = GetComponent<LineRenderer>();

        List<Vector2> pointsPositions = new List<Vector2>(); 

        float offsetY = 0.2f;

        seed = Random.Range(0f, 9999f);

        lineRenderer.positionCount = (int)maxVertexAmount;

        EdgeCollider2D col = gameObject.GetComponent<EdgeCollider2D>();
        col.points = new Vector2[(int)maxVertexAmount];

        for (float x = bounds.min.x, i = 0f; i < maxVertexAmount; x += bounds.max.x / (maxVertexAmount - 1), i++)
        {            
            lineRenderer.SetPosition((int)i, new Vector3(x, ((Mathf.PerlinNoise(x, seed))+offsetY)*bounds.max.y/2));

            pointsPositions.Add(lineRenderer.GetPosition((int)i));
        }

        col.points = pointsPositions.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
