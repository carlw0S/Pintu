using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public int size;
    public int maxSize = 10;
    public int minSize = 5;
    public Color color = Color.black;

    private Transform t;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        size = minSize;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = t.position.z;
        t.position = newPos;
    }



    public void ChangeColor(string c)
    {
        switch (c)
        {
            case "black":   color = Color.black;    break;
            case "red":     color = Color.red;      break;
            case "green":   color = Color.green;    break;
            case "blue":    color = Color.blue;     break;
            case "white":   color = Color.white;    break;
            default:        color = Color.black;    break;
        }
        sr.color = color;
    }

    public void IncreaseSize()
    {
        if (size < maxSize)
        {
            size++;
            t.localScale *= 1.125f;
        }
    }

    public void DecreaseSize()
    {
        if (size > minSize)
        {
            size--;
            t.localScale /= 1.125f;
        }
    }
}
