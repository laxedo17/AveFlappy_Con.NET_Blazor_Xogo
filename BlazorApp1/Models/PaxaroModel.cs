namespace BlazorApp1.Models
{
    public class PaxaroModel
    {
        //a posicion vertical do paxaro debe cambiar 
        //segun o momento
        //necesitamos unha variable para controlar 
        //a distancia do paxaro sobre o chan
        public int DistanciaDoChan { get; set; } = 100;
        //valor dado por defecto

        public int ForzaSalto { get; set; } = 50;

        public bool IsNoChan()
        {
            return DistanciaDoChan <= 0;
        }

        /// <summary>
        /// Indica que distancia vai cair o paxaro cada vez 
        /// que chamamos
        /// a este metodo
        /// </summary>
        public void Caida(int gravidade)
        {
            //restamos a  forza da gravidade de Newton 
            //da distancia ao chan do paxaro
            DistanciaDoChan -= gravidade;
        }

        public void Saltar()
        {
            if (DistanciaDoChan <= 520)
            {
                DistanciaDoChan += ForzaSalto;
            }
        }
    }
}