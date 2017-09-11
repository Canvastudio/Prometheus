using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
