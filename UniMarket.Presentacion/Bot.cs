using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniMarket.Logica;


namespace UniMarket.Presentacion
{
  
    [DesignerCategory("Code")]
    public class BotAyuda : Control
    {
       
        private static readonly Color Morado = Color.FromArgb(109, 63, 214);
        private static readonly Color MoradoClaro = Color.FromArgb(139, 92, 246);
        private static readonly Color Lavanda = Color.FromArgb(237, 231, 251);
        private static readonly Color Fondo = Color.FromArgb(247, 245, 253);
        private static readonly Color Texto = Color.FromArgb(30, 24, 52);

        private const int Tam = 64;                
        private const int AnchoAbierto = 360;
        private const int AltoAbierto = 540;
        private const int MaxTextoBurbuja = 235;

         
        private readonly Panel _ventana;
        private readonly Panel _cabecera;
        private readonly Label _lblTitulo;
        private readonly Label _lblCerrar;
        private readonly FlowLayoutPanel _mensajes;
        private readonly FlowLayoutPanel _sugerencias;
        private readonly TextBox _txt;

        private readonly List<Entrada> _base = new List<Entrada>();
        private bool _abierto;
        private readonly IAsistenteService _asistente = new AsistenteService();
        private string _titulo = "Engel 🦝 - Asistente Trueque";

        [Category("Apariencia")]
        [Description("Titulo que se muestra en la cabecera del chat.")]
        [DefaultValue("Asistente Trueque")]
        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value ?? "";
                if (_lblTitulo != null) _lblTitulo.Text = _titulo;
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(Tam, Tam); }
        }

         
        public BotAyuda()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Size = new Size(Tam, Tam);

            CargarBase();

             
             
            _ventana = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Visible = false };
            _ventana.SizeChanged += (s, e) => RedondearVentana();
 
            _cabecera = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Morado };
            _lblTitulo = new Label
            {
                Text = _titulo,
                ForeColor = Color.White,
                BackColor = Morado,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 8)
            };
            var lblSub = new Label
            {
                Text = "Resuelvo tus dudas sobre la app",
                ForeColor = Color.FromArgb(225, 215, 250),
                BackColor = Morado,
                Font = new Font("Segoe UI", 8.5F),
                AutoSize = true,
                Location = new Point(16, 32)
            };
            _lblCerrar = new Label
            {
                Text = "\u2715",
                ForeColor = Color.White,
                BackColor = Morado,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Cursor = Cursors.Hand,
                Top = 14
            };
            _lblCerrar.Click += (s, e) => Cerrar();
            _cabecera.Resize += (s, e) => { _lblCerrar.Left = _cabecera.Width - _lblCerrar.Width - 14; };
            _cabecera.Controls.Add(_lblTitulo);
            _cabecera.Controls.Add(lblSub);
            _cabecera.Controls.Add(_lblCerrar);

            
            _mensajes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Fondo,
                Padding = new Padding(6, 8, 0, 8)
            };

             
            _sugerencias = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 112,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Fondo,
                Padding = new Padding(10, 6, 6, 0)
            };
            foreach (var entrada in _base.Where(x => x.Sugerida))
                _sugerencias.Controls.Add(CrearChip(entrada.Pregunta));

             
            var entradaPanel = new Panel { Dock = DockStyle.Bottom, Height = 52, BackColor = Color.White, Padding = new Padding(10, 12, 10, 8) };
            _txt = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle };
            _txt.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    EnviarDesdeCaja();
                }
            };
            var separador = new Panel { Dock = DockStyle.Right, Width = 8, BackColor = Color.White };
            var btnEnviar = new Button
            {
                Text = "Enviar",
                Dock = DockStyle.Right,
                Width = 72,
                FlatStyle = FlatStyle.Flat,
                BackColor = Morado,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnEnviar.FlatAppearance.BorderSize = 0;
            btnEnviar.Click += (s, e) => EnviarDesdeCaja();

             
            entradaPanel.Controls.Add(_txt);
            entradaPanel.Controls.Add(separador);
            entradaPanel.Controls.Add(btnEnviar);

            _ventana.Controls.Add(_mensajes);
            _ventana.Controls.Add(_sugerencias);
            _ventana.Controls.Add(entradaPanel);
            _ventana.Controls.Add(_cabecera);

            Controls.Add(_ventana);
            Visible = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode) BringToFront();
        }

         
        public void Abrir()
        {
            if (_abierto) return;
            _abierto = true;

            int alto = AltoAbierto;
            if (Parent != null)
                alto = Math.Max(380, Math.Min(AltoAbierto, Parent.ClientSize.Height - 10));

            Size = new Size(AnchoAbierto, alto);
            if (Parent != null)
                Location = new Point(
                    Math.Max(0, Parent.ClientSize.Width - Width - 12),
                    Math.Max(0, Parent.ClientSize.Height - Height - 12));

            _ventana.Visible = true;
            Visible = true;
            BringToFront();

            if (_mensajes.Controls.Count == 0)
                AgregarMensaje("¡Hola! 🦝 Soy Engel, el mapache, asistente virtual oficial de Trueque. ¿En qué puedo ayudarte?", true);

            _txt.Focus();
        }

        public void Cerrar()
        {
            if (!_abierto) return;
            _abierto = false;
            Visible = false;
        }

        public void Alternar()
        {
            if (_abierto) Cerrar();
            else Abrir();
        }

        private void RedondearVentana()
        {
            if (_ventana.Width < 40 || _ventana.Height < 40) return;

            using (var path = RedondeadoPath(new Rectangle(0, 0, _ventana.Width, _ventana.Height), 16))
            {
                var anterior = _ventana.Region;
                _ventana.Region = new Region(path);
                if (anterior != null) anterior.Dispose();
            }
        }

         
        private void EnviarDesdeCaja()
        {
            string texto = _txt.Text;
            _txt.Clear();
            Enviar(texto);
        }

        private async void Enviar(string texto)
        {
            texto = (texto ?? "").Trim();
            if (texto.Length == 0) return;

            AgregarMensaje(texto, false);
            Control escribiendo = AgregarMensaje("Escribiendo....", true);

            string respuesta;
            try
            {


                if (_asistente.Configurado)
                {
                    respuesta = await _asistente.PreguntarAsync(texto);
                }
                else
                {
                    Entrada hallada = BuscarEntrada(texto);
                    respuesta = hallada != null
                        ? hallada.Respuesta
                        : MensajeNoEntendi;
                }
            }
            catch (Exception)
            {
                respuesta = MensajeNoEntendi;
            }

            if (IsDisposed) return;

            _mensajes.Controls.Remove(escribiendo);
            escribiendo.Dispose();
            AgregarMensaje(respuesta, true);
        }

        private Control AgregarMensaje(string texto, bool delBot)
        {
            int anchoFila = AnchoAbierto - SystemInformation.VerticalScrollBarWidth - 10;

            var burbuja = new Burbuja(texto, delBot, Fondo);
            var fila = new Panel
            {
                Width = anchoFila,
                Height = burbuja.Height + 6,
                BackColor = Fondo,
                Margin = new Padding(0)
            };
            burbuja.Top = 3;
            burbuja.Left = delBot ? 4 : anchoFila - burbuja.Width - 4;
            fila.Controls.Add(burbuja);

            _mensajes.Controls.Add(fila);
            _mensajes.ScrollControlIntoView(fila);
            return fila;
        }

        private Button CrearChip(string texto)
        {
            var chip = new Button
            {
                Text = texto,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Morado,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 6),
                Padding = new Padding(4, 1, 4, 1),
                UseVisualStyleBackColor = false
            };
            chip.FlatAppearance.BorderColor = Morado;
            chip.FlatAppearance.BorderSize = 1;
            chip.Click += (s, e) => Enviar(texto);
            return chip;
        }
         
        private class Entrada
        {
            public string Pregunta;
            public string Respuesta;
            public string[] Claves;
            public bool Sugerida;
        }

        private void Agregar(string pregunta, string respuesta, bool sugerida, params string[] claves)
        {
            _base.Add(new Entrada
            {
                Pregunta = pregunta,
                Respuesta = respuesta,
                Sugerida = sugerida,
                Claves = claves.Select(Normalizar).ToArray()
            });
        }

        private void CargarBase()
        {

            Agregar("¿Cómo publico un producto?",
                "Pulsa «PUBLICAR» en el menú de la izquierda. Escribe el título y las unidades disponibles, y elige la modalidad: " +
                "«Solo venta», «Solo trueque» o «Venta o trueque». Si aceptas venta, pon el precio; si aceptas trueque, escribe qué buscas a cambio. " +
                "Puedes agregar una descripción y una foto. Pulsa «Guardar» y tu producto aparece en Inicio para los demás.",
                true, "publicar", "publico", "vender", "vendo", "subir");

            Agregar("¿Cómo compro un producto?",
                "En Inicio pulsa «Agregar al carrito» en el producto que quieras. Luego abre el carrito (el ícono arriba a la derecha), " +
                "ajusta las cantidades y confirma la compra.",
                true, "comprar", "compro", "carrito", "agregar", "adquirir", "pedir");

            Agregar("¿Qué pasa al confirmar la compra?",
                "Se envía una solicitud a cada vendedor y ellos reciben una notificación. Tu solicitud queda «Pendiente» " +
                "hasta que el vendedor la acepte o la rechace. Puedes ver el estado en «" + BotonSolicitudes + "», pestaña «Enviadas».",
                true, "confirmar", "confirmo", "solicitud", "pendiente", "despues");

            Agregar("¿Cómo acepto o rechazo una solicitud?",
                "Cuando alguien te pide un producto o te propone un trueque, te llega una notificación. Entra a «" + BotonSolicitudes + "», pestaña «Recibidas», " +
                "elige la solicitud pendiente y pulsa «Aceptar» o «Rechazar». Si es un trueque, verás lo que te ofrecen antes de confirmar. " +
                "Al aceptar se descuentan las unidades del stock.",
                true, "aceptar", "acepto", "rechazar", "rechazo");

            Agregar("¿Qué hace la campana?",
                "La campana de arriba te avisa cuando alguien te hace una solicitud de compra o una propuesta de trueque, " +
                "o cuando atienden la tuya. El número rojo indica cuántas notificaciones tienes sin leer.",
                true, "campana", "notificacion", "aviso", "avisa");

            Agregar("¿Qué límites hay al publicar?",
                "El título admite hasta 120 caracteres y la descripción hasta 500. Si aceptas venta, el precio debe ser mayor a cero; " +
                "si aceptas trueque, lo que buscas a cambio admite hasta 300 caracteres. Tienes que publicar con al menos 1 unidad.",
                false, "limite", "maximo", "caracteres", "cuantos", "minimo");

            Agregar("¿Puedo comprar mis propios productos?",
                "No. En tu propia publicación el botón aparece desactivado («Tu publicación»).",
                false, "propia", "propio", "mi publicacion");

            Agregar("¿Por qué no puedo pedir más unidades?",
                "No puedes pedir más unidades de las que hay disponibles. El carrito te avisa cuántas quedan.",
                false, "stock", "mas unidades", "agotado", "disponible");

            Agregar("¿Cómo veo mis compras?",
                "«" + BotonSolicitudes + "» tiene dos pestañas: «Enviadas» (las que pediste tú) y «Recibidas» (las que te piden a ti), " +
                "con su estado: Pendiente, Aceptada o Rechazada.",
                false, "historial", "mis compras", "estado", "ver compras", "mis solicitudes");

            Agregar("¿Cómo contacto al vendedor?",
                "Cuando el vendedor acepta tu solicitud, en «" + BotonSolicitudes + "», pestaña «Enviadas», aparecen su nombre y su teléfono " +
                "para ponerse de acuerdo en la entrega. Antes de que la acepte, no se muestran.",
                false, "contacto", "contactar", "telefono", "comunicar");

            Agregar("¿Cómo cierro sesión?",
                "Pulsa «CERRAR SESIÓN» al final del menú de la izquierda y confirma.",
                false, "cerrar sesion", "salir", "cerrar la sesion");

            Agregar("¿Cómo edito mis datos o mi contraseña?",
                "Entra a «MI CUENTA» y pulsa «Editar». Puedes cambiar tu correo, teléfono, foto y contraseña (el nombre no se puede cambiar). " +
                "Pulsa «Guardar» para aplicar los cambios o «Cancelar» para descartarlos.",
                false, "mi cuenta", "editar cuenta", "cambiar correo", "cambiar telefono", "contrasena", "foto de perfil", "mis datos");

            Agregar("¿Cómo desactivo mi cuenta?",
                "En «MI CUENTA», al final, pulsa «Desactivar cuenta». Te pedirá confirmación y tu contraseña. " +
                "Se desactivan tu cuenta y tus publicaciones, se rechazan tus solicitudes pendientes y se cierra tu sesión.",
                false, "desactivar", "eliminar cuenta", "borrar cuenta", "dar de baja");

            Agregar("¿Cómo propongo un trueque?",
                "Abre la publicación (haz clic en ella) y pulsa «Proponer trueque». Elige cuántas unidades quieres y escribe qué ofreces a cambio " +
                "(mínimo 10 caracteres). El vendedor recibe una notificación y puede aceptar o rechazar. Los trueques no pasan por el carrito.",
                true, "proponer", "propongo", "ofrecer", "ofrezco", "propuesta", "trueque");

            Agregar("¿Cómo publico algo para hacer trueque?",
                "En «PUBLICAR» elige la modalidad «Solo trueque» o «Venta o trueque» y escribe qué buscas a cambio. " +
                "Si es solo trueque no lleva precio; en el catálogo aparecerá como «Solo trueque».",
                false, "publico un trueque", "publicar trueque", "publicar para trueque", "modalidad", "a cambio", "intercambiar", "intercambio", "trueque");

            Agregar("¿Qué pasa cuando aceptan mi trueque?",
                "Te llega una notificación. Se descuentan las unidades de la publicación y, en «" + BotonSolicitudes + "», pestaña «Enviadas», " +
                "aparecen el nombre y el teléfono del vendedor (puedes tocar el teléfono para abrir WhatsApp). " +
                "El intercambio lo coordinan entre ustedes dos, fuera de la aplicación. Tu cuenta suma un «Trueque» en «MI CUENTA».",
                false, "aceptan mi trueque", "acepto mi trueque", "aceptaron mi trueque", "trueque aceptado", "trueque");

            Agregar("¿Puedo pagar y hacer trueque en la misma solicitud?",
                "No. Cuando una publicación es «Venta o trueque», cada solicitud es por dinero o por trueque, no las dos a la vez. " +
                "Además, solo puedes tener una solicitud pendiente por publicación.",
                false, "las dos", "ambas", "a la vez", "dinero y trueque", "pagar y", "mismo tiempo", "trueque");
        }

        // Nombre del botón del menú donde se ven las solicitudes (un solo lugar para cambiarlo)
        private const string BotonSolicitudes = "MIS SOLICITUDES";
        private const string MensajeNoEntendi =
               "¡Ups! 🦝 No pude responder tu pregunta en este momento. " +
    "Puedes intentar nuevamente o elegir una de las preguntas sugeridas. "
;

        private Entrada BuscarEntrada(string texto)
        {
            string norm = Normalizar(texto);
            Entrada mejor = null;
            int mejorPuntos = 0;

            foreach (var entrada in _base)
            {
                int puntos = 0;
                if (Normalizar(entrada.Pregunta) == norm)
                {
                    puntos = 100;
                }
                else
                {
                    foreach (string clave in entrada.Claves)
                        if (norm.Contains(clave)) puntos++;
                }

                if (puntos > mejorPuntos)
                {
                    mejor = entrada;
                    mejorPuntos = puntos;
                }
            }

            return mejor;
        }

        private static string Normalizar(string s)
        {
            string descompuesto = (s ?? "").ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in descompuesto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString();
        }

         
        private static GraphicsPath RedondeadoPath(Rectangle r, int radio)
        {
            var path = new GraphicsPath();
            int d = Math.Min(radio * 2, Math.Min(r.Width, r.Height));
            if (d <= 0)
            {
                path.AddRectangle(r);
                return path;
            }

            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        
        private class Burbuja : Control
        {
            private readonly bool _delBot;
            private readonly Color _fondoDetras;
            private static readonly TextFormatFlags Flags =
                TextFormatFlags.WordBreak | TextFormatFlags.NoPadding | TextFormatFlags.Left | TextFormatFlags.NoPrefix;

            public Burbuja(string texto, bool delBot, Color fondoDetras)
            {
                SetStyle(ControlStyles.UserPaint |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

                Text = texto;
                _delBot = delBot;
                _fondoDetras = fondoDetras;
                Font = new Font("Segoe UI", 9.5F);

                Size medido = TextRenderer.MeasureText(texto, Font, new Size(MaxTextoBurbuja, 0), Flags);
                Size = new Size(medido.Width + 30, medido.Height + 20);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(_fondoDetras);

                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (var path = RedondeadoPath(rect, 14))
                using (var brush = new SolidBrush(_delBot ? Lavanda : Morado))
                {
                    g.FillPath(brush, path);
                }

                var zonaTexto = new Rectangle(14, 10, Width - 28, Height - 20);
                TextRenderer.DrawText(g, Text, Font, zonaTexto, _delBot ? Texto : Color.White, Flags);
            }
        }
         

    }
}
