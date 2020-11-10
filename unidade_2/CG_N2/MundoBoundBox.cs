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
    class MundoBoundBox : GameWindow
    {
        private static MundoBoundBox instanciaMundo = null;

        private MundoBoundBox(int width, int height) : base(width, height) { }

        private static int TamanhoCamera = 600;

        public static MundoBoundBox GetInstance()
        {
            if (instanciaMundo == null)
                instanciaMundo = new MundoBoundBox(TamanhoCamera, TamanhoCamera);
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

        private BoundBox ObjetoBoundBox;
        private Retangulo RetanguloBoundBox;
        private Circulo CirculoBoundBox;
        private Circulo CirculoConfinado;

        private bool IsMouseDown = false;
        private Ponto4D PosicaoInicialMouse;
        private Ponto4D PosicaoInicialCirculo;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -TamanhoCamera; camera.xmax = TamanhoCamera; camera.ymin = -TamanhoCamera; camera.ymax = TamanhoCamera;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            GerarBoundBox();

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

        private void GerarBoundBox()
        {
            ObjetoBoundBox = new BoundBox(new Ponto4D(0, 0), 300);

            RetanguloBoundBox = new Retangulo("A", null, new Ponto4D(ObjetoBoundBox.MinimoX, ObjetoBoundBox.MinimoY), new Ponto4D(ObjetoBoundBox.MaximoX, ObjetoBoundBox.MaximoY));
            RetanguloBoundBox.ObjetoCor.CorR = 214; RetanguloBoundBox.ObjetoCor.CorG = 177; RetanguloBoundBox.ObjetoCor.CorB = 217;
            objetosLista.Add(RetanguloBoundBox);

            CirculoBoundBox = new Circulo("B", null, ObjetoBoundBox.CentroBoundBox, ObjetoBoundBox.RaioBoundBox);
            CirculoBoundBox.PrimitivaTipo = PrimitiveType.LineLoop;
            CirculoBoundBox.ObjetoCor.CorR = 0; CirculoBoundBox.ObjetoCor.CorG = 0; CirculoBoundBox.ObjetoCor.CorB = 0;
            objetosLista.Add(CirculoBoundBox);

            CirculoConfinado = new Circulo("C", null, ObjetoBoundBox.CentroBoundBox, 50);
            CirculoConfinado.PrimitivaTipo = PrimitiveType.LineLoop;
            CirculoConfinado.ObjetoCor.CorR = 0; CirculoConfinado.ObjetoCor.CorG = 0; CirculoConfinado.ObjetoCor.CorB = 0;
            objetosLista.Add(CirculoConfinado);
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
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!IsMouseDown)
            {
                return;
            }

            var difXMouse = -PosicaoInicialMouse.X + e.Position.X;
            var difYMouse = PosicaoInicialMouse.Y - e.Position.Y;

            var xNovo = difXMouse + PosicaoInicialCirculo.X;
            var yNovo = difYMouse + PosicaoInicialCirculo.Y;
            var pontoNovo = new Ponto4D(xNovo, yNovo);

            if (CirculoConfinado != null && ObjetoBoundBox.IsInBiggerArea(pontoNovo))
            {
                RetanguloBoundBox.ObjetoCor.CorR = 214; RetanguloBoundBox.ObjetoCor.CorG = 177; RetanguloBoundBox.ObjetoCor.CorB = 217;
                CirculoConfinado.PontoCentral = pontoNovo;
            }
            else if (CirculoConfinado != null && ObjetoBoundBox.PermiteMover(pontoNovo))
            {
                RetanguloBoundBox.ObjetoCor.CorR = 252; RetanguloBoundBox.ObjetoCor.CorG = 250; RetanguloBoundBox.ObjetoCor.CorB = 208;
                CirculoConfinado.PontoCentral = pontoNovo;
            }
            else
            {
                RetanguloBoundBox.ObjetoCor.CorR = 202; RetanguloBoundBox.ObjetoCor.CorG = 240; RetanguloBoundBox.ObjetoCor.CorB = 236;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            PosicaoInicialMouse = new Ponto4D(e.Position.X, e.Position.Y);
            PosicaoInicialCirculo = CirculoConfinado.PontoCentral;
            IsMouseDown = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            IsMouseDown = false;
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

    class ProgramBoundBox
    {

        static void Main(string[] args)
        {
            MundoBoundBox window = MundoBoundBox.GetInstance();
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }

    }
}
