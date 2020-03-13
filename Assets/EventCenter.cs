using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventCenter
{
    private static Dictionary<EventType, Delegate> EventTable = new Dictionary<EventType, Delegate>();

    private static void AddListen(EventType eventType,Delegate callBack)
    {
        if (EventTable.ContainsKey(eventType))
        {
            EventTable.Add(eventType,null);
        }
        Delegate d = callBack;
        if (d != null || d.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    private static void RemoveListen(EventType eventType, Delegate callBack)
    {
        if (EventTable.ContainsKey(eventType))
        {
            Delegate d = EventTable[eventType]; if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
    }

    public static void AddListener<T>(EventType eventType,CallBack callBack)
    {
        AddListen(eventType,callBack);
        EventTable[eventType] = (CallBack)EventTable[eventType] + callBack;
    }



}
