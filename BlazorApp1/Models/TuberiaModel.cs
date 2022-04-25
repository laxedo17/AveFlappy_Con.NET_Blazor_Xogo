namespace BlazorApp1.Models
{
    public class TuberiaModel
    {
        //Este e o ancho do Contenedor do Xogo
        //o cal fara que se xenera a tuberia xusto fora no extremo do Contenedor do Xogo
        public int DistanciaDaIzquierda { get; private set; } = 500;
        //Non queremos ter unha distancia fixa do chan desde a tuberia
        //porque se facemos iso cada tuberia que xeneremos
        //estaria a misma altura e permitiria que o paxaro fose nunha linha recta todo o tempo
        //e o que se quere e que o paxaro vaia esquivando obstaculos-
        //Damoslle un valor aleatorio -random- de 0 a 60
        public int DistanciaDoChan { get; private set; } = new Random().Next(0, 60);
        public int Velocidade { get; set; } = 2;

        //Representa o oco entre o punto mais alto da tuberia de abaixo
        //e o punto mais baixo da tuberia de arriba
        public int OcoInferior => 300 - 150 + DistanciaDoChan; //altura tuberia - altura chan + distancia tuberia do centro
        public int OcoSuperior => 430 - 150 + DistanciaDoChan; // oco tuberia - altura do chan + distancia tuberia do chan

        /// <summary>
        /// Comproba se a distancia da izquierda da tuberia e igual ou inferior a -60
        /// para saber se esta a tuberia fora da pantalla e asi eliminala da memoria.
        /// -60 e o ancho dunha tuberia asi que se a tuberia enteira esta fora,
        /// borraremola no metodo XestorDeTuberias
        /// </summary>
        /// <returns></returns>
        public bool IsForaPantalla()
        {
            return DistanciaDaIzquierda <= -60;
        }

        /// <summary>
        /// Metodo que movera a tuberia cara a esquerda e que chamaremos desde o loop principal do xogo
        /// </summary>
        public void Mover()
        {
            DistanciaDaIzquierda -= Velocidade;
        }

        /// <summary>
        /// Valores hardcodeados. O centro e a distancia ata a esquerda 
        /// da metade do ancho do contenedor e a metade do ancho do paxaro
        /// </summary>
        /// <returns>Devolve true se o paxaro entrou no centro do xogo e non saiu do centro</returns>
        public bool IsCentrado()
        {
            bool entrouNoCentro = DistanciaDaIzquierda <= (500 / 2) + (60 / 2);
            bool saiuDoCentro = DistanciaDaIzquierda <= (500 / 2) - (60 / 2) - 60;

            return entrouNoCentro && !saiuDoCentro;
        }
    }
}