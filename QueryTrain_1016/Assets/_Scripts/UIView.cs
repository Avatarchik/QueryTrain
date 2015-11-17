using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIView : MonoBehaviour
{
    public UILabel inputLabel;      //用来接收用户输入的车次名称
    public GameObject trainInfoShow;        //车次信息展示 在其下挂有label用来显示车次信息
    public GameObject stationInfoShow;      //站点信息展示 在其下挂有label用来显示站点信息
    public UIButton backButton;     //上一站   点击后切换到上一个站点
    public UIButton nextButton;     //下一站   点击后切换到下一个站点

    public GameObject errorShow;      //错误信息  在其下挂有label用来显示错误信息
    public GameObject query;        //查询管理对象
    public GameObject appkey;       //Appkey 管理对象

    private TrainController trainController;    //TrainController的引用
    private TrainModel trainModel;      // 车次模型
    private List<StationModel> stationModelsList;    //用来存放我们获取到的站点
    private int selectedIndex = 0;   //当前选中的站点在stationModelList中的索引
    private int length = 0;     //stationModelList 的长度，用来表示有多少个站点

    private UITextList trainLabel;    //展示车次信息，将其做成可拖拽，方便查看
    private UITextList stationLabel;   //展示站点信息，将其做成可拖拽，方便查看
    private UILabel errorLabel;     //展示出错信息
    private UILabel appkeyLabel;    //用来接收用户输入的appkey
    void Start()
    {
        //获得引用，赋值等操作，并将一开始不显示的组件隐藏
        trainController = TrainController.GetInstance();
        query.SetActive(false);
        trainInfoShow.SetActive(false);
        stationInfoShow.SetActive(false);
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        errorShow.SetActive(false);
        trainLabel = trainInfoShow.transform.FindChild("TrainLabel").GetComponent<UITextList>();
        stationLabel = stationInfoShow.transform.FindChild("StationLabel").GetComponent<UITextList>();
        errorLabel = errorShow.transform.FindChild("ErrorLabel").GetComponent<UILabel>();
        appkeyLabel = appkey.transform.FindChild("AppKeyInput Field/Label").GetComponent<UILabel>();
        stationModelsList = new List<StationModel>();
    }
    //点击了查询按钮，它将添加到查询按钮和InputField的提交 事件
    public void OnQuaryButtonClick()
    {
        if (inputLabel.text != "" && Global.appkey != "")
        {
            //如果用户输入了appkey，和车次名称，则需要我们去请求数据
            #region  坑 坑 坑 // textList在激活后会清空label的显示，所以我们需要在请求数据之前，就激活它，并且最好手动清空一次
            trainInfoShow.SetActive(true);
            trainLabel.Clear();
            stationInfoShow.SetActive(true);
            stationLabel.Clear();
            #endregion

            #region 查询数据
            Global.trainName = inputLabel.text;    //获取用户输入的车次名称
            //调用 GetTrainStationInfo请求数据 并用请求到的数据来刷新显示
            trainController.GetTrainStationInfo((Dictionary<string, TrainModel> trainDic) =>
            {
                //这里也需要注意，我们为什么不直接使用我们获取到的模型？
                //原因是，我们如果使用获取到的模型，如果后续我们操作模型的话，
                //由于模型是引用类型的，我们修改的话，会导致模型变化，
                //下次再使用的话，模型就变了，导致程序出现莫名其妙的Bug。
                //所以我们在模型类中添加一个静态的通过模型来构建新模型的方法，
                //修改的话修改的是后来重新创建的模型，跟我们直接获取的模型没有关系
                trainModel = TrainModel.CreateModel(trainDic[Global.trainName]);
                UpdateTrainInfo();   //更新车次显示
            },
            (Dictionary<string, Dictionary<string, StationModel>> stationDic) =>
            {
                stationModelsList.Clear();  //清空站点列表
                //定义一个站点数据字典，用来保存获取到的站点信息
                Dictionary<string, StationModel> dic = stationDic["station_list"];
                //遍历所有的站点
                foreach (string id in dic.Keys)
                {
                    //将站点加到stationModelsList中
                    stationModelsList.Add(StationModel.CreateModel(dic[id]));
                }
                selectedIndex = 0;    //初始选中的站点索引更新为第一个
                length = stationModelsList.Count;  //获得站点的个数，
                UpdateStationInfo();    //更新站点显示
            },
            (string error) =>
            {
                HideShow();     //隐藏显示
                UpdateErrorInfo(error);   //更新出错显示
            });
            #endregion
        }
    }
    //更新车次显示
    private void UpdateTrainInfo()
    {
        errorShow.SetActive(false);      //隐藏错误信息展示
        trainInfoShow.SetActive(true);   //激活车次信息展示
        trainLabel.Clear();    //每次更新，都要清空一遍
        //将车次信息加到textList中
        trainLabel.Add("车次：" + "\t" + trainModel.trainName);
        trainLabel.Add("始发站：" + "\t" + trainModel.trainStart);
        trainLabel.Add("终点站：" + "\t" + trainModel.trainEnd);
        trainLabel.Add("出发时间：" + "\t" + trainModel.trainStartTime);
        trainLabel.Add("到站时间：" + "\t" + trainModel.trainEndTime);
        trainLabel.Add("距离：" + "\t" + trainModel.trainMileage);
    }
    //更新站点显示
    private void UpdateStationInfo()
    {
        stationInfoShow.SetActive(true);   //激活站点显示
        backButton.gameObject.SetActive(true);  //激活“上一站”按钮
        nextButton.gameObject.SetActive(true);  //激活“下一站”按钮
        stationLabel.Clear();    //每次更新，都要清空一遍
        //将当前选中的站点信息加到站点textList中
        stationLabel.Add("序号:" + "\t" + stationModelsList[selectedIndex].stationID.ToString());
        stationLabel.Add("名称:" + "\t" + stationModelsList[selectedIndex].stationName.ToString());
        stationLabel.Add("到达时间:" + "\t" + stationModelsList[selectedIndex].stationArrivedTime.ToString());
        stationLabel.Add("停留:" + "\t" + stationModelsList[selectedIndex].stationStay.ToString());
        stationLabel.Add("发车时间:" + "\t" + stationModelsList[selectedIndex].stationLeaveTime.ToString());
        stationLabel.Add("里程:" + "\t" + stationModelsList[selectedIndex].stationMileage.ToString());
        stationLabel.Add("一等座:" + "\t" + stationModelsList[selectedIndex].stationFsoftSeat.ToString());
        stationLabel.Add("二等座:" + "\t" + stationModelsList[selectedIndex].stationSsoftSeat.ToString());
        stationLabel.Add("硬座:" + "\t" + stationModelsList[selectedIndex].stationHardSeat.ToString());
        stationLabel.Add("软座:" + "\t" + stationModelsList[selectedIndex].stationSoftSeat.ToString());
        stationLabel.Add("硬卧:" + "\t" + stationModelsList[selectedIndex].stationHardSleep.ToString());
        stationLabel.Add("软卧:" + "\t" + stationModelsList[selectedIndex].stationSoftSleep.ToString());
        stationLabel.Add("无座:" + "\t" + stationModelsList[selectedIndex].stationWuZuo.ToString());
        stationLabel.Add("商务座:" + "\t" + stationModelsList[selectedIndex].stationSWZ.ToString());
        stationLabel.Add("特等座:" + "\t" + stationModelsList[selectedIndex].stationTDZ.ToString());
        stationLabel.Add("高级软卧:" + "\t" + stationModelsList[selectedIndex].stationGJRW.ToString());
    }
    //更新错误信息
    private void UpdateErrorInfo(string error)
    {
        errorShow.SetActive(true);    //激活错误信息展示
        errorLabel.text = error;      //显示错误信息
    }
    //隐藏显示
    private void HideShow()
    {
        trainInfoShow.SetActive(false);
        stationInfoShow.SetActive(false);
        backButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
    //点击了上一站按钮后执行
    public void OnBackButtonClick()
    {
        selectedIndex--;      //当前选中的站点索引减一
        if (selectedIndex == -1)       //如果索引越界，让其等于最后一个站点索引
            selectedIndex = length - 1;
        UpdateStationInfo();     //更新站点显示
    }
    //点击了下一站按钮后执行
    public void OnNextButtonClick()
    {
        selectedIndex++;     //当前选中的站点所有增一
        selectedIndex %= length;     //对长度取余，防止索引越界
        UpdateStationInfo();     //更新站点显示
    }
    //点击了OK按钮后执行，我们需要先输入appkey然后点击OK按钮，才可以输入车次名称，然后查询
    public void OnOKButtonClick()
    {
        if (appkeyLabel.text == "")
        {
            UpdateErrorInfo("Appkey不可用，请重新输入！");
        }
        else
        {
            Global.appkey = appkeyLabel.text;
            appkey.SetActive(false);
            query.SetActive(true);
        }
    }
}
