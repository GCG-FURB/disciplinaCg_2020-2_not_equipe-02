/**
  Autor: Dalton Solano dos Reis
**/

using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class RetanguloCorDiferente : ObjetoGeometria
    {
        public RetanguloCorDiferente(string rotulo, Objeto paiRef, Ponto4D ptoInfEsq, Ponto4D ptoSupDir) : base(rotulo, paiRef)
        {
            base.PontosAdicionar(ptoInfEsq);
            base.PontosAdicionar(new Ponto4D(ptoSupDir.X, ptoInfEsq.Y));
            base.PontosAdicionar(ptoSupDir);
            base.PontosAdicionar(new Ponto4D(ptoInfEsq.X, ptoSupDir.Y));
        }

        protected override void DesenharObjeto()
        {
            GL.Begin(base.PrimitivaTipo);

            GL.Color3((byte)255, (byte)255, (byte)0);
            var pto = pontosLista[0];
            GL.Vertex2(pto.X, pto.Y);

            GL.Color3((byte)0, (byte)0, (byte)0);
            pto = pontosLista[1];
            GL.Vertex2(pto.X, pto.Y);

            GL.Color3((byte)227, (byte)84, (byte)252);
            pto = pontosLista[2];
            GL.Vertex2(pto.X, pto.Y);

            GL.Color3((byte)118, (byte)249, (byte)251);
            pto = pontosLista[3];
            GL.Vertex2(pto.X, pto.Y);

            GL.End();
        }

        //TODO: melhorar para exibir não só a lsita de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Retangulo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}