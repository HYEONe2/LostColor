using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAttack_sc : MonoBehaviour
{
    GameObject Target;
    GameObject MonsterHand;
    Vector3 TargetPos;
    public static bool bIsMove = true;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        MonsterHand = GameObject.Find("Index_Proximal_R");
        TargetPos = Target.transform.position;
        Destroy(gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsMove)
        {
            //Debug.Log("발사");
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, 1.0f);
            transform.Rotate(new Vector3(0, 0, 3));
        }
        else
        {
            //Debug.Log("이동");
            transform.position = MonsterHand.transform.position;
        }
    }
}
