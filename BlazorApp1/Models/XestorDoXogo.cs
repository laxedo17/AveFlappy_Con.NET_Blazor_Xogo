namespace BlazorApp1.Models
{
    /// <summary>
    /// Implementa a interface INotifyPropertyChanged para indicarlle en tempo real
    /// a nosa app que as cousas cambian
    /// </summary>
    public class XestorDoXogo
    {
        private readonly int _gravidade = 2;

        //public event PropertyChangedEventHandler? PropertyChanged;
        //aplica a interface INotifyPropertyChanged pero non a necesitamos agora 
        //chamaremos a este evento cando o modelo do paxaro cambie

        //cambiamos o de arriba por un simple EventHandler
        public event EventHandler LoopPrincipalCompletado;

        //public TuberiaModel Tuberia { get; private set; }
        //Pasamos a Tuberia a unha lista de Tuberias, para que aparezan varias en pantalla
        public List<TuberiaModel> Tuberias { get; private set; }

        public PaxaroModel Paxaro { get; private set; }

        //esta variable comprobara se o xogo está correndo 
        //para parar o loop
        //que temos con while (true). Ponhemos false por defecto

        //O valor por defecto dun boolean en C# xa e false sempre
        public bool IsXogoCorrendo { get; private set; } = false;

        public XestorDoXogo()
        {
            //instanciamos os obxectos necesarios no constructor
            Paxaro = new PaxaroModel();
            //Tuberia = new TuberiaModel();
            Tuberias = new List<TuberiaModel>();
        }

        /// <summary>
        /// Este vai ser o bucle que se vai a executar unha e outra vez
        /// cando executamos o xogo, xerando animacion ao paxaro
        /// </summary>
        public async void LoopPrincipal()
        {
            //cambiamos o valor de IsXogoCorrendo a true para usar no bucle while
            //isto fara correr o bucle while por sempre xamais ata que terminemos
            IsXogoCorrendo = true;
            while (IsXogoCorrendo)
            {
                MoverObxectos();
                ComprobarColisions();
                XestionarTuberias();

                LoopPrincipalCompletado?.Invoke(this, EventArgs.Empty);
                //queremos que isto pase tan tan rapido que sexa 
                //tan momentaneo como sexa posible
                //o paxaro caera 2 pixels
                //cada 20 milisegundos!
                await Task.Delay(20);
                //20 milisegundos, para un segundo poriamos 1000
            }
        }

        /// <summary>
        /// //Este metodo chama ao bucle principal do xestor do xogo
        //Cando chamemos ao ContenedorDoXogo
        //con @onclick no arquivo ContenedorXogo.razor lanzarase un evento que indica que
        //fagamos a chamada
        /// </summary>
        public void ComenzarXogo()
        {
            //aseguramonos de que non empecemos un novo xogo
            //entretanto o xogo este correndo
            //se o xogo non esta correndo, podemos empezar un novo
            //en caso contrario non facemos nada
            if (!IsXogoCorrendo)
            {
                Paxaro = new PaxaroModel();
                Tuberias = new List<TuberiaModel>(); //creamos unha lista de tuberias novas, borrando asi a anterior
                LoopPrincipal();
            }
        }

        /// <summary>
        /// Permite que o paxaro salte pero unicamente se o loop principal do xogo esta correndo
        /// </summary>
        public void Saltar()
        {
            if (IsXogoCorrendo)
            {
                Paxaro.Saltar();
            }
        }

        /// <summary>
        /// Comproba se o paxaro chocou co chan ou coa tuberia de abaixo ou a de arriba, o cal sería Game Over
        /// </summary>
        void ComprobarColisions()
        {
            //se o Paxaro toca o chan, paramos o bucle do xogo
            if (Paxaro.IsNoChan())
            {
                GameOver();
                //o paxaro parara de caer cando toca o chan
            }

            // 1. Comproba se hai unha tuberia no centro
            var tuberiaCentrada = Tuberias.FirstOrDefault(t => t.IsCentrado());
            // 2. Se hai unha tuberia comprobamos colisions co paxaro
            if (tuberiaCentrada != null)
            {
                // 2a. Tuberia de abaixo
                bool chocouCoChan = Paxaro.DistanciaDoChan < tuberiaCentrada.OcoInferior - 150; //150 e a altura do chan
                // 2b.Tuberia de arriba
                bool chocouConParteSuperior = Paxaro.DistanciaDoChan + 45 > tuberiaCentrada.OcoSuperior - 150; //45 e a altura do paxaro
                if (chocouCoChan || chocouConParteSuperior)
                {
                    GameOver();
                }
            }


        }

        /// <summary>
        /// Engade unha tuberia se a lista de tuberias non ten ningun obxecto
        /// ou se a ultima tuberia na lista -a da mais a dereita- esta a unha distancia da esquerda da pantalla menor ou igual a 250 pixels.
        /// Tamen comproba cando unha tuberia esta fora de pantalla para eliminala
        /// e asi non terminar tendo centos de tuberias ocupando memoria
        /// </summary>
        void XestionarTuberias()
        {
            if (!Tuberias.Any() || Tuberias.Last().DistanciaDaIzquierda <= 250)
            {
                Tuberias.Add(new TuberiaModel());
            }

            if (Tuberias.First().IsForaPantalla())
            {
                Tuberias.Remove(Tuberias.First());
            }
        }

        void MoverObxectos()
        {
            //cada vez que se chame o metodo o paxaro cae 2 pixels
            Paxaro.Caida(_gravidade);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paxaro)));
            //o obxeto paxaro cambiou despois da caida

            //facemos que se mova a tuberia e indicamos a UI que a propiedade Tuberia cambiou
            //o cal fai que se re-renderize o component Tuberia
            //Tuberia.Mover();
            //queremos mais dunha tuberia e fainos falta un loop
            foreach (var tuberia in Tuberias)
            {
                tuberia.Mover();
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tuberia)));
        }

        /// <summary>
        /// Este metodo termina o xogo
        /// </summary>
        public void GameOver()
        {
            IsXogoCorrendo = false;
        }
    }
}