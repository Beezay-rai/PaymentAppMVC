// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//For Code Editor

function CodeEditorForClassName() {
    
    var htmlElement = document.querySelectorAll(".code");
    htmlElement.forEach((item) => {
        
        var editor = CodeMirror.fromTextArea(item, {
            lineNumbers: false,
            value: item.value,
            readOnly:true,
            mode: "application/json",
            theme:"dracula"
        });

    });

}


    

