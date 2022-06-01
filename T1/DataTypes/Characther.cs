using T1.Helpers;

namespace T1_AA.DataTypes
{
    public class Characther : Enumeration
    {
        public static readonly Characther Aang = new Characther(0,1.8F,"Aang");
        public static readonly Characther Zukko = new Characther(1,1.6F,"Zukko");
        public static readonly Characther Toph = new Characther(2,1.6F,"Toph");
        public static readonly Characther Katara = new Characther(3,1.6F,"Katara");
        public static readonly Characther Sokka = new Characther(4,1.4F,"Sokka");
        public static readonly Characther Appa = new Characther(5,0.9F,"Appa");
        public static readonly Characther Momo = new Characther(6,0.7F,"Momo");
        public string nome { get; set; }
        public int id { get; set; } 
        public float agility { get; set; }
        public int energia { get; set; }
        public Characther() { }
        public Characther (int _id, float _agi, string _nome) : base(_id, _nome)
        {
            id = _id;
            nome = _nome;
            agility = _agi;
            energia = 8;
        }
        public void DiminuiEnergia (int gasto)
        {
            this.energia = this.energia - gasto;
        }
    }
}
