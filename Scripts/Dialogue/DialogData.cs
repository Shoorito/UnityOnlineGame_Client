using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_DIALOG_TYPE
{
    E_ALERT,
    E_CONFIRM,
    E_RANKING
}

public class DialogData
{
    public E_DIALOG_TYPE m_eType { get; set; }

    public DialogData(E_DIALOG_TYPE eType)
    {
        m_eType = eType;

        Debug.Log("대화창 데이터가 생성되었습니다!");
    }
}
