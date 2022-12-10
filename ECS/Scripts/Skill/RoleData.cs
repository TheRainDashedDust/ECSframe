using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

public enum RoleType
{
    None,玩家,怪物,NPC,
}
public enum ClipType
{
    None,动画,声音,特效,位移,
}
public class RoleData
{
    public string roleName;
    public string photoPath;
    public RoleType roleType;
    public string prefabPath;
    public string defaultAnimtionPath;
    public string runAnimtionPath;
    public int hp, mp;
    public int wk, mk;
    public Dictionary<string,SkillData> skillDatas=new Dictionary<string, SkillData>();
}