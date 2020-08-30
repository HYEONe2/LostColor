using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorChange_sc : MonoBehaviour
{
    public enum STAGE { STAGE_1, STAGE_2, STAGE_3, STAGE_ENDING, STAGE_END};

    [SerializeField] private bool IsParent = true;
    [SerializeField] private Color color;
    public STAGE CheckStage = STAGE.STAGE_END;

    private Material SkyboxMaterial;
    private Stage_Manager StageManager;
    private GameObject ClearCamera1;
    private GameObject ClearCamera2;

    private Transform[] m_transformArr;
    private STAGE m_Stage = STAGE.STAGE_1;
    private Shader shader;

    private float m_fTime = 0f;
    private float m_fChangeTime = 0.5f;

    private string SceneName= "";

    // Start is called before the first frame update
    void Start()
    {
        SkyboxMaterial = Resources.Load<Material>("Skybox Cubemap Extended Day");
        StageManager = GameObject.Find("StageManager").GetComponent<Stage_Manager>();
        ClearCamera1 = GameObject.Find("ClearCamera1");
        ClearCamera2 = GameObject.Find("ClearCamera2");

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
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckScene())
            return;

        if (StayColor())
            return;

            ChangeColor();
    }

    bool CheckScene()
    {
        SceneName = SceneManager.GetActiveScene().name;

        if (SceneName != "MainStage")
            return true;

        if (!StageManager.stage1_open && StageManager.stage2_open && !StageManager.stage3_open)
            m_Stage = STAGE.STAGE_2;
        else if (!StageManager.stage1_open && !StageManager.stage2_open && StageManager.stage3_open)
            m_Stage = STAGE.STAGE_3;
        else if (!StageManager.stage1_open && !StageManager.stage2_open && !StageManager.stage3_open)
            m_Stage = STAGE.STAGE_ENDING;

            return false;
    }

    bool StayColor()
    {
        if(CheckStage == STAGE.STAGE_2 && m_Stage == STAGE.STAGE_3)
            Change_ChildColor();
        else if((CheckStage == STAGE.STAGE_2 || CheckStage == STAGE.STAGE_3) && m_Stage == STAGE.STAGE_ENDING)
        {
            RenderSettings.skybox = SkyboxMaterial;
            Change_ChildColor();
        }

        return false;
    }

    void ChangeColor()
    {
        if (m_Stage == CheckStage)
        {
            if (ClearCamera1.GetComponent<Camera>().enabled)
                if (!TargetInScreen_Direction(0))
                    return;
            if (ClearCamera2.GetComponent<Camera>().enabled)
                if (!TargetInScreen_Direction(1))
                    return;


            if (m_Stage == STAGE.STAGE_ENDING)
                GameObject.Find("CameraManager").GetComponent<CameraManager>().m_bChangeSkyBox = true;

            Change_ChildColor();
        }

        if (m_Stage == STAGE.STAGE_ENDING)
            GameObject.Find("EndingText").GetComponent<Ending_sc>().m_bStartEnding = true;
    }
    
    void Change_ChildColor()
    {
        if (IsParent)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                if (m_transformArr[i].GetComponent<MeshRenderer>().material != null)
                    m_transformArr[i].GetComponent<MeshRenderer>().material.color = color;
                else
                {
                    Material material = new Material(shader);
                    material.color = color;
                    m_transformArr[i].GetComponent<MeshRenderer>().material = material;
                }
            }
        }
        else
            transform.GetComponent<MeshRenderer>().material.color = color;
    }

    bool TargetInScreen_Direction(int iCameraNum)
    {
        Vector3 localTargetPos = new Vector3(0,0,0);
        Vector3 forward = new Vector3(0,0,0);
        Vector3 toDir = new Vector3(0,0,0);

        switch (iCameraNum)
        {
            case 0:
                {
                    // 카메라를 기준으로 타겟의 로컬 좌표를 구한다.
                    localTargetPos = ClearCamera1.transform.InverseTransformPoint(transform.position);
                    // 카메라의 전방
                    forward = ClearCamera1.transform.TransformDirection(Vector3.forward);
                    // 타겟과 카메라의 바라보는 방향
                    toDir = transform.position - ClearCamera1.transform.position;
                }
                break;
            case 1:
                {
                    localTargetPos = ClearCamera2.transform.InverseTransformPoint(transform.position);
                    forward = ClearCamera2.transform.TransformDirection(Vector3.forward);
                    toDir = transform.position - ClearCamera2.transform.position;
                }
                break;
            case 2:
                {

                }
                break;
            default:
                break;
        }

        // 내적: 0보다 크면 전방 180도 내부에 있다는뜻 0보다 작으면 뒤로 180도에 있다
        float dot = Vector3.Dot(forward, toDir);
        if (dot > 0f) // 내적이 0보다 크다는건 전방에 있다는 뜻
        {
            m_fTime += Time.deltaTime;
            if(m_fTime> m_fChangeTime)
                return true;
        }

        return false;
    }
}

//https://dorasol.tistory.com/5