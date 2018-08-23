using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MyBezier : MonoBehaviour
{
    //曲线的对象
    public GameObject Yellowline;

    //曲线对象的曲线组件
    private LineRenderer YellowlineRenderer;

    //拖动条用来控制贝塞尔曲线的两个点
    public float hSliderValue0;
    public float hSliderValue1;

    void Start()
    {
        //得到曲线组件
        YellowlineRenderer = GetComponent<LineRenderer>();
        //为了让曲线更加美观，设置曲线由100个点来组成
        YellowlineRenderer.SetVertexCount(100);
    }

    void OnGUI()
    {
        //拖动条得出 -5.0 - 5.0之间的一个数值
        hSliderValue0 = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue0, -5.0F, 5.0F);
        hSliderValue1 = GUI.HorizontalSlider(new Rect(25, 70, 100, 30), hSliderValue1, -5.0F, 5.0F);
    }

    void Update()
    {
        //循环100遍来绘制贝塞尔曲线每个线段
        for (int i = 1; i <= 100; i++)
        {
            //在这里来计算贝塞尔曲线
            //四个参数 表示当前贝塞尔曲线上的4个点 第一个点和第四个点
            //我们是不需要移动的，中间的两个点是由拖动条来控制的。
            Vector3 pos = Utils.BezierCurve(new Vector3(-5f, 0f, 0f), new Vector3(hSliderValue1, hSliderValue0, 0f), new Vector3(hSliderValue1, hSliderValue0, 0f), new Vector3(5f, 0f, 0f), i * 0.02f);
            //把每条线段绘制出来 完成塞尔曲线的绘制
            YellowlineRenderer.SetPosition(i - 1, pos);
        }

    }
}
