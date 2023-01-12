using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{

    private Camera m_Camera;
    [SerializeField] private float m_NormalSpeed; 
    [SerializeField] private float m_FastSpeed;
    [SerializeField] private Vector3 m_StartLocation;
    private Vector3 m_CurrentLocation;
    [SerializeField] private float m_ScrollSpeed;


    public void Start()
    {
        m_Camera = Camera.main;

        m_CurrentLocation = m_StartLocation;
        m_Camera.transform.position = m_StartLocation;
    }

    public void Update()
    {
        HandleMovement();
        HandleScrolling();

        m_Camera.transform.position = m_CurrentLocation;
    }

    //Moving the Camera
    private void HandleMovement()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementInput.Normalize(); // Normalize to avoid faster diagonal movement

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_CurrentLocation += (new Vector3(movementInput.x, 0f, movementInput.y) * m_FastSpeed * Time.deltaTime);
        }
        else
        {
            m_CurrentLocation += (new Vector3(movementInput.x, 0f, movementInput.y) * m_NormalSpeed * Time.deltaTime);
        }

    }

    //Zooming in and out
    private void HandleScrolling()
    {
        float scrollInput = -(Input.GetAxis("Mouse ScrollWheel"));

        m_CurrentLocation.y += scrollInput * m_ScrollSpeed;

        if (m_CurrentLocation.y <= 5.0f)
        {
            m_CurrentLocation.y = 5.0f;
        }
    }


}
