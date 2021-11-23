using LibDeImagensGbaDs.Sprites;
using System.Collections.Generic;

namespace FormatosNitro.Imagens.FNcer
{
    public class Ebk
    {
        public int Id { get; set; }
        public ushort QuantidadeDeCelulas { get; set; }
        public ushort InfoSomenteLeituraCelula { get; set; }
        public uint EnderecoCelula { get; set; }
        public uint EnderecoParticao { get; set; }
        public uint TamanhoParticao { get; set; }
        public short LarguraMaxima { get; set; }
        public short AlturaMaxima { get; set; }
        public short LarguraMinima { get; set; }
        public short AlturaMinima { get; set; }
        public List<Oam> Oams { get; set; }

        public override string ToString()
        {
            return $"{Id}";
        }
    }

   
}
