using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DialogManager
{
    // Member Values
    private List<DialogData> m_listDialogQueue = null;
    private Dictionary<E_DIALOG_TYPE, DialogController> m_dictionaryLogs = null;
    private DialogController m_dcCurrentDialog = null;

    // Singleton Member
    private static DialogManager m_thisInstance = new DialogManager();
    
    // Member Methods

    public static DialogManager GetInstance()
    {
        return m_thisInstance;
    }

    public void Regist(E_DIALOG_TYPE eType, DialogController dcController)
    {
        m_dictionaryLogs[eType] = dcController;
    }

    public void Push(DialogData dialogData)
    {
        if (dialogData == null)
            return;

        m_listDialogQueue.Add(dialogData);

        if(m_dcCurrentDialog == null)
        {
            ShowNext();
        }
    }

    public void Pop()
    {
        if(m_dcCurrentDialog != null)
        {
            m_dcCurrentDialog.Close
            (
                delegate
                {
                    m_dcCurrentDialog = null;

                    if (m_listDialogQueue.Count > 0)
                    {
                        ShowNext();
                    }
                }
            );
        }
    }

    public bool IsShowing()
    {
        return m_dcCurrentDialog != null;
    }

    private DialogManager()
    {
        m_listDialogQueue = new List<DialogData>();
        m_dictionaryLogs  = new Dictionary<E_DIALOG_TYPE, DialogController>();
    }

    private void ShowNext()
    {
        DialogData       dialogNext = null;
        DialogController controller = null;

        dialogNext        = m_listDialogQueue[0];
        controller        = m_dictionaryLogs[dialogNext.m_eType].GetComponent<DialogController>();
        m_dcCurrentDialog = controller;

        m_dcCurrentDialog.Build(dialogNext);
        m_dcCurrentDialog.Show(delegate{ });
        m_listDialogQueue.RemoveAt(0);
    }
}
