using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public Dictionary<int,string> buffIdToPath=new Dictionary<int, string>()
    {
        {1,"ZhuoShao" },
        {2,"BingDong" },
        {3,"LiuXue" },

    };
    static private BuffSystem _instance;
    static public BuffSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuffSystem();
            }
            return _instance;
        }
    }


    public List<int> buffs = new List<int>();


    public void AddBuff(PlayerObj p, int id)
    {
        if (!buffs.Contains(id))
        {
            buffs.Add(id);
        }

        p.AddBuff(buffIdToPath[id]);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
