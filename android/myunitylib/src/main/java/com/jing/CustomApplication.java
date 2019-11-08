package com.jing;

import android.app.Application;
import android.util.Log;

public class CustomApplication extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        Log.i("unity_with_android_plus", "老子就是自定义的Application");
    }
}
