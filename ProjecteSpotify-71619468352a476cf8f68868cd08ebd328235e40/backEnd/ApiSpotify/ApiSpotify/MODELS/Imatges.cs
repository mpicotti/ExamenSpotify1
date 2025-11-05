using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiSpotify.MODELS
{
    public class Imatges
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Dimensions { get; set; }
        public string Alcada { get; set; }
        public string Amplada { get; set; }

    }
}
