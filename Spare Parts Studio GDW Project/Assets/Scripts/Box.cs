using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject future_version;
    bool is_pushed;
    Vector3 start_location;
    Vector3 push_location;
    float push_speed;
    float push_timer;
    public LayerMask valid_collisions;
    public bool on_button = false;
    public Button held_button;
    Box other_pushed_box;
    public List<Vector2> start_positions = new List<Vector2>();

    void Start()
    {
        
    }

    void Update()
    {
        if (is_pushed)
        {
            transform.position = new Vector3(Mathf.Lerp(start_location.x, push_location.x, push_timer), Mathf.Lerp(start_location.y, push_location.y, push_timer), 0);
            push_timer += Time.deltaTime * push_speed;
            if (push_timer > 1)
            {
                is_pushed = false;
                transform.position = push_location;
            }
        }
    }

    public bool can_be_pushed(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, valid_collisions);
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Player"))
            {
                return false;
            }
            if (hit.transform.CompareTag("Box"))
            {
                other_pushed_box = hit.transform.gameObject.GetComponent<Box>();
                if (other_pushed_box.can_be_pushed(direction))
                {
                    return true;
                }
                else
                {
                    other_pushed_box = null;
                    return false;
                }
            }
            if (hit.transform.CompareTag("Button"))
            {
                held_button = hit.transform.gameObject.GetComponent<Button>();
                if (!on_button)
                {
                    held_button.hold();
                }
                on_button = true;
                return true;
            }
        }
        else
        {
            if (on_button)
            {
                held_button.release();
                on_button = false;
            }
        }
        return true;
    }

    public void push(Vector3 direction, float speed)
    {
        if (can_be_pushed(direction))
        {
            is_pushed = true;
            start_location = transform.position;
            push_timer = 0;
            push_location = direction + transform.position;
            push_speed = speed;
            if (future_version != null)
            {
                future_version.GetComponent<Box>().push(direction, speed);
            }
        }
        if (other_pushed_box != null)
        {
            other_pushed_box.push(direction, speed);
            other_pushed_box = null;
        }
    }

    public void set_position_to_beginning(int level)
    {
        transform.position = new Vector3(start_positions[level].x, start_positions[level].y, 0);
        if (on_button)
        {
            held_button.release();
            on_button = false;
        }
    }
}
