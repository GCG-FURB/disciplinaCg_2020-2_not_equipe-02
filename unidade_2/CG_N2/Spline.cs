using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;
using System;

namespace gcgcg
{

    internal class Spline : ObjetoGeometria
    {
        private Ponto4D _ponto1 { get; set; }
        public Ponto4D Ponto1
        {
            get { return _ponto1; }
            set
            {
                this._ponto1 = value;
                GerarPontos();
            }
        }
        private Ponto4D _ponto2 { get; set; }
        public Ponto4D Ponto2
        {
            get { return _ponto2; }
            set
            {
                this._ponto2 = value;
                GerarPontos();
            }
        }
        private Ponto4D _ponto3 { get; set; }
        public Ponto4D Ponto3
        {
            get { return _ponto3; }
            set
            {
                this._ponto3 = value;
                GerarPontos();
            }
        }
        private Ponto4D _ponto4 { get; set; }
        public Ponto4D Ponto4
        {
            get { return _ponto4; }
            set
            {
                this._ponto4 = value;
                GerarPontos();
            }
        }

        private int _quantidadePontos = 2;
        public int QuantidadePontos
        {
            get { return _quantidadePontos; }
            set
            {
                if (value > 1)
                {
                    _quantidadePontos = value;
                    GerarPontos();
                }
            }
        }

        public Spline(string rotulo, Objeto paiRef, Ponto4D ponto1, Ponto4D ponto2, Ponto4D ponto3, Ponto4D ponto4) : base(rotulo, paiRef)
        {
            _ponto1 = ponto1;
            _ponto2 = ponto2;
            _ponto3 = ponto3;
            _ponto4 = ponto4;

            GerarPontos();
        }

        private void GerarPontos()
        {
            base.PontosRemoverTodos();

            var baseCalculo = 1d / _quantidadePontos;
            for (var t = 0d; t <= 1; t += baseCalculo)
            {
                var x = Bezier(_ponto4.X, _ponto3.X, _ponto2.X, _ponto1.X, t);
                var y = Bezier(_ponto4.Y, _ponto3.Y, _ponto2.Y, _ponto1.Y, t);

                var pontoNovo = new Ponto4D(x, y, 0, 0);

                base.PontosAdicionar(pontoNovo);
            }
        }

        private double Bezier(double p0, double p1, double p2, double p3, double t)
        {
            var peso = 1 - t;
            return (Math.Pow(peso, 3) * p0) +
                (3 * t * Math.Pow(peso, 2) * p1) +
                (3 * Math.Pow(t, 2) * peso * p2) +
                (Math.Pow(t, 3) * p3);
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitivaTipo);

            GL.Color3((byte)118, (byte)249, (byte)251);
            GL.Vertex2(_ponto1.X, _ponto1.Y);
            GL.Vertex2(_ponto2.X, _ponto2.Y);
            GL.Vertex2(_ponto3.X, _ponto3.Y);
            GL.Vertex2(_ponto4.X, _ponto4.Y);

            GL.Color3((byte)255, (byte)255, (byte)0);
            foreach (Ponto4D pto in pontosLista)
            {
                GL.Vertex2(pto.X, pto.Y);
            }

            GL.End();
        }

    }

}