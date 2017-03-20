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

    public Vector4 m_deselectedColor;
    public Vector4 m_selectedColor;
    public Vector4 m_pathingColor;

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
        SetColor();
    }


    //Public Functions
    public void SetColor()
    {
        //Used for changing the colors of the tiles based on boolean values;
        if (m_isSelected)
        {
            m_spriteRenderer.color = m_selectedColor;
        }

        else if (m_lightUp)
        {
            m_spriteRenderer.color = m_pathingColor;
        }

        else
        {
            m_spriteRenderer.color = m_deselectedColor;
        }
    }
}
