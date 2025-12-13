using Domain.Attributes;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class PdfReportGeneratorExtensions
    {
        private static PdfConverter converter= new();
        public static async Task<byte[]>  GeneratePdf<T>(this IPdfReportGenerator pdfReportGenerator,IEnumerable<T> table)
        {
            var convertedTable = converter.Convert(table);
            var pdf = await pdfReportGenerator.CreatePdfTable(convertedTable);
            return pdf;
        }
        public static List<string> GetRows()
        {
            return new List<string>(); 
        }
    }
    internal class PdfConverter
    {
        private readonly Dictionary<Type, PropertyInfo[]> _cache;
        public PdfConverter()
        {
            _cache = new Dictionary<Type, PropertyInfo[]>();
        }
        public Table Convert<T>(IEnumerable<T> table)
        {

            var type = typeof(T);
            if (_cache.TryGetValue(type, out var properties))
            {
                List<string> headers = GetHeaders(properties);
                List<TableRow> rows = GetRows(properties, table);
                return new Table(headers, rows);
            }
            else
            {
                properties =type.GetProperties();
                _cache.Add(type, properties);
                List<string> headers = GetHeaders(properties);
                List<TableRow> rows = GetRows(properties, table);
                return new Table(headers, rows);
            }

           
        }
       
        private List<TableRow> GetRows<T>(PropertyInfo[] properties, IEnumerable<T> table)
        {
            List<TableRow> rows = new List<TableRow>();
            foreach (var row in table)
            {
                
                List<string> cells = new();
                foreach (var property in properties)
                {
                    var value = property.GetValue(row);
                    if (value is not null)
                    {
                        var valueStr = value.ToString();
                        if (valueStr is not null)
                        {
                            cells.Add(valueStr);
                        }
                        else
                        {
                            cells.Add("NULL");
                        }
                    }
                    else
                    {
                        cells.Add("NULL");
                    }
                }
                rows.Add(new TableRow(cells));
            }
            return rows;
        }

        private List<string> GetHeaders(PropertyInfo[] properties)
        {
            List<string> headers = new();
            foreach (var property in properties)
            {
                var pdfDisplayAttribute = property.GetCustomAttribute<PdfReportDisplayAttribute>();
                headers.Add(pdfDisplayAttribute is null?property.Name:pdfDisplayAttribute.Text);
            }
            return headers;
        }
    }
}
