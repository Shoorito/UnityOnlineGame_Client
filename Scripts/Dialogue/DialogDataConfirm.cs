using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDataConfirm : DialogData
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

    public Action<bool> m_actCallback
    {
        get;
        private set;
    }


    public DialogDataConfirm(string strTitle, string strMessage, Action<bool> actCallback)
    : base(E_DIALOG_TYPE.E_CONFIRM)
    {
        m_strTitle    = strTitle;
        m_strMessage  = strMessage;
        m_actCallback = actCallback;
    }
}
