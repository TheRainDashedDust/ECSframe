using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Move : SkillBase
{
    public Vector3 vPos;
    public Skill_Move(Role player) : base(player)
    {
    }

    public void SetClip(Vector3 vector3)
    {
        vPos = vector3;
    }

    public override void Play()
    {
        base.Play();
        if (player!=null)
        {
            player.gameObject.transform.Translate(vPos);
        }
        
    }

    public override void Stop()
    {
        base.Stop();
    }
}
