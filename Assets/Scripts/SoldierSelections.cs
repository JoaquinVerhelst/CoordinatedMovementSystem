
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Soldier;

public class SoldierSelections : MonoBehaviour
{
    public List<GameObject> m_SoldiersList = new List<GameObject>();
    public List<GameObject> m_SoldiersSelected = new List<GameObject>();
    public List<NavMeshAgent> m_Agents = new List<NavMeshAgent>();

    public List<NavMeshAgent> m_Points = new List<NavMeshAgent>();

    public GameObject m_King= null;

    public enum AIFormations
    {
        NORMAL,
        CIRCLE,
        RECTLONG,
        RECTWIDTH
    }

    public AIFormations m_AIFormation;


    
    // Singleton 
    private static SoldierSelections m_Instance;
    public static SoldierSelections g_Instance { get {return m_Instance; } }



    // Will Only Get Called Once
    void Awake()
    {
        // If an instance already exists && it's not this one -> Destroy
        if ( m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        // Else make this the instance
        else
        {
            m_Instance = this;
        }

    }


    public void ClickSelect(GameObject selectedsoldier)
    {
        DeselectAll();
        m_SoldiersSelected.Add(selectedsoldier);
        selectedsoldier.transform.GetChild(0).gameObject.SetActive(true);
        selectedsoldier.GetComponent<SoldierMovement>().enabled = true;
    }


    public void ShiftClickSelect(GameObject selectedsoldier)
    {
        if (!m_SoldiersSelected.Contains(selectedsoldier))
        {
            m_SoldiersSelected.Add(selectedsoldier);
            selectedsoldier.transform.GetChild(0).gameObject.SetActive(true);
            selectedsoldier.GetComponent<SoldierMovement>().enabled = true;
            UpdateNavMeshAgents();
        }
        else
        {
            selectedsoldier.GetComponent<SoldierMovement>().enabled = false;
            selectedsoldier.transform.GetChild(0).gameObject.SetActive(false);
            m_SoldiersSelected.Remove(selectedsoldier);
            UpdateNavMeshAgents();
        }
    }
    public void DragSelect(GameObject selectedsoldier)
    {
        if (!m_SoldiersSelected.Contains(selectedsoldier))
        {
            m_SoldiersSelected.Add(selectedsoldier);
            selectedsoldier.transform.GetChild(0).gameObject.SetActive(true);
            selectedsoldier.GetComponent<SoldierMovement>().enabled = true;
            UpdateNavMeshAgents();
        }
    }
    public void DeselectAll()
    {
        foreach (var soldier in m_SoldiersSelected)
        {
            soldier.GetComponent<SoldierMovement>().enabled = false;
            soldier.transform.GetChild(0).gameObject.SetActive(false);
        }
        m_SoldiersSelected.Clear();
        m_Agents.Clear();
    }



    public void UpdateNavMeshAgents()
    {
        m_Agents.Clear();

        foreach (var soldier in m_SoldiersSelected)
        {
            m_Agents.Add(soldier.GetComponent<NavMeshAgent>());
        }
    }


    public void UpdateKing(GameObject soldier)
    {

        if (m_King == null)
        {
            m_King = soldier;
            soldier.GetComponent<Soldier>().m_SoldierClass = SoldierClass.KING;
        }
        else
        {
            for (int i = 0; i < m_SoldiersList.Count; i++)
            {
                if (m_SoldiersList[i].GetComponent<Soldier>().m_SoldierClass == Soldier.SoldierClass.KING)
                {
                    m_SoldiersList[i].GetComponent<Soldier>().m_SoldierClass = Soldier.SoldierClass.SWORD;
                }
            }
            m_King = soldier;
            soldier.GetComponent<Soldier>().m_SoldierClass = SoldierClass.KING;
        }
    }
}
