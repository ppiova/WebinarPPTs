using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorOCRImages.Models
{
    public class OcrResultDTO
    {
        public string Language { get; set; }
        public string DetectedText  { get; set; }
    }
}
