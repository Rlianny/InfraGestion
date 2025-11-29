using Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;


QuestPDF.Settings.License = LicenseType.Community;

namespace Infrastructure.Services
{
    public class QuestPdfReportGenerator : IPdfReportGenerator
    {
        public Task<byte[]> CreatePdfTable(Table tableData)
        {
            var document =Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));


                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // 2. Define and populate the table
                            column.Item().Table(table =>
                            {

                                table.ColumnsDefinition(columns =>
                                {
                                    foreach (var header in tableData.Headers)
                                    {
                                        columns.RelativeColumn();
                                    }
                                });

                                // Add table header (repeats on new pages automatically)
                                table.Header(header =>
                                {
                                    foreach (var headerText in tableData.Headers)
                                    {
                                        header.Cell().Padding(5).BorderBottom(1).Text(headerText).Bold();
                                    }
                                });

                                // Add table rows from data source
                                foreach (var row in tableData.Rows)
                                {
                                    foreach (var cell in row.Cells)
                                    {
                                        table.Cell().Padding(5).Text(cell);
                                    }
                                }
                                
                            });
                        });
                });
                });
            
            
            return Task.FromResult(document.GeneratePdf());
        }
    } 
}


