using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Boomlagoon.JSON;

public class RankCell : MonoBehaviour
{
    public RawImage m_rawimgProfile = null;
    public Text     m_textRank      = null;
    public Text     m_textName      = null;
    public Text     m_textPoint     = null;

    public void SetData(JSONObject jsonUserData)
    {
        string strURL = "";

        strURL =  "http://graph.facebook.com/";
        strURL += jsonUserData["FacebookID"].Str;
        strURL += "/picture?type=square";

        m_textRank.text  = jsonUserData["Rank"].Number.ToString();
        m_textName.text  = jsonUserData["FacebookName"].Str;
        m_textPoint.text = jsonUserData["Point"].Number.ToString();

        HTTPClient.GetInstance().GET(strURL, DataDelegate);
    }

    private void DataDelegate(WWW www)
    {
        m_rawimgProfile.texture = www.texture;
    }
}
