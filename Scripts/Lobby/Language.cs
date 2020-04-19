using System;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using UnityEngine;

public class Language : MonoBehaviour
{
    private static Language m_refInstance = null;
    private JSONObject m_jsonLanguageText = null;

    public static Language Create()
    {
        if (m_refInstance)
        {
            Debug.Log("이미 'Language' 클래스가 생성되어 있습니다.");

            return null;
        }

        GameObject objContainer = null;

        objContainer  = new GameObject("Language_Manager");
        m_refInstance = objContainer.AddComponent(typeof(Language)) as Language;

        m_refInstance.InitLanguage();

        DontDestroyOnLoad(objContainer);

        return m_refInstance;
    }

    public static Language GetInstance()
    {
        return m_refInstance;
    }

    public void InitLanguage()
    {
        TextAsset textAsset = null;

        textAsset          = Resources.Load("Text/Language", typeof(TextAsset)) as TextAsset;
        m_jsonLanguageText = JSONObject.Parse(textAsset.text);
    }

    public string GetLanguage(string strKey)
    {
        if(m_jsonLanguageText == null)
        {
            InitLanguage();
        }

        return m_jsonLanguageText.GetString(strKey);
    }
}
