using UnityEngine;
using System.Collections;

public class MoveableCharacter : MonoBehaviour
{
    public Vector2 m_CurrentLocation;
    public Vector2 m_SpawnCell;
    public Vector2 m_Destination;

    //0(above) 1(below) 2(right) 3(left)
    public Vector2[] m_surroundingTilePositions = new Vector2[4];

    public float m_speed;
    public Vector3 m_direction;

    public bool m_isSelectable;
    public bool m_isSelected;

    public bool m_hasMoved;
    public bool m_hasAttacked;
    public bool m_moving;

    public bool m_movingUp;
    public bool m_movingDown;
    public bool m_movingLeft;
    public bool m_movingRight;

    //MonoBehavior
    //private void Start()
    //{
    //    MoveToSpawn();
    //}

    public void Initialize()
    {
        MoveToSpawn();
    }

    public void Update()
    {
        if (m_isSelected)
        {
            if (m_moving)
            {
                MoveToDestination();
            }

            else
            {
                UpdateDestination();
            }
        }
    }

    //Private functions
    private void MoveToSpawn()
    {
        print("Moving TO:" + m_SpawnCell);
        this.transform.position = Map.Instance.GetPosition(m_SpawnCell.x, m_SpawnCell.y);
        Map.Instance.SelectedTile = m_CurrentLocation = m_SpawnCell;
    }

    public void LightPath(Vector2 coord)
    {
        Map.Instance.MAP[(int)coord.x, (int)coord.y].m_lightUp = true;
        //Debug.Log(coord + " has been lit up.");

        if (coord.x < m_Destination.x)
        {
            coord.x++;
            LightPath(coord);
        }

        else if (coord.x > m_Destination.x)
        {
            coord.x--;
            LightPath(coord);
        }

        else if (coord.y < m_Destination.y)
        {
            coord.y++;
            LightPath(coord);
        }

        else if (coord.y > m_Destination.y)
        {
            coord.y--;
            LightPath(coord);
        }

        else
        {
            ToggleMoving(true);
            SelectionManager.Instance.log.AddEvent(this.gameObject.name + " moving to: " + m_Destination);
        }

    }

    private void ToggleMoving(bool x)
    {
        m_moving = x;
    }

    private void UpdateDestination()
    {
        m_Destination = Map.Instance.SelectedTile;
    }

    private void MoveToDestination()
    {
        m_direction = Vector3.zero;

        m_movingUp = m_movingDown = m_movingRight = m_movingLeft = false;

        if (m_CurrentLocation.x > m_Destination.x)
        {
            //Destination coordinate is higher on grid
            m_direction = Vector3.up;
            m_movingUp = true;
        }

        else if (m_CurrentLocation.x < m_Destination.x)
        {
            //Destination coordinate is lower on grid
            m_direction = Vector3.down;
            m_movingDown = true;
        }

        else if (m_CurrentLocation.y > m_Destination.y)
        {
            //Destination coordinate is to the left on grid
            m_direction = Vector3.left;
            m_movingLeft = true;
        }

        else if (m_CurrentLocation.y < m_Destination.y)
        {
            //Destination coordinate is to the right on grid
            m_direction = Vector3.right;
            m_movingRight = true;
        }

        this.transform.Translate(m_direction * m_speed * Time.deltaTime);

        UpdateLocation();

    }

    private void UpdateLocation()
    {
        GetCurrentTilePositions();
        //Update above;
        if (m_movingUp && this.transform.position.y >= m_surroundingTilePositions[0].y)
        {
            Map.Instance.MAP[(int)m_CurrentLocation.x, (int)m_CurrentLocation.y].m_lightUp = false;

            m_CurrentLocation.x--;
        }

        //Update below;
        if (m_movingDown && this.transform.position.y <= m_surroundingTilePositions[1].y)
        {
            Map.Instance.MAP[(int)m_CurrentLocation.x, (int)m_CurrentLocation.y].m_lightUp = false;
            m_CurrentLocation.x++;
        }

        //Update right;
        if (m_movingRight && this.transform.position.x >= m_surroundingTilePositions[2].x)
        {
            Map.Instance.MAP[(int)m_CurrentLocation.x, (int)m_CurrentLocation.y].m_lightUp = false;
            m_CurrentLocation.y++;
        }

        //Update left;
        if (m_movingLeft && this.transform.position.x <= m_surroundingTilePositions[3].x)
        {
            Map.Instance.MAP[(int)m_CurrentLocation.x, (int)m_CurrentLocation.y].m_lightUp = false;
            m_CurrentLocation.y--;
        }

        if (m_CurrentLocation == m_Destination)
        {
            //Unlight destination
            Map.Instance.MAP[(int)m_CurrentLocation.x, (int)m_CurrentLocation.y].m_lightUp = false;

            //disable move;
            ToggleMoving(false);

            //enable action select; (AFTER MOVEMENT)
            GameManager.Instance.GameState = (int)GameManager.GameStates.Action;

            //TEMPORARY::set gamestate to selecting;
            //GameManager.Instance.GameState = (int)GameManager.GameStates.Selecting;

            //deselect character
            m_isSelected = false;
            m_movingRight = m_movingLeft = m_movingUp = m_movingDown = false;
            //SelectionManager.Instance.DeSelectCharacter();

            //ARRIVED AT DESTINATION set hasmoved to tru
            m_hasMoved = true;
        }
    }

    private void GetCurrentTilePositions()
    {
        //0(above) 1(below) 2(right) 3(left)
        //This function checks the current Tiles surrounding pieces.
        if (m_CurrentLocation.x >= 1)
        {
            m_surroundingTilePositions[0] = Map.Instance.GetPosition(m_CurrentLocation.x - 1, m_CurrentLocation.y);
        }

        if (m_CurrentLocation.x < Map.Instance.Rows - 1)
        {
            m_surroundingTilePositions[1] = Map.Instance.GetPosition(m_CurrentLocation.x + 1, m_CurrentLocation.y);
        }

        if (m_CurrentLocation.y < Map.Instance.Columns - 1)
        {
            m_surroundingTilePositions[2] = Map.Instance.GetPosition(m_CurrentLocation.x, m_CurrentLocation.y + 1);
        }

        if (m_CurrentLocation.y >= 1)
        {
            m_surroundingTilePositions[3] = Map.Instance.GetPosition(m_CurrentLocation.x, m_CurrentLocation.y - 1);
        }
    }

}
