using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCotrollerConfirm : DialogController
{
    public Text m_textTitle   = null;
    public Text m_textMessage = null;
   
    private DialogDataConfirm m_dataConfirm
    {
        get;
        set;
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        DialogManager.GetInstance().Regist(E_DIALOG_TYPE.E_CONFIRM, this);
    }

    public override void Build(DialogData dialogData)
    {
        base.Build(dialogData);

        if(!(dialogData is DialogDataConfirm))
        {
            Debug.LogError("Invaild dialog data!");

            return;
        }

        m_dataConfirm       = dialogData as DialogDataConfirm;
        m_textTitle.text    = m_dataConfirm.m_strTitle;
        m_textMessage.text  = m_dataConfirm.m_strMessage;
    }

    public void OnClickOK()
    {
        if(m_dataConfirm.m_actCallback != null)
        {
            m_dataConfirm.m_actCallback(true);
        }

        transform.localPosition = new Vector2(-2000.0f, -2000.0f);

        Debug.Log("Clicked Ok!");

        DialogManager.GetInstance().Pop();
    }

    public void OnClickCancel()
    {
        if (m_dataConfirm.m_actCallback != null)
        {
            m_dataConfirm.m_actCallback(false);
        }

        transform.localPosition = new Vector2(-2000.0f, -2000.0f);

        Debug.Log("Clicked Cancel!");

        DialogManager.GetInstance().Pop();
    }
}
