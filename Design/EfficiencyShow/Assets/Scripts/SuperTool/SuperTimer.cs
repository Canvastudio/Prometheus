using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;


/// <summary>
/// 管理游戏时间、协程、帧调用等。
/// 所有游戏逻辑相关（非UI）的时间应该用SuperTimer.DeltaTime，而不是用Timer.deltaTime
/// </summary>
public class SuperTimer : SingleObject<SuperTimer>
{
    protected override void Init()
    {
        DeltaTime = Time.deltaTime * timeScale;
    }

    /// <summary>
    /// SuperTimer的初始化方法
    /// </summary>
    public void CreatAndBound(Component target, int targetFrame = 30, bool showFPS = true)
    {
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = targetFrame;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        target.gameObject.AddComponent<_SuperTimer>().showFPS = showFPS;
    }

    /// <summary>
    /// 内部类负责更新SuperTimer的Update
    /// </summary>
    private class _SuperTimer : SingleGameObject<_SuperTimer>
    {

        public bool showFPS;
        /// <summary>
        /// FPS刷新时间
        /// </summary>
        public float fpsFreshInterval = 0.5F;
        private float f_LastInterval;
        private int i_Frames = 0;
        private float f_Fps;

        public bool checkCor;
        /// <summary>
        /// 这是显示到外面检查协程数量的，赋值无效
        /// </summary>
        public int countCor;

        public bool checkFrameFunc;
        /// <summary>
        /// 检查帧函数数量
        /// </summary>
        public int countFrameFunc;


        public bool checkMsg;
        /// <summary>
        /// 检查监听消息的数量
        /// </summary>
        public int countMsg;

        public bool checkClickLimit;
        public string ClickLimitInf;


        protected override void Init()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            f_LastInterval = Time.realtimeSinceStartup;
            i_Frames = 0;
        }

        void Update()
        {
            SuperTimer.Instance.Update();
            CalculateFps();
        }

        void OnGUI()
        {
            if (!showFPS) return;
            GUIStyle bb = new GUIStyle
            {
                normal =
                {
                    background = null,
                    textColor = new Color(1, 1, 0),
                },
                fontSize = 20
            };
            GUI.Label(new Rect(0, 0, 200, 200), "FPS:" + f_Fps.ToString("f2"), bb);
        }

        void CalculateFps()
        {
            if (!showFPS) return;
            ++i_Frames;
            if (Time.realtimeSinceStartup > f_LastInterval + fpsFreshInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
                i_Frames = 0;
                f_LastInterval = Time.realtimeSinceStartup;
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 内部类负责对Inspector的一些修改
    /// </summary>
    [CustomEditor(typeof(_SuperTimer))]
    public class SuperTimerInspector : Editor
    {
        private bool b_disFps;//是否显示FPS
        bool b_disCor = false;//是否显示协程数量
        bool b_disFra = false;//是否显示帧函数数量
        bool b_disMsg = false;//是否显示监听消息数量
        bool b_disClickLimit = false;//是否检查按键锁定

        void OnEnable() { }

        void OnDisable() { }

        void OnDestory() { }


        public override void OnInspectorGUI()
        {

            _SuperTimer.Instance.showFPS = EditorGUILayout.Toggle("是否显示FPS", _SuperTimer.Instance.showFPS);
            if (_SuperTimer.Instance.showFPS)
            {
                int progress = (int)(_SuperTimer.Instance.fpsFreshInterval * 1000);
                progress = EditorGUILayout.IntSlider("刷新间隔(ms)", progress, 1, 1000);
                _SuperTimer.Instance.fpsFreshInterval = (float)progress / 1000;
            }


            b_disCor = EditorGUILayout.Toggle("检查协程", b_disCor);
            _SuperTimer.Instance.checkCor = b_disCor;
            if (b_disCor) EditorGUILayout.LabelField("运行中的协程数量：" + _SuperTimer.Instance.countCor);


            b_disFra = EditorGUILayout.Toggle("检查帧函数", b_disFra);
            _SuperTimer.Instance.checkFrameFunc = b_disFra;
            if (b_disFra) EditorGUILayout.LabelField("运行中的帧函数数量：" + _SuperTimer.Instance.countFrameFunc);


            b_disMsg = EditorGUILayout.Toggle("监听消息数量", b_disMsg);
            _SuperTimer.Instance.checkMsg = b_disMsg;
            if (b_disMsg) EditorGUILayout.LabelField("监听的消息数量：" + _SuperTimer.Instance.countMsg);


            b_disClickLimit = EditorGUILayout.Toggle("检查按键锁定", b_disClickLimit);
            _SuperTimer.Instance.checkClickLimit = b_disClickLimit;
            if (b_disClickLimit) EditorGUILayout.LabelField(_SuperTimer.Instance.ClickLimitInf, GUILayout.ExpandHeight(true));

        }
    }
#endif


    /// <summary>
    /// 每帧回调方法
    /// </summary>
    private readonly List<XY<PerFrameHandle, object>> perFrameFunc = new List<XY<PerFrameHandle, object>>();
    private readonly List<XY<PerFrameHandle, object>> perFrameFunc_addList = new List<XY<PerFrameHandle, object>>();
    private readonly List<PerFrameHandle> perFrameFunc_removeList = new List<PerFrameHandle>();
    public delegate bool PerFrameHandle(object arg = null);

    public delegate void TimerHandle(object arg = null);

    public delegate bool SuperWaitHandle(object arg = null);

    public static float DeltaTime { get; private set; }
    public static float timeScale = 1.0f;
    private static readonly List<TimerTick> TimerList = new List<TimerTick>();

    private DateTime now;

    /// <summary>
    /// 游戏内的当前时间
    /// </summary>
    public DateTime Now
    {
        private set { now = value; }
        get
        {
            if (!isDateLapses)
            {
                throw new NullReferenceException("必须先设置标准日期");
            }
            return now;
        }
    }

    /// <summary>
    /// 游戏内的当前时间的字符串形式
    /// </summary>
    public string NowString
    {
        get { return Now.ToString(CultureInfo.InvariantCulture); }
    }

    /// <summary>
    /// 日期名字，添加时间
    /// </summary>
    private readonly List<XY<string, DateTime>> gameTimeDates = new List<XY<string, DateTime>>();

    private bool isDateLapses = false;

    private readonly List<SuperCoroutine> coroutineList = new List<SuperCoroutine>();
    private readonly List<SuperCoroutine> coroutineAddList = new List<SuperCoroutine>();



    void Update()
    {
        DeltaTime = Time.deltaTime * timeScale;
        //计时器更新
        for (int i = 0; i < TimerList.Count; i++)
        {
            if (TimerList[i].IsDone())
                TimerList.RemoveAt(i--);
            else
                TimerList[i].Update();
        }
        //帧函数更新
        foreach (var v in perFrameFunc_addList) perFrameFunc.Add(v);
        perFrameFunc_addList.Clear();
        foreach (var v in perFrameFunc_removeList) perFrameFunc.RemoveAll(xy => xy.x == v);
        perFrameFunc_removeList.Clear();
        for (int i = 0; i < perFrameFunc.Count; i++)
        {
            if (SuperTool.CheckDelegate(perFrameFunc[i].x))
            {
                if (perFrameFunc[i].x(perFrameFunc[i].y))
                    perFrameFunc.RemoveAt(i--);
            }
            else
            {
                perFrameFunc.RemoveAt(i--);
            }

        }

        //游戏时间更新
        if (isDateLapses) Now = Now.AddSeconds(DeltaTime);

        //协程更新
        coroutineList.AddRange(coroutineAddList);
        coroutineAddList.Clear();

        List<SuperCoroutine> coroutineRemoveList = new List<SuperCoroutine>();
        foreach (var v in coroutineList)
        {
            if (v.over) coroutineRemoveList.Add(v);
            else v.Update();
        }
        foreach (var o in coroutineRemoveList) coroutineList.Remove(o);

        if (_SuperTimer.Instance.checkCor)
            _SuperTimer.Instance.countCor = coroutineList.Count;

        if (_SuperTimer.Instance.checkFrameFunc)
            _SuperTimer.Instance.countFrameFunc = perFrameFunc.Count;

        if (_SuperTimer.Instance.checkMsg)
            _SuperTimer.Instance.countMsg = MessageCenter.Instance.CheckEventCount();

        if (_SuperTimer.Instance.checkClickLimit)
            _SuperTimer.Instance.ClickLimitInf = ClickLimit.Check;
    }


    #region 协程管理



    /// <summary>
    /// 启动一个新协程，它有一个自己的标记，用于结束的控制
    /// </summary>
    public SuperCoroutine CoroutineStart(IEnumerator func, object mark)
    {
        SuperCoroutine sc = new SuperCoroutine(mark);
        coroutineAddList.Add(sc);
        sc.StartCoroutine(func);
        return sc;
    }

    /// <summary>
    /// 在一个中开启一个协程使用该方法，它会继承父协程的mark。注意要将该方法yield return 
    /// </summary>
    public SuperCoroutine YieldCoroutineStart(IEnumerator func)
    {
        SuperCoroutine sc = new SuperCoroutine();
        coroutineAddList.Add(sc);
        sc.StartCoroutine(func, false);
        return sc;
    }


    /// <summary>
    /// 停止一个指定标记的协程
    /// </summary>
    public void CoroutineStop(object mark, CoroutineStopMode stopMode = CoroutineStopMode.Normal)
    {
        var t = coroutineList.Find(coroutine => coroutine.Mark == mark || coroutine.Mark.Equals(mark));
        var t2 = coroutineAddList.Find(coroutine => coroutine.Mark == mark || coroutine.Mark.Equals(mark));
        if (t == null && t2 == null)
        {
            if (stopMode == CoroutineStopMode.Warning)
                Debug.LogWarning("停止了一个不存在的协程：" + mark);
            else if (stopMode == CoroutineStopMode.Error)
                throw new NullReferenceException("停止了一个不存在的协程：" + mark);
            return;
        }
        foreach (var v in coroutineList) if (v.Mark == mark || v.Mark.Equals(mark)) v.StopCoroutine();
        coroutineAddList.RemoveAll(coroutine => coroutine.Mark == mark || coroutine.Mark.Equals(mark));
    }



    /// <summary>
    /// 停止所有协程
    /// </summary>
    public void CoroutineStopAll()
    {
        foreach (var v in coroutineList)
            v.StopCoroutine();
        coroutineAddList.Clear();

    }

    /// <summary>
    /// 协程等待时间，注意这个时间并不是十分准确
    /// </summary>
    public static SuperWait WaitTime(float time)
    {
        return SuperWait.WaitTime(time);
    }

    /// <summary>
    /// 协程等待帧数
    /// </summary>
    public static SuperWait WaitFrame(int count = 0)
    {
        return SuperWait.WaitFrame(count);
    }

    /// <summary>
    /// 协程等待返回true，判断函数最好不要匿名，这样可以检查对象为空的情况,
    /// </summary>
    public static SuperWait WaitTrue(SuperWaitHandle waitHandle, object par = null)
    {
        return SuperWait.WaitTrue(waitHandle, par);
    }

    /// <summary>
    /// 协程等待消息，如果有判断方法，返回true表示结束等待。
    /// 判断方法不建议使用匿名、静态函数，
    /// 在等待监听消息的过程中，如果判断方法所属对象被销毁，匿名、静态函数是感知不到的，
    /// 无法在内部处理（就只能交由外部来处理）
    /// </summary>
    public static SuperWait WaitMsg(object msg, SuperWaitHandle waitHandle = null)
    {
        return SuperWait.WaitMsg(msg, waitHandle);
    }



    /// <summary>
    /// 自管理协程，等待对象
    /// </summary>
    public class SuperWait : IEnumerator
    {
        public WaitType WaitType { private set; get; }
        private float waitArg;
        private bool waitMark = true;
        private object msg;
        private object waitTruePar;
        private SuperWaitHandle waitHandle;


        private SuperWait() { }

        public static SuperWait WaitTime(float time)
        {
            var t = new SuperWait
            {
                WaitType = WaitType.WaitTime,
                waitArg = time,
            };
            return t;
        }

        public static SuperWait WaitFrame(int count)
        {
            var t = new SuperWait
            {
                WaitType = WaitType.WaitFrame,
                waitArg = count,
            };
            return t;
        }

        public static SuperWait WaitTrue(SuperWaitHandle waitHandle, object arg = null)
        {
            var t = new SuperWait
            {
                WaitType = WaitType.WaitTrue,
                waitHandle = waitHandle,
                waitTruePar = arg,
            };
            return t;
        }

        public static SuperWait WaitMsg(object msg, SuperWaitHandle waitHandle = null)
        {
            var t = new SuperWait
            {
                WaitType = WaitType.WaitMsg,
                msg = msg,
                waitHandle = waitHandle,
            };
            if (waitHandle != null && waitHandle.Method.IsStatic)
                throw new ArgumentException("WaitMsg的回调方法不能是匿名或者静态，否则在清空对象时，不能回收监听的消息");
            MessageCenter.Instance.AddListener(msg, t.MsgCallBack);

            return t;
        }

        private void MsgCallBack(object o)
        {
            if (this.waitHandle == null || this.waitHandle.Equals(null))
            {
                waitMark = false;
            }
            else
            {
                if (SuperTool.CheckDelegate(waitHandle))
                    waitMark = !waitHandle.Invoke(o);
                else
                {
                    Current = CurrentState.Quit;
                    MessageCenter.Instance.RemoveListener(msg, MsgCallBack);
                }
            }
            if (!waitMark) MessageCenter.Instance.RemoveListener(msg, MsgCallBack);
        }


        public bool MoveNext()
        {

            if (WaitType == WaitType.WaitTime)
            {
                CheckTime();
            }
            else if (WaitType == WaitType.WaitFrame)
            {
                CheckFrame();
            }
            else if (WaitType == WaitType.WaitTrue)
            {
                CheckTrue();
            }
            else if (WaitType == WaitType.WaitMsg)
            {
                CheckMsg();
            }
            return waitMark;
        }

        private void CheckTime()
        {
            waitArg -= DeltaTime;
            waitMark = !(waitArg <= 0);
        }

        private void CheckFrame()
        {
            waitMark = !(--waitArg <= 0);
        }

        private void CheckTrue()
        {
            if (SuperTool.CheckDelegate(waitHandle))
                waitMark = !waitHandle.Invoke(waitTruePar);
            else
                Current = CurrentState.Quit;
        }

        private void CheckMsg()
        {
            if (waitHandle == null) return;
            if (!SuperTool.CheckDelegate(waitHandle))
            {
                MessageCenter.Instance.RemoveListener(msg, MsgCallBack);
                Current = CurrentState.Quit;
            }
        }


        public void Reset()
        {
            throw new System.NotImplementedException();
        }


        public object Current { get; private set; }

        public void ForceQuit()
        {
            if (WaitType == WaitType.WaitMsg)
                MessageCenter.Instance.RemoveListener(msg, MsgCallBack);
        }

    }

    /// <summary>
    /// 协程管理，要每帧调用update，外部不要使用这个类
    /// </summary>
    public class SuperCoroutine
    {
        private readonly Stack<IEnumerator> stack = new Stack<IEnumerator>();
        public bool over;
        private IEnumerator checkFunc;
        public object Mark { private set; get; }

        public string check
        {
            get
            {
                var res = checkFunc.ToString();
                if (checkFunc.Current is SuperWait)
                {
                    res += "@" + ((SuperWait)checkFunc.Current).WaitType;
                }
                return res;
            }
        }

        public SuperCoroutine(object mark)
        {
            Mark = mark;
        }

        public SuperCoroutine()
        { }

        /// <summary>
        /// 开始一个协程，外部不要使用该方法，请使用SuperTime对应方法
        /// </summary>
        public void StartCoroutine(IEnumerator func, bool imme = true)
        {
            over = false;
            stack.Push(func);
            if (imme) Update();
        }

        /// <summary>
        /// 停止一个协程，外部不要使用该方法，请使用SuperTime对应方法
        /// </summary>
        public void StopCoroutine()
        {
            over = true;
            if (stack.Count > 0)
            {
                var wait = stack.Pop() as SuperWait;
                if (wait != null) wait.ForceQuit();
                stack.Clear();
            }
        }

        public void Update()
        {
            if (!over && stack.Count > 0)
            {
                if (Mark == null || Mark.Equals(null)) over = true;
                line1:
                var func = checkFunc = stack.Peek();
                if (func.MoveNext())//如果在IEnumerator方法里移除了自己，那么这里会改变stack
                {
                    if (func.Current is IEnumerator)
                    {
                        stack.Push((IEnumerator)func.Current);
                        goto line1;//如果不跳转，等待帧数的方法，都要多等1帧
                    }
                    else if (func.Current is SuperCoroutine)
                    {
                        SuperCoroutine t = (SuperCoroutine)func.Current;
                        t.Mark = Mark;
                        t.Update();
                    }
                }
                else
                {
                    if (stack.Count <= 0) return;
                    if (func == stack.Peek()) stack.Pop();
                }
                if (func.Current is CurrentState && (CurrentState)func.Current == CurrentState.Quit)
                    over = true;
            }
            else
            {
                over = true;
            }
        }

    }

    public enum WaitType
    {
        WaitTime,
        WaitFrame,
        WaitTrue,
        WaitMsg,
    }

    public enum CurrentState
    {
        /// <summary>
        /// 结束该协程
        /// </summary>
        Quit,
    }

    #endregion

    #region 计时器

    /// <summary>
    /// 计时器，时间到后，回调传入的函数
    /// </summary>
    /// <param name="time">计时</param>
    /// <param name="callBack">回调的委托</param>
    /// <param name="arg">回调时的参数</param>
    public void SetTimer(float time, TimerHandle callBack, object arg = null)
    {
        TimerTick timer = new TimerTick(time, callBack, arg);
        TimerList.Add(timer);
    }

    /// <summary>
    /// 注册一个方法，每帧被调用
    /// 1.注册的方法返回true表示结束每帧调用；
    /// 2.重复注册同一个函数（方法和参数都相同）是可以的；
    /// 3.支持一个参数（arg）；
    /// 4.不建议注册静态或匿名方法。
    /// </summary>
    public void RegisterFrameFunction(PerFrameHandle perFrame, object arg = null)
    {
        var func = new XY<PerFrameHandle, object>(perFrame, arg);
        perFrameFunc_addList.Add(func);
    }

    /// <summary>
    /// 注册一个方法，每帧被调用
    /// 1.注册的方法返回true表示结束每帧调用；
    /// 2.如果注册的方法已经存在（并且参数都一样），会放弃注册；
    /// 3.支持一个参数（arg）；
    /// 4.不建议注册静态或匿名方法。
    /// </summary>
    public void TryRegisterFrameFunction(PerFrameHandle perFrame, object arg = null)
    {
        var find1 = perFrameFunc.Find(xy =>
        {
            if (arg == null)
                return xy.x == perFrame;
            else
                return xy.x == perFrame && xy.y == arg;
        });
        var find2 = perFrameFunc_addList.Find(xy =>
        {
            if (arg == null)
                return xy.x == perFrame;
            else
                return xy.x == perFrame && xy.y == arg;
        });
        if (find1 == null && find2 == null) RegisterFrameFunction(perFrame, arg);
    }

    /// <summary>
    /// 注销已注册的方法
    /// </summary>
    public void LogoutFrameFunction(PerFrameHandle perFrame)
    {
        if (!perFrameFunc_removeList.Contains(perFrame))
            perFrameFunc_removeList.Add(perFrame);
    }


    /// <summary>
    /// 注销所有注册的方法
    /// </summary>
    public void LogoutAllFrameFunction()
    {
        perFrameFunc.Clear();
    }


    #endregion


    #region 日期
    /// <summary>
    /// 用“2017-02-04 12:03:33”这种格式设定游戏内部标准时间
    /// </summary>
    public void SetNowDate(string time)
    {
        SetNowDate(DateTime.Parse(time));
    }

    /// <summary>
    /// 用DateTime设置游戏内部标准时间
    /// </summary>
    public void SetNowDate(DateTime time)
    {
        isDateLapses = true;
        Now = time;
    }

    /// <summary>
    /// 添加一个时间点
    /// </summary>
    public DateTime AddTimeDate(string name, DateTime time)
    {
        if (!isDateLapses) throw new ArgumentException("必须先设置标准比对日期");
        XY<string, DateTime> temp = gameTimeDates.Find(xy => Equals(xy.x, name));
        if (temp == null)
        {
            XY<string, DateTime> temp2 = new XY<string, DateTime>(name, time);
            gameTimeDates.Add(temp2);
        }
        else
        {
            if (temp.y == time) return time;
            temp.y = time;
        }
        return time;
    }

    /// <summary>
    /// 添加一个时间点
    /// </summary>
    public DateTime AddTimeDate(string name, string time = null)
    {
        var dateTime = time == null ? Now : DateTime.Parse(time);
        return AddTimeDate(name, dateTime);
    }

    /// <summary>
    /// 移除一个时间点
    /// </summary>
    public void RemoveTimeDate(string name)
    {
        int index = gameTimeDates.FindIndex(xy => xy.x == name);
        if (index < 0) throw new NullReferenceException("正常移除一个不存在的日期：" + name);
        gameTimeDates.RemoveAt(index);
    }

    /// <summary>
    /// 获取从设置的时间点到现在经历的时间（秒）
    /// </summary>
    public long GetDateLapses(string name)
    {
        if (!isDateLapses) throw new ArgumentException("必须先设置标准比对日期");
        int index = gameTimeDates.FindIndex(xy => xy.x == name);
        if (index < 0) throw new NullReferenceException("不存在的日期：" + name);
        DateTime dt1 = gameTimeDates[index].y;
        TimeSpan span = Now.Subtract(dt1);
        long delta = span.Seconds + span.Minutes * 60 + (long)span.Hours * 3600 + (long)span.Days * 3600 * 24;
        return delta;
    }

    #endregion


}
/// <summary>
/// 控制协程停止模式
/// </summary>
public enum CoroutineStopMode
{
    /// <summary>
    /// 即时没有停止的目标，也可以正常运行
    /// </summary>
    Normal,
    /// <summary>
    /// 没有停止的目标，会发出警告
    /// </summary>
    Warning,
    /// <summary>
    /// 没有停止的目标，会出错
    /// </summary>
    Error,
}

/// <summary>
/// 不要在外部使用这个类
/// </summary>
public class TimerTick
{
    private float leftTime;
    private readonly SuperTimer.TimerHandle callBack;
    private bool isDone;
    private readonly object arg;


    public TimerTick(float leftTime, SuperTimer.TimerHandle callBack, object arg)
    {
        this.leftTime = leftTime;
        this.callBack = callBack;
        isDone = false;
        this.arg = arg;
    }

    public bool IsDone()
    {
        return isDone;
    }

    public void Update()
    {
        if (!isDone)
        {
            leftTime -= SuperTimer.DeltaTime;
            if (leftTime <= 0)
            {
                try
                {
                    if (SuperTool.CheckDelegate(callBack))
                    {
                        callBack(arg);
                    }
                }
                catch (Exception)
                {
                    Debug.LogError("Timer回调异常：检查回调方法的所属对象是否为空");
                    throw;
                }
                isDone = true;
            }
        }
    }
}

/// <summary>
/// 根据提供的http网址获取网络时间
/// </summary>
public class GetNetTime
{
    /// <summary>
    /// 0表示未完成，1表示完成，2表示超时
    /// </summary>
    public short IsDone { get; private set; }

    /// <summary>
    /// 如果读取完成，该字段存储就是网络时间
    /// </summary>
    public string Date { get; private set; }

    /// <summary>
    /// 自定义超时时间（秒），这个时间不能太大以超过WebRequest本身的超时时间，否则无效
    /// </summary>
    public double timeOut = 3;

    /// <summary>
    /// 读取等待的时间，这是一个没有转化的时间，不确定单位
    /// </summary>
    public double WaitTime { get; private set; }

    private Thread loadThead;//读取网页时间的线程
    private Thread timeThead;//超时检测线程
    private Stopwatch sw;//计时器
    private string url;//网址，注意不能是https

    /// <summary>    
    /// 本地时间转成GMT时间    
    /// string s = ToGMTString(DateTime.Now);  
    /// 本地时间为：2014-9-29 15:04:39  
    /// 转换后的时间为：Thu, 29 Sep 2014 07:04:39 GMT  
    /// </summary>    
    public static string ToGMTString(DateTime dt)
    {
        return dt.ToUniversalTime().ToString("r");
    }

    /// <summary>    
    /// 本地时间转成GMT格式的时间  
    /// string s = ToGMTFormat(DateTime.Now);  
    /// 本地时间为：2014-9-29 15:04:39  
    /// 转换后的时间为：Thu, 29 Sep 2014 15:04:39 GMT+0800  
    /// </summary>    
    public static string ToGMTFormat(DateTime dt)
    {
        return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
    }

    /// <summary>    
    /// GMT时间转成本地时间   
    /// DateTime dt1 = GMT2Local("Thu, 29 Sep 2014 07:04:39 GMT");  
    /// 转换后的dt1为：2014-9-29 15:04:39  
    /// DateTime dt2 = GMT2Local("Thu, 29 Sep 2014 15:04:39 GMT+0800");  
    /// 转换后的dt2为：2014-9-29 15:04:39  
    /// </summary>    
    /// <param name="gmt">字符串形式的GMT时间</param>    
    /// <returns></returns>    
    public static DateTime GMT2Local(string gmt)
    {
        DateTime dt = DateTime.MinValue;
        try
        {
            string pattern = "";
            if (gmt.IndexOf("+0") != -1)
            {
                gmt = gmt.Replace("GMT", "");
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
            }
            if (gmt.ToUpper().IndexOf("GMT") != -1)
            {
                pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
            }
            if (pattern != "")
            {
                dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                dt = dt.ToLocalTime();
            }
            else
            {
                dt = Convert.ToDateTime(gmt);
            }
        }
        catch
        {
        }
        return dt;
    }

    /// <summary>
    /// 启动网络时间获取
    /// </summary>
    /// <param name="url"></param>
    public void StartGetNetDate(string url)
    {
        this.url = url;
        timeThead = new Thread(StopThread);
        sw = new Stopwatch();
        loadThead = new Thread(StartLoad);
        IsDone = 0;
        WaitTime = 0;
        sw.Start();
        loadThead.Start();
        timeThead.Start();
    }

    private void StopThread()
    {
        Thread.Sleep((int)(timeOut * 1000));
        if (IsDone == 1) return;
        IsDone = 2;
        loadThead.Abort();
        sw.Stop();
    }


    /// <summary>  
    /// 通过分析网页报头，查找Date对应的值，获得GMT格式的时间。可通过GMT2Local(string gmt)方法转化为本地时间格式。  
    /// 用法 DateTime netTime = GetNetTime.GMT2Local(GetNetTime.StartGetNetDate());  
    /// </summary>  
    private void StartLoad()
    {
        try
        {
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            WebHeaderCollection myWebHeaderCollection = response.Headers;

            for (int i = 0; i < myWebHeaderCollection.Count; i++)
            {
                string header = myWebHeaderCollection.GetKey(i);
                string[] values = myWebHeaderCollection.GetValues(header);
                if (header == null || header.Length <= 0)
                {
                    continue;
                }
                else if (header == "Date")
                {
                    sw.Stop();
                    Date = values[0];
                    IsDone = 1;
                    WaitTime = sw.Elapsed.TotalMilliseconds;
                    timeThead.Abort();
                    return;
                }
            }
            throw new ArgumentNullException();
        }
        catch (Exception ex)
        {
            if (ex is ArgumentNullException)
            {
                Debug.LogError("报头不包含时间信息");
            }
            else
            {
                Debug.LogError("链接失败");
            }
            IsDone = 2;
            sw.Stop();
            WaitTime = sw.Elapsed.TotalMilliseconds;
            timeThead.Abort();
        }
    }

    /// <summary>
    /// 测试一个网址是否能够正常返回时间
    /// </summary>
    /// <param name="url">网址</param>
    /// <param name="waitTime">总共等待的时间</param>
    /// <param name="data">获取到的网络时间</param>
    /// <returns>是否成功获取到网络时间</returns>
    public static bool Test(string url, out double waitTime, out string data)
    {
        Stopwatch watch = new Stopwatch();
        try
        {
            watch.Start();
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            WebHeaderCollection myWebHeaderCollection = response.Headers;

            for (int i = 0; i < myWebHeaderCollection.Count; i++)
            {
                string header = myWebHeaderCollection.GetKey(i);
                string[] values = myWebHeaderCollection.GetValues(header);
                if (header == null || header.Length <= 0)
                {
                    continue;
                }
                else if (header == "Date")
                {
                    watch.Stop();
                    data = values[0];
                    waitTime = watch.Elapsed.TotalMilliseconds;
                    return true;
                }
            }
            throw new ArgumentNullException();
        }
        catch (Exception)
        {
            watch.Stop();
            waitTime = watch.Elapsed.TotalMilliseconds;
            data = "";
            return false;
        }
    }
    /// <summary>
    /// 测试一个网址是否能够正常返回时间
    /// </summary>
    /// <param name="url">网址</param>
    public static bool Test(string url)
    {
        double temp1;
        string temp2;
        return Test(url, out temp1, out temp2);
    }
}

