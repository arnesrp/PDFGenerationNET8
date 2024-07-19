using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace HelloPdf
{
    public class SimpleTable
    {
        public static readonly string DEST = "test-fact-sheet.pdf";

        public static void Main(String[] args)
        {
            // FileInfo file = new FileInfo(DEST);
            // file.Directory.Create();

            new SimpleTable().ManipulatePdf(DEST);
        }

        private class BorderlessCell: Cell
        {
            public BorderlessCell()
            {
                SetBorder(Border.NO_BORDER);
            }

            public BorderlessCell(int rowspan, int colspan) : base(rowspan, colspan)
            {
                SetBorder(Border.NO_BORDER);
            }
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new(new PdfWriter(dest));
            Document doc = new(pdfDoc);

            // Can extract styling as constants in separate file
            Style smallFont = new Style()
                .SetFontSize(8);
            
            Style sectionHeadFont = new Style()
                .SetFontSize(9)
                .SetBold();

            doc.SetFontSize(9);

            // Header
            Table header = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            
            Cell headerCell = new BorderlessCell()
                .Add(new Paragraph("IKEA").SetFontSize(20))
                .Add(new Paragraph("IKEA of Sweden AB"));
            header.AddCell(headerCell);

            Cell headerCell2 = new BorderlessCell()
                .Add(new Paragraph("Material Grade Facts").SetFontSize(15))
                .SetTextAlignment(TextAlignment.CENTER);

            header.AddCell(headerCell2);

            header.AddCell(new BorderlessCell()); // Empty cell
            
            // Create a nested table inside the next cell
            Table headerDate = new Table(UnitValue.CreatePercentArray(2))
            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
            .AddCell(
                new BorderlessCell().Add(new Paragraph("Created")))
            .AddCell(
                new BorderlessCell().Add(new Paragraph("2024-07-18")))
            .AddCell(
                new BorderlessCell().Add(new Paragraph("Updated")))
            .AddCell(
                new BorderlessCell().Add(new Paragraph("2024-07-18")))
            .AddCell(
                new BorderlessCell().Add(new Paragraph("Status")))
            .AddCell(
                new BorderlessCell().Add(new Paragraph("Created")));


            Cell headerCellDate = new BorderlessCell()
                .Add(headerDate)
                .SetWidth(0.5f)
                .SetPadding(0)
                .AddStyle(smallFont);

            header.AddCell(headerCellDate);

            header.AddCell(new BorderlessCell(1, 2).Add(new Paragraph("Issued By Material Management")));
            
            doc.Add(header);

            SolidLine line = new SolidLine(1f);
            LineSeparator lineSeparator = new LineSeparator(line)
                .SetMarginTop(5)
                .SetMarginBottom(5);

            doc.Add(lineSeparator);

            Table detail = new Table(UnitValue.CreatePercentArray(2))
                .UseAllAvailableWidth()
                .AddStyle(smallFont);

            detail.AddCell(
                new BorderlessCell(1, 2)
                    .Add(new Paragraph("Basic Information"))
                    .AddStyle(sectionHeadFont)
            );

            // Can iterate over, make data in key-value form

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("Material Grade")) // key
            );

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("Paraffin wax")) // value
            );

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("ID")) // key
            );

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("MATXXXX")) // value
            );

            detail.AddCell(
                new BorderlessCell(1, 2)
                    .Add(new Paragraph("Requirement Specification"))
                    .AddStyle(sectionHeadFont)
            );

            // Long text should wrap
            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("PREPARATIONS AND INTENDED RELEASE - REACH AND DOCUMENTATION REQUIREMENTS"))
            );

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("12345 IOS-MAT-XXXX"))
            );

            // Subsequent cells should fall under long text in a new row
            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("Chemical requirements, candle raw materials"))
            );

            detail.AddCell(
                new BorderlessCell()
                    .Add(new Paragraph("12345 IOS-MAT-XXXX"))
            );

            doc.Add(detail);

            doc.Close();
        }
    }
}