﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable] public class MessagesMapping : SerializableDictionary<string, string> { }

public class Log : MonoBehaviour, IPointerClickHandler
{
    public class Parser
    {
        internal static string Sad(string msg)
        {
            return "<color=#FF6445FF>" + msg + "</color>\n";
        }
        internal static string Neutral(string msg)
        {
            return "<color=#222222FF>" + msg + "</color>\n";
        }
        internal static string Happy(string msg)
        {
            return "<color=#7BFF6BFF>" + msg + "</color>\n";
        }
    }

    public float OpenPosition;
    public float ClosePosition;
    public float TransitTime;
    public int TextLimit = 300;

    public MessagesMapping SadMessages;

    public MessagesMapping HappyMessages;

    public MessagesMapping NeutralMessages;

    private float mUiProcessTime;
    private float mSelectedPosition;
    private TextMeshProUGUI mText;
    private RectTransform mCanvasRect;

    public bool movingUI { get { return mUiProcessTime > 0; } }

    public void OnPointerClick(PointerEventData eventData)
    {

        mUiProcessTime = Time.timeSinceLevelLoad;

        if (mCanvasRect.localPosition.y == ClosePosition)
        {
            mSelectedPosition = OpenPosition;
        } else
        {
            mSelectedPosition = ClosePosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mText = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        mCanvasRect = GetComponent<RectTransform>();

        mCanvasRect.localPosition = new Vector3(mCanvasRect.localPosition.x, ClosePosition, 0);

        // Subscribe to every event
        foreach (KeyValuePair<String, String> entry in HappyMessages)
        {
            EventManager.StartListening(entry.Key, (object[] args) => EventHandler(entry, args));
        }
        foreach (KeyValuePair<String, String> entry in NeutralMessages)
        {
            EventManager.StartListening(entry.Key, (object[] args) => EventHandler(entry, args));
        }
        foreach (KeyValuePair<String, String> entry in SadMessages)
        {
            EventManager.StartListening(entry.Key, (object[] args) => EventHandler(entry, args));
        }
    }

    void EventHandler(KeyValuePair<String, String> message, object[] args)
    {
        string parsedMessage;

        if (HappyMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Happy(String.Format(message.Value, args));
        else if (NeutralMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Neutral(String.Format(message.Value, args));
        else if (SadMessages.ContainsKey(message.Key))
            parsedMessage = Parser.Sad(String.Format(message.Value, args));
        else
            parsedMessage = "Unknown event\n";

        parsedMessage += mText.text;
        
        mText.text = (parsedMessage.Length > TextLimit) ? parsedMessage.Substring(0, TextLimit) : parsedMessage;
    }

    private void proceedUI()
    {
        float percentage = (Time.timeSinceLevelLoad - mUiProcessTime) / TransitTime;

        // Set our position as a fraction of the distance between the markers.
        mCanvasRect.localPosition = new Vector3(mCanvasRect.localPosition.x, Mathf.Lerp(mCanvasRect.localPosition.y, mSelectedPosition, percentage), 0);

        if (percentage >= 1)
        {
            mUiProcessTime = -1;
        }
    }

    // Update is called once per frame
    void OnGUI()
    {
        // UI
        if (movingUI)
            proceedUI();
    }
}
