async function ExportAsPDF(className, filename) {
    // var elementContent = document.getElementById(elementId);
    var classElements = document.getElementsByClassName(className);

    const options = {
        margin: [0, 0],
        filename: filename,
        html2canvas: { scale: 1, dpi: 192, letterRendering: true },
        image: { type: 'jpeg', quality: 0.98 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'p' }
    };
    const doc = new jsPDF(options.jsPDF);
    const pageSize = jsPDF.getPageSize(options.jsPDF)
    for (let i = 0; i < classElements.length; i++) {
        const page = classElements[i];
        const pageImage = await html2pdf().from(page).set(options).outputImg();
        if (i != 0) {
            doc.addPage();
        }
        doc.addImage(pageImage.src, 'jpeg', options.margin[0], options.margin[1], pageSize.width, pageSize.height);
    }
    pdf.save(options.filename);
}