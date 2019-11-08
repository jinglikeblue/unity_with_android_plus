package com.jing;

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
