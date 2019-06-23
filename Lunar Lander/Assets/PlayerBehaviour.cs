using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    void Start()
    {
        GameObject g = new GameObject();
        g.AddComponent<SpriteRenderer>();
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.sprite = GetComponent<SpriteRenderer>().sprite;
        s.transform.localScale = transform.localScale;
        s.transform.position = transform.position + Vector3.right * 2f;
        
        List<Vector2> l = new List<Vector2>();
        g.AddComponent<EdgeCollider2D>();
        EdgeCollider2D ec = s.GetComponent<EdgeCollider2D>();
        
        ec.points = new Vector2[3];
        l.Add(new Vector2(1, -1));
        l.Add(new Vector2(1, 0));
        l.Add(new Vector2(1, 1));
        ec.points = l.ToArray();
        Instantiate(s);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right,1);
        Debug.DrawRay(transform.position, transform.right);
        if(raycast)
        {
            Debug.Log(raycast.transform.name);
        }
    }
}
