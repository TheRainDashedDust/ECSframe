using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : ObjectBase
{
    public PlayerInfo m_info;

    public PlayerObj(PlayerInfo info)
    {
        m_info = info;
    }

    public override void Destory()
    {
        base.Destory();
    }

    public override void OnCreate()
    {
        base.OnCreate();
        if (m_go != null)
        {
            m_pate = m_go.AddComponent<UIPate>();
            m_pate.InitPate();
            m_pate.m_gather.SetActive(false);
            m_pate.SetData(m_info.m_name,m_info.m_HP,m_info.m_HPMAX,m_info.m_MP,m_info.m_MPMAX);
            
        }
    }

    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }

    public void AddBuff(string path)
    {

    }
}
public class HostPlayer : PlayerObj
{
    Role player;
    public HostPlayer(PlayerInfo info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_prefabPath;
    }
    public override void CreateObj(ObjectType type)
    {
        SetPos(m_info.m_pos.GetVector3());
        base.CreateObj(type);
    }
    public override void OnCreate()
    {
        base.OnCreate();
        player = m_go.AddComponent<Role>();
        player.InitData();
        MessageCenter.Instance.AddListener("atkActionPlay", (notify) => {
            if (notify.msgtype.Equals("ByServer"))
            {
                int skillid = (int)notify.data[0];
                player.SetData(skillid);
                //player.play();
            }

        });
    }
    Notification notify = new Notification();
    public void JoystickHandlerMoving(float h, float v)
    {
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 1);
            notify.Refresh("Player", m_go.transform.position);
            MessageCenter.Instance.BroadCast("MovePos", notify);
        }
    }

    public void JoyButtonHandler(string btnName)
    {
        //List<SkillBase> componentList;
        switch (btnName)
        {
            case "attack":

                Notification m_notify = new Notification();
                m_notify.Refresh("atkOther", 1, 2, 1);
                MessageCenter.Instance.BroadCast("ByClent_Battle", m_notify);
                
                break;

        }
    }

}
public class OtherPlayer : PlayerObj
{
    public OtherPlayer(PlayerInfo info) : base(info)
    {
        m_insID=info.ID;
        m_modelPath=info.m_prefabPath;
    }
    public override void CreateObj(ObjectType type)
    {
        SetPos(m_info.m_pos.GetVector3());
        base.CreateObj(type);
    }
}