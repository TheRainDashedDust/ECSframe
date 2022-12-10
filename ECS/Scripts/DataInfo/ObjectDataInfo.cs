using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VectorPos
{
    public float x;
    public float y;
    public float z;
    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
    public void SetPos(Vector3 v3)
    {
        this.x = v3.x;

        this.y = v3.y;
        this.z = v3.z;
    }
}
/// <summary>
/// Data数据
/// </summary>
public class ObjectDataInfo
{
    public int ID;
    public string m_name;
    public VectorPos m_pos;
    public string m_prefabPath;
    public ObjectType m_type;
    public string ideleAnimPath;
    public string runAnimPath;
}
/// <summary>
/// 玩家信息
/// </summary>
public class PlayerInfo:ObjectDataInfo
{
    public int m_level;
    public int m_HP;
    public int m_HPMAX;
    public int m_MP;
    public int m_MPMAX;
    List<SkillData> skillDatas = new List<SkillData>();
}
/// <summary>
/// NPC信息
/// </summary>
public class NPCInfo:ObjectDataInfo
{
    public int m_plotID = 0;//0不响应，1响应

    public NPCInfo(int plotID,ObjectDataInfo dataInfo)
    {
        m_plotID = plotID;
        m_name = dataInfo.m_name;
        m_pos = dataInfo.m_pos;
        m_prefabPath = dataInfo.m_prefabPath;
        m_type = ObjectType.NPC;
        
    }
}
/// <summary>
/// 怪物及其他类型角色信息
/// </summary>
public class MonsterInfo : ObjectDataInfo
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type">怪物类型，包括怪物、采集物、跟随物</param>
    /// <param name="dataInfo">对应的数据</param>
    public MonsterInfo(ObjectType type,ObjectDataInfo dataInfo)
    {
        ID = dataInfo.ID;
        m_name= dataInfo.m_name;
        m_pos= dataInfo.m_pos;
        m_prefabPath= dataInfo.m_prefabPath;
        m_type= type;
    }
}
public class SkillXml
{
    public string name;
    public Dictionary<string,List<string>> skillsDic=new Dictionary<string, List<string>>();
}
public class SkillBaseData
{
    public string skillName;
    public string clipPath;
    public float delayTime;
}
