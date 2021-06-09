using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;
using IronOcr;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

namespace IronOcrSampleProject
{
    public class IronOcrSampleTest
    {
        public const string File1 = "test1.jpg";
        public const string File2 = "test2.jpg";

        public IronOcrSampleTest()
        {
            IronOcr.Installation.LicenseKey = "<insert license key here>";
        }

        [Theory]
        [MemberData(nameof(GetFilesToTest))]
        public void IronOcrImageTest(string fileName)
        {
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput())
            {
                string imgText = "";
                if (fileName == File1) imgText = "Test";
                if (fileName == File2) imgText = "GRANDE LARGE";

                var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
                var path = $@"{dir}\images\{fileName}";

                Input.AddImage(path);
                var result = Ocr.Read(Input);
                
                Assert.True(result.Confidence > 80, "The result needs to be of high confidence.");
                Assert.True(result.Text == imgText, $"The text needs to say '{imgText}'.");
            }
        }

        [Fact]
        public void IronOcrPdfTest()
        {
            var Ocr = new IronTesseract();
            using (var input = new OcrInput())
            {
                var dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent?.FullName;
                var path = $@"{dir}\pdfs\test1.pdf";
                input.AddPdf(path);
                var Result = Ocr.Read(input);

                Assert.True(Result.Confidence > 80, "The result needs to be of high confidence.");
                Assert.True(Result.Text == "Test", "The text needs to say 'Test'.");
            }
        }

        public static IEnumerable<object[]> GetFilesToTest() => new List<string[]>() {new[]{File1}, new []{File2}};
    }
}
