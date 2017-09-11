using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 内部类负责更新SuperTimer的Update
/// </summary>
public class _SuperTimer : SingleGameObject<_SuperTimer>
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
