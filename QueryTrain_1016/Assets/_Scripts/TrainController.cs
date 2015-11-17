using UnityEngine;
using System.Collections;
using System.Collections.Generic;     //泛型
using LitJson;      //导入LitJson


public delegate void DidLoadedTrainDataDelgate(Dictionary<string, TrainModel> trainDic);    //请求到车次数据的委托
public delegate void DidLoadedStationDataDelgate(Dictionary<string, Dictionary<string, StationModel>> stationDic); //请求到站点数据的委托
public delegate void DidFailedDelgate(string error);   //没有对应车次的委托
public class TrainController    //不需要继承MonoBehaviour
{
    #region 单例
    private static TrainController _instance = null;
    private TrainController()
    {
        nwk = NetWorkKit.GetInstance();   //获取NetWorkKit实例
    }
    public static TrainController GetInstance()    //静态的获取实例方法
    {
        if (_instance == null)
            _instance = new TrainController();
        return _instance;
    }
    #endregion
    private static NetWorkKit nwk;      //定义NetWorkKit的实例
    /// <summary>
    /// 进行Get请求，我们使用lambda表达式来获取数据，并解析
    /// </summary>
    /// <param name="didLoadedTrainDataDelgate">请求到车次信息</param>
    /// <param name="didLoadedStationDataDelgate">请求到站点信息</param>
    /// <param name="didFailedDelgate">请求不到用户输入的车次，给用户一个提示</param>
    public void GetTrainStationInfo(DidLoadedTrainDataDelgate didLoadedTrainDataDelgate, DidLoadedStationDataDelgate didLoadedStationDataDelgate, DidFailedDelgate didFailedDelgate)
    {
        Dictionary<string, TrainModel> trainDic = new Dictionary<string, TrainModel>();  //用来存放车次信息的字典
        //用来存放站点信息的字典，关于这两个字典的定义，可以根据返回的数据类型来定义，格式不唯一
        Dictionary<string, Dictionary<string, StationModel>> stationDic = new Dictionary<string, Dictionary<string, StationModel>>();   
        string urlStr = Global.urlTrain + "?name=" + Global.trainName + "&key=" + Global.appkey; //拼接请求需要的Url
        nwk.GetRequestData(urlStr, (string dateText) =>
        {
            //使用Try - catch 语句，防止查询不到对应的车次，程序报错
            try
            {
                #region 解析数据 ，并将解析出来的数据加到我们创建的对应的字典中
                JsonData root = JsonMapper.ToObject(dateText);  //将json字符串转换为 JsonData 对象
                JsonData result = root["result"];             //继续往下
                JsonData trainInfo = result["train_info"];    //获取车次信息的JsonData 对象
                JsonData stationList = result["station_list"];  //获取站点信息 数组的JsonData 对象
                //创建车次模型
                TrainModel train = TrainModel.Create(trainInfo["name"].ToString(), trainInfo["start"].ToString(), trainInfo["end"].ToString(), trainInfo["starttime"].ToString(), trainInfo["endtime"].ToString(), trainInfo["mileage"].ToString());
                //将车次名称作为Key，车次模型实例作为Value加到我们的车次字典中
                trainDic.Add(Global.trainName, train);
                //定义站点数据字典，Key为站点序号，Value为站点模型
                Dictionary<string, StationModel> dic = new Dictionary<string, StationModel>();
                //遍历整个站点数组
                for (int i = 0; i < stationList.Count; i++)
                {
                    JsonData data = stationList[i];   //获取站点的JsonData 对象 
                    //创建站点模型
                    StationModel station = StationModel.Create(data["train_id"].ToString(), data["station_name"].ToString(), data["arrived_time"].ToString(), data["leave_time"].ToString(),
                        data["mileage"].ToString(), data["fsoftSeat"].ToString(), data["ssoftSeat"].ToString(), data["hardSead"].ToString(), data["softSeat"].ToString(),
                        data["hardSleep"].ToString(), data["softSleep"].ToString(), data["wuzuo"].ToString(), data["swz"].ToString(), data["tdz"].ToString(),
                        data["gjrw"].ToString(), data["stay"].ToString());
                    dic.Add(data["train_id"].ToString(), station);  //将站点模型加入到站点数据字典
                }
                //将得到的包含所有站点的站点字典加入站点信息字典
                stationDic.Add("station_list", dic);
                #endregion

                if (didLoadedTrainDataDelgate != null)
                    didLoadedTrainDataDelgate(trainDic);    //如果外界传进来这个方法了，则将车次字典传过去
                if (didLoadedStationDataDelgate != null)
                    didLoadedStationDataDelgate(stationDic);  //如果外界传进来这个方法了，则将站点信息字典传过去
            }
            catch (System.Exception)       //查询不到用户输入从车次
            {
                if (didFailedDelgate != null)
                    didFailedDelgate("对不起，没有找到对应的车次");
                Debug.Log("输入信息有误");
            }
        }, null);     //我们暂不考虑请求失败的情况
    }
}
