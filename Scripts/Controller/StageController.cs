using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Boomlagoon.JSON;

public class StageController : MonoBehaviour
{
    public static StageController m_thisInstance = null;

    public int m_nStagePoint = 0;
    public Text m_textPointText = null;

    private bool              m_isGameDataSave = false;
    private SkillAttackPool[] m_arrSkillPool   = { };

    private void Awake()
    {
        m_thisInstance = this;
        m_arrSkillPool = new SkillAttackPool[(int)PlayerAttack.E_SKILL_TYPE.E_MAX];

        SlimePool.Create("Prefab/Slime", "SlimePool", 50);
        DamageTextPool.Create("Prefab/DamageText", "DamageTextPool", 50);

        m_arrSkillPool[0] = SkillAttackPool.Create("Prefab/SkillAttack1", "SkillAttack1Pool", 50);
        m_arrSkillPool[1] = SkillAttackPool.Create("Prefab/SkillAttack2", "SkillAttack2Pool", 50);

        m_arrSkillPool[0].gameObject.layer = 2;
        m_arrSkillPool[1].gameObject.layer = 1;
    }

    private void Start()
    {
        DialogDataAlert alert = new DialogDataAlert("START", "GAME START!", delegate { Debug.Log("OK_Pressed!"); });

        DialogManager.GetInstance().Push(alert);
    }

    public SkillAttackPool GetSkillPool(int nType)
    {
        return m_arrSkillPool[nType];
    }

    public void AddPoint(int nPoint)
    {
        m_nStagePoint += nPoint;
        m_textPointText.text = "StagePoint : " + m_nStagePoint.ToString();
    }

    public void FinishGame()
    {
        DialogDataConfirm confirm = null;

        confirm = new DialogDataConfirm
        (
            "스테이지를 종료하시겠습니까?",
            "'확인'버튼을 누르면 스테이지가 재시작됩니다.",
            delegate (bool isYseNo)
            {
                if (isYseNo)
                {
                    Debug.Log("Game Restart!");

                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else
                {
                    Debug.Log("Game End");

                    SaveGainPoint();
                    InvokeRepeating("GoLobby", 0.1f, 0.1f);
                }
            }
        );

        DialogManager.GetInstance().Push(confirm);
    }

    private void SaveGainPoint()
    {
        JSONObject    objData   = null;
        UserSingleton singleton = null;

        objData   = new JSONObject();
        singleton = UserSingleton.GetInstance();

        objData.Add("UserID",   singleton.m_nUserID);
        objData.Add("AddPoint", m_nStagePoint);

        Debug.Log("Data : " + objData.ToString());

        HTTPClient.GetInstance().POST
        (
            string.Format("{0}/Upgrade/AddPoint", singleton.GetServerDomain()),
            objData.ToString(),
            GainPointWithFinished
        );

        singleton.m_nUserScore = m_nStagePoint;

        Debug.Log("SavePoint!");
    }

    private void GainPointWithFinished(WWW www)
    {
        JSONObject jsonData = null;

        jsonData = JSONObject.Parse(www.text);

        if ((int)jsonData["ResultCode"].Number == 1)
        {
            Debug.Log("점수 흭득에 성공했습니다!");
            Debug.Log("Data : " + jsonData.ToString());

            m_isGameDataSave = true;
        }
        else
        {
            Debug.LogError("점수에 관련하여 서버에 문제가 발생했습니다!");
        }
    }

    private void GoLobby()
    {
        if(m_isGameDataSave)
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
