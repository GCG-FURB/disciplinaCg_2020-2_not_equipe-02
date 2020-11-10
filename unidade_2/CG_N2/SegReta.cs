using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{

    internal class SegReta : ObjetoGeometria
    {
        private Ponto4D _ponto1 { get; set; }
        public Ponto4D Ponto1
        {
            get { return _ponto1; }
            set
            {
                pontosLista[0] = value;
                this._ponto1 = value;
            }
        }
        private Ponto4D _ponto2 { get; set; }
        public Ponto4D Ponto2
        {
            get { return _ponto2; }
            set
            {
                pontosLista[1] = value;
                this._ponto2 = value;
            }
        }


        public SegReta(string rotulo, Objeto paiRef, Ponto4D ponto1, Ponto4D ponto2) : base(rotulo, paiRef)
        {
            base.PrimitivaTipo = PrimitiveType.Lines;

            GerarPontos(ponto1, ponto2);

            Ponto1 = ponto1;
            Ponto2 = ponto2;
        }

        private void GerarPontos(Ponto4D ponto1, Ponto4D ponto2)
        {
            base.PontosRemoverTodos();

            base.PontosAdicionar(ponto1);
            base.PontosAdicionar(ponto2);
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