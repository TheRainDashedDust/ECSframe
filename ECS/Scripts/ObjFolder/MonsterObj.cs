using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObj : ObjectBase
{
    public MonsterInfo m_info;
    public MonsterObj(ObjectType type,MonsterInfo info)
    {
        m_info.m_type = type;
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_prefabPath;
    }
    public override void OnCreate()
    {
        base.OnCreate();
    }
}
public class Normal : MonsterObj
{
    public Normal(MonsterInfo info):base(ObjectType.Normal,info)
    {

    }
    public Normal(ObjectDataInfo info) : base(ObjectType.Normal,new MonsterInfo(ObjectType.Normal,info))
    {
    }
    public override void CreateObj(ObjectType type)
    {
        SetPos(m_info.m_pos.GetVector3());
        base.CreateObj(type);

    }
    public override void OnCreate()
    {
        base.OnCreate();

        

    }
}
public class Gather : MonsterObj
{
    public Gather(MonsterInfo info) : base(ObjectType.Gather, info)
    {

    }
    public Gather(ObjectDataInfo info) : base(ObjectType.Gather, new MonsterInfo(ObjectType.Gather, info))
    {
    }
    public override void CreateObj(ObjectType type)
    {
        SetPos(m_info.m_pos.GetVector3());
        base.CreateObj(type);
    }
    public override void OnCreate()
    {
        base.OnCreate();

    }

}
public class Biaoche:MonsterObj
{
    public Biaoche(MonsterInfo info) : base(ObjectType.Biaoche, info)
    {

    }
    public Biaoche(ObjectDataInfo info) : base(ObjectType.Biaoche, new MonsterInfo(ObjectType.Biaoche, info))
    {
    }
    public override void CreateObj(ObjectType type)
    {
        SetPos(m_info.m_pos.GetVector3());
        base.CreateObj(type);
    }
    public override void OnCreate()
    {
        base.OnCreate();
        /*StaticCircleCheck check=m_go.AddComponent<StaticCircleCheck>();
        check.m_call = (isenter) =>
        {

        };*/
    }
}