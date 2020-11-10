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
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
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

        private RetanguloCorDiferente obj_Retangulo;

        private SegReta SrPalito;
        private int RaioSrPalito = 100;
        private int AnguloSrPalito = 45;

        private List<PrimitiveType> listaTipos = new List<PrimitiveType>()
        {
            PrimitiveType.Points,
            PrimitiveType.Lines,
            PrimitiveType.LineLoop,
            PrimitiveType.LineStrip,
            PrimitiveType.Triangles,
            PrimitiveType.TriangleStrip,
            PrimitiveType.TriangleFan,
            PrimitiveType.Quads,
            PrimitiveType.QuadStrip,
            PrimitiveType.Polygon
        };
        private int indexListaTipos = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -300; camera.xmax = 300; camera.ymin = -300; camera.ymax = 300;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            GerarSrPalito();

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

        private void GerarUmCirculo()
        {
            var circulo = new Circulo("A", null, new Ponto4D(0, 0, 0), 100);
            circulo.ObjetoCor.CorR = 0; circulo.ObjetoCor.CorG = 0; circulo.ObjetoCor.CorB = 0;
            circulo.PrimitivaTamanho = 5;
            objetosLista.Add(circulo);
        }

        private void GerarTrianguloComCirculos()
        {

            var ponto1 = new Ponto4D(0, 100, 0);
            var ponto2 = new Ponto4D(-100, -100, 0);
            var ponto3 = new Ponto4D(100, -100, 0);

            var segReta1 = new SegReta("A", null, ponto1, ponto2);
            segReta1.PrimitivaTamanho = 5;
            segReta1.ObjetoCor.CorR = 118; segReta1.ObjetoCor.CorG = 249; segReta1.ObjetoCor.CorB = 251;
            objetosLista.Add(segReta1);

            var segReta2 = new SegReta("B", null, ponto2, ponto3);
            segReta2.PrimitivaTamanho = 5;
            segReta2.ObjetoCor.CorR = 118; segReta2.ObjetoCor.CorG = 249; segReta2.ObjetoCor.CorB = 251;
            objetosLista.Add(segReta2);

            var segReta3 = new SegReta("C", null, ponto1, ponto3);
            segReta3.PrimitivaTamanho = 5;
            segReta3.ObjetoCor.CorR = 118; segReta3.ObjetoCor.CorG = 249; segReta3.ObjetoCor.CorB = 251;
            objetosLista.Add(segReta3);

            var circulo1 = new Circulo("D", null, ponto1, 100);
            circulo1.ObjetoCor.CorR = 0; circulo1.ObjetoCor.CorG = 0; circulo1.ObjetoCor.CorB = 0;
            circulo1.PrimitivaTamanho = 5;
            objetosLista.Add(circulo1);

            var circulo2 = new Circulo("E", null, ponto2, 100);
            circulo2.ObjetoCor.CorR = 0; circulo2.ObjetoCor.CorG = 0; circulo2.ObjetoCor.CorB = 0;
            circulo2.PrimitivaTamanho = 5;
            objetosLista.Add(circulo2);

            var circulo3 = new Circulo("F", null, ponto3, 100);
            circulo3.ObjetoCor.CorR = 0; circulo3.ObjetoCor.CorG = 0; circulo3.ObjetoCor.CorB = 0;
            circulo3.PrimitivaTamanho = 5;
            objetosLista.Add(circulo3);
        }

        private void GerarRetanguloTrocaForma()
        {
            obj_Retangulo = new RetanguloCorDiferente("A", null, new Ponto4D(-200, -200, 0), new Ponto4D(200, 200, 0));
            obj_Retangulo.PrimitivaTamanho = 5;
            obj_Retangulo.PrimitivaTipo = PrimitiveType.Points;
            obj_Retangulo.ObjetoCor.CorR = 118; obj_Retangulo.ObjetoCor.CorG = 249; obj_Retangulo.ObjetoCor.CorB = 251;
            objetosLista.Add(obj_Retangulo);
            objetoSelecionado = obj_Retangulo;
        }

        private void GerarSrPalito()
        {
            var pontoCentral = new Ponto4D(0, 0, 0);
            var pontoMatematico = SegundoPontoAngular(pontoCentral, AnguloSrPalito, RaioSrPalito);
            SrPalito = new SegReta("A", null, pontoCentral, pontoMatematico);
            SrPalito.ObjetoCor.CorR = 0; SrPalito.ObjetoCor.CorG = 0; SrPalito.ObjetoCor.CorB = 0;
            SrPalito.PrimitivaTamanho = 5;
            objetosLista.Add(SrPalito);
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
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;   //TODO: falta atualizar a BBox do objeto
            else if (e.Key == Key.E)
                PanLeft();
            else if (e.Key == Key.D)
                PanRight();
            else if (e.Key == Key.C)
                PanUp();
            else if (e.Key == Key.B)
                PanDown();
            else if (e.Key == Key.I)
                ZoomIn();
            else if (e.Key == Key.O)
                ZoomOut();
            else if (e.Key == Key.Q)
                MoverSrPalitoEsquerda();
            else if (e.Key == Key.W)
                MoverSrPalitoDireita();
            else if (e.Key == Key.A)
                DiminuirSrPalito();
            else if (e.Key == Key.S)
                AumentarSrPalito();
            else if (e.Key == Key.Z)
                DiminuirAnguloSrPalito();
            else if (e.Key == Key.X)
                AumentarAnguloSrPalito();
            else if (e.Key == Key.Space)
                CicleObject();
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }

        private void ZoomIn()
        {
            if (camera.xmin + 1 < camera.xmax && camera.ymin + 1 < camera.xmax)
            {
                camera.xmin++;
                camera.xmax--;
                camera.ymin++;
                camera.ymax--;
            }
        }

        private void ZoomOut()
        {
            camera.xmin--;
            camera.xmax++;
            camera.ymin--;
            camera.ymax++;
        }

        private void PanLeft()
        {
            camera.xmin--;
            camera.xmax--;
        }

        private void PanRight()
        {
            camera.xmin++;
            camera.xmax++;
        }

        private void PanUp()
        {
            camera.ymin++;
            camera.ymax++;
        }

        private void PanDown()
        {
            camera.ymin--;
            camera.ymax--;
        }

        private void MoverSrPalitoEsquerda()
        {
            if (SrPalito == null)
                return;

            SrPalito.Ponto1.X--;
            SrPalito.Ponto2.X--;
        }

        private void MoverSrPalitoDireita()
        {
            if (SrPalito == null)
                return;

            SrPalito.Ponto1.X++;
            SrPalito.Ponto2.X++;
        }

        private void AumentarSrPalito()
        {
            if (SrPalito == null)
                return;

            RaioSrPalito++;
            var pontoMatematico = SegundoPontoAngular(SrPalito.Ponto1, AnguloSrPalito, RaioSrPalito);
            SrPalito.Ponto2.X = pontoMatematico.X;
            SrPalito.Ponto2.Y = pontoMatematico.Y;
        }

        private void DiminuirSrPalito()
        {
            if (SrPalito == null || RaioSrPalito < 2)
                return;

            RaioSrPalito--;
            var pontoMatematico = SegundoPontoAngular(SrPalito.Ponto1, AnguloSrPalito, RaioSrPalito);
            SrPalito.Ponto2.X = pontoMatematico.X;
            SrPalito.Ponto2.Y = pontoMatematico.Y;
        }

        private void DiminuirAnguloSrPalito()
        {
            if (SrPalito == null)
                return;

            AnguloSrPalito--;
            var pontoMatematico = SegundoPontoAngular(SrPalito.Ponto1, AnguloSrPalito, RaioSrPalito);
            SrPalito.Ponto2.X = pontoMatematico.X;
            SrPalito.Ponto2.Y = pontoMatematico.Y;
        }

        private void AumentarAnguloSrPalito()
        {
            if (SrPalito == null)
                return;

            AnguloSrPalito++;
            var pontoMatematico = SegundoPontoAngular(SrPalito.Ponto1, AnguloSrPalito, RaioSrPalito);
            SrPalito.Ponto2.X = pontoMatematico.X;
            SrPalito.Ponto2.Y = pontoMatematico.Y;
        }

        private void CicleObject()
        {
            if (obj_Retangulo == null)
                return;

            if (indexListaTipos < listaTipos.Count - 1)
            {
                indexListaTipos++;
            }
            else
            {
                indexListaTipos = 0;
            }

            var tipo = listaTipos[indexListaTipos];

            obj_Retangulo.PrimitivaTipo = tipo;
        }

        private Ponto4D SegundoPontoAngular(Ponto4D ponto, int angulo, int raio)
        {
            var pontoMatematico = Matematica.GerarPtosCirculo(angulo, raio);
            return new Ponto4D(pontoMatematico.X + ponto.X, pontoMatematico.Y + ponto.Y, 0);
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

    class Program
    {
        /*
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
        */
    }
}
