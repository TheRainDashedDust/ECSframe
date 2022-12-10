using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SkillCD : MonoBehaviour
{
    public GameObject player, skillScope;
    public Image mask;
    SkillData skillData;
    bool cdflag = false;
    float timer;
    public bool isMask = false;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player!=null)
        {
            skillScope = new GameObject("skillScope");
            skillScope.AddComponent<MeshFilter>();
            skillScope.AddComponent<MeshRenderer>();
            skillScope.transform.SetParent(player.transform);
            skillScope.transform.localPosition = new Vector3(0,0.1f,0);
            skillScope.AddComponent<SkillScopeMesh>();
            skillScope.GetComponent<SkillScopeMesh>().archMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/ArtResources/Shader/ZTJJ_zhao.mat");
            skillScope.GetComponent<SkillScopeMesh>().Radius = 5;
            skillScope.GetComponent<SkillScopeMesh>().Thickness = 5;
            
            

        }
        if (mask==null && isMask)
        {
            if (transform.Find("SkillMask"))
            {
                mask = transform.Find("SkillMask").GetComponent<Image>();
                
            }
            else
            {
                mask = Instantiate(Resources.Load<Image>("SkillMask"),transform);
            }
            mask.fillAmount = 1;
            mask.transform.localPosition = Vector3.zero;
            mask.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
            return;
        }
        if (cdflag)
        {
            timer -= Time.deltaTime;
            if (mask!=null)
            {
                mask.fillAmount = timer / skillData.skillCD;
            }
            
            if (timer<=0)
            {
                if (mask!=null)
                {
                    mask.gameObject.SetActive(false);
                    mask.fillAmount = 1;
                }
                
                cdflag = false;
                
            }
        }

    }
    public void OnClickAckAction(int skillIndex)
    {
        if (!cdflag)
        {
            skillData = GameStart.Instance.OnAckAction(skillIndex);
            //SkillScopeMesh.ToDrawSectorSolid(player.transform,skillScope.transform.position,skillData.sectorScope,skillData.distanceScope);
            //SkillScopeMesh.go.transform.SetParent(skillScope.transform);
            //skillScope.GetComponent<SkillScopeMesh>().n = 24;
        }
        else
        {
            Debug.Log("技能冷却中");
        }
        
    }
    public void OnUpAction()
    {
        if (skillData!=null&& !cdflag)
        {
            if (mask!=null)
            {
                mask.gameObject.SetActive(true);
            }
            
            timer = skillData.skillCD;
            cdflag = true;
            
        }
    }
    
}
