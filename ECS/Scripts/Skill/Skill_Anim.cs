using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Anim : SkillBase
{
    Animator animator;
    public AnimationClip animationClip;
    AnimatorOverrideController overrideController;
    public Skill_Anim(Role player) : base(player)
    {
        if (player!=null)
        {
            animator = player.gameObject.GetComponent<Animator>();
            overrideController = player.m_AnimatorOverrideController;
        }
    }

    public void SetClip(AnimationClip clip)
    {
        if (clip != null)
        {
            animationClip = clip;
            name = animationClip.name;
            overrideController["Skill1"] = animationClip;
        }
    }
    public void SetClip(AnimationClip clip,string animName)
    {
        if (clip != null&&animName!=null)
        {
            overrideController[animName] = clip;
        }
    }

    public override void Play()
    {
        base.Play();
        animator.StopPlayback();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle")|| stateInfo.IsName("Run"))
        {
            animator.SetTrigger("PlaySkill1");
        }
    }

    public override void Stop()
    {
        base.Stop();
        animator.StopPlayback();
    }
}
