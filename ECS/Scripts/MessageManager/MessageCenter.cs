using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCenter : Singleton<MessageCenter>
{
    Dictionary<string, Action<Notification>> messages = new Dictionary<string, Action<Notification>>();
    public void AddListener(string msg,Action<Notification> action)
    {
        if (messages.ContainsKey(msg))
        {
            messages[msg] += action;
        }
        else
        {
            messages.Add(msg,action);
        }
    }
    public void RemoveListener(string msg,Action<Notification> action)
    {
        if (messages.ContainsKey(msg))
        {
            messages[msg] -= action;
            if (messages[msg]==null)
            {
                messages.Remove(msg);
            }
        }
    }
    public void BroadCast(string msg,Notification notification)
    {
        if (messages.ContainsKey(msg))
        {
            messages[msg]?.Invoke(notification);
        }
    }
}
public class Notification
{
    public string msgtype;
    public object[] data;
    public void Refresh(string _msgtype,params object[] _data)
    {
        this.msgtype = _msgtype;
        this.data = _data;
    }
    public void Clear()
    {
        this.msgtype = string.Empty;
        this.data = null;
    }
}
