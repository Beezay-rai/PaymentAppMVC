//

function CodeEditorForClasses() {

    var codeElement = document.querySelectorAll(".code");
    codeElement.forEach((item) => {

        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            value:item.value,
            readOnly: true,
            theme: "dracula"
        });
        item.CodeMirrorInstance = editor;

    });




    var jsonElement = document.querySelectorAll(".code-json");
    jsonElement.forEach((item) => {
        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            mode: "application/json",
            readOnly: true,
            value: item.value,
            theme: "dracula"
        });
        item.CodeMirrorInstance = editor;

    });



    var xmlElement = document.querySelectorAll(".code-xml");
    xmlElement.forEach((item) => {

        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            mode: "xml",
            readOnly: true,
            value: item.value,
            theme: "dracula"
        });
        item.CodeMirrorInstance = editor;

    });

    var javascriptElement = document.querySelectorAll(".code-javascript");
    javascriptElement.forEach((item) => {

        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            readOnly: true,
            value: item.value,
            mode: "javascript",
            theme: "dracula"
        });
        item.CodeMirrorInstance = editor;

    });
    var requestUrl = document.querySelectorAll(".code-requestUrl");
    requestUrl.forEach((item) => {
        
        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            theme: "dracula"
        });
        editor.setSize("100%","40px")
        item.CodeMirrorInstance = editor;

    });

    var responseHeader = document.querySelectorAll(".code-responseHeader");
    responseHeader.forEach((item) => {

        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            theme: "dracula"
        });
        editor.setSize("100%", "60px")
        item.CodeMirrorInstance = editor;

    });

}



