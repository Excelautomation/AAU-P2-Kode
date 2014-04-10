using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    public class Skadesblanket
    {
        public DateTime Dato { get; set; }
        public string AnmeldersNavn { get; set; }
        public int AnmeldersMedlemsNr { get; set; }
        public Baad SkadetBåd { get; set; }
        public SkadeBeskrivelse Skade { get; set; }
    }
}
