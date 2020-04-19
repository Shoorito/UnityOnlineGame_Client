using System;
using UnityEngine;
using Facebook.Unity;
using Boomlagoon.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    // 로그인 루틴
    // 1. LoginInit()               함수 호출을 통한 초기화.
    // 2. LoginFacebook()           페이스북 계정을 통해 로그인.
    // 3. LoadDataFromFacebook()    계정 정보를 통한 유저 정보 로드.
    // 4. LoginGameServer()         유저 정보를 통해 게임 서버 접속.
    // 5. LoadDataFromGameServer()  서버의 유저 데이터와 클라이언트의 유저 정보 대조.
    // 6. LoadNextScene()           게임 플레이 씬을 로드.

    public  int        m_nFinishLevel      = 5;
    public  GameObject m_objFacebookButton = null;

    private int      m_nNowLoadingStage = 0;
    private bool     m_isNowLoading     = false;
    private Action[] m_arrLoadFunction  = { };

    private static LoginController m_refInstance = null;

    public static LoginController GetInstance()
    {
        return m_refInstance;
    }

    private void Awake()
    {
        m_arrLoadFunction = new Action[m_nFinishLevel];
        m_refInstance     = this;

        m_arrLoadFunction[0] = LoadFacebookDataMe;
        m_arrLoadFunction[1] = LoadFacebookDataFriend;
        m_arrLoadFunction[2] = UserDataUpdate;
        m_arrLoadFunction[3] = LoadTotalRank;
        m_arrLoadFunction[4] = LoadFriendRank;
    }
    
    private void Start()
    {
        HTTPClient.Create();
        UserSingleton.Create();

        LoginInit();
    }

    private void LoginInit()
    {
        bool isUserIDNotZero   = false;
        bool isGainAccessToken = false;

        isUserIDNotZero   = UserSingleton.GetInstance().m_nUserID        != 0;
        isGainAccessToken = UserSingleton.GetInstance().m_strAccessToken != "";

        RankSingleton.Create();

        if (isUserIDNotZero && isGainAccessToken)
        {
            LoginFacebook();
        }
        else
        {
            m_objFacebookButton.SetActive(true);
        }
    }

    public void LoginFacebook()
    {
        FB.Init(LoginInitialize);
    }

    private void LoginInitialize()
    {
        List<string> listPermissions = null;

        listPermissions = new List<string> { "public_profile", "email", "user_friends" };

        FB.ActivateApp();
        FB.LogInWithReadPermissions(listPermissions, FacebookLoginCheck);
    }

    private void FacebookLoginCheck(ILoginResult loginResult)
    {
        bool          isLoginResult = false;
        JSONObject    objResult     = null;
        UserSingleton userSingleton = null;

        objResult     = JSONObject.Parse(loginResult.RawResult);
        userSingleton = UserSingleton.GetInstance();

        userSingleton.m_strFacebookID           = objResult["user_id"].Str;
        userSingleton.m_strFacebookPhotoURL     = "http://graph.facebook.com/";
        userSingleton.m_strFacebookPhotoURL    += userSingleton.m_strFacebookID;
        userSingleton.m_strFacebookPhotoURL    += "/picture?type=square";
        userSingleton.m_strFacebookAccessToken  = objResult["access_token"].Str;

        isLoginResult = (userSingleton.m_strFacebookID != null);

        Debug.Log("Facebook Login Result : " + loginResult.RawResult);

        if (userSingleton.m_strFacebookID != null)
        {
            LoadData(isLoginResult);
        }
        else
        {
            Debug.LogError("로그인 실패..");
        }
    }

    private void LoadData(bool isLogin)
    {
        Debug.Log("로그인 완료.");

        if (isLogin)
        {
            Debug.Log("유저의 페이스북 데이터를 불러옵니다..");

            StartCoroutine(LoadDataFromFacebook());
        }
        else
        {
            Debug.LogError("페이스북 계정 데이터 체크에 실패했습니다.");
            Debug.LogError("발생위치 : FacebookLoginCheck(ILoginResult loginResult)");

            return;
        }
    }

    private IEnumerator LoadDataFromFacebook()
    {
        m_arrLoadFunction[0]();

        m_arrLoadFunction[1]();

        while (m_nNowLoadingStage != 2)
        {
            yield return new WaitForSeconds(0.1f);

            Debug.Log("페이스북 내 정보 데이터를 불러오고 있습니다...");
        }

        Debug.Log("유저 데이터를 불러오는데에 성공했습니다.");

        LoginServer();
    }

    private void LoginServer()
    {
        string      strDomain = "";
        JSONObject  jsonBody  = null;

        jsonBody  = new JSONObject();
        strDomain = UserSingleton.GetInstance().GetServerDomain() + "/Login/Facebook";

        jsonBody.Add("FacebookID",          UserSingleton.GetInstance().m_strFacebookID);
        jsonBody.Add("FacebookAccessToken", UserSingleton.GetInstance().m_strFacebookAccessToken);
        jsonBody.Add("FacebookName",        UserSingleton.GetInstance().m_strName);
        jsonBody.Add("FacebookPhotoURL",    UserSingleton.GetInstance().m_strFacebookPhotoURL);

        Debug.Log("서버에 전달 할 값 : " + jsonBody.ToString());

        HTTPClient.GetInstance().POST(strDomain, jsonBody.ToString(), ServerLoginCheck);
    }

    private void ServerLoginCheck(WWW www)
    {
        if (www.text.Length == 0)
        {
            Debug.LogError("서버 로그인 정보 값이 전달 되지 않았습니다..");

            return;
        }

        int        nResultCode  = 0;
        JSONObject jsonResponse = null;

        jsonResponse = JSONObject.Parse(www.text);
        nResultCode  = (int)jsonResponse["ResultCode"].Number;

        Debug.Log(www.text);

        if (nResultCode == 1 || nResultCode == 2)
        {
            string     strIDCode = "";
            JSONObject jsonData  = null;

            jsonData  = jsonResponse.GetObject("Data");
            strIDCode = nResultCode == 1 ? "새로 가입하는 ID입니다!" : "이미 존재하는 ID입니다!";

            UserSingleton.GetInstance().m_nUserID = int.Parse(jsonData["UserID"].Number.ToString());
            UserSingleton.GetInstance().m_strAccessToken = jsonData["AccessToken"].Str;

            Debug.Log("현재 접속한 ID는 " + strIDCode);
            Debug.Log("ID : " + UserSingleton.GetInstance().m_nUserID.ToString());
            Debug.Log("AccessToken : " + UserSingleton.GetInstance().m_strAccessToken);
            Debug.Log("서버 로그인 성공!");

            InvokeRepeating("LoadDataFromGameServer", 0.01f, 0.2f);
        }
        else
        {
            Debug.LogError("로그인에 실패했습니다..");

            return;
        }
    }

    private void LoadDataFromGameServer()
    {
        if (m_nNowLoadingStage == 5)
        {
            LoadNextScene();

            return;
        }

        if (!m_isNowLoading)
        {
            m_isNowLoading = true;

            m_arrLoadFunction[m_nNowLoadingStage]();
        }
        else
        {
            Debug.Log("현재 다른 데이터를 로드하고 있습니다..");
        }
    }

    private void LoadFacebookDataMe()
    {
        UserSingleton.GetInstance().LoadFacebookMe
        (
            delegate (bool isSuccess, string strResponse)
            {
                m_nNowLoadingStage++;

                Debug.Log("페이스북 내 정보 로드 결과 : " + strResponse);
            }
        );
    }

    private void LoadFacebookDataFriend()
    {
        UserSingleton.GetInstance().LoadFacebookFriend
        (
            delegate (bool isSuccess, string strResponse)
            {
                m_nNowLoadingStage++;

                Debug.Log("페이스북 내 친구 정보 로드 결과 : " + strResponse);
            }
        );
    }

    private void UserDataUpdate()
    {
        UserSingleton.GetInstance().Refresh(UpdateDelegate);
    }

    private void LoadTotalRank()
    {
        RankSingleton.GetInstance().LoadTotalRank(UpdateDelegate);
    }

    private void LoadFriendRank()
    {
        RankSingleton.GetInstance().LoadFriendRank(UpdateDelegate);
    }

    private void UpdateDelegate()
    {
        m_nNowLoadingStage++;
        m_isNowLoading = false;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Lobby");

        Debug.Log("Load Lobby Scene.");
    }
}
