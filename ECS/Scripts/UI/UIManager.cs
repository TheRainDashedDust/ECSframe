using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject m_uiroot;
    public GameObject m_hudroot;
    public Dictionary<string, UIBase> m_uiDic;
    
    public void Init(GameObject root,GameObject hud)
    {
        m_uiroot = root;
        m_hudroot = hud;
        m_uiDic=new Dictionary<string, UIBase>();
        m_uiDic.Add("Lobby", new Lobbysys());
        m_uiDic.Add("battle", new Battlesys());
        m_uiDic.Add("minimap", new MinimapSys());
        m_uiDic.Add("taskPanel", new TaskSys());

        Open("Lobby");
        Open("battle");
        Open("minimap");
        Open("taskPanel");
    }
    public void Open(string key)
    {
        UIBase ui;
        if (m_uiDic.TryGetValue(key, out ui))
        {
            ui.DoCreate(key);
        }
    }

    public void Close(string key)
    {
        UIBase ui;
        if (m_uiDic.TryGetValue(key, out ui))
        {
            ui.Destory();
        }
    }
}
public class UIBase
{
    public GameObject m_go;

    public virtual void DoCreate(string path)
    {
        InsGo(path);
        DoShow(true);
    }

    private void InsGo(string path)
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>(path));
        m_go.transform.SetParent(UIManager.Instance.m_uiroot.transform, false);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;
    }

    public virtual void DoShow(bool active)
    {
        if (m_go)
        {
            m_go.SetActive(active);
        }
    }

    public virtual void Destory()
    {
        GameObject.Destroy(m_go);
    }

}

public class Lobbysys:UIBase
{
    private Image m_head;
    private List<Image> m_buffs;
    public override void DoCreate(string path)
    {
        m_buffs = new List<Image>();
        base.DoCreate(path);
    }
    public override void DoShow(bool active)
    {
        base.DoShow(active);
        m_head = m_go.transform.Find("head").GetComponent<Image>();
        Transform buffgo = m_go.transform.Find("bufflayout").transform;
        for (int i = 0; i < buffgo.childCount; i++)
        {
            m_buffs.Add(buffgo.GetChild(i).GetComponent<Image>());
        }
    }
    public override void Destory()
    {
        base.Destory();
    }
}
public class Battlesys:UIBase
{
    private Button m_gatherBtn;
    private Slider m_gatherSlider;
    private int m_gatherInsid;

    public override void DoCreate(string path)
    {
        base.DoCreate(path);

        MessageCenter.Instance.AddListener("ClientMsg", RefreshBtn);

        MessageCenter.Instance.AddListener("ServerMsg", ServerNotify);
    }

    private void ServerNotify(Notification obj)
    {
        if (obj.msgtype.Equals("gather_callback"))
        {
            m_gatherSlider.gameObject.SetActive(true);
        }
    }

    public override void DoShow(bool active)
    {
        base.DoShow(active);
        m_gatherBtn = m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherBtn.onClick.AddListener(() => {
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("gather", 1);
            MessageCenter.Instance.BroadCast("ServerMsg", notify);
        });
        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent<Slider>();
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }

    public override void Destory()
    {
        base.Destory();
        MessageCenter.Instance.RemoveListener("ClientMsg", RefreshBtn);
    }


    private void RefreshBtn(Notification notiy)
    {
        if (notiy.msgtype.Equals("gather_trigger"))
        {
            m_gatherInsid = (int)notiy.data[0];
            m_gatherBtn.gameObject.SetActive(true);
        }
    }
}
public class MinimapSys:UIBase
{
    public MapControl m_map;

    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        Transform map = m_go.transform.Find("minimap/map");
        m_map = map.gameObject.AddComponent<MapControl>();

    }

    public override void DoShow(bool active)
    {
        base.DoShow(active);
    }

    public override void Destory()
    {
        GameObject.Destroy(m_map);
        base.Destory();
    }
}
public class TaskSys:UIBase
{
    private Text taskText;
    private Button acceptBtn;
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
    }
    public override void DoShow(bool active)
    {
        base.DoShow(active);
        taskText = m_go.transform.Find("TaskText").GetComponent<Text>();
        acceptBtn = m_go.transform.Find("AcceptButton").GetComponent<Button>();

        taskText.text = GameData.Instance.GetTaskDataById(1).taskName;

        acceptBtn.onClick.AddListener(() => {
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("AcceptTask", 1);
            MessageCenter.Instance.BroadCast("ServerMsg", notify);
        });

    }
}