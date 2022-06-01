using T1.Helpers;

namespace T1_AA.DataTypes
{
    public class TipoTerreno : Enumeration
    {
        public static readonly TipoTerreno Plano = new TipoTerreno(1,1,'.');
        public static readonly TipoTerreno Rochoso = new TipoTerreno(2,5,'R');
        public static readonly TipoTerreno Floresta = new TipoTerreno(3,10,'F');
        public static readonly TipoTerreno Agua = new TipoTerreno(4,15,'A');
        public static readonly TipoTerreno Montanhoso = new TipoTerreno(5,200, 'M');
        public int id { get; set; }
        public int tempo { get; set; }
        public char letraRepresentante { get; set; } 
        public TipoTerreno() { }
        public TipoTerreno (int _id, int tem, char letra) : base(_id,letra.ToString())
        {
            id = _id;
            tempo = tem;
            letraRepresentante = letra;
        }
    }
}
