/**
  Autor: Dalton Solano dos Reis
**/

using System;
using System.Collections.Generic;
using System.Linq;

namespace CG_Biblioteca
{
    /// <summary>
    /// Classe com funções matemáticas.
    /// </summary>
    public abstract class Matematica
    {
        /// <summary>
        /// Função para calcular um ponto sobre o perímetro de um círculo informando um ângulo e raio.
        /// </summary>
        /// <param name="angulo"></param>
        /// <param name="raio"></param>
        /// <returns></returns>
        public static Ponto4D GerarPtosCirculo(double angulo, double raio)
        {
            Ponto4D pto = new Ponto4D();
            pto.X = (raio * Math.Cos(Math.PI * angulo / 180.0));
            pto.Y = (raio * Math.Sin(Math.PI * angulo / 180.0));
            pto.Z = 0;
            return (pto);
        }

        public static double GerarPtosCirculoSimétrico(double raio)
        {
            return raio * Math.Cos(Math.PI * 45 / 180.0);
        }

        public static Ponto4D GetPontoMaisProximo(IEnumerable<Ponto4D> listaPontos, Ponto4D pontoComparacao)
        {
            var pontoMaisProximo = listaPontos.FirstOrDefault();
            var distanciaMaisProximo = GetDistanciaPontos(pontoMaisProximo, pontoComparacao);

            foreach (var ponto in listaPontos)
            {
                var dist = GetDistanciaPontos(ponto, pontoComparacao);

                if (dist < distanciaMaisProximo)
                {
                    distanciaMaisProximo = dist;
                    pontoMaisProximo = ponto;
                }
            }

            return pontoMaisProximo;
        }

        public static double GetDistanciaPontos(Ponto4D ponto1, Ponto4D ponto2)
        {
            var distancia = Math.Pow(ponto1.X - ponto2.X, 2) + Math.Pow(ponto1.Y - ponto2.Y, 2);

            return distancia;
        }

        public static double GetDistanciaPontosSqrt(Ponto4D ponto1, Ponto4D ponto2)
        {
            var distancia = GetDistanciaPontos(ponto1, ponto2);

            return Math.Sqrt(distancia);
        }

        public static bool IsPontoDentroPoligono(IEnumerable<Ponto4D> listaPontos, Ponto4D pontoSelecionado)
        {

            var pontoAnterior = listaPontos.FirstOrDefault();
            var quantidadeIntersec = 0;

            foreach (var ponto in listaPontos)
            {
                if (ponto == pontoAnterior)
                    continue;

                if (ponto.Y != pontoAnterior.Y)
                {
                    var pontoIntersec = BuscarPontoIntersec(ponto, pontoAnterior, pontoSelecionado);
                
                    if (pontoIntersec == null)
                    {
                        continue;
                    }

                    if (pontoIntersec.X == pontoSelecionado.X)
                    {
                        return true;
                    }
                    else
                    {
                        if (pontoIntersec.X > pontoSelecionado.X &&
                        pontoIntersec.Y >= Math.Min(ponto.Y, pontoAnterior.Y) &&
                        pontoIntersec.Y <= Math.Max(ponto.Y, pontoAnterior.Y))
                        {
                            quantidadeIntersec++;
                        }
                    }
                }
                else
                {
                    if (pontoSelecionado.Y == pontoAnterior.Y &&
                        pontoSelecionado.X >= Math.Min(ponto.X, pontoAnterior.X) &&
                        pontoSelecionado.X <= Math.Max(ponto.X, pontoAnterior.X))
                    {
                        return true;
                    }
                }
                pontoAnterior = ponto;
            }

            return quantidadeIntersec % 2 == 1;
        }

        public static Ponto4D BuscarPontoIntersec(Ponto4D ponto1, Ponto4D ponto2, Ponto4D pontoSelecionado)
        {
            var coef = (pontoSelecionado.Y - ponto1.Y) / (ponto2.Y - ponto1.Y);
            if (coef < 0 || coef > 1)
            {
                return null;
            }

            var xIntersec = ponto1.X + ((ponto2.X - ponto1.X) * coef);
            var pontoIntersec = new Ponto4D(xIntersec, pontoSelecionado.Y);

            return pontoIntersec;
        }

    }
}