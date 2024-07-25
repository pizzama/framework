mergeInto(LibraryManager.library, {
  JSAlert: function (str) {
  var content=Pointer_stringify(str);
    window.alert(content);
  },
  JSSyncDB: function() {
        FS.mkdir('/idbfs');
        FS.mount(IDBFS, {}, '/idbfs');
        FS.syncfs(true, function (err) {
            if (err) {
                console.error('Error syncing file system:', err);
            } else {
                console.log('File system synced successfully.');
            }
        });
    }
});