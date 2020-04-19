using System;
using UnityEngine;
using Facebook.Unity;
using Boomlagoon.JSON;
using System.Collections;
using System.Collections.Generic;

public class UserSingleton : MonoBehaviour
{
    // Values.
    public int m_nUserScore          = 0;
    public int m_nLevel              = 0;
    public int m_nExperience         = 0;
    public int m_nDamage             = 0;
    public int m_nHealth             = 0;
    public int m_nDefense            = 0;
    public int m_nSpeed              = 0;
    public int m_nDamageLevel        = 0;
    public int m_nHealthLevel        = 0;
    public int m_nDefenseLevel       = 0;
    public int m_nSpeedLevel         = 0;
    public int m_nDiamond            = 0;
    public int m_nExpAfterLastLevel  = 0;
    public int m_nExpForNextLevel    = 0;

    public JSONArray m_arrayFriendList = null;

    private string m_strServerDomain = "http://shooter-db.azurewebsites.net";

    // Singleton Instance.
    private static UserSingleton m_refInstance = null;

    public static UserSingleton Create()
    {
        if(m_refInstance)
        {
            Debug.Log("이미 'UserSingleton'이 생성되어 있습니다!");
            Debug.Log("'GetInstance'함수를 이용해 클래스를 참조하세요.");

            return null;
        }

        GameObject objContainer = null;

        objContainer  = new GameObject("UserSingleton");
        m_refInstance = objContainer.AddComponent(typeof(UserSingleton)) as UserSingleton;

        DontDestroyOnLoad(m_refInstance);

        return m_refInstance;
    }

    public static UserSingleton GetInstance()
    {
        return m_refInstance;
    }

    public string GetServerDomain()
    {
        return m_strServerDomain;
    }

    public int m_nUserID
    {
        get
        {
            return PlayerPrefs.GetInt("UserID");
        }

        set
        {
            PlayerPrefs.SetInt("UserID", value);
        }
    }

    public string m_strAccessToken
    {
        get
        {
            return PlayerPrefs.GetString("AccessToken");
        }

        set
        {
            PlayerPrefs.SetString("AccessToken", value);
        }
    }

    public string m_strFacebookID
    {
        get
        {
            return PlayerPrefs.GetString("FacebookID");
        }

        set
        {
            PlayerPrefs.SetString("FacebookID", value);
        }
    }

    public string m_strFacebookAccessToken
    {
        get
        {
            return PlayerPrefs.GetString("FacebookAccessToken");
        }

        set
        {
            PlayerPrefs.SetString("FacebookAccessToken", value);
        }
    }

    public string m_strName
    {
        get
        {
            return PlayerPrefs.GetString("Name");
        }

        set
        {
            PlayerPrefs.SetString("Name", value);
        }
    }

    public string m_strFacebookPhotoURL
    {
        get
        {
            return PlayerPrefs.GetString("FacebookPhotoURL");
        }

        set
        {
            PlayerPrefs.SetString("FacebookPhotoURL", value);
        }
    }

    // Methods(Member Functions)

    // Load Me Method.
    public void LoadFacebookMe(Action<bool, string> actCallback, int nRetryCount = 0)
    {
        FB.API
        (
            "/me",
            HttpMethod.GET,
            delegate(IGraphResult graphResult)
            {
                if(graphResult.Error != null)
                {
                    if(nRetryCount >= 3)
                    {
                        actCallback(false, graphResult.Error);

                        Debug.LogError(graphResult.Error);

                        return;
                    }
                    else
                    {
                        Debug.LogError("Error occured, start retrying.. " + graphResult.Error);

                        LoadFacebookMe(actCallback, nRetryCount + 1);

                        return;
                    }
                }

                JSONObject objMe = null;

                objMe = JSONObject.Parse(graphResult.RawResult);

                m_strName = objMe["name"].Str;

                actCallback(true, graphResult.RawResult);

                Debug.Log("Facebook Name : " + m_strName);
                Debug.Log("Load Success!");
            }
        );
    }

    // Load Friend List Method.
    // 페이스북 친구 목록을 "JSON" 리스트로 불러옵니다.
    public void LoadFacebookFriend(Action<bool, string> actCallback, int nRetryCount = 0)
    {
        FB.API
        (
            "/me/friends",
            HttpMethod.GET,
            delegate(IGraphResult graphResult)
            {
                if (graphResult.Error != null)
                {
                    if (nRetryCount >= 3)
                    {
                        actCallback(false, graphResult.Error);

                        Debug.LogError(graphResult.Error);

                        return;
                    }
                    else
                    {
                        Debug.LogError("Error occured, start retrying.. " + graphResult.Error);

                        LoadFacebookFriend(actCallback, nRetryCount + 1);

                        return;
                    }
                }

                JSONArray  arrayResponse = null;
                JSONObject objResponse   = null;

                objResponse   = JSONObject.Parse(graphResult.RawResult);
                arrayResponse = objResponse["data"].Array;

                m_arrayFriendList = arrayResponse;

                actCallback(true, graphResult.RawResult);
                Debug.Log("친구 데이터 : " + graphResult.RawResult);
            }
        );
    }

    public void Refresh(Action actCallback)
    {
        string strURL = "";

        strURL = m_strServerDomain + "/User/Info?UserID=" + m_nUserID;

        Debug.Log("플레이어 스테이터스 로드 시도.");
        Debug.Log("ID : " + m_nUserID.ToString());

        HTTPClient.GetInstance().GET
        (
            strURL,
            delegate(WWW www)
            {
                if(www.text.Length == 0)
                {
                    Debug.LogError("유저 정보 로드 실패!");

                    return;
                }

                int         nResultCode  = 0;
                JSONObject  jsonData     = null;
                JSONObject  jsonResponse = null;

                Debug.Log(www.text);

                jsonResponse = JSONObject.Parse(www.text);
                nResultCode  = (int)jsonResponse["ResultCode"].Number;
                jsonData     = jsonResponse["Data"].Obj;

                m_nLevel             = (int)jsonData["Level"].Number;
                m_nUserScore         = (int)jsonData["Point"].Number;
                m_nExperience        = (int)jsonData["Experience"].Number;
                m_nDamage            = (int)jsonData["Damage"].Number;
                m_nHealth            = (int)jsonData["Health"].Number;
                m_nDefense           = (int)jsonData["Defense"].Number;
                m_nSpeed             = (int)jsonData["Speed"].Number;
                m_nDamageLevel       = (int)jsonData["DamageLevel"].Number;
                m_nHealthLevel       = (int)jsonData["HealthLevel"].Number;
                m_nDefenseLevel      = (int)jsonData["DefenseLevel"].Number;
                m_nSpeedLevel        = (int)jsonData["SpeedLevel"].Number;
                m_nDiamond           = (int)jsonData["Diamond"].Number;
                m_nExpForNextLevel   = (int)jsonData["ExpForNextLevel"].Number;
                m_nExpAfterLastLevel = (int)jsonData["ExpAfterLastLevel"].Number;

                Debug.Log("유저 정보 로드 성공!");

                actCallback();
            }
        );
    }
}
