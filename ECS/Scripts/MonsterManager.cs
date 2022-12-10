using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager:Singleton<MonsterManager>
{
    public Dictionary<int,GameObject> mons=new Dictionary<int, GameObject>();
    public void CreateMonster(RoleData roleData)
    {
        Role role = Role.Init(roleData);
        role.gameObject.tag = "Monster";
        role.InitGamePlayer();

        Vector2 vector2 = Random.insideUnitCircle * 50;
        role.transform.position = new Vector3(vector2.x, 1, vector2.y);
        if (!role.GetComponent<BoxCollider>())
        {
            role.AddComponent<BoxCollider>();
        }
        
        role.GetComponent<BoxCollider>().center = new Vector3(0, 1, 0);
        role.GetComponent<BoxCollider>().size = new Vector3(1, 2, 1);
        role.AddComponent<Rigidbody>();
        role.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        mons.Add(role.gameObject.GetInstanceID(),role.gameObject);
        
    }
    public List<GameObject> GetMonstersAtScope(GameObject player,float distanceScope,float sectorScope)
    {
        List<GameObject> monsters = new List<GameObject>();
        foreach (var item in mons)
        {
            if (Vector3.Distance(player.transform.position,item.Value.transform.position)<= distanceScope&&Vector3.Angle(player.transform.forward,item.Value.transform.position-player.transform.position)<= sectorScope/2)
            {
                monsters.Add(item.Value);
            }
        }
        return monsters;
    }
    public void MonsterDeath(GameObject monster)
    {
        if (mons.ContainsKey(monster.GetInstanceID()))
        {
            mons.Remove(monster.GetInstanceID());
            monster.GetComponent<Role>().GameDestroy();
        }
    }
}
