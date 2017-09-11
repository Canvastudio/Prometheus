using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

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



    public void Update()
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
