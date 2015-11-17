using UnityEngine;
using System.Collections;

public class TrainModel     //不需要继承MonoBehaviour
{
    /* "name": "G656",
       "start": "西安北",
       "end": "北京西",
       "starttime": "10:10",
       "endtime": "16:27",
       "mileage": "1212km"
    */
    public string trainName;       //车名
    public string trainStart;      //始发站
    public string trainEnd;        //终点站
    public string trainStartTime;  //出发时间
    public string trainEndTime;    //到站时间
    public string trainMileage;    //距离

    //私有化默认构造函数
    private TrainModel() { }
    //构造函数
    private TrainModel(string trainName, string trainStart, string trainEnd, string trainStartTime, string trainEndTime, string trainMileage)
    {
        Init(trainName, trainStart, trainEnd, trainStartTime, trainEndTime, trainMileage);
    }
    //初始化函数
    private void Init(string trainName, string trainStart, string trainEnd, string trainStartTime, string trainEndTime, string trainMileage)
    {
        this.trainName = trainName;
        this.trainStart = trainStart;
        this.trainEnd = trainEnd;
        this.trainStartTime = trainStartTime;
        this.trainEndTime = trainEndTime;
        this.trainMileage = trainMileage;
    }
    //静态的创建TrainModel的方法
    public static TrainModel Create(string trainName, string trainStart, string trainEnd, string trainStartTime, string trainEndTime, string trainMileage)
    {
        return new TrainModel(trainName, trainStart, trainEnd, trainStartTime, trainEndTime, trainMileage);
    }
    //静态的创建TrainModel的方法  ，这个方法在后面的显示脚本中会用到
    public static TrainModel CreateModel(TrainModel model)
    {
        return new TrainModel(model.trainName, model.trainStart, model.trainEnd, model.trainStartTime, model.trainEndTime, model.trainMileage);
    }
}
