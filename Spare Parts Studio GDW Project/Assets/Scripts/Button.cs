using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject attached_door;
    public int held_down = 0;
    public List<Vector2> start_positions = new List<Vector2>();

    public void hold()
    {
        held_down++;
        check_held_status();
    }

    public void release()
    {
        held_down--;
        check_held_status();
    }

    void check_held_status()
    {
        if (held_down > 0)
        {
            attached_door.SetActive(false);
        }
        else
        {
            attached_door.SetActive(true);
        }
    }

    public void set_position_to_beginning(int level)
    {
        transform.position = new Vector3(start_positions[level].x, start_positions[level].y, 0);
    }
}
