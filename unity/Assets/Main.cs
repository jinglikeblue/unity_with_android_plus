using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    /// <summary>
    /// 场景上的文本框用来显示android发送过来的内容
    /// </summary>
    public Text text;

    /// <summary>
    /// android原生代码对象
    /// </summary>
    AndroidJavaObject _ajc;

    void Start () {
        //通过该API来实例化导入的arr中对应的类
        _ajc = new AndroidJavaObject("com.jing.unity.Unity2Android");
    }
	
	void Update () {
		
	}

    /// <summary>
    /// 场景上按点击时触发该方法
    /// </summary>
    public void OnBtnClick()
    {
        //通过API来调用原生代码的方法
        bool success = _ajc.Call<bool>("showToast","this is unity");
        if(true == success)
        {
            //请求成功
        }
    }

    /// <summary>
    /// 原生层通过该方法传回信息
    /// </summary>
    /// <param name="content"></param>
    public void FromAndroid(string content)
    {
        text.text = content;
    }
}
