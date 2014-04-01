using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK.Model
{
    class Medlem
    {
        public int MedlemsNr { get; set; }
        public int ID { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public int PostNr { get; set; }
        public string By { get; set; }
        public string Telefon { get; set; }
        public string TelefonArbejde { get; set; }
        public string TelefonMobil { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public DateTime Fødselsdato { get; set; }
        public bool Frigivet { get; set; }
        public bool Svømmeprøve { get; set; }
        public bool KortTStyrmand { get; set; }
        public bool LangTStyrmand { get; set; }
        public bool ScullerRet { get; set; }
        public bool OutriggerRet { get; set; }
        public bool Kajakret { get; set; }
    }
}
