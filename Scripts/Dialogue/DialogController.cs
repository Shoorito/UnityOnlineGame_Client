using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    // Member Values.

    public Transform m_transWindow = null;

    // Interface.

    public bool Visible
    {
        get
        {
            return m_transWindow.gameObject.activeSelf;
        }
        private set
        {
            m_transWindow.localPosition = Vector3.zero;

            m_transWindow.gameObject.SetActive(value);
        }
    }

    // Method / Function.

    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {

    }

    public virtual void Build(DialogData dialogData)
    {

    }

    public void Show(Action actCallback)
    {
        StartCoroutine(OnEnter(actCallback));
    }

    public void Close(Action actCallback)
    {
        StartCoroutine(OnExit(actCallback));
    }

    private IEnumerator OnEnter(Action actCallback)
    {
        Visible = true;

        if(actCallback != null)
        {
            actCallback();
        }

        yield break;
    }

    private IEnumerator OnExit(Action actCallback)
    {
        Visible = false;

        if(actCallback != null)
        {
            actCallback();
        }

        Debug.Log("Close Window");

        yield break;
    }
}