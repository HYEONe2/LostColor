using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange_sc : MonoBehaviour
{
    public enum STAGE { STAGE_1, STAGE_2, STAGE_3, STAGE_END};

    [SerializeField] private bool IsParent = true;
    [SerializeField] private Color color;
    public STAGE CheckStage = STAGE.STAGE_END;

    private GameObject ClearCamera1;
    private bool m_bCanChange = false;

    private Transform[] m_transformArr;
    private STAGE m_Stage = STAGE.STAGE_END;
    private Shader shader;

    private float m_fTime = 0f;
    private float m_fChangeTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        ClearCamera1 = GameObject.Find("ClearCamera1");
        m_fChangeTime = Random.Range(1, 5) * 0.2f;

        if (IsParent)
        {
            m_transformArr = new Transform[transform.childCount];
            int i = 0;

            foreach (Transform mr in transform)
            {
                m_transformArr[i++] = mr;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
            m_Stage = STAGE.STAGE_1;
        if (Input.GetKeyDown("2"))
            m_Stage = STAGE.STAGE_2;
        if (Input.GetKeyDown("3"))
            m_Stage = STAGE.STAGE_3;

        m_Stage = STAGE.STAGE_2;

        if (ClearCamera1.GetComponent<Camera>().enabled == false)
            return;

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


    public bool TargetInScreen_Direction()
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