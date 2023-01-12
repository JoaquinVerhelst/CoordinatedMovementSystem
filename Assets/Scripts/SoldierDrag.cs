using UnityEngine;

public class SoldierDrag : MonoBehaviour
{

    private Camera m_Camera;
    [SerializeField]
    private RectTransform m_BoxVisual;
    private Rect m_SelectionBox;
    private Vector2 m_StartPosition;
    private Vector2 m_EndPosition;



    void Start()
    {
        m_Camera = Camera.main;
        m_StartPosition = Vector2.zero;
        m_EndPosition = Vector2.zero;
        DrawVisual();
    }


    void Update()
    {
        //Click
        if (Input.GetMouseButtonDown(0))
        {
            m_StartPosition = Input.mousePosition;
            m_SelectionBox = new Rect();
        }

        //Drag
        if (Input.GetMouseButton(0))
        {
            m_EndPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        //Release
        if (Input.GetMouseButtonUp(0))
        {
            SelectSoldiers();
            m_StartPosition = Vector2.zero;
            m_EndPosition = Vector2.zero;
            DrawVisual();
        }


    }

    //Draw The Box visual
    void DrawVisual()
    {
        Vector2 boxStart = m_StartPosition;
        Vector2 boxEnd = m_EndPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        m_BoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        m_BoxVisual.sizeDelta = boxSize;
    }

    //Calculate The Box Selection Rect
    void DrawSelection()
    {
        // X calculations
        if (Input.mousePosition.x < m_StartPosition.x) {
            //Dragging Left
            m_SelectionBox.xMin = Input.mousePosition.x;
            m_SelectionBox.xMax = m_StartPosition.x;
        }
        else {
            //Dragging Right
            m_SelectionBox.xMin = m_StartPosition.x;
            m_SelectionBox.xMax = Input.mousePosition.x;
        }


        // Y calculations        
        if (Input.mousePosition.y < m_StartPosition.y) {
            //Dragging Down
            m_SelectionBox.yMin = Input.mousePosition.y;
            m_SelectionBox.yMax = m_StartPosition.y;
        }
        else {
            //Dragging Up
            m_SelectionBox.yMin = m_StartPosition.y;
            m_SelectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectSoldiers()
    {
        //loop over all soldiers
        foreach (var soldier in SoldierSelections.g_Instance.m_SoldiersList)
        {
            //if soldier is within bounds
            if (m_SelectionBox.Contains(m_Camera.WorldToScreenPoint(soldier.transform.position)))
            {
                //Add it to selected
                SoldierSelections.g_Instance.DragSelect(soldier);
            }
        }
    }
}
