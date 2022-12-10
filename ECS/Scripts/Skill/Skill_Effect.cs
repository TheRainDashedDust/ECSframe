using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Effect : SkillBase
{
    public GameObject prefab;
    ParticleSystem[] particle;
    GameObject game;
    
    public Skill_Effect(Role player) : base(player)
    {
    }
    
    public void SetClip(GameObject clip)
    {
        if (clip != null && clip.GetComponentInChildren<ParticleSystem>())
        {
            if (game != null)
            {
                GameObject.DestroyImmediate(game.gameObject);
            }
            
            game = GameObject.Instantiate(clip, player.transform);
            game.transform.localPosition = Vector3.zero;
            /*if (!game.GetComponent<ParticleSystem>())
            {
                game.AddComponent<ParticleSystem>();
                
            }*/
            particle = game.GetComponentsInChildren<ParticleSystem>();
            Stop();
        }
    }
    public override void Play()
    {
        base.Play();

        if (particle!=null)
        {
            foreach (var item in particle)
            {
                item.Play();
            }
            //particle.Play();
        }
    }

    public override void Stop()
    {
        base.Stop();
        if (particle != null)
        {
            foreach (var item in particle)
            {
                item.Stop();
            }
            //particle.Stop();
        }
    }
}
