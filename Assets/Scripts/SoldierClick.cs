
using UnityEngine;

public class SoldierClick : MonoBehaviour
{
    private Camera m_Camera;
    [SerializeField] public GameObject m_GroundMarker;
    public LayerMask m_Clickable;
    public LayerMask m_StaticLayer;


    void Start()
    {
        m_Camera = Camera.main;
    }


    void Update()
    {
         //Selecting and Deselecting Soldiers

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_Clickable))
            {
                // This will check if we hit a clickable object with raycast

                if (Input.GetKey(KeyCode.LeftShift)) // Shift Click
                {
                    SoldierSelections.g_Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else // Normal Click
                {
                    SoldierSelections.g_Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                // If we didn't && normal Click
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    SoldierSelections.g_Instance.DeselectAll();
                }
            }
        }

        //Marking the ground 
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_StaticLayer))
            {
                m_GroundMarker.transform.position = hit.point;
            }
        }
    }
}
