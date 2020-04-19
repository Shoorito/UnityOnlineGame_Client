using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControllerAlert : DialogController
{
    public Text m_textLabelTitle   = null;
    public Text m_textLabelMessage = null;

    private DialogDataAlert m_dialogData { get; set; }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        DialogManager.GetInstance().Regist(E_DIALOG_TYPE.E_ALERT, this);
    }

    public override void Build(DialogData dialogData)
    {
        base.Build(dialogData);

        if(!(dialogData is DialogDataAlert))
        {
            Debug.Log("Invaild dialog data!");
            return;
        }

        m_dialogData = dialogData as DialogDataAlert;

        m_textLabelTitle.text   = m_dialogData.m_strTitle;
        m_textLabelMessage.text = m_dialogData.m_strMessage;
    }

    public void OnClickOK()
    {
        if (m_dialogData != null && m_dialogData.m_actCallback != null)
        {
            m_dialogData.m_actCallback();
        }

        transform.localPosition = new Vector2(-2000.0f, -2000.0f);

        DialogManager.GetInstance().Pop();
    }
}
