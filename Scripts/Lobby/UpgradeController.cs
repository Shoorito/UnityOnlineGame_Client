using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Boomlagoon.JSON;

public class UpgradeController : MonoBehaviour
{
    public Text m_textSpeed         = null;
    public Text m_textDamage        = null;
    public Text m_textHealth        = null;
    public Text m_textDefense       = null;
    public Text m_textUpgradeStatus = null;
    public GameObject m_objPopup    = null;

    public GameObject m_objAlertDialog  = null;
    public GameObject m_objConfirmDialog = null;

    private void Start()
    {
        NotificationCenter.GetInstance().Add(NotificationCenter.E_SUBJECT.E_PLAYER_DATA, UpdatePlayerData);

        m_objPopup.SetActive(false);

        UpdatePlayerData();
    }

    private void UpdatePlayerData()
    {
        UserSingleton userSingleton = UserSingleton.GetInstance();

        m_textUpgradeStatus.text = string.Format
        (
            "PlayerStatus\n\nHealth : {0}\nDefense : {1}\nDamage : {2}\nSpeed : {3}",
            userSingleton.m_nHealth,
            userSingleton.m_nDefense,
            userSingleton.m_nDamage,
            userSingleton.m_nSpeed
        );

        m_textSpeed.text    = string.Format("Lv. {0}",  userSingleton.m_nSpeedLevel);
        m_textHealth.text   = string.Format("Lv. {0}",  userSingleton.m_nHealthLevel);
        m_textDamage.text   = string.Format("Lv. {0}",  userSingleton.m_nDamageLevel);
        m_textDefense.text  = string.Format("Lv. {0}",  userSingleton.m_nDefenseLevel);
    }

    // 추후 if문을 없애고 보다 효율적으로 개선해야함.
    private void UpgradeDelegate(WWW www)
    {
        if (www.text == "")
        {
            Debug.Log("서버에서 업그레이드 정보를 전달하는 중 문제가 발생했습니다.");
            return;
        }

        int             nResultCode = 0;
        JSONObject      jsonResult  = null;
        DialogDataAlert dialogAlert = null;

        jsonResult = JSONObject.Parse(www.text);
        nResultCode = (int)jsonResult["ResultCode"].Number;

        if (nResultCode == 1) // Success_Code
        {
            UserSingleton.GetInstance().Refresh
            (
                delegate ()
                {
                    NotificationCenter.GetInstance().Notify(NotificationCenter.E_SUBJECT.E_PLAYER_DATA);
                }
            );

            dialogAlert = new DialogDataAlert
            (
                Language.GetInstance().GetLanguage("Upgrade Success"),
                Language.GetInstance().GetLanguage("Success"),
                delegate () { }
            );
        }
        else if (nResultCode == 4) // Player_is_MaxLevel
        {
            dialogAlert = new DialogDataAlert
            (
                Language.GetInstance().GetLanguage("Upgrade Failed"),
                Language.GetInstance().GetLanguage("Max Level"),
                delegate () { }
            );
        }
        else if (nResultCode == 5) // Not_Enough_Diamond
        {
            dialogAlert = new DialogDataAlert
            (
                Language.GetInstance().GetLanguage("Upgrade Failed"),
                Language.GetInstance().GetLanguage("Not Enough Diamond"),
                delegate () { }
            );
        }

        m_objAlertDialog.SetActive(true);

        DialogManager.GetInstance().Push(dialogAlert);
    }

    private void UpgradeHealthDelegate(bool isYes)
    {
        if (isYes)
            Upgrade("Health");
    }

    private void UpgradeDefenseDelegate(bool isYes)
    {
        if (isYes)
            Upgrade("Defense");
    }

    public void ConfirmUpgrade(string strUpgradeType)
    {
        DialogDataConfirm confirm = null;

        confirm = new DialogDataConfirm
        (
            "",
            "",
            delegate(bool isYesOrNo)
            {
                if (isYesOrNo)
                    Upgrade(strUpgradeType);
            }
        );

        DialogManager.GetInstance().Push(confirm);
    }

    public void Upgrade(string strUpgradeType)
    {
        JSONObject    jsonUser      = null;
        HTTPClient    client        = null;
        UserSingleton userSingleton = null;

        client        = HTTPClient.GetInstance();
        jsonUser      = new JSONObject();
        userSingleton = UserSingleton.GetInstance();

        jsonUser.Add("UserID", userSingleton.m_nUserID);
        jsonUser.Add("UpgradeType", strUpgradeType);

        Debug.Log("jsonUser : " + jsonUser.ToString());

        client.POST(userSingleton.GetServerDomain() + "/Upgrade/Execute", jsonUser.ToString(), UpgradeDelegate);
    }

    public void UpgradeHealth()
    {
        Language          language      = null;
        DialogDataConfirm dialogConfirm = null;

        language = Language.GetInstance();

        Debug.Log("Call_Health_Upgrade");

        dialogConfirm = new DialogDataConfirm
        (
            language.GetLanguage("Health Upgrade Confirm"),
            string.Format
            (
                language.GetLanguage("Diamonds are required"),
                UserSingleton.GetInstance().m_nHealthLevel * 2
            ),
            UpgradeHealthDelegate
        );

        m_objConfirmDialog.SetActive(true);

        DialogManager.GetInstance().Push(dialogConfirm);
    }

    public void UpgradeDefense()
    {
        Language language = null;
        DialogDataConfirm dialogConfirm = null;

        language = Language.GetInstance();

        dialogConfirm = new DialogDataConfirm
        (
            language.GetLanguage("Defense Upgrade Confirm"),
            string.Format
            (
                language.GetLanguage("Diamonds are required"),
                UserSingleton.GetInstance().m_nDefenseLevel * 2
            ),
            UpgradeDefenseDelegate
        );

        m_objConfirmDialog.SetActive(true);

        DialogManager.GetInstance().Push(dialogConfirm);
    }

    public void UpgradeDamage()
    {
        Language language = null;
        DialogDataConfirm dialogConfirm = null;

        language = Language.GetInstance();

        dialogConfirm = new DialogDataConfirm
        (
            language.GetLanguage("Damage Upgrade Confirm"),
            string.Format
            (
                language.GetLanguage("Diamonds are required"),
                UserSingleton.GetInstance().m_nDamageLevel * 2
            ),
            delegate (bool isYes) { if (isYes) Upgrade("Damage"); }
        );

        m_objConfirmDialog.SetActive(true);

        DialogManager.GetInstance().Push(dialogConfirm);
    }

    public void UpgradeSpeed()
    {
        Language language = null;
        DialogDataConfirm dialogConfirm = null;

        language = Language.GetInstance();

        dialogConfirm = new DialogDataConfirm
        (
            language.GetLanguage("Speed Upgrade Confirm"),
            string.Format
            (
                language.GetLanguage("Diamonds are required"),
                UserSingleton.GetInstance().m_nSpeedLevel * 2
            ),
            delegate (bool isYes) { if (isYes) Upgrade("Speed"); }
        );

        m_objConfirmDialog.SetActive(true);

        DialogManager.GetInstance().Push(dialogConfirm);
    }

    public void GoUpgrade()
    {
        GetComponent<LobbyController>().m_objMainMenu.SetActive(false);

        UpdatePlayerData();
        m_objPopup.SetActive(true);

        m_objPopup.transform.localPosition = new Vector3(590.0f, -15.0f, 0.0f);
    }

    public void CloseMenu()
    {
        m_objPopup.SetActive(false);
        GetComponent<LobbyController>().m_objMainMenu.SetActive(true);

        m_objPopup.transform.localPosition = new Vector2(-1500.0f, -1500.0f);
    }
}
