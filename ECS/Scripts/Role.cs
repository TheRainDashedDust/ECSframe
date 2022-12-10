using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

public class Role : MonoBehaviour
{
    public Dictionary<string, SkillData> m_Skills = new Dictionary<string, SkillData>();
    public Animator m_Animator;
    public AnimatorOverrideController m_AnimatorOverrideController;
    public RuntimeAnimatorController m_RuntimeAnimatorController;
    public AudioSource m_AudioSource;
    public RoleData m_RoleData=new RoleData();
    public RoleState roleState;
    public string[] skillNames;
    public HealthBar healthBar;
    public RectTransform hpBar;
    public static Role Init(RoleData roleData)
    {
        Role player =null;
        if (roleData!=null && roleData.prefabPath!=null)
        {
            GameObject go=AssetDatabase.LoadAssetAtPath<GameObject>(roleData.prefabPath);
            if (!go.GetComponent<HealthBar>())
            {
                go.AddComponent<HealthBar>().enabled=false;
            }
            if (go!=null)
            {
                GameObject play = Instantiate(go);
                play.transform.localScale = Vector3.one;
                
                if (play!=null)
                {
                    if (!play.GetComponent<Role>())
                    {
                        play.AddComponent<Role>();
                    }
                    player=play.GetComponent<Role>();
                    if (!play.GetComponent<Animator>())
                    {
                        play.AddComponent<Animator>();
                    }
                    player.m_Animator = play.GetComponent<Animator>();
                    if (!play.GetComponent<AudioSource>())
                    {
                        play.AddComponent<AudioSource>();
                    }
                    player.m_AudioSource = play.GetComponent<AudioSource>();
                    if (!play.GetComponent<RoleState>())
                    {
                        play.AddComponent<RoleState>();
                    }
                    player.roleState=play.GetComponent<RoleState>();
                    player.m_RoleData = roleData;
                    player.healthBar = play.GetComponent<HealthBar>();
                    play.name = roleData.roleName;
                    player.m_AnimatorOverrideController = new AnimatorOverrideController();
                    player.m_RuntimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Player");
                    player.m_AnimatorOverrideController.runtimeAnimatorController = player.m_RuntimeAnimatorController;
                    player.m_Animator.runtimeAnimatorController = player.m_AnimatorOverrideController;
                }
            }
        }
        return player;
    }
    public void InitData()
    {

    }
    public void InitSkill(SkillBase skill)
    {
        switch (skill)
        {
            case Skill_Anim:
                if (skill.entityPath!=""&&skill.entityPath!=null)
                {
                    AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(skill.entityPath);
                    (skill as Skill_Anim).SetClip(clip);
                }
                break;
            case Skill_Audio:
                if (skill.entityPath != "" && skill.entityPath != null)
                {
                    AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(skill.entityPath);
                    (skill as Skill_Audio).SetClip(clip);
                }
                break;
            case Skill_Move:
                if ((skill as Skill_Move).vPos!=null)
                {
                    (skill as Skill_Move).SetClip((skill as Skill_Move).vPos);
                }
                break;
            case Skill_Effect:
                if (skill.entityPath != "" && skill.entityPath != null)
                {
                    GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>(skill.entityPath);
                    (skill as Skill_Effect).SetClip(clip); 
                }
                break;
            default:
                break;
        }
    }
    public void InitGamePlayer()
    {
        
        if (m_RoleData !=null)
        {
            foreach (var item in m_RoleData.skillDatas)
            {
                if (item.Value.skillClipPath!=null)
                {
                    SkillData skillData = new SkillData(item.Key);
                    for (int i = 0; i < item.Value.skillClipPath.Count; i++)
                    {
                        Object go = AssetDatabase.LoadAssetAtPath<Object>(item.Value.skillClipPath[i]);
                        switch (go)
                        {
                            case AnimationClip:
                                Skill_Anim skill_Anim = new Skill_Anim(this);
                                skill_Anim.SetClip(go as AnimationClip);
                                skill_Anim.SetDelayTime(item.Value.delayTimes[i]);
                                skill_Anim.SetEntityPath(item.Value.skillClipPath[i]);
                                skillData.GetSkillBases().Add(skill_Anim);
                                break;
                            case AudioClip:
                                Skill_Audio skill_Audio = new Skill_Audio(this);
                                skill_Audio.SetClip(go as AudioClip);
                                skill_Audio.SetDelayTime(item.Value.delayTimes[i]);
                                skill_Audio.SetEntityPath(item.Value.skillClipPath[i]);
                                skillData.GetSkillBases().Add(skill_Audio);
                                break;
                            case GameObject:
                                Skill_Effect skill_Effect = new Skill_Effect(this);
                                skill_Effect.SetClip(go as GameObject);
                                skill_Effect.SetDelayTime(item.Value.delayTimes[i]);
                                skill_Effect.SetEntityPath(item.Value.skillClipPath[i]);
                                skillData.GetSkillBases().Add(skill_Effect);
                                break;
                            case null:
                                if (item.Value.x!=-1&&item.Value.y!=-1&&item.Value.z!=-1)
                                {
                                    Skill_Move skill_Move = new Skill_Move(this);
                                    skill_Move.SetClip(new Vector3(item.Value.x, item.Value.y, item.Value.z));
                                    skill_Move.SetDelayTime(item.Value.delayTimes[i]);
                                    skillData.GetSkillBases().Add(skill_Move);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    skillData.skillClipPath = item.Value.skillClipPath;
                    skillData.delayTimes = item.Value.delayTimes;
                    skillData.x= item.Value.x;
                    skillData.y= item.Value.y;
                    skillData.z= item.Value.z;
                    skillData.distanceScope=item.Value.distanceScope;
                    skillData.sectorScope=item.Value.sectorScope;
                    skillData.aTK= item.Value.aTK;
                    skillData.aTKdelayTime = item.Value.aTKdelayTime;
                    skillData.skillCD = item.Value.skillCD;
                    if (m_Skills.ContainsKey(skillData.skillName))
                    {
                        m_Skills[skillData.skillName] = skillData;
                    }
                    else
                    {
                        m_Skills.Add(skillData.skillName, skillData);
                    }

                }
                
            }
            m_RoleData.skillDatas = m_Skills;
            skillNames = m_Skills.Keys.ToArray();
            if (m_RoleData.defaultAnimtionPath!=null&& AssetDatabase.LoadAssetAtPath<Object>(m_RoleData.defaultAnimtionPath)!=null)
            {
                Skill_Anim skill_Anim = new Skill_Anim(this);
                skill_Anim.animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(m_RoleData.defaultAnimtionPath);
                skill_Anim.SetClip(skill_Anim.animationClip,"Idle");
            }
            if (m_RoleData.runAnimtionPath != null && AssetDatabase.LoadAssetAtPath<Object>(m_RoleData.runAnimtionPath) != null)
            {
                Skill_Anim skill_Anim = new Skill_Anim(this);
                skill_Anim.animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(m_RoleData.runAnimtionPath);
                skill_Anim.SetClip(skill_Anim.animationClip, "Run");
            }

            roleState.hp = m_RoleData.hp;
            roleState.mp = m_RoleData.mp;
            roleState.wk = m_RoleData.wk;
            roleState.mk = m_RoleData.mk;
            
            
            /*healthBar.healthLink = new RemoteIntHealth();
            healthBar.healthLink.targetScript = GetComponent<RoleState>();
            healthBar.HealthbarPrefab = AssetDatabase.LoadAssetAtPath<RectTransform>("Assets/Plugins/X-BARS/Prefabs/HealthBarExample.prefab");
            healthBar.healthLink.fieldName = "hp";
            healthBar.yOffset = 2;
            healthBar.showHealthInfo = true;
            healthBar.enabled = true;*/
            
        }
        
    }
    public void SetData(int skillid)
    {
        if (skillNames[skillid]!=null&&skillid<skillNames.Length)
        {
            Play(m_Skills[skillNames[skillid]]);
        }
    }

    public void Play(SkillData skillData)
    {
        if (skillData!=null)
        {
            List<SkillBase> bases = skillData.GetSkillBases();
            if (skillData.skillCD==0)
            {
                Attack(skillData);
            }
            else
            {
                StartCoroutine("Attack", skillData);
            }
            
            
            foreach (var item in bases)
            {
                StartCoroutine("PlayerSkill", item);
            }
        }
    }
    
    public IEnumerator PlayerSkill(SkillBase skill)
    {
        yield return new WaitForSeconds(skill.delayTime);
        InitSkill(skill);

        skill.Play();
    }
    public IEnumerator Attack(SkillData skillData)
    {
        yield return new WaitForSeconds(skillData.aTKdelayTime);
        List<GameObject> mons = MonsterManager.Instance.GetMonstersAtScope(gameObject, skillData.distanceScope, skillData.sectorScope);
        for (int i = 0; i < mons.Count; i++)
        {
            if (mons[i]!=null)
            {
                mons[i].GetComponent<RoleState>().hp -= (skillData.aTK- (mons[i].GetComponent<RoleState>().wk*2))>0? (skillData.aTK - (mons[i].GetComponent<RoleState>().wk*2)):0;
                if (mons[i].GetComponent<RoleState>().hp <= 0)
                {
                    MonsterManager.Instance.MonsterDeath(mons[i]);

                    mons = MonsterManager.Instance.GetMonstersAtScope(gameObject, skillData.distanceScope, skillData.sectorScope);
                    break;
                }
            }
        }
    }

    

    public void Destroy()
    {
        DestroyImmediate(gameObject);
        
    }
    public void GameDestroy()
    {
        Destroy(gameObject);
        /*if (hpBar != null)
        {
            hpBar.gameObject.SetActive(false);
        }*/
    }
    Vector3 nowPos;
    // Start is called before the first frame update
    void Start()
    {
        nowPos = transform.position;
        /*if (healthBar.HealthbarPrefab!=null)
        {
            hpBar = healthBar.HealthbarPrefab;
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (healthBar==null||!healthBar.enabled)
        {
            return;
        }
        if (healthBar.HealthbarPrefab.name== "HealthBar")
        {
            hpBar = healthBar.HealthbarPrefab;
        }
        else
        {
            return;
        }*/
    }
    
    private void FixedUpdate()
    {
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
            return;
        }
            
        if (Vector3.Distance(nowPos,transform.position)>0f)
        {
            if (!m_Animator.GetBool("Move"))
                m_Animator.SetBool("Move", true);
        }
        else
        {
            if (m_Animator.GetBool("Move"))
            {
                m_Animator.SetBool("Move", false);
            }
            
        }
        nowPos = transform.position;
    }
}
