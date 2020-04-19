using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using System.Collections.Generic;
using System;

public class RankSingleton : MonoBehaviour
{
    public enum E_RANK_TYPE
    {
        E_TOTAL,
        E_FRIEND,
        E_MAX
    }

    private E_RANK_TYPE                   m_eNowUseRankType = E_RANK_TYPE.E_TOTAL;
    private Action                        m_delegateLoaded  = null;
    public Dictionary<int, JSONObject>[]  m_arrRankMaps     = null;
    private static RankSingleton          m_refInstance     = null;

    public static RankSingleton Create()
    {
        if(m_refInstance)
        {
            Debug.Log("이미 'RankSingleton'이 생성되어 있습니다.");

            return null;
        }

        GameObject objContainer = null;

        objContainer  = new GameObject("RankSingleton");
        m_refInstance = objContainer.AddComponent(typeof(RankSingleton)) as RankSingleton;

        m_refInstance.Init();

        DontDestroyOnLoad(m_refInstance);

        return m_refInstance;
    }

    public static RankSingleton GetInstance()
    {
        return m_refInstance;
    }

    public ref Dictionary<int, JSONObject> GetRankerList(E_RANK_TYPE eType)
    {
        return ref m_arrRankMaps[(int)eType];
    }

    private void Init()
	{
        m_arrRankMaps = new Dictionary<int, JSONObject>[2];

        m_arrRankMaps[(int)E_RANK_TYPE.E_TOTAL]  = new Dictionary<int, JSONObject>();
        m_arrRankMaps[(int)E_RANK_TYPE.E_FRIEND] = new Dictionary<int, JSONObject>();
    }

    private void RankDelegate(WWW www)
    {
        string      strResponse         = "";
        JSONArray   jsonDataList        = null;
        JSONObject  jsonConvertObject   = null;

        // 응답 결과 대입
        strResponse = www.text;

        // "응답 결과 데이터"를 "JSON 객체"로 변환합니다.
        jsonConvertObject = JSONObject.Parse(strResponse);
        jsonDataList      = jsonConvertObject["Data"].Array;

        Debug.Log("Ranking_Data" + jsonConvertObject.ToString());

        // 랭킹 리스트 조회.
        foreach (JSONValue jsonItem in jsonDataList)
        {
            int nRank = 0;

            nRank = (int)jsonItem.Obj["Rank"].Number;

            // 랭킹 조회 중 중복 되는 랭킹 정보를 발견했을 경우, 제거합니다.
            if (m_arrRankMaps[(int)m_eNowUseRankType].ContainsKey(nRank))
            {
                m_arrRankMaps[(int)m_eNowUseRankType].Remove(nRank);
            }

            // 해당 순위에 유저를 입력합니다.
            m_arrRankMaps[(int)m_eNowUseRankType].Add(nRank, jsonItem.Obj);
        }

        Debug.Log("Total_Rank_Length : " + m_arrRankMaps[(int)m_eNowUseRankType].Count.ToString());

        // 전체 랭킹의 로드 완료를 callback()함수에 알려줍니다.
        m_delegateLoaded();
    }

    // 전체랭킹을 서버에서 조회하여 변수에 저장하는 함수입니다.
    public void LoadTotalRank(Action actCallback)
    {
        string strRankLoadURL = "";

        strRankLoadURL    = UserSingleton.GetInstance().GetServerDomain() + "/Rank/Total?Start=0&Count=50";
        m_delegateLoaded  = actCallback;
        m_eNowUseRankType = E_RANK_TYPE.E_TOTAL;

        HTTPClient.GetInstance().GET(strRankLoadURL, RankDelegate);
    }

    // "LoadTotalRank" 함수의 "Friend"버전입니다.
    public void LoadFriendRank(Action actCallback)
    {
        // 친구 랭킹 리스트 저장용 변수
        string        strPostURL      = "";
        JSONArray     jsonFriendList  = null;
        JSONObject    jsonRequestBody = null;
        UserSingleton userSingleton   = null;

        userSingleton     = UserSingleton.GetInstance();
        strPostURL        = userSingleton.GetServerDomain() + "/Rank/Friend";
        jsonFriendList    = new JSONArray();
        jsonRequestBody   = new JSONObject();
        m_delegateLoaded  = actCallback;
        m_eNowUseRankType = E_RANK_TYPE.E_FRIEND;

        if (userSingleton.m_arrayFriendList.Length == 0)
        {
            Debug.Log("이 계정에는 친구가 없습니다.");

            actCallback();

            return;
        }

        // 페이스북 친구의 "id_list"를 "JSON 배열"에 저장합니다.
        foreach (JSONValue jsonItem in userSingleton.m_arrayFriendList)
        {
            JSONObject jsonFriendObj = null;

            jsonFriendObj = jsonItem.Obj;

            jsonFriendList.Add(jsonFriendObj["id"]);
		}

        // 랭킹의 "Body"에 들어갈 "친구 리스트"정보를 넣어줍니다.
        jsonRequestBody.Add("UserID",     userSingleton.m_nUserID);
        jsonRequestBody.Add("FriendList", jsonFriendList);

        Debug.Log(jsonRequestBody.ToString());

        // 친구랭킹 "httpClient"를 통해 전송합니다.
        HTTPClient.GetInstance().POST(strPostURL, jsonRequestBody.ToString(), RankDelegate);
    }
}
