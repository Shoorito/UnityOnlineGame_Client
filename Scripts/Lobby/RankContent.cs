using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class RankContent : MonoBehaviour
{
    public RankCell[] m_arrRankCell        = { };
    public int        m_nRankContentHeight = 0;
    public GameObject m_objFriendRankBtn   = null;
    public GameObject m_objTotalRankBtn    = null;

    private RankSingleton.E_RANK_TYPE m_eCurrentTab  = RankSingleton.E_RANK_TYPE.E_TOTAL;
    private static RankContent m_refInstance = null;

    public static RankContent GetInstance()
    {
        return m_refInstance;
    }

    private void Awake()
    {
        m_refInstance = this;
    }

    public void TabTotalRank()
    {
        m_eCurrentTab = RankSingleton.E_RANK_TYPE.E_TOTAL;

        TabEvent();
    }

    public void TabFriendRank()
    {
        m_eCurrentTab = RankSingleton.E_RANK_TYPE.E_FRIEND;

        TabEvent();
    }

    public void RemoveRankCell()
    {
        RankCell[] arrRank = { };

        arrRank = gameObject.GetComponentsInChildren<RankCell>();

        foreach (RankCell rankCell in arrRank)
        {
            RankCellPool.GetInstance().ReleaseObject(rankCell.gameObject);
        }
    }

    public void LoadRankList()
    {
        Dictionary<int, JSONObject> mapRankList    = null;
        float                       fUnit          = 0.0f;
        float                       fStartYpos     = 0.0f;
        float                       fContentHeight = 0.0f;

        fUnit           = 50.0f;
        fStartYpos      = -20.0f;
        fContentHeight  = 50.0f;
        mapRankList     = RankSingleton.GetInstance().GetRankerList(m_eCurrentTab);

        Debug.Log("RankListLength : " + mapRankList.Count.ToString());

        foreach (KeyValuePair<int, JSONObject> item in mapRankList)
        {
            int           nUserRank   = 0;
            RankCell      rankCell    = null;
            GameObject    objRankCell = null;
            JSONObject    objUserInfo = null;
            RectTransform rectTransform   = null;

            nUserRank     = item.Key;
            objUserInfo   = item.Value;
            objRankCell   = RankCellPool.GetInstance().EnableObject();
            rankCell      = objRankCell.GetComponent<RankCell>();
            rectTransform = objRankCell.GetComponent<RectTransform>();

            rankCell.SetData(objUserInfo);
            objRankCell.transform.SetParent(transform);

            rectTransform.localPosition = new Vector2(0.0f,   fStartYpos);
            rectTransform.offsetMin     = new Vector2(20.0f,  fStartYpos);
            rectTransform.offsetMax     = new Vector2(-20.0f, fStartYpos);

            rectTransform.SetDefaultScale();
            rectTransform.SetHeight(40.0f);

            fStartYpos     -= fUnit;
            fContentHeight += fUnit;
        }

        GetComponent<RectTransform>().SetHeight(fContentHeight);
    }

    private void TabEvent()
    {
        RemoveRankCell();
        LoadRankList();
    }
}
