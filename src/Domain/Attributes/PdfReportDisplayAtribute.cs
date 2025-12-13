using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PdfReportDisplayAttribute:Attribute
    {
        public string Text { get; }
        public PdfReportDisplayAttribute(string text) =>Text = text;
    }
}
