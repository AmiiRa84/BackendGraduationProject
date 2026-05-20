using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Shared.DTOs
{
    public class AssessmentRequest
    {
        public string? Email { get; set; }
        public string? ChildName { get; set; }      // 👈 ضيفي ده
        public string? SpecialistName { get; set; }
        public VisualData ?Visual { get; set; }
        public LanguageData? Language { get; set; }
        public MemoryData ?Memory { get; set; }
    }

    public class VisualData
    {
        public int CorrectFound { get; set; }
        public int Total { get; set; }
        public int Time { get; set; }
    }

    public class LanguageData
    {
        public int Words { get; set; }
        public string? Sentence { get; set; }
        public string ?Sequence { get; set; }
    }

    public class MemoryData
    {
        public int Correct { get; set; }
        public int Total { get; set; }
        public string ?Order { get; set; }
        public string ?Repeat { get; set; }
    }
}
