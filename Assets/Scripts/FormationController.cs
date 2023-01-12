using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    public GameObject m_Soldier;


    public void Update() {

        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            print("normal");
            SoldierSelections.g_Instance.m_AIFormation = SoldierSelections.AIFormations.NORMAL;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2)) {
            print("Circle");
            SoldierSelections.g_Instance.m_AIFormation = SoldierSelections.AIFormations.CIRCLE;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3)) {
            SoldierSelections.g_Instance.m_Points.Clear();
            print("RectLong");
            SoldierSelections.g_Instance.m_AIFormation = SoldierSelections.AIFormations.RECTLONG;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4)) {
            SoldierSelections.g_Instance.m_Points.Clear();
            print("RectWidth");
            SoldierSelections.g_Instance.m_AIFormation = SoldierSelections.AIFormations.RECTWIDTH;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            print("Add 1 Soldier to scene");
            Instantiate(m_Soldier, new Vector3(0, 0, 0), Quaternion.identity);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            print("Add 5 Soldiers to scene");

            for (int i = 0; i < 5; ++i)
            {
                Instantiate(m_Soldier, new Vector3(i, 0, 0), Quaternion.identity);
            }
        }
    }
}
