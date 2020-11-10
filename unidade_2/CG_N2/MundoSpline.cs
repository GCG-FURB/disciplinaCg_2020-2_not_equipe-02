/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
    class MundoSpline : GameWindow
    {
        private static MundoSpline instanciaMundo = null;

        private MundoSpline(int width, int height) : base(width, height) { }

        public static MundoSpline GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new MundoSpline(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   //TODO: achar método MouseDown para não ter variável Global
        private bool mouseMoverPto = false;
#if CG_Privado
    private Privado_SegReta obj_SegReta;
    private Privado_Circulo obj_Circulo;
#endif

        private PontoGeometrico _ponto1;
        private PontoGeometrico _ponto2;
        private PontoGeometrico _ponto3;
        private PontoGeometrico _ponto4;
        private int _pontoSelecionado = 0;

        private Spline _spline;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            GerarSpline();

#if CG_Privado
      obj_SegReta = new Privado_SegReta("B", null, new Ponto4D(50, 150), new Ponto4D(150, 250));
      obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
      objetosLista.Add(obj_SegReta);
      objetoSelecionado = obj_SegReta;

      obj_Circulo = new Privado_Circulo("C", null, new Ponto4D(100, 300), 50);
      obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
      objetosLista.Add(obj_Circulo);
      objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        }

        private void GerarSpline()
        {
            var ponto1 = new Ponto4D(-100, -100, 0, 0);
            var ponto2 = new Ponto4D(-100, 100, 0, 0);
            var ponto3 = new Ponto4D(100, 100, 0, 0);
            var ponto4 = new Ponto4D(100, -100, 0, 0);

            _spline = new Spline("A", null, ponto1, ponto2, ponto3, ponto4);
            _spline.PrimitivaTipo = PrimitiveType.LineLoop;
            objetosLista.Add(_spline);

            _ponto1 = new PontoGeometrico("B", null, ponto1);
            _ponto1.ObjetoCor.CorR = 0; _ponto1.ObjetoCor.CorG = 0; _ponto1.ObjetoCor.CorB = 0;
            _ponto1.PrimitivaTamanho = 5;
            objetosLista.Add(_ponto1);

            _ponto2 = new PontoGeometrico("C", null, ponto2);
            _ponto2.ObjetoCor.CorR = 0; _ponto2.ObjetoCor.CorG = 0; _ponto2.ObjetoCor.CorB = 0;
            _ponto2.PrimitivaTamanho = 5;
            objetosLista.Add(_ponto2);

            _ponto3 = new PontoGeometrico("D", null, ponto3);
            _ponto3.ObjetoCor.CorR = 0; _ponto3.ObjetoCor.CorG = 0; _ponto3.ObjetoCor.CorB = 0;
            _ponto3.PrimitivaTamanho = 5;
            objetosLista.Add(_ponto3);

            _ponto4 = new PontoGeometrico("E", null, ponto4);
            _ponto4.ObjetoCor.CorR = 0; _ponto4.ObjetoCor.CorG = 0; _ponto4.ObjetoCor.CorB = 0;
            _ponto4.PrimitivaTamanho = 5;
            objetosLista.Add(_ponto4);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.R)
                ResetarPontos();
            else if (e.Key == Key.Plus)
                _spline.QuantidadePontos++;
            else if (e.Key == Key.Minus)
                _spline.QuantidadePontos--;
            else if (e.Key == Key.Number1)
                SelecionarPonto(1);
            else if (e.Key == Key.Number2)
                SelecionarPonto(2);
            else if (e.Key == Key.Number3)
                SelecionarPonto(3);
            else if (e.Key == Key.Number4)
                SelecionarPonto(4);
            else if (e.Key == Key.C)
                MoverPonto(0);
            else if (e.Key == Key.B)
                MoverPonto(1);
            else if (e.Key == Key.E)
                MoverPonto(2);
            else if (e.Key == Key.D)
                MoverPonto(3);
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        private void MoverPonto(int direcao)
        {
            var ponto = GetPontoSelecionado();
            if (ponto == null)
            {
                return;
            }

            var ponto4D = ponto.Ponto;
            switch (direcao)
            {
                case 0:
                    ponto4D.Y++;
                    break;
                case 1:
                    ponto4D.Y--;
                    break;
                case 2:
                    ponto4D.X--;
                    break;
                case 3:
                    ponto4D.X++;
                    break;
                default:
                    return;
            }

            AtualizarPontoSelecionado(ponto4D);
        }

        private void ResetarPontos()
        {
            var ponto1 = new Ponto4D(-100, -100, 0, 0);
            var ponto2 = new Ponto4D(-100, 100, 0, 0);
            var ponto3 = new Ponto4D(100, 100, 0, 0);
            var ponto4 = new Ponto4D(100, -100, 0, 0);

            _spline.Ponto1 = ponto1;
            _spline.Ponto2 = ponto2;
            _spline.Ponto3 = ponto3;
            _spline.Ponto4 = ponto4;

            _ponto1.Ponto = ponto1;
            _ponto2.Ponto = ponto2;
            _ponto3.Ponto = ponto3;
            _ponto4.Ponto = ponto4;
        }

        private void SelecionarPonto(int num)
        {
            if (num > 4 || num < 1)
            {
                return;
            }
            _ponto1.ObjetoCor.CorR = 0; _ponto1.ObjetoCor.CorG = 0; _ponto1.ObjetoCor.CorB = 0;
            _ponto2.ObjetoCor.CorR = 0; _ponto2.ObjetoCor.CorG = 0; _ponto2.ObjetoCor.CorB = 0;
            _ponto3.ObjetoCor.CorR = 0; _ponto3.ObjetoCor.CorG = 0; _ponto3.ObjetoCor.CorB = 0;
            _ponto4.ObjetoCor.CorR = 0; _ponto4.ObjetoCor.CorG = 0; _ponto4.ObjetoCor.CorB = 0;

            _pontoSelecionado = num;
            var ponto = GetPontoSelecionado();

            ponto.ObjetoCor.CorR = 255; ponto.ObjetoCor.CorG = 0; ponto.ObjetoCor.CorB = 0;
        }

        private void AtualizarPontoSelecionado(Ponto4D valorNovo)
        {
            var ponto = GetPontoSelecionado();
            if (ponto == null)
            {
                return;
            }

            ponto.Ponto = valorNovo;
            switch (_pontoSelecionado)
            {
                case 1:
                    _spline.Ponto1 = ponto.Ponto;
                    break;
                case 2:
                    _spline.Ponto2 = ponto.Ponto;
                    break;
                case 3:
                    _spline.Ponto3 = ponto.Ponto;
                    break;
                case 4:
                    _spline.Ponto4 = ponto.Ponto;
                    break;
            }
        }

        private PontoGeometrico GetPontoSelecionado()
        {
            switch (_pontoSelecionado)
            {
                case 1:
                    return _ponto1;
                case 2:
                    return _ponto2;
                case 3:
                    return _ponto3;
                case 4:
                    return _ponto4;
                default:
                    return null;
            }
        }

        //TODO: não está considerando o NDC
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null))
            {
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            GL.End();
        }
#endif
    }

    class ProgramSpline
    {
        /*
                static void Main(string[] args)
                {
                    MundoSpline window = MundoSpline.GetInstance(600, 600);
                    window.Title = "CG_N2";
                    window.Run(1.0 / 60.0);
                }
        */
    }
}
