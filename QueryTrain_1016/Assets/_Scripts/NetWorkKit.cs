using UnityEngine;
using System.Collections;

public delegate void DidReceiveDataDelgate(string text);         //请求成功
public delegate void DidFailedDataDelgate(string error);         //请求失败
public class NetWorkKit : MonoBehaviour
{
    #region 单例
    private static NetWorkKit _instance = null;
    private NetWorkKit() { }
    public static NetWorkKit GetInstance()
    {
        if (_instance == null)
        {
            GameObject netObj = new GameObject("NetWorkKit");    //创建一个对象
            _instance = netObj.AddComponent<NetWorkKit>();       //为其添加NetWorkKit脚本， _instance 指向创建的实例
        }
        return _instance;     //返回该实例
    }
    #endregion
   //进行Get请求
    public void GetRequestData(string urlStr, DidReceiveDataDelgate didReceiveDataDelgate, DidFailedDataDelgate didFailedDataDelgate)
    {
        StartCoroutine(RequestData(urlStr, null, didReceiveDataDelgate, didFailedDataDelgate));   //开启协程
    }
    //进行Post请求，在这个Demo中没有用到，但是为了后续的扩展性，我们还是加上了
    public void PostRequestData(string urlStr, WWWForm form, DidReceiveDataDelgate didReceiveDataDelgate, DidFailedDataDelgate didFailedDataDelgate)
    {
        StartCoroutine(RequestData(urlStr, form, didReceiveDataDelgate, didFailedDataDelgate));    //开启协程
    }
    //协程，根据传进来的URL，从网站上请求数据，成功后，通过委托得到请求到的数据，失败类似
    IEnumerator RequestData(string urlStr, WWWForm form, DidReceiveDataDelgate didReceiveDataDelgate, DidFailedDataDelgate didFailedDataDelgate)
    {
        WWW www = null;     
        if (form == null)
            www = new WWW(urlStr);    //进行Get请求
        else
            www = new WWW(urlStr, form);   //进行Post请求
        while (!www.isDone)
        {
            yield return null;   //等待请求完成
        }
        if (www.error == null)
            didReceiveDataDelgate(www.text);    //没有错误，外界通过向委托中传递方法来获得请求到的数据
        else
            didFailedDataDelgate(www.error);   //请求出错了，外界通过向委托中传递方法来获得错误信息
    }
}
