/**
  Autor: Dalton Solano dos Reis
**/

using System.Collections.Generic;
using CG_Biblioteca;

namespace gcgcg
{
    internal abstract class ObjetoGeometria : Objeto
    {
        protected List<Ponto4D> pontosLista = new List<Ponto4D>();

        public ObjetoGeometria(char rotulo, Objeto paiRef) : base(rotulo, paiRef) { }

        protected override void DesenharGeometria()
        {
            DesenharObjeto();
        }
        protected abstract void DesenharObjeto();
        public void PontosAdicionar(Ponto4D pto)
        {
            pontosLista.Add(pto);
            if (pontosLista.Count.Equals(1))
                base.BBox.Atribuir(pto);
            else
                base.BBox.Atualizar(pto);
            base.BBox.ProcessarCentro();
        }

        public void PontosRemoverUltimo()
        {
            pontosLista.RemoveAt(pontosLista.Count - 1);
        }

        protected void PontosRemoverTodos()
        {
            pontosLista.Clear();
        }

        public void RemoverPonto(Ponto4D ponto)
        {
            pontosLista.Remove(ponto);
        }

        public Ponto4D PontosUltimo()
        {
            return pontosLista[pontosLista.Count - 1];
        }

        public void PontosAlterar(Ponto4D pto, int posicao)
        {
            pontosLista[posicao] = pto;
        }

        public IEnumerable<Ponto4D> GetPontos()
        {
            return pontosLista;
        }

        public bool IsPontoDentro(Ponto4D pontoComparacao)
        {
            if (!BBox.IsPontoDentro(pontoComparacao))
                return false;

            return Matematica.IsPontoDentroPoligono(GetPontos(), pontoComparacao);
        }

        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
    }
}