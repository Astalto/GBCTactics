using UnityEngine;
using System.Collections;

/// <summary>
/// This script contains the Map pieces in m_map (Access via Map.Instance.MAP[index.x, index.y])
/// it is responsible for knowing where the map pieces are located globally as well as determining
/// which tile is currently selected.
/// </summary>
public class Map : singleton<Map>
{
    public int Rows;
    public int Columns;

    public MapPiece[,] m_map;
    public Vector2 m_currentSelection;

    public MapPiece[,] MAP { get { return m_map; } }
    public Vector2 SelectedTile { get { return m_currentSelection; } set { m_currentSelection = value; } }

    public MoveableCharacter LastSelected;


    //MonoBehaviors
    private void OnEnable()
    {
        m_map = new MapPiece[Rows, Columns];

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                m_map[i, j] = GameObject.Find("MapPieceOutline (" + (i * Columns + j) + ")").GetComponent<MapPiece>();
                //print("Map (" + i + ", " + j + ") Set to:" + m_map[i, j].name);
            }
        }
        
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != (int)GameManager.GameStates.Initialize && GameManager.Instance.GameState != (int)GameManager.GameStates.GameOver)
        {
            LightCurrentTile();

            //print(LastSelected);
            if (m_map[(int)m_currentSelection.x, (int)m_currentSelection.y].m_occupiedBy != null)
            {
                LastSelected = m_map[(int)m_currentSelection.x, (int)m_currentSelection.y].m_occupiedBy;
            }
        }

    }

    //Public functions
    public void DeselectTile()
    {
        m_map[(int)m_currentSelection.x, (int)m_currentSelection.y].m_isSelected = false;
    }

    public Vector2 GetPosition(float Xindex, float Yindex)
    {
        //print("(" + Xindex + ", " + Yindex + ")");
        return m_map[(int)Xindex, (int)Yindex].gameObject.transform.position;
    }

    private void LightCurrentTile()
    {
        m_map[(int)m_currentSelection.x, (int)m_currentSelection.y].m_isSelected = true;
    }

    public void InitializePieces()
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                m_map[i, j].Initialize();
            }
        }
    }

}
