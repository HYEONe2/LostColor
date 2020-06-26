using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorChange_sc : MonoBehaviour
{
    public enum STAGE { STAGE_1, STAGE_2, STAGE_3, STAGE_END};

    [SerializeField] private bool IsParent = true;
    [SerializeField] private Color color;
    public STAGE CheckStage = STAGE.STAGE_END;

    private GameObject ClearCamera1;

    private Transform[] m_transformArr;
    private STAGE m_Stage = STAGE.STAGE_END;
    private Shader shader;

    private float m_fTime = 0f;
    private float m_fChangeTime = 0.5f;

    private string SceneName= "";
    private bool[] m_StageChecked = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        ClearCamera1 = GameObject.Find("ClearCamera1");
        m_fChangeTime = Random.Range(1, 5) * 0.25f;

        if (IsParent)
        {
            m_transformArr = new Transform[transform.childCount];
            int i = 0;

            foreach (Transform mr in transform)
            {
                m_transformArr[i++] = mr;
            }
        }

        for (int i = 0; i < 3; ++i)
            m_StageChecked[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("1"))
        //    m_Stage = STAGE.STAGE_1;
        //if (Input.GetKeyDown("2"))
        //    m_Stage = STAGE.STAGE_2;
        //if (Input.GetKeyDown("3"))
        //    m_Stage = STAGE.STAGE_3;

        m_Stage = STAGE.STAGE_2;

        if (CheckScene())
            return;

        if (ClearCamera1.GetComponent<Camera>().enabled == false)
            return;

        ChangeColor();
    }

    bool CheckScene()
    {
        SceneName = SceneManager.GetActiveScene().name;

        if (SceneName == "MainStage")
            return false;
        else if (SceneName == "Stage_1")
        {
            m_StageChecked[0] = true;
            m_StageChecked[1] = false;
            m_StageChecked[2] = false;
        }
        else if (SceneName == "Stage_2")
        {
            m_StageChecked[0] = false;
            m_StageChecked[1] = true;
            m_StageChecked[2] = false;
        }
        else if (SceneName == "Stage_3")
        {
            m_StageChecked[0] = false;
            m_StageChecked[1] = false;
            m_StageChecked[2] = true;
        }

        if (m_StageChecked[0])
            m_Stage = STAGE.STAGE_1;
        else if (m_StageChecked[1])
            m_Stage = STAGE.STAGE_2;
        else if (m_StageChecked[2])
            m_Stage = STAGE.STAGE_3;

        return true;
    }

    void ChangeColor()
    {
        if (m_Stage == CheckStage && TargetInScreen_Direction())
        {
            if (IsParent)
            {
                for (int i = 0; i < transform.childCount; ++i)
                {
                    if (m_transformArr[i].GetComponent<MeshRenderer>().material != null)
                    {
                        m_transformArr[i].GetComponent<MeshRenderer>().material.color = color;
                    }
                    else
                    {
                        Material material = new Material(shader);
                        material.color = color;
                        m_transformArr[i].GetComponent<MeshRenderer>().material = material;
                    }
                }
            }
            else
            {
                transform.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }

    bool TargetInScreen_Direction()
    {
        // 카메라를 기준으로 타겟의 로컬 좌표를 구한다.
        Vector3 localTargetPos = ClearCamera1.transform.InverseTransformPoint(transform.position);
        // 카메라의 전방
        Vector3 forward = ClearCamera1.transform.TransformDirection(Vector3.forward);
        // 타겟과 카메라의 바라보는 방향
        Vector3 toDir = transform.position - ClearCamera1.transform.position;

        // 내적: 0보다 크면 전방 180도 내부에 있다는뜻 0보다 작으면 뒤로 180도에 있다
        float dot = Vector3.Dot(forward, toDir);
        if (dot > 0f) // 내적이 0보다 크다는건 전방에 있다는 뜻
        {
            m_fTime += Time.deltaTime;
            if(m_fTime> m_fChangeTime)
                return true;
        }

        //Debug.Log(dot);

        return false;
    }
}

//https://dorasol.tistory.com/5