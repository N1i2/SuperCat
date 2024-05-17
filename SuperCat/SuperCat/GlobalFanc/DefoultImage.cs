using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SuperCat.GlobalFanc
{
    internal class DefoultImage
    {
        public List<byte[]> img { get; set; } = null!;

        public DefoultImage()
        {
            using (var file = new FileStream("../../../defoultImage.json", FileMode.Open))
            {
                var op = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };


                img = JsonSerializer.Deserialize<List<byte[]>>(file, op) ?? new List<byte[]>();
            }
        }
    }
}
