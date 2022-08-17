function Print() {
    var divContent = document.getElementById("DocumentBody").innerHTML;
    var Window = window.open("", "", "height = 842, width = 700");
    Window.document.write("<html>");
    Window.document.write("<body><br/>");
    Window.document.write(divContent);
    Window.document.write("</body></html>");
    Window.document.close();
    Window.print();
}

function saveAsPDF(elementId, filename) {
    debugger;
    var elementContent = document.getElementById(elementId).innerHTML;
    var options = {
        margin: 10,
        filename: filename,
        html2canvas: { scale: 1 },
        image: { type: 'png', quality: 1 },
        jsPDF: { unit: 'mm', format: 'letter', orientation: 'p' }
    };

    html2pdf(elementContent, options);
}