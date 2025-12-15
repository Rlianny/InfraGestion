using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class QuestPdfReportGenerator : IPdfReportGenerator
    {
        public async Task<byte[]> CreatePdfTable(Table tableData)
        {
            // QuestPDF Community License configuration
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.Tabloid);
                    page.PageColor(Colors.White);

                    // Using a clean Sans-Serif font to match the image UI
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                    page.Content().PaddingVertical(10).Table(pdfTable =>
                    {
                        // 1. DYNAMIC COLUMN DEFINITION
                        // We infer the column count directly from the Header DTO
                        var columnCount = tableData.Headers.Count;

                        pdfTable.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < columnCount; i++)
                            {
                                columns.RelativeColumn();
                            }
                        });

                        // 2. DYNAMIC HEADER GENERATION
                        pdfTable.Header(header =>
                        {
                            foreach (var headerName in tableData.Headers)
                            {
                                header.Cell()
                                    .BorderLeft(1)
                                    .BorderRight(1)
                                    .BorderTop(1)
                                    .BorderBottom(1)    
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Element(HeaderStyle)
                                    .Text(headerName);
                            }

                            // Horizontal divider after headers
                            header.Cell().ColumnSpan((uint)columnCount)
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2);
                        });

                        // 3. DYNAMIC ROW/CELL GENERATION
                        foreach (var row in tableData.Rows)
                        {
                            foreach (var cellValue in row.Cells)
                            {
                                pdfTable.Cell()
                                    .BorderLeft(1)
                                    .BorderRight(1)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten4)
                                    .Element(CellStyle)
                                    .Text(cellValue ?? string.Empty);
                            }
                        }
                    });
                });
            });

            return await Task.FromResult(document.GeneratePdf());
        }

        // --- Visual Styling (Matches the TablePdf.png design) ---

        private IContainer HeaderStyle(IContainer container)
        {
            return container
                .PaddingVertical(12)
                .PaddingHorizontal(5)
                .AlignLeft()
                .DefaultTextStyle(x => x.SemiBold().FontColor("#2d3748").FontSize(11));
        }

        private IContainer CellStyle(IContainer container)
        {
            return container
                .PaddingVertical(12)
                .PaddingHorizontal(5)
                .AlignLeft()
                .DefaultTextStyle(x => x.FontColor("#4a5568"));
        }
    }
}