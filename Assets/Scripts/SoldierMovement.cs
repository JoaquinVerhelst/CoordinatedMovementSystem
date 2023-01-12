using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SoldierMovement : MonoBehaviour
{
    private Camera m_Camera;
    private NavMeshAgent m_Agent;
    public LayerMask m_StaticLayer;
    private Vector3 m_Target;
    public static List<NavMeshAgent> m_MeshAgents = new List<NavMeshAgent>();

    private List<Transform> m_SoldiersTransforms = new List<Transform>();


    public int m_Swordmen = 0;
    public int m_Shields = 0;
    public int m_Rangers = 0;
    //private int m_TotalClasses = 0;


    private Rigidbody m_Rigidbody;


    void Start()
    {
        m_Camera = Camera.main;
        m_Agent = GetComponent<NavMeshAgent>();
        //m_MeshAgents = SoldierSelections.g_Instance.m_Agents;
        m_MeshAgents.Add(m_Agent);
        m_Rigidbody = GetComponent<Rigidbody>();

    }



    public void Update() {

        // Calculate the movement
        switch (SoldierSelections.g_Instance.m_AIFormation)
        {
        case SoldierSelections.AIFormations.NORMAL:
            NormalMovement();
            break;

        case SoldierSelections.AIFormations.CIRCLE:
            CircleMovement();
            break;

        case SoldierSelections.AIFormations.RECTLONG:
            //RectFormation();
            break;

        case SoldierSelections.AIFormations.RECTWIDTH:
            break;

        }

 
    }

    void NormalMovement() {
        //right click
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_StaticLayer)) {
                m_Agent.SetDestination(hit.point);
            }
        }
    }

    void CircleMovement()
    {
        // Making sure m_Agent is added
        if(!m_MeshAgents.Contains(m_Agent))
        {
            m_MeshAgents.Add(m_Agent);
        }
        //if right clicked
        if (Input.GetMouseButtonDown(1) && m_MeshAgents.Contains(m_Agent))
        {

            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, m_StaticLayer))
            {
                if  (m_Agent.GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.KING)
                {
                    m_Agent.SetDestination(hit.point);
                }

                //m_Center = hit.point;
                //m_Agent.SetDestination(hit.point);
                m_Target = hit.point;
            }

            //Calculate the amount of different classes
            
            List<KeyValuePair<Soldier.SoldierClass, int>> soldierClassInfo = new List<KeyValuePair<Soldier.SoldierClass, int>>();

            soldierClassInfo.Capacity = 3;

            int totalCircles = 0;

            UpdateSelections();

            if (m_Rangers != 0)
            {
                ++totalCircles;
                soldierClassInfo.Add(new KeyValuePair<Soldier.SoldierClass, int>(Soldier.SoldierClass.RANGE, m_Rangers));
            }
            if (m_Swordmen != 0)
            {
                ++totalCircles;
                soldierClassInfo.Add( new KeyValuePair<Soldier.SoldierClass, int>( Soldier.SoldierClass.SWORD , m_Swordmen ));
            }
            if (m_Shields != 0)
            {
                ++totalCircles;
                soldierClassInfo.Add( new KeyValuePair<Soldier.SoldierClass, int>( Soldier.SoldierClass.SHIELD , m_Shields ));
            }   

            //start radius
            float radius = 2;

            for ( int i = 0; i < totalCircles; i++)
            {

                int addedCircles = CalculateCircle(radius, soldierClassInfo[i]);
                if (addedCircles != 0)
                {
                    radius += 3 * addedCircles;
                }
                radius += 3;
                    
            }
            
        }
    }
    int CalculateCircle(float radius, KeyValuePair<Soldier.SoldierClass, int> soldierClassInfo)
    {

        List<NavMeshAgent> currentSoldiers = new List<NavMeshAgent>();
        // get the selected and this class soldiers
        for (int i = 0; i < m_MeshAgents.Count; i++)
        {
            if (SoldierSelections.g_Instance.m_SoldiersSelected.Contains(m_MeshAgents[i].gameObject) && m_MeshAgents[i].GetComponent<Soldier>().m_SoldierClass == soldierClassInfo.Key)
            {
                currentSoldiers.Add(m_MeshAgents[i]);
            }
        }

        int addedCircles = 0;
        float offset = 0.5f;
        float soldierRadius = m_Agent.GetComponent<CapsuleCollider>().radius * 2 + offset;

        int soldierIndex = 0;
        int remainingSoldiers = currentSoldiers.Count;

        do
        {
            int maxAmountSoldiersOnCircle = (int)((radius * 2 * Mathf.PI) / soldierRadius);

            //if the amount of soldiers is bigger than it can fit on circle -> create new circle
            if (maxAmountSoldiersOnCircle > remainingSoldiers)
            {
                float angleDegree = 360 / remainingSoldiers; // angular step
                float angleRad = angleDegree * Mathf.PI / 180.0f;
                float totalAngle = 0;

                for (int i = 0; i < remainingSoldiers; ++i)
                {
                    totalAngle = angleRad * i;
                    float x = m_Target.x + radius * Mathf.Cos(totalAngle);
                    float z = m_Target.z + radius * Mathf.Sin(totalAngle);
                    Vector3 target = new Vector3(x, m_Target.y, z);

                    currentSoldiers[soldierIndex + i].SetDestination(target);
                }
                remainingSoldiers = 0;
            }
            else
            {
                float angleDegree = 360 / maxAmountSoldiersOnCircle; // angular step
                float angleRad = angleDegree * Mathf.PI / 180.0f;
                float totalAngle = 0;

                for (int i = soldierIndex; i < maxAmountSoldiersOnCircle; i++)
                {
                    totalAngle = angleRad * i;
                    float x = m_Target.x + radius * Mathf.Cos(totalAngle);
                    float z = m_Target.z + radius * Mathf.Sin(totalAngle);
                    Vector3 target = new Vector3(x, m_Target.y, z);

                    currentSoldiers[i].SetDestination(target);
                    ++soldierIndex;
                }
                ++addedCircles;
                radius += 3.0f;
                remainingSoldiers -= maxAmountSoldiersOnCircle;
            }
        }
        while (remainingSoldiers != 0);

        return addedCircles;
    }


    void RectFormation()
    {
        UpdateSelections();



        List<NavMeshAgent> currentSoldiers = new List<NavMeshAgent>();



        if (Input.GetMouseButtonDown(1) && m_Agent.GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.KING)
        {
            RaycastHit hit;
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            Vector3 direction = Vector3.zero;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_StaticLayer))
            {
                direction = m_Target - m_Agent.transform.position;
                m_Target = hit.point;

            }

            m_Agent.SetDestination(m_Target);

            for (int i = 0; i < m_MeshAgents.Count; i++)
            {
                if (SoldierSelections.g_Instance.m_SoldiersSelected.Contains(m_MeshAgents[i].gameObject) && m_MeshAgents[i].gameObject != m_Agent)
                {
                    currentSoldiers.Add(m_MeshAgents[i]);
                }
            }
        }


        int rowMax = 5;


        //currentSoldiers.Add(m_Agent);
        // get the selected and this class soldiers


        // calculate the 5 first points;
        float soldierWidth = 1.0f;
        float spacing = 1.0f;

        Vector3 positionKing = SoldierSelections.g_Instance.m_King.transform.position;

        Vector3 currentLocPoint = new Vector3(positionKing.x - soldierWidth * 2 - spacing * 2, positionKing.y, positionKing.z);

        Vector3 currentDirPoint = new Vector3(m_Target.x - soldierWidth * 2 - spacing * 2, m_Target.y, m_Target.z);

        List<Vector3> locPoints = new List<Vector3>();
        List<Vector3> targetPoints = new List<Vector3>();
        List<Vector3> dirPoints = new List<Vector3>();




        for (int i = 0; i < rowMax; i++)
        {
            locPoints.Add(new Vector3(currentLocPoint.x + soldierWidth * i + spacing * i, currentLocPoint.y, currentLocPoint.z));
            targetPoints.Add(new Vector3(currentDirPoint.x + soldierWidth * i + spacing * i, currentDirPoint.y, currentDirPoint.z));

            dirPoints.Add(targetPoints[0] - locPoints[0]);
        }

        // Move the other agents based on the leader's position and rotation
        for (int i = 0; i < currentSoldiers.Count; i++)
        {
            currentSoldiers[i].destination = locPoints[i];
            currentSoldiers[i].transform.rotation = Quaternion.Euler(-dirPoints[i]);
        }

       


        
    }

    







    void UpdateSelections()
        {
            m_Swordmen = 0;
            m_Shields = 0;
            m_Rangers = 0;

            for (int i = 0; i < SoldierSelections.g_Instance.m_SoldiersSelected.Count; i++)
            {
                if  (SoldierSelections.g_Instance.m_SoldiersSelected[i].GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.KING)
                {
                    SoldierSelections.g_Instance.m_King = SoldierSelections.g_Instance.m_SoldiersSelected[i];
                }
                if  (SoldierSelections.g_Instance.m_SoldiersSelected[i].GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.SWORD)
                {
                    ++m_Swordmen;
                }
                if  (SoldierSelections.g_Instance.m_SoldiersSelected[i].GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.SHIELD)
                {
                    ++m_Shields;
                }
                if  (SoldierSelections.g_Instance.m_SoldiersSelected[i].GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.RANGE)
                {
                    ++m_Rangers;
                }
            }

        }






}












































//  if(!m_MeshAgents.Contains(m_Agent))
//         {
//             m_MeshAgents.Add(m_Agent);
//         }
//         //if right clicked
//         if (Input.GetMouseButtonDown(1) && m_MeshAgents.Contains(m_Agent))
//         {

//             RaycastHit hit;
//             Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

//             if(Physics.Raycast(ray, out hit, Mathf.Infinity, m_StaticLayer))
//             {



//                 //m_Center = hit.point;
//                 //m_Agent.SetDestination(hit.point);
//                 m_Center = new Vector2(hit.point.x, hit.point.z);
//             }

//             int amountOfAgents = SoldierSelections.g_Instance.m_SoldiersSelected.Count; // number of agents
//             float angleDegree = 360 / amountOfAgents; // angular step
//             float angleRad = angleDegree * Mathf.PI/180.0f;
//             float totalAngle = 0;
//             float radius = 4;


//             for(int i = 0; i < amountOfAgents; i++)
//             {
//                 totalAngle = angleRad * i;
//                 float x = m_Center.x + radius * Mathf.Cos(totalAngle);
//                 float z = m_Center.y + radius * Mathf.Sin(totalAngle);
//                 Vector3 target = new Vector3(x, hit.point.y, z);
                

//                     m_MeshAgents[i].SetDestination(target);
//                 


//             }
            
//         }
//     }