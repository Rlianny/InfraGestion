using Domain.Extensions;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using Domain.Attributes;
namespace BackendTestProject.UnitTests.Domain
{
    
    public class IPdfReportGeneratorTest
    {
        [Fact]
        public async Task SampleTest()
        {
            IPdfReportGenerator reportGenerator = new QuestPdfReportGenerator();
            var pdf = await reportGenerator.GeneratePdf(new List<TypicalClass>()
            {
                new TypicalClass(0, "yo", null, null, "dp1",2.3334f),
                new TypicalClass(5, "x", "df1", null, "dp2",4/5f),
                new TypicalClass(6, "yyyo", null, "wow1", "dp3",653/3),
                new TypicalClass(8, "xiiuu", "df2", "wow2", "dp4",0),
                new TypicalClass(1, "a", null, null, "dp5",0),
                new TypicalClass(2, "b", "df3", null, "dp6",0),
                new TypicalClass(3, "c", null, "wow3", "dp7", 0),
                new TypicalClass(4, "d", "df4", "wow4", "dp8", 0),
                new TypicalClass(7, "e", null, null, "dp9", 0),
                new TypicalClass(9, "f", "df5", null, "dp10", 0),
                new TypicalClass(10, "g", null, "wow5", "dp11",MathF.Sqrt(2)),
                new TypicalClass(1, "a", null, null, "dp5",0),
                new TypicalClass(2, "b", "df3", null, "dp6",0),
                new TypicalClass(3, "c", null, "wow3", "dp7", 0),
                new TypicalClass(4, "d", "df4", "wow4", "dp8", 0),
                new TypicalClass(7, "e", null, null, "dp9", 0),
                new TypicalClass(9, "f", "df5", null, "dp10", 0),
                new TypicalClass(10, "g", null, "wow5", "dp11",MathF.Sqrt(2)),
                new TypicalClass(1, "a", null, null, "dp5",0),
                new TypicalClass(2, "b", "df3", null, "dp6",0),
                new TypicalClass(3, "c", null, "wow3", "dp7", 0),
                new TypicalClass(4, "d", "df4", "wow4", "dp8", 0),
                new TypicalClass(7, "e", null, null, "dp9", 0),
                new TypicalClass(9, "f", "df5", null, "dp10", 0),
                new TypicalClass(10, "g", null, "wow5", "dp11",MathF.Sqrt(2)),
            });
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"test.pdf");
            File.WriteAllBytes(path, pdf);
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path) { UseShellExecute = true };
            process.Start();
            await process.WaitForExitAsync();
            
        // Arrange
        // Act
        // Assert
    }
    }
    public class TypicalClass
    {
        public TypicalClass(int count, string name, string? dfName, string? wow, string dp,float floatNumber)
        {
            Count = count;
            Name = name;
            DfName = dfName;
            Wow = wow;
            Dp = dp;
            FloatNumber = floatNumber;
        }
        [PdfReportDisplay("Cuenta")]
        public int Count { get; set; }
        [PdfReportDisplay("Nombre")]
        public string Name { get; set; }
        [PdfReportDisplay("DfNombre")]
        public string? DfName { get; set; }
        [PdfReportDisplay("Wow Pedro")]
        public string? Wow { get;set;  }
        
        [PdfReportDisplay("dpppe")]
        public string Dp { get; set;  }
        public string FloatNumber1 { get; }
        [PdfReportDisplay("Numero flotante")]
        public float FloatNumber { get; set; }
    }
}
