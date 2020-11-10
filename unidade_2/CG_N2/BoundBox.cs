using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{

    internal class BoundBox
    {
        public double MinimoX { get; private set; }
        public double MaximoX { get; private set; }
        public double MinimoY { get; private set; }
        public double MaximoY { get; private set; }

        public int RaioBoundBox { get; private set; }
        public Ponto4D CentroBoundBox { get; private set; }

        public BoundBox(Ponto4D pontoCentral, int raioCirculo)
        {
            RaioBoundBox = raioCirculo;
            CentroBoundBox = pontoCentral;

            var ponto45 = GerarPontoCirculo(45, raioCirculo, pontoCentral);
            MaximoX = ponto45.X;
            MaximoY = ponto45.Y;
            var ponto225 = GerarPontoCirculo(225, raioCirculo, pontoCentral);
            MinimoX = ponto225.X;
            MinimoY = ponto225.Y;
        }

        private Ponto4D GerarPontoCirculo(int grau, int raioCirculo, Ponto4D pontoCentral)
        {
            var pontoMatematico = Matematica.GerarPtosCirculo(grau, raioCirculo);
            return new Ponto4D(pontoMatematico.X + pontoCentral.X, pontoMatematico.Y + pontoCentral.Y, 0);
        }

        public bool PermiteMover(Ponto4D pontoVerificado)
        {
            if (IsInBiggerArea(pontoVerificado))
            {
                return true;
            }

            var verificarX = pontoVerificado.X - CentroBoundBox.X;
            var verificarY = pontoVerificado.Y - CentroBoundBox.Y;
            return (verificarX * verificarX) + (verificarY * verificarY) <= (RaioBoundBox * RaioBoundBox);
        }

        public bool IsInBiggerArea(Ponto4D pontoVerificado)
        {
            return pontoVerificado.X <= MaximoX && pontoVerificado.X >= MinimoX &&
            pontoVerificado.Y <= MaximoY && pontoVerificado.Y >= MinimoY;
        }
    }

}