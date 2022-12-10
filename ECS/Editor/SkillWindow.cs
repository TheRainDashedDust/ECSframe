using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillWindow : EditorWindow
{
    public RoleWindow roleWindow;
    SkillData m_skillData;
    Role m_player;
    ClipType m_clipType;
    bool foldout = true;
    List<SkillBase> m_skillList;
    public void Init(SkillData skillData,Role player)
    {
        if (player!=null)
        {
            m_player = player;
            if (skillData != null)
            {
                m_skillData = skillData;
                m_skillList=skillData.GetSkillBases();
            }
            else
            {
                Debug.Log("技能信息不能为空");
                this.Close();
            }
        }
        else
        {
            Debug.Log("角色对象不能为空");
            this.Close();
            return;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("技能名:");
        m_skillData.skillName = GUILayout.TextField(m_skillData.skillName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能攻击距离范围");
        m_skillData.distanceScope = EditorGUILayout.Slider(m_skillData.distanceScope, 0,100,new GUILayoutOption[] {GUILayout.Width(250)});
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能攻击扇形范围");
        m_skillData.sectorScope = EditorGUILayout.Slider(m_skillData.sectorScope, 0, 360, new GUILayoutOption[] { GUILayout.Width(250) });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能攻击力");
        m_skillData.aTK = EditorGUILayout.IntSlider(m_skillData.aTK, 0, 360, new GUILayoutOption[] { GUILayout.Width(250) });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能攻击伤害延迟");
        m_skillData.aTKdelayTime = EditorGUILayout.FloatField(m_skillData.aTKdelayTime, new GUILayoutOption[] { GUILayout.Width(100) });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能CD");
        m_skillData.skillCD = EditorGUILayout.FloatField(m_skillData.skillCD, new GUILayoutOption[] { GUILayout.Width(100) });
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("技能片段类型:");
        m_clipType = (ClipType)EditorGUILayout.EnumPopup(m_clipType);
        if (GUILayout.Button("添加"))
        {
            switch (m_clipType)
            {
                case ClipType.None:
                    break;
                case ClipType.动画:
                    m_skillList.Add(new Skill_Anim(m_player));
                    break;
                case ClipType.声音:
                    m_skillList.Add(new Skill_Audio(m_player));
                    break;
                case ClipType.特效:
                    m_skillList.Add(new Skill_Effect(m_player));
                    break;
                case ClipType.位移:
                    m_skillList.Add(new Skill_Move(m_player));
                    break;
                default:
                    break;
            }
        }
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        foldout = EditorGUILayout.Foldout(foldout, "技能片段列表");
        if (foldout)
        {
            for (int i = m_skillList.Count-1; i >=0; i--)
            {
                GUILayout.BeginHorizontal();
                switch (m_skillList[i])
                {
                    case Skill_Anim:
                        ShowSkillAnim(m_skillList[i] as Skill_Anim);
                        break;
                    case Skill_Audio:
                        ShowSkillAudio(m_skillList[i] as Skill_Audio);
                        break;
                    case Skill_Effect:
                        ShowSkillEffect(m_skillList[i] as Skill_Effect);
                        break;
                    case Skill_Move:
                        ShowSkillMove(m_skillList[i] as Skill_Move);
                        break;
                    default:
                        break;
                }
                if (GUILayout.Button("删除"))
                {
                    m_skillList.RemoveAt(i);
                    break;
                }
                GUILayout.EndHorizontal();

            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("预览"))
        {
            m_skillData.SetSkillBases(m_skillList);
            m_player.Play(m_skillData);
        }
        if (GUILayout.Button("保存"))
        {
            m_skillData.SetSkillBases(m_skillList);
            roleWindow.AddSkillData(m_skillData);
            this.Close();
        }
        GUILayout.EndHorizontal();
    }
    public void ShowSkillAnim(Skill_Anim _Anim)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("动画:");
        _Anim.animationClip=EditorGUILayout.ObjectField(_Anim.animationClip,typeof(AnimationClip),false,new GUILayoutOption[] {}) as AnimationClip;
        if (_Anim.entityPath!=AssetDatabase.GetAssetPath(_Anim.animationClip)&& _Anim.animationClip!=null)
        {
            _Anim.entityPath = AssetDatabase.GetAssetPath(_Anim.animationClip);
        }
        if (_Anim.animationClip!=null)
        {
            GUILayout.Label("动画时长:"+_Anim.animationClip.length.ToString());
        }
        GUILayout.Label("延迟:");
        _Anim.delayTime = EditorGUILayout.FloatField(_Anim.delayTime);

        GUILayout.EndHorizontal();
    }
    public void ShowSkillAudio(Skill_Audio _Audio)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("音效:");
        _Audio.audioclip = EditorGUILayout.ObjectField(_Audio.audioclip, typeof(AudioClip), false, new GUILayoutOption[] { }) as AudioClip;
        if (_Audio.entityPath != AssetDatabase.GetAssetPath(_Audio.audioclip) && _Audio.audioclip != null)
        {
            _Audio.entityPath = AssetDatabase.GetAssetPath(_Audio.audioclip);
        }
        GUILayout.Label("延迟:");
        _Audio.delayTime = EditorGUILayout.FloatField(_Audio.delayTime);
        GUILayout.EndHorizontal();
    }
    public void ShowSkillEffect(Skill_Effect _Effect)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("特效:");
        _Effect.prefab = EditorGUILayout.ObjectField(_Effect.prefab, typeof(GameObject), false, new GUILayoutOption[] { }) as GameObject;
        if (_Effect.entityPath != AssetDatabase.GetAssetPath(_Effect.prefab) && _Effect.prefab != null)
        {
            _Effect.entityPath = AssetDatabase.GetAssetPath(_Effect.prefab);
        }
        GUILayout.Label("延迟:");
        _Effect.delayTime = EditorGUILayout.FloatField(_Effect.delayTime);
        GUILayout.EndHorizontal();
    }
    public void ShowSkillMove(Skill_Move _Move)
    {
        GUILayout.BeginHorizontal();
        _Move.vPos=EditorGUILayout.Vector3Field("位移:",_Move.vPos);
        GUILayout.Label("延迟:");
        _Move.delayTime = EditorGUILayout.FloatField(_Move.delayTime);
        GUILayout.EndHorizontal();
    }
}
