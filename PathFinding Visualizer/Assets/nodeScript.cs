using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeScript : MonoBehaviour
{
    public Color normal, hover, selected, overselected, start, goal, path, visited;
    SpriteRenderer spr;
    public bool wall = false;
    bool over = false;
    public int x, y;
    public float scale = (float).85;
    public float fade = 0;
    Color lastColor, currentColor;


    void Start()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
    }
    

    // Update is called once per frame
    void Update()
    {
        
        float w = 1 / (float)main.w;
        float h = 1 / (float)main.h;

        transform.localScale = new Vector3(w * scale, h * scale, 1);

        if (scale > (float).85)
        {
            scale = scale - (float).005;
        }
        if (main.visiting == (x + (y * main.w)))
        {
            fade = 1;
            scale = 1;
        }

        if (fade > 0)
        {
            fade -= (float).05;
        }

        wall = main.grid[x,y];
        if (main.editing == 1)
        {
            if (wall)
            {
                if (over)
                {
                    currentColor = overselected;
                }
                else
                {
                    currentColor = selected;
                }

            }
            else
            {
                if (over)
                {
                    currentColor = hover;
                }
                else
                {
                    currentColor = normal;
                }
            }
        }
        else
        {
            if (main.visited.Contains(x + (y * main.w)))
            {
                currentColor = visited;
            }
            if (main.finalPath.Contains(x + (y * main.w)))
            {
                currentColor = path;
            }
        }

        spr.color = Color.Lerp(currentColor, hover, fade) ;




       if (x == main.startx && y == main.starty)
        {
            spr.color = start;
        }else
        if (x == main.endx && y == main.endy)
        {
            spr.color = goal;
        }


       
    }
    void OnMouseOver()
    {
        scale = 1;
        over = true;
        fade = 1;
        if (Input.GetMouseButton(0) && main.down == 0)
        {
            if (main.startx == x && main.starty == y)
            {
                main.down = 4;
            }else
            if (main.endx == x && main.endy == y)
            {
                main.down = 5;
            }
            else
            if (main.grid[x, y] == true)
            {
                main.down = 1;
            }
            else
            {
                main.down = 2;
            }

        }
        if (Input.GetMouseButton(0) && main.down == 1)
        {
     
            main.grid[x, y] = false;
        }
        else if (main.down == 2)
        {        
            main.grid[x, y] = true;
            
        }
        else if (main.down == 4)
        {
            main.startx = x;
            main.starty = y;

        }
        else if (main.down == 5)
        {
            main.endx = x;
            main.endy = y;

        }


        
    }
    void OnMouseExit()
    {
        over = false;
        
    }

}
