using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{

    internal class PontoGeometrico : ObjetoGeometria
    {

        private Ponto4D _ponto { get; set; }
        public Ponto4D Ponto
        {
            get { return _ponto; }
            set
            {
                this._ponto = value;
                SetarPonto();
            }
        }


        public PontoGeometrico(string rotulo, Objeto paiRef, Ponto4D ponto) : base(rotulo, paiRef)
        {
            base.PrimitivaTipo = PrimitiveType.Points;

            _ponto = ponto;
            SetarPonto();
        }

        private void SetarPonto()
        {
            base.PontosRemoverTodos();

            base.PontosAdicionar(_ponto);
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