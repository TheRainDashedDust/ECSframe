using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public Dictionary<string, List<SkillXml>> AllRoleSkillList = new Dictionary<string, List<SkillXml>>();
    public void InitByRoleName(string roleName)
    {
        /*if (File.Exists("Assets/Resources/GameRole/"+roleName+".json"))
        {
            string str = File.ReadAllText("Assets/Resources/GameRole/"+roleName+".json");
            List<SkillXml> skills = JsonConvert.DeserializeObject<List<SkillXml>>(str);

            AllRoleSkillList.Add(roleName, skills);
        }*/
        //数据要改
    }
    public List<SkillXml> GetSkillsByRoleName(string roleName)
    {
        if (AllRoleSkillList.ContainsKey(roleName))
        {
            return AllRoleSkillList[roleName];
        }

        return null;
    }

    public Dictionary<int, TaskData> AllTaskDic = new Dictionary<int, TaskData>();
    public void InitTaskData()
    {
        TaskData task = new TaskData();
        task.taskId = 1;
        task.taskName = "任务1";
        AllTaskDic.Add(task.taskId, task);
    }
    public TaskData GetTaskDataById(int taskId)
    {
        if (AllTaskDic.ContainsKey(taskId))
        {
            return AllTaskDic[taskId];
        }
        return null;
    }
}
public class TaskData
{
    public int taskId;
    public string taskName;
}
