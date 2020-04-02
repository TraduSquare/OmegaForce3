using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yarhl.FileFormat;
using Yarhl.Media.Text;

namespace OmegaForce3.Text
{
    class Gsm2Po : IConverter<Gsm, Po>
    {
        private Po Po;
        public Gsm2Po()
        {
            //Read the language used by the user' OS, this way the editor can spellcheck the translation. - Thanks Liquid_S por the code
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            Po = new Po
            {
                Header = new PoHeader("Megaman Starforce 3", "anyemail@gmail.com", currentCulture.Name)
                {
                    LanguageTeam = "GlowTranslations & Transcene"
                }
            };

        }
        public Po Convert(Gsm source)
        {
            for (var i = 0; i < source.BlockCount; i++)
            {
                var entry = new PoEntry {Context = i.ToString(), Original = source.Lines[i]};
                Po.Add(entry);
            }
            return Po;
        }
    }
}
