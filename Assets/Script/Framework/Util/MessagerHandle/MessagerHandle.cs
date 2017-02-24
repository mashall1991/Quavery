using System;
using UnityEngine;
using System.Collections.Generic;
using Framework;

public static class MessagerHandle
{
    /// <summary>
    /// 消息处理器(字典)
    /// </summary>
    private static Dictionary<int, Delegate> mMessagerHandleDic = new Dictionary<int, Delegate>();

    private static Dictionary<int, Type> mMessagerTypeDic = new Dictionary<int, Type>();

  

    #region 添加消息回调

    public static void AddMessager(int id, CallBackClass.CallBack callback)
    {
        if(mMessagerHandleDic.ContainsKey(id))
        {
            if(CheakTypeEquality(mMessagerHandleDic[id],callback))
            {
                mMessagerHandleDic[id] = Delegate.Combine(mMessagerHandleDic[id], callback);
            }
        }
        else
        {
            mMessagerHandleDic.Add(id,callback);
        }
    }

    public static void AddMessager<T>(int id, CallBackClass.CallBack<T> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            if (CheakTypeEquality(mMessagerHandleDic[id], callback))
            {
                mMessagerHandleDic[id] = Delegate.Combine(mMessagerHandleDic[id], callback);
            }
        }
        else
        {
            mMessagerHandleDic.Add(id, callback);
        }
    }

    public static void AddMessager<T, U>(int id, CallBackClass.CallBack<T, U> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            if (CheakTypeEquality(mMessagerHandleDic[id], callback))
            {
                mMessagerHandleDic[id] = Delegate.Combine(mMessagerHandleDic[id], callback);
            }
        }
        else
        {
            mMessagerHandleDic.Add(id, callback);
        }
    }

    public static void AddMessager<T, U, V>(int id, CallBackClass.CallBack<T, U, V> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            if (CheakTypeEquality(mMessagerHandleDic[id], callback))
            {
                mMessagerHandleDic[id] = Delegate.Combine(mMessagerHandleDic[id], callback);
            }
        }
        else
        {
            mMessagerHandleDic.Add(id, callback);
        }
    }

    private static bool CheakTypeEquality(Delegate deleSource,Delegate deleTarget)
    {
        if(deleSource == null)
            return true;

        bool flag = deleSource.GetType() == deleTarget.GetType();
        if(!flag)
        {
            string source = deleSource.GetType().ToString();
            string target = deleTarget.GetType().ToString();
        }
        return flag;
    }

    #endregion

    #region 删除消息回调
    public static bool RemoveMessager(int id, CallBackClass.CallBack callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            mMessagerHandleDic[id] = Delegate.Remove(mMessagerHandleDic[id], callback);
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RemoveMessager<T>(int id, CallBackClass.CallBack<T> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            mMessagerHandleDic[id] = Delegate.Remove(mMessagerHandleDic[id], callback);
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RemoveMessager<T, U>(int id, CallBackClass.CallBack<T, U> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            mMessagerHandleDic[id] = Delegate.Remove(mMessagerHandleDic[id], callback);
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RemoveMessager<T, U, V>(int id, CallBackClass.CallBack<T, U, V> callback)
    {
        if (mMessagerHandleDic.ContainsKey(id))
        {
            mMessagerHandleDic[id] = Delegate.Remove(mMessagerHandleDic[id], callback);
        }
        else
        {
            return false;
        }
        return true;
    }

    #endregion

    #region 清空消息处理器
    public static bool ClearMessager(int id)
    {
        bool clearFlag = false;
        if (mMessagerHandleDic.ContainsKey(id))
        {
            //强制性清空
            mMessagerHandleDic[id] = Delegate.RemoveAll(mMessagerHandleDic[id], mMessagerHandleDic[id]);
            clearFlag = true;
        }
        else
        {
        }

        if(clearFlag)
        {
            mMessagerHandleDic.Remove(id);
        }
        return clearFlag;
    }
    #endregion

    #region 分发消息处理器
    public static bool RunMessager(int id)
    {
        Delegate deleAll;
        if (mMessagerHandleDic.TryGetValue(id, out deleAll))
        {
            if (deleAll == null || deleAll.GetType() != typeof(CallBackClass.CallBack))
                return false;

            CallBackClass.CallBack callback;
            Delegate[] deleArray = deleAll.GetInvocationList();
            if (deleArray != null)
            {
                for (int i = 0; i < deleArray.Length; i++)
                {
                    callback = (CallBackClass.CallBack)deleArray[i];
                    if (callback != null)
                    {
                        callback();
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RunMessager<T>(int id,T t)
    {
        Delegate deleAll;
        if (mMessagerHandleDic.TryGetValue(id, out deleAll))
        {
            if (deleAll == null || deleAll.GetType() != typeof(CallBackClass.CallBack<T>))
                return false;

            Delegate[] deleArray = deleAll.GetInvocationList();
            if (deleArray != null)
            {
                for (int i = 0; i < deleArray.Length; i++)
                {
                    CallBackClass.CallBack<T> callback = (CallBackClass.CallBack<T>)deleArray[i];
                    if (callback != null)
                    {
                        callback(t);
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RunMessager<T, U>(int id, T t, U u)
    {
        Delegate deleAll;
        if (mMessagerHandleDic.TryGetValue(id, out deleAll))
        {
            if (deleAll == null || deleAll.GetType() != typeof(CallBackClass.CallBack<T, U>))
                return false;

            Delegate[] deleArray = deleAll.GetInvocationList();
            if (deleArray != null)
            {
                for (int i = 0; i < deleArray.Length; i++)
                {
                    CallBackClass.CallBack<T,U> callback = (CallBackClass.CallBack<T, U>)deleArray[i];
                    if (callback != null)
                    {
                        callback(t,u);
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    public static bool RunMessager<T,U,V>(int id, T t,U u,V v)
    {
        Delegate deleAll;
        if (mMessagerHandleDic.TryGetValue(id, out deleAll))
        {
            if (deleAll == null || deleAll.GetType() != typeof(CallBackClass.CallBack<T, U, V>))
                return false;

            Delegate[] deleArray = deleAll.GetInvocationList();
            if (deleArray != null)
            {
                for (int i = 0; i < deleArray.Length; i++)
                {
                    CallBackClass.CallBack<T, U, V> callback = (CallBackClass.CallBack<T, U, V>)deleArray[i];
                    if (callback != null)
                    {
                        callback(t, u, v);
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }
    #endregion

    #region 打印信息
    
    /// <summary>
    /// 打印所有委托链方法名称
    /// </summary>
    public static void PrintAllMessagerInfo()
    {
        string msg = string.Empty;
        foreach (var item in mMessagerHandleDic)
        {
            msg += item.Key.ToString()+"     ";
            Delegate[] callback = item.Value.GetInvocationList();
            if(callback != null)
            {
                for (int i = 0; i < callback.Length; i++)
                {
                    msg += callback[i].Target.ToString()+" method is "+callback[i].Method.Name+"\t";
                }
            }
            msg += "\n";
        }
    }

    /// <summary>
    /// 打印单个委托链
    /// </summary>
    /// <param name="id"></param>
    public static void PrintMessagerInfo(int id)
    {
        string msg = string.Empty;
        foreach (var item in mMessagerHandleDic)
        {
            if (item.Key != id)
                continue;
            msg += item.Key.ToString() + "\t";
            Delegate[] callback = item.Value.GetInvocationList();
            if (callback != null)
            {
                for (int i = 0; i < callback.Length; i++)
                {
                    msg += callback[i].Target.ToString() + " method is " + callback[i].Method.Name + "\t";
                }
            }
            msg +="\n";
        }
        Debug.Log(msg);
    }

    #endregion

}



