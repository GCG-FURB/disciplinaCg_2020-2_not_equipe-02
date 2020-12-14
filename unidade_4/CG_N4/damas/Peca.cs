using CG_Biblioteca;
using gcgcg;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace CG_N4.damas
{
    internal class Peca : ObjetoGeometria
    {
        public double Altura { get; private set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public bool IsPecaJogadorUm { get; set; }
        public bool IsRainha { get; set; } = false;

        public Ponto4D PosicaoAtual { get; set; }


        protected List<int> listaTopologia = new List<int>();

        public Peca(int posX, int posY, double radius, double height, Ponto4D centroBase, char rotulo, Objeto paiRef, int segments = 40) : base(rotulo, paiRef)
        {
            PosX = posX;
            PosY = posY;

            Altura = height;

            PosicaoAtual = centroBase;

            for (double y = 0; y < 2; y++)
            {
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;
                    base.PontosAdicionar(new Ponto4D(
                        (float)(centroBase.X + (radius * Math.Cos(theta))),
                        (float)(centroBase.Y + (height * y)),
                        (float)(centroBase.Z + (radius * Math.Sin(theta)))));
                }
            }
            // ponto do centro da base
            base.PontosAdicionar(centroBase);
            // ponto do centro da topo
            base.PontosAdicionar(new Ponto4D(centroBase.X, centroBase.Y + height, centroBase.Z));

            for (int x = 0; x < segments - 1; x++)
            {
                // lados
                listaTopologia.Add(x);
                listaTopologia.Add(x + segments);
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + 1);
                listaTopologia.Add(x);
                // base
                listaTopologia.Add(x);
                listaTopologia.Add(x + 1);
                listaTopologia.Add(segments - 1);
                // topo
                listaTopologia.Add(x + segments + 1);
                listaTopologia.Add(x + segments);
                listaTopologia.Add(segments);
            }

        }

        protected override void DesenharObjeto()
        {
            GL.Begin(PrimitiveType.Triangles);
            foreach (int index in listaTopologia)
                GL.Vertex3(base.pontosLista[index].X, base.pontosLista[index].Y, base.pontosLista[index].Z);
            GL.End();
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cilindro: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
    }
}
