using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public float xMap, yMap;
    public float xoffset, yoffset;

    private Transform player;
    Dictionary<ObjectType, Transform> monsterdic = new Dictionary<ObjectType, Transform>();

    List<ObjectBase> otherGoPos = new List<ObjectBase>();
    Vector3 playerpos = new Vector3(0, 0, 0);
    List<Vector3> otherpos = new List<Vector3>();

    private void Awake()
    {
        xMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        yMap = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        xoffset = xMap / World.Instance.xlength;
        yoffset = xMap / World.Instance.ylength;

        player = transform.Find("player");
        monsterdic.Add(ObjectType.Gather, transform.Find("gather"));
        monsterdic.Add(ObjectType.Normal, transform.Find("monster"));
        monsterdic.Add(ObjectType.NPC, transform.Find("npc"));

    }

    void Update()
    {
        if (World.Instance.m_insDic.Count != otherGoPos.Count)
        {
            otherGoPos.Clear();
            otherpos.Clear();
            foreach (var item in World.Instance.m_insDic)
            {
                otherGoPos.Add(item.Value);
                otherpos.Add(new Vector3(0, 0, 0));
            }
        }
        if (player && World.Instance.m_player.m_go)
        {
            playerpos.Set(World.Instance.m_player.m_go.transform.position.x * xoffset, World.Instance.m_player.m_go.transform.position.z * yoffset, 0);
            //最终位置  x:{World.Instance.m_plyer.m_go.transform.position.x * xoffset}  y:{World.Instance.m_plyer.m_go.transform.position.z * yoffset};
            player.localPosition = playerpos;
        }
        if (otherGoPos != null && otherGoPos.Count > 0)
        {
            for (int i = 0; i < otherGoPos.Count; i++)
            {
                otherpos[i] = new Vector3(otherGoPos[i].m_go.transform.position.x * xoffset, otherGoPos[i].m_go.transform.position.z * yoffset, 0);
                monsterdic[otherGoPos[i].m_type].transform.localPosition = otherpos[i];
            }
        }

    }

    private void OnDestroy()
    {
        CancelInvoke("UpdateMap");
    }
}
