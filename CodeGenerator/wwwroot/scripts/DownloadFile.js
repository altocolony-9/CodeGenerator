function downloadFile(context, fileName) {
    var blob = new Blob([context], { "type": "text/plain" });
    //IE10以降の対策
    if (window.navigator.msSaveBlob) {
        window.navigator.msSaveBlob(blob, fileName);

        window.navigator.msSaveOrOpenBlob(blob, fileName);
    }
    else {
        // TODO オブジェクトURLを明示的に解放する処理
        document.getElementById("download").href = window.URL.createObjectURL(blob);
    }
}