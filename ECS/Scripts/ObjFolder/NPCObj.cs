using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObj : ObjectBase
{
    public NPCInfo m_info;

    public NPCObj(NPCInfo info)
    {
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_prefabPath;
    }
    public NPCObj(int plot,ObjectDataInfo info)
    {
        m_info=new NPCInfo(plot,info);
        m_insID=info.ID;
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
        /*StaticCircleCheck check =m_go.AddComponent<StaticCircleCheck>();
        check.m_call = (isender) =>
        {

        };*/
    }
}
