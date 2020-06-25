using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMgr_sc : MonoBehaviour
{
    public static ObjectMgr_sc Instance;

    public enum OBJECT { OBJ_PLAYER, OBJ_BOSS, OBJ_SHIELD, OBJ_END};

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CountOfObject(OBJECT eObject)
    {
        switch(eObject)
        {
            case OBJECT.OBJ_PLAYER:
                return 1;
            case OBJECT.OBJ_BOSS:
                return 1;
            case OBJECT.OBJ_SHIELD:
                return GameObject.FindGameObjectsWithTag("Shield").Length;
            case OBJECT.OBJ_END:
                break;
        }

        return 0;
    }
}
