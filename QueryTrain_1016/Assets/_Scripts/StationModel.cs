using UnityEngine;
using System.Collections;

public class StationModel            //不需要继承MonoBehaviour
{
    /*
      train_id int 站点序号 
  　　station_name string 站点名称 
  　　arrived_time string 到达时间 
  　　leave_time string 发车时间 
  　　stay string 停留 
  　　mileage string 里程 
  　　ssoftSeat string 二等座 
  　　fsoftSeat string 一等座 
  　　hardSead string 硬座 
  　　softSeat string 软座 
  　　hardSleep string 硬卧 
  　　softSleep string 软卧 
  　　wuzuo string 无座 
  　　swz string 商务座 
  　　tdz string 特等座 
  　　gjrw string 高级软卧 
     */
    public string stationID;        //站点序号
    public string stationName;      //站点名称
    public string stationArrivedTime;   //到达时间
    public string stationLeaveTime;     //发车时间
    public string stationMileage;      //里程
    public string stationFsoftSeat;     //一等座
    public string stationSsoftSeat;    //二等座
    public string stationHardSeat;      //硬座
    public string stationSoftSeat;      //软座
    public string stationHardSleep;     //硬卧
    public string stationSoftSleep;     //软卧
    public string stationWuZuo;         //无座
    public string stationSWZ;           //商务座
    public string stationTDZ;           //特等座
    public string stationGJRW;          //高级软卧
    public string stationStay;          //停留
   
    //私有化默认构造函数
    private StationModel() { }
    //构造函数
    private StationModel(string stationID, string stationName, string stationArrivedTime,
         string stationLeaveTime, string stationMileage, string stationFsoftSeat,
         string stationSsoftSeat, string stationHardSeat, string stationSoftSeat,
         string stationHardSleep, string stationSoftSleep, string stationWuZuo,
         string stationSWZ, string stationTDZ, string stationGJRW, string stationStay)
    {
        Init(stationID, stationName, stationArrivedTime,
          stationLeaveTime, stationMileage, stationFsoftSeat,
          stationSsoftSeat, stationHardSeat, stationSoftSeat,
          stationHardSleep, stationSoftSleep, stationWuZuo,
          stationSWZ, stationTDZ, stationGJRW, stationStay);
    }
    //初始化函数
    private void Init(string stationID, string stationName, string stationArrivedTime,
         string stationLeaveTime, string stationMileage, string stationFsoftSeat,
         string stationSsoftSeat, string stationHardSeat, string stationSoftSeat,
         string stationHardSleep, string stationSoftSleep, string stationWuZuo,
         string stationSWZ, string stationTDZ, string stationGJRW, string stationStay)
    {
        this.stationID = stationID;
        this.stationName = stationName;
        this.stationArrivedTime = stationArrivedTime;
        this.stationLeaveTime = stationLeaveTime;
        this.stationMileage = stationMileage;
        this.stationFsoftSeat = stationFsoftSeat;
        this.stationSsoftSeat = stationSsoftSeat;
        this.stationHardSeat = stationHardSeat;
        this.stationSoftSeat = stationSoftSeat;
        this.stationHardSleep = stationHardSleep;
        this.stationSoftSleep = stationSoftSleep;
        this.stationWuZuo = stationWuZuo;
        this.stationSWZ = stationSWZ;
        this.stationTDZ = stationTDZ;
        this.stationGJRW = stationGJRW;
        this.stationStay = stationStay;
    }
    //静态的创建StationModel的方法
    public static StationModel Create(string stationID, string stationName, string stationArrivedTime,
         string stationLeaveTime, string stationMileage, string stationFsoftSeat,
         string stationSsoftSeat, string stationHardSeat, string stationSoftSeat,
         string stationHardSleep, string stationSoftSleep, string stationWuZuo,
         string stationSWZ, string stationTDZ, string stationGJRW, string stationStay)
    {
        return new StationModel(stationID, stationName, stationArrivedTime,
          stationLeaveTime, stationMileage, stationFsoftSeat,
          stationSsoftSeat, stationHardSeat, stationSoftSeat,
          stationHardSleep, stationSoftSleep, stationWuZuo,
          stationSWZ, stationTDZ, stationGJRW, stationStay);
    }
    //静态的创建StationModel的方法  ，这个方法在后面的显示脚本中会用到
    public static StationModel CreateModel(StationModel model)
    {
        return new StationModel(model.stationID, model.stationName, model.stationArrivedTime,
             model.stationLeaveTime, model.stationMileage, model.stationFsoftSeat,
             model.stationSsoftSeat, model.stationHardSeat, model.stationSoftSeat,
             model.stationHardSleep, model.stationSoftSleep, model.stationWuZuo,
             model.stationSWZ, model.stationTDZ, model.stationGJRW, model.stationStay);
    }
}
