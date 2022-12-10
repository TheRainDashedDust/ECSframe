using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerActionBase
{
    public SysType type;
    public ServerPlayer m_player;
    public ServerActionBase(ServerPlayer m_player)
    {
        this.m_player = m_player;
    }
}

public class ServerBattleAction:ServerActionBase
{
    public ServerPlayer m_target;
    public ServerBattleAction(ServerPlayer player) :base(player)
    {
       
    }
    public void PlaySkill(int skillid)
    {
        ServerSkillInfo info = m_player.m_info.m_skills[skillid];
        if (Vector3.Distance(m_player.m_info.m_pos,m_target.m_info.m_pos)>info.range)
        {
            m_target.RefreshBaseData(1, info.atk_value * -1);
            //可以判断僵直霸体无敌等特殊情况
            m_target.Strike();
        }
    }
}
public class ServerTaskAction : ServerActionBase
{
    public ServerTaskAction(ServerPlayer m_player) : base(m_player)
    {
    }
}
public enum SysType
{
    battle,
    task,
}
public class ServerSkillInfo
{
    public float range;
    public float mp;
    public float check_atk;
    public float buff_group;
    public float lock_target;
    public float skill_cd;
    public float atk_value;
}
public class serverTaskInfo
{
    public int id;
    public string name;
    public int limitLev;
}
public class ServerPlayerInfo
{
    public Vector3 m_pos;
    public float m_mp;
    public float m_hp;
    public Dictionary<int, ServerSkillInfo> m_skills;
    public List<serverTaskInfo> m_tasks;
}
public class ServerPlayer
{
    public int m_insid;
    public ServerPlayerInfo m_info;
    public Dictionary<SysType,ServerActionBase> m_allAction=new Dictionary<SysType, ServerActionBase>();
    Notification notify=new Notification();
    public ServerPlayer()
    {
        InitPlayerInfo();
    }
    public void InitPlayerInfo()
    {
        m_info=new ServerPlayerInfo();
        //或者读取本地信息
    }
    public void RefreshPos(Vector3 pos)
    {
        m_info.m_pos=pos;
    }

    public void RefreshBaseData(int key,float value)
    {
        switch (key)
        {
            case 1:
                m_info.m_hp += value;
                break;
            case 2:
                m_info.m_mp += value;
                break;
            default:
                break;
        }

        SendMsg2Client("TOCLIENT", "RefreshBaseInfo", m_insid, m_info);
    }
    public void SendMsg2Client(string typecode,string msgcode,params object[] para)
    {
        notify.Refresh(msgcode, para);
        MessageCenter.Instance.BroadCast(typecode, notify); 
        notify.Clear();
    }
    public void Strike()
    {
        SendMsg2Client("TOCLIENT", "Strike", m_insid);
    }

}
public class GameLogic : MonoBehaviour
{
    public Dictionary<int,ServerPlayer> m_allplayer=new Dictionary<int, ServerPlayer>();
    public GameLogic()
    {
        MessageCenter.Instance.AddListener("TOSERVER", ToServer);

    }
    public void ToServer(Notification notify)
    {
        switch (notify.msgtype)
        {
            case "PlayerMove":
                int insid = (int)notify.data[0];
                Vector3 pos=(Vector3)notify.data[1];
                m_allplayer[insid].RefreshPos(pos);
                break;
            case "BattleSkill":

                break;
            default:
                break;
        }
    }
    public void InitWorld()
    {
        ServerPlayer player;
        for (int i = 0; i < 10; i++)
        {
            player = new ServerPlayer();
            player.m_insid = i + 1;
            player.RefreshPos(Vector2.zero);
            player.SendMsg2Client("TOCLIENT","RefreshPlayer",player.m_info);
        }
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


