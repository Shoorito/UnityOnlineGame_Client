using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusController : MonoBehaviour
{
    public Text   m_textName    = null;
    public Text   m_textLevel   = null;
    public Text   m_textExp     = null;
    public Text   m_textDiamond = null;
    public Slider m_sliderExp   = null;

    private void Awake()
    {
        Language.Create();
        NotificationCenter.Create();
    }

    private void Start()
    {
        NotificationCenter.GetInstance().Add(NotificationCenter.E_SUBJECT.E_PLAYER_DATA, UpdatePlayerData);

        UpdatePlayerData();
    }

    public void UpdatePlayerData()
    {
        UserSingleton userSingleton = UserSingleton.GetInstance();

        m_textName.text    = userSingleton.m_strName;
        m_textLevel.text   = "Lv. " + userSingleton.m_nLevel.ToString();
        m_textExp.text     = userSingleton.m_nExpAfterLastLevel.ToString() + '/' + userSingleton.m_nExpForNextLevel.ToString();
        m_textDiamond.text = userSingleton.m_nDiamond.ToString();

        m_sliderExp.maxValue = userSingleton.m_nExpForNextLevel;
        m_sliderExp.value    = userSingleton.m_nExpAfterLastLevel;
    }
}
