using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTTPClient : MonoBehaviour
{
    private static HTTPClient m_refInstance  = null;

    public static HTTPClient Create()
    {
        if(m_refInstance)
        {
            Debug.Log("이미 'HTTPClient'가 생성되어 있습니다.");

            return null;
        }

        GameObject objContainer = null;

        objContainer  = new GameObject("HTTPClient");
        m_refInstance = objContainer.AddComponent(typeof(HTTPClient)) as HTTPClient;

        DontDestroyOnLoad(objContainer);

        return m_refInstance;
    }

    public static HTTPClient GetInstance()
    {
        return m_refInstance;
    }

    public void GET(string strURL, Action<WWW> actCallback)
    {
        WWW www = null;

        www = new WWW(strURL);

        Debug.Log("플레이어의 유저 정보를 가져옵니다." + strURL);

        StartCoroutine(WaitWWW(www, actCallback));
    }

    public void POST(string strURL, string strInput, Action<WWW> actCallback)
    {
        WWW www = null;
        byte[] arrayBody = { };
        Dictionary<string, string> pairHeaders = null;

        pairHeaders = new Dictionary<string, string>();
        arrayBody   = Encoding.UTF8.GetBytes(strInput);

        pairHeaders.Add("Content-Type", "application/json");

        Debug.Log("HTTPClient Array Body : " + arrayBody.Length.ToString());
        Debug.Log("HTTPClient BOM Check! : " + strInput);

        www = new WWW(strURL, arrayBody, pairHeaders);

        if(www == null)
        {
            Debug.LogError("www 객체 생성 오류! 위치 : HTTPClient.POST");

            return;
        }
        else
        {
            Debug.Log("www : " + www.url);
            StartCoroutine(WaitWWW(www, actCallback));
        }
    }

    public IEnumerator WaitWWW(WWW www, Action<WWW> actCallback)
    {
        yield return www;

        actCallback(www);
    }
}
