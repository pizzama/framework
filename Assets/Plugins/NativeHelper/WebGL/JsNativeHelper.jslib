mergeInto(LibraryManager.library, {
    JSAlert: function (str) {
        var content=Pointer_stringify(str);
        window.alert(content);
    },
    JSRestartGame: function() {
        window.location.reload();
    }, 
    JSVibrate:function(time) {
        var support = navigator.vibrate(time);
        if (support) {
            print("设备支持震动，已触发震动。");
        } else {
            print("设备不支持震动。");
        }
    }
});