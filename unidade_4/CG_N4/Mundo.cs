﻿#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;
using CG_N4;
using CG_N4.damas;

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

        private CameraPerspective camera = new CameraPerspective();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private char objetoId = '@';
        private String menuSelecao = "";
        private char menuEixoSelecao = 'z';
        private float deslocamento = 0;
        private bool bBoxDesenhar = false;

        private DamaController controller;

#if CG_Privado
    private Cilindro obj_Cilindro;
    private Esfera obj_Esfera;
    private Cone obj_Cone;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            objetoId = Utilitario.CharProximo(objetoId);

            var tabuleiro = new Tabuleiro(new Ponto4D(0, 0, 0), 5, 1, objetoId, null);
            objetosLista.Add(tabuleiro);
            objetoSelecionado = tabuleiro;

            controller = new DamaController(tabuleiro);
            objetosLista.Add(controller.Seletor);

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(camera.Fovy, Width / (float)Height, camera.Near, camera.Far);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.LookAt(camera.Eye, camera.At, camera.Up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
#if CG_Gizmo
            Sru3D();
#endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            // Console.Clear(); //TODO: não funciona.
            if (e.Key == Key.H) Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape) Exit();
            //--------------------------------------------------------------
            else if (e.Key == Key.Number9)
                objetoSelecionado = null;                     // desmacar objeto selecionado
            else if (e.Key == Key.B)
                bBoxDesenhar = !bBoxDesenhar;     //FIXME: bBox não está sendo atualizada.
            else if (e.Key == Key.E)
            {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                {
                    Console.WriteLine(objetosLista[i]);
                }
            }
            //--------------------------------------------------------------
            else if (e.Key == Key.X) menuEixoSelecao = 'x';
            else if (e.Key == Key.Y) menuEixoSelecao = 'y';
            else if (e.Key == Key.Z) menuEixoSelecao = 'z';
            else if (e.Key == Key.Minus) deslocamento--;
            else if (e.Key == Key.Plus) deslocamento++;
            else if (e.Key == Key.C) menuSelecao = "[menu] C: Câmera";
            else if (e.Key == Key.O) menuSelecao = "[menu] O: Objeto";
            else if (controller != null && controller.HandleEntrada(e.Key)) { }
            // Menu: seleção
            else if (menuSelecao.Equals("[menu] C: Câmera")) camera.MenuTecla(e.Key, menuEixoSelecao, deslocamento);
            else if (menuSelecao.Equals("[menu] O: Objeto"))
            {
                if (objetoSelecionado != null) objetoSelecionado.MenuTecla(e.Key, menuEixoSelecao, deslocamento, bBoxDesenhar);
                else Console.WriteLine(" ... Objeto NÃO selecionado.");
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");

            if (!(e.Key == Key.LShift)) //FIXME: não funciona.
                Console.WriteLine("__ " + menuSelecao + "[" + deslocamento + "]");
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
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
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif
    }
    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N4";
            window.Run(1.0 / 60.0);
        }
    }
}
