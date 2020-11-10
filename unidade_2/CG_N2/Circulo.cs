using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{

    internal class Circulo : ObjetoGeometria
    {

        private Ponto4D _pontoCentral { get; set; }
        public Ponto4D PontoCentral
        {
            get { return _pontoCentral; }
            set
            {
                this._pontoCentral = value;
                GerarPontos();
            }
        }

        private int _raioCirculo { get; set; }
        public int RaioCirculo
        {
            get { return _raioCirculo; }
            set
            {
                this._raioCirculo = value;
                GerarPontos();
            }
        }


        public Circulo(string rotulo, Objeto paiRef, Ponto4D pontoCentral, int raioCirculo) : base(rotulo, paiRef)
        {
            base.PrimitivaTipo = PrimitiveType.Points;
            _raioCirculo = raioCirculo;
            _pontoCentral = pontoCentral;

            GerarPontos();
        }

        private void GerarPontos()
        {
            base.PontosRemoverTodos();
            for (var i = 0; i < 360; i += 5)
            {
                var pontoMatematico = Matematica.GerarPtosCirculo(i, RaioCirculo);
                var pontoFinal = new Ponto4D(pontoMatematico.X + PontoCentral.X, pontoMatematico.Y + PontoCentral.Y, 0);
                base.PontosAdicionar(pontoFinal);
            }
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }

    }

}