using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<Vector2> start_positions = new List<Vector2>();

    public void set_position_to_beginning(int level)
    {
        transform.position = new Vector3(start_positions[level].x, start_positions[level].y, 0);
    }
}
