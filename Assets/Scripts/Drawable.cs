using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawable : MonoBehaviour
{
    public int pencilSize = 10;
    public int maxPencilSize = 30;
    public Color color = Color.black;



    private Transform spriteTransform;
    private SpriteRenderer spriteRenderer;
    private Texture2D texture;
    private int previousX, previousY;



    void Start()
    {
        spriteTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        texture = spriteRenderer.sprite.texture;
        ClearCanvas();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            if (pencilSize < maxPencilSize)
            {
                ++pencilSize;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            if (pencilSize > 1)
            {
                --pencilSize;
            }
        }

        string key = Input.inputString;
        switch (key)
        {
            case "0":   color = Color.white;    pencilSize = 30;    break;
            case "1":   color = Color.black;    pencilSize = 10;    break;
            case "2":   color = Color.red;      pencilSize = 10;    break;
            case "3":   color = Color.green;    pencilSize = 10;    break;
            case "4":   color = Color.blue;     pencilSize = 10;    break;
            default: break;
        }
    }

    void FixedUpdate()      // Para que no dependa de los frames renderizados (con VSync iba peor, por ejemplo)
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
 
            // if (hit.collider != null && hit.collider.tag == "Canvas")    // Esto no funca al hacer una build :(
            if (hit.collider != null)
            {
                Vector2 point = hit.point;

                int currentX = (texture.width / 2) + (int) (point.x * 100) - (int) (spriteTransform.position.x * 100);
                int currentY = (texture.height / 2) + (int) (point.y * 100) - (int) (spriteTransform.position.y * 100);



                if (previousX != -1)
                {
                    int xInc = (int) Mathf.Sign(currentX - previousX);
                    int yInc = (int) Mathf.Sign(currentY - previousY);

                    int xDistance = Mathf.Abs(currentX - previousX);
                    int yDistance = Mathf.Abs(currentY - previousY);                        

                    if (xDistance > yDistance)
                    {
                        float floatY = 0.0f;
                        for (int x = previousX; x != currentX; x += xInc)
                        {
                            floatY += (float) yDistance / xDistance;
                            int y = previousY + (int) floatY * yInc;
                            DrawPoint(x, y);
                        }
                        Debug.Log(floatY);
                    }
                    else
                    {
                        float floatX = 0.0f;
                        for (int y = previousY; y != currentY; y += yInc)
                        {
                            floatX += (float) xDistance / yDistance;
                            int x = previousX + (int) floatX * xInc;
                            DrawPoint(x, y);
                        }
                    }
                }
                DrawPoint(currentX, currentY);



                previousX = currentX;
                previousY = currentY;

                Debug.Log(point);
            }
            else
            {
                Debug.Log("¿Sabes pintar sin salirte de las líneas?");
                previousX = -1;
            }
        }
        else
        {
            previousX = -1;
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearCanvas();
        }



        texture.Apply();    // ESTO MEJOR SOLO AQUÍ PARA NO HACER MÁS LLAMADAS DE LA CUENTA
    }



    void ClearCanvas()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
    }

    void DrawPoint(int x, int y)
    {
        for (int i = y - pencilSize/2; i <= y + pencilSize/2; ++i)
        {
            if (i >= 0 && i <= texture.height)
            {
                for (int j = x - pencilSize/2; j <= x + pencilSize/2; ++j)
                {
                    if (j >= 0 && j <= texture.width)
                    {
                        float distanceFromCenterPixel = (float) (Mathf.Abs(y - i) + Mathf.Abs(x - j)) / pencilSize;
                        if (distanceFromCenterPixel < 0.75f)
                        {
                            texture.SetPixel(j, i, color);
                        }
                    }
                }
            }
        }
    }
}
