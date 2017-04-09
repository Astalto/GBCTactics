using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used by every piece that is on the map, it has some booleans to determine which state it is currently in
/// and different colors to set for different states.
/// </summary>

public class MapPiece : MonoBehaviour
{
    public bool m_isSelected;
    public bool m_lightUp;

    public Color m_deselectedColor;
    public Color m_selectedColor;
    public Color m_pathingColor;
    public Color m_playerSelectedColor;

    public bool m_isOccupied;
    public MoveableCharacter m_occupiedBy;
    

    //Use later in the project (possibly to represent wether the player can move a unit or not);
    //public Vector4 m_movableColor;
    //public Vector4 m_unMovableColor;


    public SpriteRenderer m_spriteRenderer;

    //Monobehaviors
    private void Start()
    {
        //Get a reference to the sprite renderer on this current tile.
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != (int)GameManager.GameStates.Initialize && GameManager.Instance.GameState != (int)GameManager.GameStates.GameOver)
        {
            SetColor();


            if (m_occupiedBy != null && m_occupiedBy.GetComponent<CharacterStats>().HP < 0)
            {
                m_occupiedBy = null;
                m_isOccupied = false;
            }
        }
    }


    //Public Functions
    public void Initialize()
    {
        m_occupiedBy = null;
        m_isOccupied = false;
        m_isSelected = false;
        m_lightUp = false;
    }

    public void SetColor()
    {
        //Used for changing the colors of the tiles based on boolean values;
        if (m_isSelected)
        {
            //print("Character selected");
            m_spriteRenderer.color = m_selectedColor;
        }

        else if (m_lightUp)
        {
            //print("Path lit");
            m_spriteRenderer.color = m_pathingColor;
        }

        else if (m_isOccupied && m_occupiedBy.m_isSelected)
        {
            //print("Selected character preforming action");
            if (!m_occupiedBy.m_isEnemy)
            {
                m_spriteRenderer.color = m_playerSelectedColor;
            }
        }

        else
        {
            //print("deselected");
            m_spriteRenderer.color = m_deselectedColor;
        }
    }
}
