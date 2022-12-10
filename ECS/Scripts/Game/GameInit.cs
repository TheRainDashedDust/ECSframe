using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInit : MonoBehaviour
{
    public GameObject[] DontDestory;
    public List<ETCButton> attack;
    public ETCJoystick joystick;
    public GameData uiroot;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < DontDestory.Length; i++)
        {
            GameObject.DontDestroyOnLoad(DontDestory[i]);
        }
            GameSceneUtils.LoadSceneAsync("GameScene", () => {
            JoyStickMgr.Instance.m_joyGO = DontDestory[0];
            JoyStickMgr.Instance.m_joystick = joystick;
            JoyStickMgr.Instance.m_skillBtn = attack;

            //配置数据解析
            GameData.Instance.InitByRoleName("Player");
            //任务配置表解析 C端
            GameData.Instance.InitTaskData();

            World.Instance.Init();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

