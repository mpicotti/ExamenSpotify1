using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSpotify.MODELS
{
    internal class Perfils
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdUsuari { get; set; }
        public string Nom { get; set; }
        public string Descripcio { get; set; }
        public string Estat { get; set; }

    }
}
