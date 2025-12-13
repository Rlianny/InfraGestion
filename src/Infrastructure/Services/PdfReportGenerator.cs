using Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuestPdfReportGenerator : IPdfReportGenerator
    {
        public QuestPdfReportGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public Task<byte[]> CreatePdfTable(Table tableData)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    page.DefaultTextStyle(x => x.FontSize(24).FontFamily(Fonts.Verdana));

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);
                        // Add a title to the report
                       // column.Item().Text("Reporte: Ejemplo de Reporte").FontSize(26).SemiBold().FontColor(Colors.Black);

                        column.Item().Table(table =>
                        {
                            var OddRowColor = Colors.White;
                            var EvenRowColor = Colors.Grey.Lighten4;
                            var HeaderColor = Colors.White;

                            // Dynamic Column Definition
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < tableData.Headers.Count; i++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            // Styled Header (Centered Text and Data Cleaning)
                            table.Header(header =>
                            {
                                foreach (var headerTextRaw in tableData.Headers)
                                {
                                    // FIX: Remove surrounding quotes and trim whitespace/newlines
                                    var headerTextClean = headerTextRaw.Trim().Replace("\"", "");

                                    header.Cell()
                                        .Background(HeaderColor)
                                        .BorderBottom(2)
                                        .BorderColor(Colors.Black)
                                        .Text(headerTextClean)
                                        .FontColor(Colors.Black)
                                        .FontSize(22)
                                        .Bold()
                                        .AlignCenter();
                                }
                            });

                            // Data Rows with Centered Text, Alternating Colors, and Increased Height (Data Cleaning)
                            uint rowIndex = 0;
                            foreach (var row in tableData.Rows)
                            {
                                var backgroundColor = rowIndex % 2 == 0 ? EvenRowColor : OddRowColor;

                                foreach (var cellRaw in row.Cells)
                                {
                                    // FIX: Remove surrounding quotes and trim whitespace/newlines
                                    var cellClean = cellRaw.Trim().Replace("\"", "");

                                    var cellContainer = table.Cell()
                                        .Background(backgroundColor)
                                        //.PaddingVertical(10) // Increased height
                                        .PaddingHorizontal(5)
                                        .BorderColor(Colors.Grey.Lighten2)
                                        .AlignCenter();

                                    // Handle NULL values
                                    if (cellClean.ToUpper() == "NULL")
                                    {
                                        cellContainer.Text(cellClean).Italic().FontColor(Colors.Grey.Medium);
                                    }
                                    else
                                    {
                                        cellContainer.Text(cellClean ?? "");
                                    }
                                }
                                rowIndex++;
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return Task.FromResult(document.GeneratePdf());
        }
    }
}