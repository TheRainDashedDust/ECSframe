using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase
{
    public string name=string.Empty;
    public string entityPath=string.Empty;
    public float delayTime = 0;
    public Role player;
  
    protected SkillBase(Role player)
    {
        this.player = player;
    }

    public virtual void Init()
    {

    }
    public virtual void Play()
    {

    }
    public virtual void Stop()
    {

    }
    public virtual void SetDelayTime(float time)
    {
        delayTime = time;
    }
    public virtual void SetEntityPath(string path)
    {
        entityPath = path;
    }
    

}
