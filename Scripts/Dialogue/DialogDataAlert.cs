using System;

public class DialogDataAlert : DialogData
{
    public string m_strTitle
    {
        get;
        private set;
    }

    public string m_strMessage
    {
        get;
        private set;
    }

    public Action m_actCallback
    {
        get;
        private set;
    }

    public DialogDataAlert(string strTitle, string strMessage, Action actCallback = null)
    : base(E_DIALOG_TYPE.E_ALERT)
    {
        m_strTitle    = strTitle;
        m_strMessage  = strMessage;
        m_actCallback = actCallback;

    }
}
