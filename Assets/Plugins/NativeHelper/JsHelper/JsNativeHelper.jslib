mergeInto(LibraryManager.library, {
  JSAlert: function (str) {
  var content=Pointer_stringify(str);
    window.alert(content);
  },
});