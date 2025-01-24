using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public bool is_past;
    public GameObject other_player;
    public int level = 0;
    [SerializeField] KeyCode up_key;
    [SerializeField] KeyCode left_key;
    [SerializeField] KeyCode down_key;
    [SerializeField] KeyCode right_key;
    [SerializeField] KeyCode reset_key;
    public Levels level_manager;
    public LayerMask valid_collisions;
    bool is_moving;
    float movement_timer;
    Vector3 original_location;
    Vector3 destination_location;
    float movement_speed = 10;
    Button held_button;
    public bool waiting_at_end = false;
    bool on_button = false;
    List<Vector2> start_positions = new List<Vector2>();

    void Start()
    {
        start_positions.Add(new Vector2(0, -4)); //level 0 coordinates
        start_positions.Add(new Vector2(0, 5)); //level 1 coordinates
        start_positions.Add(new Vector2(0, 4)); //level 2 coordinates
        start_positions.Add(new Vector2(0, 5)); //level 3 coordinates
        set_position_to_beginning();
    }

    void Update()
    {
        if (Input.GetKeyDown(up_key)) {
            move("up");
        }
        if (Input.GetKeyDown(down_key))
        {
            move("down");
        }
        if (Input.GetKeyDown(left_key))
        {
            move("left");
        }
        if (Input.GetKeyDown(right_key))
        {
            move("right");
        }
        if (Input.GetKeyDown(reset_key))
        {
            set_position_to_beginning();
        }

        if (is_moving)
        {
            transform.position = new Vector3(Mathf.Lerp(original_location.x, destination_location.x, movement_timer), Mathf.Lerp(original_location.y, destination_location.y, movement_timer), 0);
            movement_timer += Time.deltaTime * movement_speed;
            if (movement_timer > 1) {
                is_moving = false;
                transform.position = destination_location;
                if (waiting_at_end && other_player.GetComponent<Player_Movement>().waiting_at_end)
                {
                    level++;
                    set_position_to_beginning();
                    other_player.GetComponent<Player_Movement>().level++;
                    other_player.GetComponent<Player_Movement>().set_position_to_beginning();
                }
            }
        }
    }

    void move(string direction)
    {
        if (!is_moving)
        {
            waiting_at_end = false;
            Vector3 movement_direction = Vector3.zero;
            if (direction == "up")
            {
                movement_direction = Vector3.up;
            }
            else if (direction == "down")
            {
                movement_direction = Vector3.down;
            }
            else if (direction == "left")
            {
                movement_direction = Vector3.left;
            }
            else if (direction == "right")
            {
                movement_direction = Vector3.right;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, movement_direction, 1, valid_collisions);
            if (hit.collider != null)
            {
                if (hit.transform.CompareTag("Wall"))
                {
                    movement_direction = Vector3.zero;
                }
                if (hit.transform.CompareTag("Box"))
                {
                    var box_script = hit.transform.gameObject.GetComponent<Box>();
                    if (box_script.on_button)
                    {
                        on_button = true;
                        held_button = box_script.held_button;
                        held_button.hold();
                    }
                    if (box_script.can_be_pushed(movement_direction))
                    {
                        box_script.push(movement_direction, movement_speed);
                    }
                    else
                    {
                        movement_direction = Vector3.zero;
                    }
                }
                if (hit.transform.CompareTag("Button"))
                {
                    on_button = true;
                    held_button = hit.transform.gameObject.GetComponent<Button>();
                    held_button.hold();
                }
                if (hit.transform.CompareTag("End"))
                {
                    waiting_at_end = true;
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
            is_moving = true;
            movement_timer = 0;
            original_location = transform.position;
            destination_location = transform.position + movement_direction;
        }
    }

    public void set_position_to_beginning()
    {
        level_manager.load_level(level);
        transform.position = new Vector3(start_positions[level].x, start_positions[level].y, 0);
        Transform element = this.transform.parent.Find("Puzzle_Elements");
        Transform boxes = element.Find("Boxes");
        for (int i = 0; i < boxes.childCount; i++)
        {
            boxes.GetChild(i).GetComponent<Box>().set_position_to_beginning(level);
        }
        element.Find("Button").GetComponent<Button>().set_position_to_beginning(level);
        element.Find("Door").GetComponent<Door>().set_position_to_beginning(level);
    }
}
