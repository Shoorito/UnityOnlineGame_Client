using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public GameObject m_objRankContent = null;
    public GameObject m_objMainMenu    = null;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, true);

        RankCellPool.Create("Prefab/RankCell", "RankCellPool", 50, 2);

        m_objRankContent.SetActive(false);

        UserSingleton.GetInstance().Refresh(delegate() { });
    }

    public void GoGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoRank()
    {
        m_objMainMenu.SetActive(false);
        m_objRankContent.SetActive(true);
        m_objRankContent.GetComponentInChildren<RankContent>().LoadRankList();

        m_objRankContent.transform.localPosition = Vector3.zero;
    }

    public void CloseRank()
    {
        RankContent.GetInstance().RemoveRankCell();

        m_objMainMenu.SetActive(true);
        m_objRankContent.SetActive(false);

        m_objRankContent.transform.localPosition = new Vector3(-1500.0f, 0.0f, 0.0f);
    }
}
