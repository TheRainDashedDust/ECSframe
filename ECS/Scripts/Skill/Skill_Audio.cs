using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Audio : SkillBase
{
    AudioSource audioSource;
    public AudioClip audioclip;
    public Skill_Audio(Role player) : base(player)
    {
        audioSource=player.gameObject.GetComponent<AudioSource>();

    }
    public void SetClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioclip = audioClip;
            name = audioClip.name;
            audioSource.clip = audioclip;
            Stop();
        }
    }

    public override void Play()
    {
        base.Play();
        if (audioSource!=null)
        {
            audioSource.Play();
        }
    }

    public override void Stop()
    {
        base.Stop();
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
