using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public GameObject start_warp;
    public GameObject end_warp;
    List<Vector2> start_points = new List<Vector2>();
    List<Vector2> end_points = new List<Vector2>();

    void Start()
    {
        start_points.Add(new Vector2(0, -5)); //level 0 start coordinates
        end_points.Add(new Vector2(0, 5)); //level 0 end coordinates
        start_points.Add(new Vector2(0, 5)); //level 1 start coordinates
        end_points.Add(new Vector2(0, -6)); //level 1 end coordinates
        start_points.Add(new Vector2(0, 3)); //level 2 start coordinates
        end_points.Add(new Vector2(0, -4)); //level 2 end coordinates
        start_points.Add(new Vector2(0, 5)); //level 3 start coordinates
        end_points.Add(new Vector2(0, -5)); //level 3 end coordinates
    }

    public void load_level(int level)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.active)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            if (i == level + 1)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        //start_warp.transform.position = start_points[level];
        end_warp.transform.position = end_points[level];
    }
}
