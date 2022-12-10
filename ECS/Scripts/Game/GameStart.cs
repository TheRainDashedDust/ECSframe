using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameStart : MonoBehaviour
{
    public static GameStart Instance;
    public string playerfileName="Player";
    public string monsterFileName="Monster";
    public RoleData roleData;
    Role player;
    public string[] m_skillNames;
    public GameObject healthBar;
    public Transform healthParent;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitGame();
    }
    public void InitGame()
    {
        string path = "Assets/Resources/GameRole/" + playerfileName + ".json";
        if (File.Exists(path))
        {
            string str=File.ReadAllText(path);
            roleData=JsonConvert.DeserializeObject<RoleData>(str);
            player = Role.Init(roleData);
            player.InitGamePlayer();
            player.gameObject.tag = "Player";
            player.transform.position = Vector3.zero;
            player.AddComponent<BoxCollider>();
            player.GetComponent<BoxCollider>().center = new Vector3(0, 1, 0);
            player.GetComponent<BoxCollider>().size = new Vector3(1, 2, 1);
            player.AddComponent<Rigidbody>();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            m_skillNames = roleData.skillDatas.Keys.ToArray();
        }
        else
        {
            Debug.Log("角色文件路径为空");
        }
        string monsPath= "Assets/Resources/GameRole/" + monsterFileName + ".json";
        if (File.Exists(monsPath))
        {
            string str = File.ReadAllText(monsPath);
            RoleData monsterroleData = JsonConvert.DeserializeObject<RoleData>(str);
            for (int i = 0; i < 5; i++)
            {
                MonsterManager.Instance.CreateMonster(monsterroleData);
            }
        }
    }
    public SkillData OnAckAction(int skillIndex)
    {
        SkillData skillData=null;
        if (player!=null&&m_skillNames!=null && skillIndex < m_skillNames.Length)
        {
            if (roleData.skillDatas.ContainsKey(m_skillNames[skillIndex]))
            {
                player.Play(roleData.skillDatas[m_skillNames[skillIndex]]);
                skillData=roleData.skillDatas[m_skillNames[skillIndex]];
                
            }
            
        }
        return skillData;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
