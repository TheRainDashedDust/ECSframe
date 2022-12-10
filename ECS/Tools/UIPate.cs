using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPate : MonoBehaviour
{
    public Text m_roleName,hp_text,mp_text;
    private GameObject m_go;
    public Scrollbar m_hp;
    public Scrollbar m_mp;
    public GameObject m_gather;
    public List<Image> m_gathers;

    public void InitPate()
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>("RoleInfo"));
        m_go.transform.SetParent(UIManager.Instance.m_hudroot.transform);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;

        m_roleName = m_go.transform.Find("RoleName").GetComponent<Text>();
        m_hp = m_go.transform.Find("Hp").GetComponent<Scrollbar>();
        m_mp = m_go.transform.Find("Mp").GetComponent<Scrollbar>();
        m_gather = m_go.transform.Find("Gather").gameObject;
        hp_text= m_go.transform.Find("Hp/Hp_text").GetComponent<Text>();
        mp_text = m_go.transform.Find("Mp/Mp_text").GetComponent<Text>();
        m_gathers = new List<Image>();
        for (int i = 0; i < m_gather.transform.childCount; i++)
        {
            m_gathers.Add(m_gather.transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void Show(bool active)
    {
        if (m_go)
        {
            m_go.SetActive(active);
        }
    }

    public void SetData(string name, float hp,float maxhp, float mp,float maxmp)
    {
        m_roleName.text = name;
        m_hp.size = hp/maxhp;
        m_mp.size = mp/maxmp;
        hp_text.text = hp + "/" + maxhp;
        mp_text.text = mp + "/" + maxmp;

    }

    public void SetData(int gather)
    {
        for (int i = 0; i < m_gathers.Count; i++)
        {
            m_gathers[i].gameObject.SetActive(i < gather);
        }
    }
    Vector3 camerapos = Vector3.zero;
    private void Update()
    {
        camerapos.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
        m_go.transform.position = World.Instance.m_main.WorldToScreenPoint(camerapos);
    }
    /// <summary>
    /// 这个方法？？？
    /// </summary>
    ~UIPate()
    {
        m_roleName = null;
        m_hp = null;
        m_mp = null;
        m_gather = null;
        if (m_gathers != null)
        {
            m_gathers.Clear();
        }
        m_gathers = null;
    }
}
