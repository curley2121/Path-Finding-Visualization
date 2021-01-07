using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{
    public static bool[,] grid;
    public int width = 30;
    public int height = 20;
    public static int w;
    public static int h;
    public static GameObject[,] nodes;
    public GameObject node;
    public static int editing;
    public static int startx, starty, endx, endy;
    public int mode = 0;
    public Dropdown modeDrop, speedDrop;
    public static LinkedList<int>[] graph;
    public Text stepCounter;
    public static int step, lastStep;
    public Button runButton;
    public float stepSpeed = (float)1;
    float startTime;
    public static List<int>  visited, unvisited, finalPath;
    public static int[] shortestFromStart, prevVertex, toGoal;
    public int nextVertex;
    public static int visiting = -1;
    public static LinkedList<int> queue;
    bool noSpeed = true;
    public static List<KeyValuePair<int,int>> pQueue;
    int screenW = 0, ScreenH = 0;
    public GameObject editPanel, runPanel;
    public static int down = 0;
    
    






    // Start is called before the first frame update
    void Start()
    {
        down = 0;


        visited = new List<int>();
        finalPath = new List<int>();
        unvisited = new List<int>();
        queue = new LinkedList<int>();
        pQueue = new List<KeyValuePair<int, int>>();
        
        

        editing = 1;
        


        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0) == false)
        {
            down = 0;
        }
        if (modeDrop.transform.childCount == 5)
        {
            down = 3;
        }
        
        editPanel.SetActive((editing == 1));
        runPanel.SetActive((editing != 1));

        if (speedDrop.value == 0)
        {
            noSpeed = true;
        }
        else if (speedDrop.value == 1)
        {
            noSpeed = false;
            stepSpeed = (float).1;
        }
        else if (speedDrop.value == 2)
        {
            noSpeed = false;
            stepSpeed = (float)1;
        }

        
        if (Screen.width != screenW || Screen.height -100 != ScreenH)
        {
            screenW = Screen.width;
            ScreenH = Screen.height - 100;
            createNodes();
        }
        if (editing == 0)
        {
            stepCounter.text = modeDrop.options[modeDrop.value].text + " Algorithm      Iteration: " + step ;
        }
        else
        {
            stepCounter.text = modeDrop.options[modeDrop.value].text + " Algorithm      Iteration: " + step + "      Path Length: " + finalPath.Count;
        }
        
        w = width;
        h = height;
        mode = modeDrop.value;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.localPosition);
        if (Input.GetMouseButtonDown(0))
        {
            
            print(screenPosition - Input.mousePosition);
            
        }

        if (editing == 1)
        {
            visiting = -1;
            runButton.GetComponentInChildren<Text>().text = "Run";
        }
        else
        {
            
            runButton.GetComponentInChildren<Text>().text = "Stop";
        }
        if (editing == 0 && (noSpeed || Time.time- startTime > step*stepSpeed))
        {
            step += 1;
        }
        if (step > lastStep)
        {
            stepThruAlgorithm();
            lastStep = step;
        }


    }

    void stepThruAlgorithm()
    {
        if (mode == 0)
        {
            dijkstraVisit(nextVertex);
        }
        else if (mode == 1)
        {
            aStar(nextVertex);
        }
        else if (mode == 2)
        {
            BFSvisit();
        }
        else if (mode == 3)
        {
            DFSVisit(nextVertex);
        }
        else if (mode == 4)
        {
            BestFirstSearch();
        }
    }

    
    void createNodes()
    {
        editing = 1;
        finalPath.Clear();
        visited.Clear();
        unvisited.Clear();

        Camera.main.rect = new Rect(0,0,1, (Screen.height - 100) / (float)Screen.height);
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        width = Camera.main.pixelWidth/64 ;
        height = Camera.main.pixelHeight / 64 ;
        Camera.main.orthographicSize = height / (float)4;

        nodes = new GameObject[width, height];
        shortestFromStart = new int[width * height];
        toGoal = new int[width * height];
        prevVertex = new int[width * height];

        transform.localScale = new Vector3(width, height, 1);
        grid = new bool[width, height];

        startx = width/5;
        starty = height/2;
        endx = width * 4 / 5;
        endy = height/2;

        float w = 1 / (float)width;
        float h = 1 / (float)height;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                GameObject current = Instantiate(node, new Vector3(0, 0, 0), Quaternion.identity, transform);
                current.transform.localPosition = new Vector3((i/(float)width/2) + (w/4)-(float).25, j / (float)height/2 + (h / 4) - (float).25, 0);
                current.transform.localScale = new Vector3(w, h, 1);
                current.GetComponent<nodeScript>().x = i;
                current.GetComponent<nodeScript>().y = j;
            }

        }
    }

    public void pushRun()
    {
        if (editing == 1)
        {
            buildGraphFromGrid();
            editing = 0;
            startTime = Time.time;
            step = 0;
            lastStep = -1;
            visited.Clear();
            for (int i = 0; i < height * width; i++)
            {
                unvisited.Add(i);
            }

            for (int i = 0; i < shortestFromStart.Length; i++)
            {
                shortestFromStart[i] = 1000000;
            }
            for (int i = 0; i < toGoal.Length; i++)
            {
                int x = i % width;
                int y = i / width;
                toGoal[i] = (int)Vector2.Distance(new Vector2(x,y), new Vector2(endx,endy));
            }
            nextVertex = (startx + (starty * width));
            shortestFromStart[nextVertex] = 0;

            if (mode == 2)
            {
                queue.Clear();

                queue.AddFirst(startx + (starty * width));
                visited.Add(startx + (starty * width));
            }
            if (mode == 4)
            {

                pQueue.Clear();
                pQueue.Add(new KeyValuePair<int, int>(Mathf.Abs(startx - endx) + Mathf.Abs(starty - endy), startx + (starty * width)));
                //visited.Add(startx + (starty * width));

            }



        }
        else 
        {
            
            editing = 1;
            finalPath.Clear();
            visited.Clear();
            unvisited.Clear();
        }
        






    }
    public void dijkstraVisit(int v)
    {
        visited.Add(v);
        unvisited.Remove(v);
        LinkedListNode<int> node = graph[v].First;
        int nextShortest = 10000;
        int nextV = -1;
        for (int i = 0; i < graph[v].Count; i++)
        {
            if (shortestFromStart[v] + 1 < shortestFromStart[node.Value])
            {
                shortestFromStart[node.Value] = shortestFromStart[v] + 1;
                prevVertex[node.Value] = v;
            }
            
            
            node = node.Next;
        }

        for (int i = 0; i < unvisited.Count; i++)
        {
            if (shortestFromStart[ unvisited[i]] < nextShortest)
            {
                nextV = unvisited[i];
                nextShortest = shortestFromStart[unvisited[i]];
            }
        }

        if (nextV == -1)
        {
            editing = 3;
        }

        nextVertex = nextV;
        visiting = nextVertex;
        
        if (v == (endx + (endy * width)))
        {
            editing = 2;
            visiting = -1;

            while(v != (startx + (starty * width)))
            {
                finalPath.Add(v);
                v = prevVertex[v];
            }
            

        }
    }


    public void DFSVisit(int v)
    {
        visited.Add(v);
        
        LinkedListNode<int> node = graph[v].First;
                
        for (int i = 0; i < graph[v].Count; i++)
        {
            
            queue.AddFirst(node.Value);
            node = node.Next;
        }

        

        nextVertex = queue.First.Value;

        while (visited.Contains(nextVertex))
        {
            queue.RemoveFirst();
            nextVertex = queue.First.Value;
        }
        prevVertex[nextVertex] = v;
        visiting = nextVertex;

        if (v == (endx + (endy * width)))
        {
            editing = 2;
            visiting = -1;

            while (v != (startx + (starty * width)))
            {
                finalPath.Add(v);
                print(v);
                v = prevVertex[v];
            }


        }
    }

    public void BestFirstSearch()
    {
        if (pQueue.Count > 0)
        {
            int s = pQueue[0].Value;
            pQueue.RemoveAt(0);
            visiting = s;

            if (s == (endx + (endy * width)))
            {
                editing = 2;
                visiting = -1;

                while (s != (startx + (starty * width)))
                {
                    finalPath.Add(s);
                    s = prevVertex[s];
                }
            }
            else
            {
                LinkedListNode<int> node = graph[s].First;

                for (int i = 0; i < graph[s].Count; i++)
                {
                    if (!visited.Contains(node.Value))
                    {
                        
                        visited.Add(node.Value);
                        pQueue.Add( new KeyValuePair<int,int>(toGoal[node.Value], node.Value));
                        pQueue.Sort(compareByKey );
                        prevVertex[node.Value] = s;
                    }

                    node = node.Next;
                }
            }

            

            
        }
        else
        {
            print("done");
            editing = 2;
            visiting = -1;
        }
    }



    public void BFSvisit()
    {
        if (queue.Count > 0)
        {
            int s = queue.First.Value;
            queue.RemoveFirst();

            LinkedListNode<int> node = graph[s].First;

            for (int i = 0; i < graph[s].Count; i++)
            {
                if (!visited.Contains(node.Value))
                {
                    visiting = node.Value;
                    visited.Add(node.Value);
                    queue.AddLast(node.Value);
                    prevVertex[visiting] = s;
                }

                node = node.Next;
            }

            if (s == (endx + (endy * width)))
            {
                editing = 2;
                visiting = -1;

                while (s != (startx + (starty * width)))
                {
                    finalPath.Add(s);
                    s = prevVertex[s];
                }
            }
        }
        else
        {
            editing = 2;
            visiting = -1;
        }
    }


    public void aStar(int v)
    {
        visited.Add(v);
        unvisited.Remove(v);
        LinkedListNode<int> node = graph[v].First;
        int nextShortest = 10000;
        int nextV = -1;
        for (int i = 0; i < graph[v].Count; i++)
        {
            if (shortestFromStart[v] + 1 < shortestFromStart[node.Value])
            {
                shortestFromStart[node.Value] = shortestFromStart[v] + 1;
                prevVertex[node.Value] = v;
            }


            node = node.Next;
        }

        for (int i = 0; i < unvisited.Count; i++)
        {
            if (shortestFromStart[unvisited[i]] + toGoal[unvisited[i]] < nextShortest)
            {
                nextV = unvisited[i];
                nextShortest = shortestFromStart[unvisited[i]] + toGoal[unvisited[i]];
            }
        }

        if (nextV == -1)
        {
            editing = 3;
        }

        nextVertex = nextV;
        visiting = nextVertex;

        if (v == (endx + (endy * width)))
        {
            editing = 2;
            visiting = -1;

            while (v != (startx + (starty * width)))
            {
                finalPath.Add(v);
                v = prevVertex[v];
            }


        }
    }

    public void buildGraphFromGrid()
    {
        int size = width * height;
        graph = new LinkedList<int>[size];

        grid[startx, starty] = false;
        grid[endx, endy] = false;

        for (int k = 0; k < size; k++)
        {
            graph[k] = new LinkedList<int>();

            int x = k % width;
            int y = k / width;

            addEdgeFromGrid(graph, x, y, x, y + 1);
            addEdgeFromGrid(graph, x, y, x + 1, y);
            
            addEdgeFromGrid(graph, x, y, x, y - 1);
            addEdgeFromGrid(graph, x, y, x - 1, y);



        }

    }

    


    public void pushClear()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = false;
            }
        }
    }


    void addEdgeFromGrid(LinkedList<int>[] graph, int x, int y, int i, int j)
    {
        if (i >= 0 && i < width && j >= 0 && j < height)
        {
            if (!grid[x, y] && !grid[i, j] && !(x == i && y == j))
            {
                graph[(x + (y * main.w))].AddLast((i + (j * main.w)));
            }
        }
        
    }

    int compareByKey(KeyValuePair<int,int> x, KeyValuePair<int, int> y)
    {
        if (x.Key > y.Key)
        {
            return 1;
        }else
        if (x.Key < y.Key)
        {
            return -1;
        }
        else
        {
            return 0;
        }

       
    }
    
}
