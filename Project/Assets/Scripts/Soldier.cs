
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public enum SoldierClass
    {
        KING,
        SWORD,
        SHIELD,
        RANGE
    }

    public SoldierClass m_SoldierClass;


    void Start()
    {
        SoldierSelections.g_Instance.m_SoldiersList.Add(this.gameObject);
        this.transform.GetChild(0).gameObject.SetActive(false);

        m_SoldierClass = SoldierClass.SWORD;
        ChangeColor();
    }

    void Update()
    {
        if (SoldierSelections.g_Instance.m_SoldiersSelected.Contains(this.gameObject))
        {
            if (Input.GetKeyUp(KeyCode.Alpha5) && SoldierSelections.g_Instance.m_SoldiersSelected.Count == 1) {

                print("KING");
                SoldierSelections.g_Instance.UpdateKing(this.gameObject);

            }
            if (Input.GetKeyUp(KeyCode.Alpha6)) {
                print("SWORD");
                m_SoldierClass = SoldierClass.SWORD;

            }
            if (Input.GetKeyUp(KeyCode.Alpha7)) {
                print("SHIELD");
                m_SoldierClass = SoldierClass.SHIELD;

            }
            if (Input.GetKeyUp(KeyCode.Alpha8)) {
                print("RANGE");
                m_SoldierClass = SoldierClass.RANGE;

            }
        }

        ChangeColor();
    }

    void OnDestroy()
    {
        SoldierSelections.g_Instance.m_SoldiersList.Remove(this.gameObject);
    }


    void ChangeColor()
    {
        switch (m_SoldierClass)
        {
        case SoldierClass.KING:
            this.gameObject.GetComponent<Renderer>().material.color = new Color(100.0f, 64.7f , 0.0f);
            break;
        case SoldierClass.SWORD:
            this.gameObject.GetComponent<Renderer>().material.color = new Color(0.0f, 128.0f , 255.0f);
            break;
        case SoldierClass.SHIELD:
            this.gameObject.GetComponent<Renderer>().material.color = new Color(96.0f, 96.7f , 96.0f);
            break;
        case SoldierClass.RANGE:
            this.gameObject.GetComponent<Renderer>().material.color = new Color(0.0f, 255.7f , 0.0f);
            break;
        }
    }


}
