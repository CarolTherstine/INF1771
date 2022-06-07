using T1.Helpers;

namespace T1_AA.DataTypes
{
    public class Etapa : Enumeration
    {
        public static readonly Etapa Etapa0 = new Etapa(0, 0, '0');
        public static readonly Etapa Etapa1 = new Etapa(1,10,'1'); 
        public static readonly Etapa Etapa2 = new Etapa(2,20,'2'); 
        public static readonly Etapa Etapa3 = new Etapa(3,30,'3'); 
        public static readonly Etapa Etapa4 = new Etapa(4,40,'4'); 
        public static readonly Etapa Etapa5 = new Etapa(5,50,'5'); 
        public static readonly Etapa Etapa6 = new Etapa(6,60,'6'); 
        public static readonly Etapa Etapa7 = new Etapa(7,70,'7'); 
        public static readonly Etapa Etapa8 = new Etapa(8,80,'8'); 
        public static readonly Etapa Etapa9 = new Etapa(9,90,'9'); 
        public static readonly Etapa Etapa10 = new Etapa(10,100,'B'); 
        public static readonly Etapa Etapa11 = new Etapa(11,110,'C'); 
        public static readonly Etapa Etapa12 = new Etapa(12,120,'D'); 
        public static readonly Etapa Etapa13 = new Etapa(13,130,'E'); 
        public static readonly Etapa Etapa14 = new Etapa(14,140,'G'); 
        public static readonly Etapa Etapa15 = new Etapa(15,150,'H'); 
        public static readonly Etapa Etapa16 = new Etapa(16,160,'I'); 
        public static readonly Etapa Etapa17 = new Etapa(17,170,'J'); 
        public static readonly Etapa Etapa18 = new Etapa(18,180,'K'); 
        public static readonly Etapa Etapa19 = new Etapa(19,190,'L'); 
        public static readonly Etapa Etapa20 = new Etapa(20,200,'N'); 
        public static readonly Etapa Etapa21 = new Etapa(21,210,'O'); 
        public static readonly Etapa Etapa22 = new Etapa(22,220,'P'); 
        public static readonly Etapa Etapa23 = new Etapa(23,230,'Q'); 
        public static readonly Etapa Etapa24 = new Etapa(24,240,'S'); 
        public static readonly Etapa Etapa25 = new Etapa(25,250,'T'); 
        public static readonly Etapa Etapa26 = new Etapa(26,260,'U'); 
        public static readonly Etapa Etapa27 = new Etapa(27,270,'V'); 
        public static readonly Etapa Etapa28 = new Etapa(28,280,'W'); 
        public static readonly Etapa Etapa29 = new Etapa(29,290,'X'); 
        public static readonly Etapa Etapa30 = new Etapa(30,300,'Y'); 
        public static readonly Etapa Etapa31 = new Etapa(31,310,'Z'); 
        public int dificuldade { get; set; }  
        public char identificador { get; set; }
        public int id { get; set; }
        public Etapa() { }
        public Etapa(int _id, int dif, char representacao)
        {
            dificuldade = dif;
            identificador = representacao;
            id = _id;
        }
    }
}
