window.html2canvas = html2canvas;
window.jsPDF = jsPDF;

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
    const page = classElements[i].innerHTML;
    const pageImage = await html2pdf().from(page).set(options).outputImg();
    if (i != 0) {
      doc.addPage();
    }
    doc.addImage(pageImage.src, 'jpeg', options.margin[0], options.margin[1], pageSize.width, pageSize.height);
  }
  pdf.save(options.filename);
}

async function makePDF(className, filename) {
  const elements = Array.prototype.slice.call(document.getElementsByClassName(className));
  var doc = new jsPDF('p', 'in', 'letter');

  await Promise.all(
    elements.map(
      async (element) => {
        await html2canvas(element, {
          allowTaint: true,
          useCORS: true,
          scale: 1
        }).then(canvas => {
          if (element != elements[0])
            doc.addPage('letter');
          var img = canvas.toDataURL("image/png");
          doc.addImage(img, 'PNG', 0, 0);
        });
      }
    )
  );

  doc.save(filename);
}