using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScript : MonoBehaviour
{
    public Canvas canvas;
    private Texture2D tex;
    public GameObject image;

    public Dictionary<Vector2, Pixel> pixelGrid = new Dictionary<Vector2, Pixel>();
    public Color customColor;
    public Color backgroundColor;
    public PixelTypes chosenPixelType;
    public PixelTypes redPixelType;
    public PixelTypes otherPixelType;

    public int brushSize;
    public bool update;
    public bool switchType;

    void Start()
    {
        otherPixelType = new PixelTypes();
        redPixelType = new PixelTypes();
        redPixelType.color = Color.red;
        chosenPixelType = redPixelType;
        InitTexture();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 mousePos = Input.mousePosition;
            {
                //ReadPixel((int)mousePos.x, (int)mousePos.y);
                DrawPixel((int)mousePos.x, (int)mousePos.y, chosenPixelType, true, brushSize);
                Debug.Log(mousePos.x); Debug.Log(mousePos.y);
            }
        }
        if (switchType)
        {
            switchType = false;
            otherPixelType.color = customColor;
            if(chosenPixelType == redPixelType)
            {
                chosenPixelType = otherPixelType;
            }
            else
            {
                chosenPixelType = redPixelType;
            }
        }
    }

    private void FixedUpdate()
    {
        if (update)
        {
            UpdateGrid();
        }
    }

    private void InitTexture()
    {
        tex = new Texture2D((int)canvas.pixelRect.width, (int)canvas.pixelRect.height, TextureFormat.RGB24, false);
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                    tex.SetPixel(x,y,backgroundColor);
            }
        }
        tex.Apply();

        // Assign the texture to the RawImage component
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(tex.width, tex.height);
        image.GetComponent<RawImage>().texture = tex;
    }

    public Color ReadPixel(int x, int y)
    {
        return tex.GetPixel(x, y);
    }

    
    public void DrawPixel(int x, int y, PixelTypes pixType, bool apply, int size) //Kinda Simple rn but will add things like size so will be usefull
    {
        if (size != 1)
        {
            for (int i = -(size); i < (size); i++)
            {
                for (int v = -(size); v < (size); v++)
                {
                    AddToPixelGrid(x + i, y + v, pixType);
                   
                }
            }
        }
        else
        {
            AddToPixelGrid(x, y, pixType);
        }
        if (apply)
        {
            tex.Apply();
        }
    }

    public void AddToPixelGrid(int x, int y, PixelTypes pixType)
    {
        if (!IsInBounds(x, y)) { return; }
        var coord = new Vector2(x, y);
        if (!pixelGrid.ContainsKey(coord)){
            var pix = new Pixel();
            pix.type = pixType;
            pixelGrid.Add(coord, pix);
        }
        else
        {
            var pix = pixelGrid[coord];
            pix.type = pixType;
            pixelGrid[coord] = pix;
        }
        tex.SetPixel(x, y, pixType.color);
    }

    public void RemoveFromPixelGrid(int x,int y)
    {
        pixelGrid.Remove(new Vector2(x,y));
        tex.SetPixel(x, y, backgroundColor);
    }

    public void UpdateGrid()
    {

        foreach (var pix in new Dictionary<Vector2, Pixel>(pixelGrid))
        {
            if (pix.Value.type != otherPixelType && (!pixelGrid.ContainsKey(new Vector2(pix.Key.x,pix.Key.y - 1) )&& pix.Key.y - 1 < tex.height))
            {
                    RemoveFromPixelGrid((int)pix.Key.x, (int)pix.Key.y);
            }
        }
        
        tex.Apply();
    }

    public bool IsInBounds(int x, int y)
    {
        return (y > -1 && y < tex.height && x > -1 && x < tex.width);
    }
    public bool IsInBounds(Vector2 coords)
    {
        int y = (int) coords.y;
        int x = (int)coords.x;
        return (y > -1 && y < tex.height && x > -1 && x < tex.width);
    }
}
