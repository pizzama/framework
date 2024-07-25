mergeInto(LibraryManager.library, {
  JSAlert: function (str) {
  var content=Pointer_stringify(str);
    window.alert(content);
  },
  SyncDB: function () {
      FS.syncfs(false, function (err) {
          if (err) console.log("syncfs error: " + err);
      });
  }
});