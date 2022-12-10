using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public static class LocalProps
{
    public static Dictionary<long,SPlayer> players=new Dictionary<long, SPlayer> ();
}
public class ServerInit : MonoBehaviour
{
    public Vector3 m_playerPos;
    public Dictionary<int, Vector3> m_otherPosDic;
    private void Awake()
    {
        MessageCenter.Instance.AddListener("MovePos", (notify) =>
        {
            if (notify.msgtype.Equals("Player"))
            {
                m_playerPos = (Vector3)notify.data[0];
            }
            else if(notify.msgtype.Equals("Other"))
            {
                if (m_otherPosDic==null)
                {
                    m_otherPosDic = new Dictionary<int, Vector3>();

                }
                int insid=(int)notify.data[0];
                Vector3 pos = (Vector3)notify.data[1];
                if (!m_otherPosDic.ContainsKey(insid))
                {
                    m_otherPosDic.Add(insid, pos);
                }
                else
                {
                    m_otherPosDic[insid] = pos;
                }
            }
        });

        MessageCenter.Instance.AddListener("ServerMsg", (notify) =>
        {
            if (notify.msgtype.Equals("gather"))
            {
                Debug.Log($"点击采集按钮 Insid:{(int)notify.data[0]}");
                int insid=(int)notify.data[0];
                notify.Refresh("gather_callback", insid, 2);
                MessageCenter.Instance.BroadCast("ServerMsg", notify);
                MessageCenter.Instance.BroadCast("GatherAction", notify);

            }
            if (notify.msgtype.Equals("AcceptTask"))
            {
                int taskid = (int)notify.data[0];
                foreach (var item in LocalProps.players)
                {
                    if (item.Key==1)
                    {
                        item.Value.components.Add(ComponentType.task, new TaskComponent());
                        item.Value.components[ComponentType.task].Init();
                    }
                }
            }
        });
        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 1;
        splayer.Hp = 100;
        splayer.components.Add(ComponentType.battle, new BattleComponent());
        LocalProps.players.Add(splayer.m_insid, splayer);

        if (LocalProps.players==null)
        {
            return;
        }
        foreach (var item in LocalProps.players)
        {
            foreach (var temp in item.Value.components)
            {
                temp.Value.GetPlayerById = GetPlayer;
                temp.Value.Init();
            }
        }
    }
    public SPlayer GetPlayer(long id)
    {
        using(var temp=LocalProps.players.GetEnumerator())
        {
            while (temp.MoveNext())
            {
                if (temp.Current.Key==id)
                {
                    return temp.Current.Value;
                }
            }
        }
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum ComponentType:byte
{
    nil=0,
    task,
    battle,
}
public class SkillProp
{
    public float range;
}
public class SPlayer
{
    public long m_insid;
    public Vector3 m_pos;
    public float Hp;
    public float Mp;
    public float Atk;
    public List<int> buffs;
    public List<SkillProp> skills;
    public Dictionary<ComponentType, SComponent> components;

    public void InitPlayer()
    {
        buffs = new List<int>();
        skills = new List<SkillProp>();
        components = new Dictionary<ComponentType, SComponent>();
    }
    public void PropOperation(int type,float value)
    {
        switch (type)
        {
            case 1:
                Hp += value;
                break;
            case 2:
                Mp += value;
                break;
            default:
                break;
        }
        Notification notification = new Notification();
        notification.Refresh("ByServer", type, value);
        MessageCenter.Instance.BroadCast("propchange", notification);
    }
}
public class SComponent
{
    public Func<long, SPlayer> GetPlayerById;
    Notification m_notify;
    public virtual void S2CMsg(string cmd,object value)
    {
        if (m_notify==null)
        {
            m_notify=new Notification();
        }
        m_notify.Refresh("ByServer", value);
        MessageCenter.Instance.BroadCast(cmd, m_notify);
    }
    public virtual void Init()
    {

    }
}
public class TaskComponent:SComponent
{
    public List<int> Tasks;
    public override void Init()
    {
        base.Init();
        MessageCenter.Instance.AddListener("GatherAction", (notify) =>
        {
            //采集
        });
    }
}
public class BattleComponent:SComponent
{
    public override void Init()
    {
        base.Init();
        MessageCenter.Instance.AddListener("ByClent_Battle", (notify) =>
        {
            if (notify.msgtype.Equals("atkOther"))
            {
                int atkId = (int)notify.data[0];
                int targetID = (int)notify.data[1];
                int skillID = (int)notify.data[2];

                AtkPlayer(atkId,targetID, skillID);
            }
        });
    }
    public void AtkPlayer(long atk,long target,int skillid)
    {
        SPlayer p1 = GetPlayerById(atk);
        SPlayer p2 = GetPlayerById(target);
        //实际逻辑为下方逻辑
        //float tmp = p1.skills[skillid].range;
        //判断攻击距离  修改伤害数据   攻击减防御  可能再加上各种系数
        //if (tmp >= Vector3.Distance(p1.m_pos,p2.m_pos))
        //{
        //    p2.PropOperation(1, -10);
        //    S2CMsg("atkover", true);
        //    S2CMsg("shouji", p2.m_insid);
        //}
        //===临时消息
        S2CMsg("atkActionPlay", skillid);
        
    }
}