using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public string skillName;
    List<SkillBase> skillBaseList;
    public List<string> skillClipPath;
    public List<float> delayTimes;
    public float x = -1, y = -1, z = -1;
    public float distanceScope, sectorScope;
    public int aTK;
    public float aTKdelayTime;
    public float skillCD;
    public SkillData(string skillName)
    {
        this.skillName = skillName;
        skillBaseList = new List<SkillBase>();
        skillClipPath = new List<string>();
        delayTimes = new List<float>();
    }
    public void SetSkillBases(List<SkillBase> skillBases)
    {
        if (skillBases != null)
        {
            skillBaseList = skillBases;
            if (delayTimes.Count >= skillBaseList.Count)
            {
                delayTimes.Clear();
            }

            if (skillClipPath.Count >= skillBaseList.Count)
            {
                skillClipPath.Clear();
            }
            foreach (var item in skillBaseList)
            {
                skillClipPath.Add(item.entityPath);
                delayTimes.Add(item.delayTime);
                if (item is Skill_Move)
                {
                    Vector3 pos = (item as Skill_Move).vPos;
                    x = pos.x;
                    y = pos.y;
                    z = pos.z;
                }
            }
        }
    }
    public List<SkillBase> GetSkillBases()
    {


        return skillBaseList;
    }
}
