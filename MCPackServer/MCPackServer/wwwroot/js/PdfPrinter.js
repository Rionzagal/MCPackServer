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