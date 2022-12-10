using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum ObjectType
{
    Null=0,
    Normal,//怪物
    Gather,//采集物
    Biaoche,//跟随物
    NPC,//NPC
    
}
public abstract class ObjectBase
{
    public GameObject m_go;  // 存储当前物体
    public Vector3 m_local_pos;   //当前物体在本地的位置
    public Animator m_anim;
    public UIPate m_pate;
    public ObjectType m_type;

    public int m_insID;
    public string m_modelPath;  //模型路径

    public ObjectBase()
    {
    }
    public virtual void CreateObj(ObjectType type)
    {
        m_type = type;
        if (!string.IsNullOrEmpty(m_modelPath) && m_insID >= 0)
        {
            m_go = (GameObject)GameObject.Instantiate(Resources.Load(m_modelPath));
            m_go.name = m_insID.ToString();
            m_go.transform.position = m_local_pos;
            if (m_go)
            {
                OnCreate();
            }
        }
    }
    public virtual void OnCreate()
    {
        
        
    }
    public virtual void SetPos(Vector3 pos)
    {
        m_local_pos = pos;
    }
    public void MoveByTranslate(Vector3 look,Vector3 move)
    {
        m_go.transform.LookAt(look);
        m_go.transform.Translate(move);
    }

    public void AutoMove(Vector3 look,Vector3 move)
    {
        MoveByTranslate(look,move);
    }
    public virtual void Destory()
    {
        if (m_pate)
        {
            GameObject.Destroy(m_pate);
        }
        GameObject.Destroy(m_go);
        m_local_pos = Vector3.zero;
        m_anim = null;
        m_insID = -1;
    }
}
