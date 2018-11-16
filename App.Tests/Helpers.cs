using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using Core;
using Dicom;
using Dicom.Imaging;
using Xunit;

namespace App.Tests
{
    public class Helpers
    {
        private readonly string _pathToTestData = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Data");

        [Fact]
        public void GenerateDicomTagClass()
        {
            var sb = new StringBuilder();
            
            sb.Append("public class PatientData {");
            sb.Append(Environment.NewLine);
            foreach (var tag in typeof(DicomTag).GetFields().Where(x => x.FieldType == typeof(DicomTag)))
            {
                sb.Append("public " + tag.FieldType.Name + " " + tag.Name + " {get; set;}");
                sb.Append(Environment.NewLine);
            }
            sb.Append(@"}");
            File.WriteAllText("PatientData.cs", sb.ToString());
        }
    }
}
