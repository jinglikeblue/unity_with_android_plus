## 引言
> 在写了[Unity与Android交互方案优化版](https://www.jianshu.com/p/86b275da600e
)这篇文章得到了大家的认可，很多大佬评论说该方案用起来还可以。不过也有很多人咨询，如果需求要在原生的Activity里实现一些回调的重写，或者是某些SDK需要自定义的Activity开发时，该怎么做。其实实现起来还是比较简单的，这里也提供一个方案，同样是减少了那些繁琐的操作步骤，尽量简单的让您快速实现需求。

## 本文适用对象
* 有一定的Unity开发经验，会使用Unity
* 有一定的Android开发经验，会使用AndroidStudio，会看查看调试信息
* 本文的所有内容基于[Unity与Android交互方案优化版](https://www.jianshu.com/p/86b275da600e
)提供的方案之上，为其扩展、加强

## 方案优势
* 需要引用unity下的class.jar，但是该jar并不会被打包出来，所以没有网上其他方案的那些繁琐的解压、拷贝等步骤。
* 需要在Unity的/Plugins/Android下放置AndroidManifest.xml文件，因为必须要描述自定义的Activity等信息。
* Unity打包时PackageName不依赖于引用文件
* 发布简单，只需要导出arr并直接拷贝到/Plugins/Android目录下即可使用，不用对文件做任何修改


## 文章DEMO对应的IDE版本
* AndroidStudio 3.4.1     (亲测通过) 
* Unity 2018.4.0          (亲测通过) 

# 流程

> 因为项目是基于[Unity与Android交互方案优化版](https://www.jianshu.com/p/86b275da600e
)扩展而来的，所以我们这里省略掉前戏，而是直接开始高潮部分。

### Android部分

##### 放置unity的class.jar
首先我们拷贝unity安装目录下针对的class.jar到AndroidStudio中库项目下的tempLibs目录中。这里我用的是Unity 2018.4.0版本，拷贝的文件是"[Unity安装路径]\Unity\Editor\Data\PlaybackEngines\AndroidPlayer\Variations\mono\Release\Classes\class.jar"

一般来说说Android开发时，依赖的库文件都是放到libs目录中的，但是该目录中的内容默认是打包时会一起编译进包的，所以我们自己建立一个tempLibs（名字你随便取）目录，来放这种不参与编译的jar。

##### 修改build.gradle文件

```
这一个地方很关键，请仔细阅读
```

修改build.gradle文件，的dependencies部分，添加class.jar的引用，这里切记用compileOnly，这样打包aar的时候，该class.jar不会被打包出去。

```
dependencies {
    implementation fileTree(include: ['*.jar'], dir: 'libs')
    implementation 'com.android.support:appcompat-v7:26.1.0'
    compileOnly files('tempLibs/classes.jar') //使用compileOnly指定该jar只是开发时引用，编译时不导出
}
```

##### 写一个自定义的Application

我们创建一个CustomApplication文件，继承自Application。为了方便测试，我们重写onCreate方法
```
import android.app.Application;
import android.util.Log;
public class CustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        Log.i("unity_with_android_plus", "老子就是自定义的Application");
    }
}
```

##### 写一个自定义的Activity

我们创建一个CustomActivity文件，继承自UnityPlayerActivity。为了方便测试，我们重写onCreate方法

>这就是为什么要引入class.jar的原因，因为我们要继承 "com.unity3d.player.UnityPlayerActivity" 这个类

```
import android.os.Bundle;
import android.util.Log;
import com.unity3d.player.UnityPlayerActivity;
public class CustomActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle bundle) {
        super.onCreate(bundle);
        Log.i("unity_with_android_plus", "老子就是自定义的Activity");
    }
}
```

### Unity部分

##### 编写AndroidManifest.xml文件

我们在Assets/Plugins/Android目录下，新建一个文件AndroidManifest.xml。内容如下

```
<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    package="com.jing.unity" >
    <application
        android:name="com.jing.CustomApplication"
        android:label="@string/app_name">
        <activity android:name="com.jing.CustomActivity"
            android:label="@string/app_name"
            android:configChanges="fontScale|keyboard|keyboardHidden|locale|mnc|mcc|navigation|orientation|screenLayout|screenSize|smallestScreenSize|uiMode|touchscreen">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
            <meta-data android:name="android.app.lib_name" android:value="unity" />
            <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="false" />
        </activity>
    </application>
</manifest>
```

在这里，可以看到我们明确指出了Application使用的是我们自定义的CustomApplication，Activity使用的是CustomActivity。至于其它的参数的含义，请各位大佬自行百度android知识。

### 测试

我们配置一下BuildSettings，确保勾上Development Build。然后打包APK开始测试。

让程序运行起来，我们可以在AndroidStudio的Logcat里看到我们自定义的Application以及Activity已经起作用了，并打印了日志信息。

![android_logcat_info.png](https://upload-images.jianshu.io/upload_images/9825434-625ddf32722f0f75.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

## 结束语
基本上该文章配合之前的文章，可以解决我们在Unity和Android交互开发中遇到的所有问题，为所欲为了。如果后续还有其它的更好的方案，我会再更新。
