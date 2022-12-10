using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;

public class RoleWindow : EditorWindow 
{
    //public static string assetSrtFileParh;
    public static string assetDstFolderPath = "Assets";
    RoleData m_roleData;
    Texture2D m_texture;
    Role m_player;
    GameObject prefab;
    Skill_Anim defaultAnimtion, runAnimtion;
    string skillName;
    bool foldout = true, naturefoldout=true;
    public static string fileName = string.Empty;
    
    [MenuItem("Tools/RoleWindow")]
    public static void Init()
    {
        RoleWindow window = EditorWindow.GetWindow<RoleWindow>();
        if (window!=null)
        {
            window.Show();
        }
    }
    private void OnEnable()
    {
        
        string path = assetDstFolderPath + "/" + fileName + ".json";
        if (File.Exists(path))
        {
            string str = File.ReadAllText(path);
            m_roleData=JsonConvert.DeserializeObject<RoleData>(str);
        }
        else
        {
            m_roleData = new RoleData();
        }
    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        m_texture=EditorGUILayout.ObjectField(m_texture,typeof(Texture2D),false,new GUILayoutOption[] {GUILayout.Width(100),GUILayout.Height(100)}) as Texture2D;
        if (m_texture!=null&&m_roleData.photoPath!= AssetDatabase.GetAssetPath(m_texture))
        {
            m_roleData.photoPath = AssetDatabase.GetAssetPath(m_texture);
        }
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("玩家名:");
        m_roleData.roleName=GUILayout.TextField(m_roleData.roleName,new GUILayoutOption[] {GUILayout.Width(160)});
        GUILayout.Space(20);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("角色类型:");
        m_roleData.roleType = (RoleType)EditorGUILayout.EnumPopup(m_roleData.roleType, new GUILayoutOption[] { GUILayout.Width(160) });
        GUILayout.Space(20);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("角色模型:");
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false, new GUILayoutOption[] { GUILayout.Width(160) }) as GameObject;
        InitPlayer();
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        if (prefab!=null&&m_player!=null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("待机动画:");
            defaultAnimtion.animationClip = EditorGUILayout.ObjectField(defaultAnimtion.animationClip, typeof(AnimationClip), false, new GUILayoutOption[] { GUILayout.Width(160) }) as AnimationClip;
            if (defaultAnimtion.animationClip != null && m_roleData.defaultAnimtionPath != AssetDatabase.GetAssetPath(defaultAnimtion.animationClip))
            {
                m_roleData.defaultAnimtionPath = AssetDatabase.GetAssetPath(defaultAnimtion.animationClip);

                defaultAnimtion.SetClip(defaultAnimtion.animationClip, "Idle");
            }
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("移动动画:");
            runAnimtion.animationClip = EditorGUILayout.ObjectField(runAnimtion.animationClip, typeof(AnimationClip), false, new GUILayoutOption[] { GUILayout.Width(160) }) as AnimationClip;
            if (runAnimtion.animationClip != null && m_roleData.runAnimtionPath != AssetDatabase.GetAssetPath(runAnimtion.animationClip))
            {
                m_roleData.runAnimtionPath = AssetDatabase.GetAssetPath(runAnimtion.animationClip);
                runAnimtion.SetClip(runAnimtion.animationClip, "Run");
            }
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }
        

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        naturefoldout= EditorGUILayout.Foldout(naturefoldout, "角色属性列表:");
        if (naturefoldout)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("生命值:");
            m_roleData.hp = EditorGUILayout.IntSlider(m_roleData.hp, 50, 200,new GUILayoutOption[] {GUILayout.Width(250)});
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("能量值:");
            m_roleData.mp = EditorGUILayout.IntSlider(m_roleData.mp, 2, 4, new GUILayoutOption[] { GUILayout.Width(250) });
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("魔抗值:");
            m_roleData.mk = EditorGUILayout.IntSlider(m_roleData.mk, 10, 20, new GUILayoutOption[] { GUILayout.Width(250) });
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("物抗值:");
            m_roleData.wk = EditorGUILayout.IntSlider(m_roleData.wk, 10, 20, new GUILayoutOption[] { GUILayout.Width(250) });
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.Label("技能名:");
        skillName = GUILayout.TextField(skillName, new GUILayoutOption[] { GUILayout.Width(120) });
        GUILayout.Space(50);
        if (GUILayout.Button("添加+"))
        {
            OpenSkillWindow(new SkillData(skillName));
        }
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        foldout = EditorGUILayout.Foldout(foldout,"技能列表:");
        if (foldout)
        {
            foreach (var item in m_roleData.skillDatas)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                if (GUILayout.Button(item.Key))
                {
                    OpenSkillWindow(item.Value);
                }
                GUILayout.Space(20);
                if (GUILayout.Button("删除"))
                {
                    m_roleData.skillDatas.Remove(item.Key);
                    break;
                }
                GUILayout.Space(20);
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(50);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择保存源文件夹");
        EditorGUILayout.TextField(assetDstFolderPath);
        if (GUILayout.Button("选择"))
        {
            assetDstFolderPath = EditorUtility.OpenFolderPanel("选择文件夹",assetDstFolderPath,"");
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.Label("角色文件名:");
        fileName = GUILayout.TextField(fileName, new GUILayoutOption[] { GUILayout.Width(160) });
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("保存"))
        {
            string str=JsonConvert.SerializeObject(m_roleData);
            string path = assetDstFolderPath + "/" + fileName + ".json";
            /*if (File.Exists(path))
            {
                GUILayout.Label("文件已存在,确认覆盖?");
                GUILayout.BeginHorizontal();
                GUILayout.Space(50);
                if (GUILayout.Button("确认"))
                {
                    File.WriteAllText(path,str);
                    Debug.Log("覆盖成功");
                }
                if (GUILayout.Button("取消"))
                {
                    fileName=string.Empty;
                }
                GUILayout.Space(50);
                GUILayout.EndHorizontal();
            }
            else
            {*/
                File.WriteAllText(path, str);
                Debug.Log("写入json成功");

            //}
        }
        
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
    }

    public void OpenSkillWindow(SkillData skillData)
    {
        SkillWindow window=EditorWindow.GetWindow<SkillWindow>("SkillWindow");
        if (window!=null)
        {
            window.Show();
            window.Init(skillData, m_player);
            window.roleWindow = this;
        }
    }
    public void InitPlayer()
    {
        if (prefab!=null&& m_roleData.prefabPath != AssetDatabase.GetAssetPath(prefab))
        {
            m_roleData.prefabPath=AssetDatabase.GetAssetPath(prefab);
            if (m_player!=null)
            {
                m_player.Destroy();
            }
            m_player = Role.Init(m_roleData);
            m_player.gameObject.transform.position = Vector3.zero;
            defaultAnimtion = new Skill_Anim(m_player);
            runAnimtion = new Skill_Anim(m_player);
        }
    }
    public void AddSkillData(SkillData skillData)
    {
        if (skillData!=null)
        {
            if (m_roleData.skillDatas.ContainsKey(skillData.skillName))
            {
                m_roleData.skillDatas[skillData.skillName] = skillData;
            }
            else
            {
                m_roleData.skillDatas.Add(skillData.skillName, skillData);
                skillName = string.Empty;
            }
            
        }
    }
    private void OnDisable()
    {
        if (m_player!=null)
        {
            m_player.Destroy();
        }
    }
}
