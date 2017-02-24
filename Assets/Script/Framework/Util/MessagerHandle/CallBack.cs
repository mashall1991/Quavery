using System;

public class CallBackClass
{
    public delegate void CallBack();

    public delegate void CallBack<T>(T arg1);

    public delegate void CallBack<T, U>(T arg1, U arg2);

    public delegate void CallBack<T, U, V>(T arg1, U arg2, V arg3);

    /// <summary>
    /// 从网络层抛出的回调方法
    /// </summary>
    /// <param name="obj"></param>
    public delegate void NetworkCallback(object obj);

}
