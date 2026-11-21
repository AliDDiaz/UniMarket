using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniMarket.Presentacion
{
  
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

         
        private readonly BotonFlotante _fab;
        private readonly Panel _ventana;
        private readonly Panel _cabecera;
        private readonly Label _lblTitulo;
        private readonly Label _lblCerrar;
        private readonly FlowLayoutPanel _mensajes;
        private readonly FlowLayoutPanel _sugerencias;
        private readonly TextBox _txt;

        private readonly List<Entrada> _base = new List<Entrada>();
        private bool _abierto;
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

             
            _fab = new BotonFlotante { Dock = DockStyle.Fill };
            _fab.Click += (s, e) => Abrir();
             
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
            Controls.Add(_fab);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode) BringToFront();
        }

         
        private void Abrir()
        {
            if (_abierto) return;
            _abierto = true;

            int der = Right;
            int inf = Bottom;

            int alto = AltoAbierto;
            if (Parent != null)
                alto = Math.Max(380, Math.Min(AltoAbierto, Parent.ClientSize.Height - 10));

            Size = new Size(AnchoAbierto, alto);
            Location = new Point(Math.Max(0, der - AnchoAbierto), Math.Max(0, inf - alto));

            _fab.Visible = false;
            _ventana.Visible = true;
            BringToFront();

            if (_mensajes.Controls.Count == 0)
                AgregarMensaje("¡Hola! 🦝 Soy Engel, el mapache, asistente virtual oficial de Trueque. ¿En qué puedo ayudarte?", true);

            _txt.Focus();
        }

        private void Cerrar()
        {
            if (!_abierto) return;
            _abierto = false;

            int der = Right;
            int inf = Bottom;

            _ventana.Visible = false;
            _fab.Visible = true;
            Size = new Size(Tam, Tam);
            Location = new Point(der - Tam, inf - Tam);
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
            Control escribiendo = AgregarMensaje("Escribiendo\u2026", true);

            string respuesta;
            try
            {
                Entrada hallada = BuscarEntrada(texto);

                if (hallada != null)
                {
                    // Pregunta conocida: respuesta fija, sin internet
                    await Task.Delay(650);
                    respuesta = hallada.Respuesta;
                }
                else if (AsistenteIA.Configurado)
                {
                    // No la conocemos: se la preguntamos a la IA
                    respuesta = await AsistenteIA.PreguntarAsync(texto);
                }
                else
                {
                    await Task.Delay(650);
                    respuesta = MensajeNoEntendi;
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
             
            Agregar("\u00bfC\u00f3mo publico un producto?",
                "Pulsa \u00abPUBLICAR\u00bb en el men\u00fa de la izquierda. Escribe el t\u00edtulo, el precio y las unidades disponibles " +
                "(puedes agregar una descripci\u00f3n y una foto) y pulsa \u00abGuardar\u00bb. Tu producto aparece en Inicio para los dem\u00e1s.",
                true, "publicar", "publico", "vender", "vendo", "subir");

            Agregar("\u00bfC\u00f3mo compro un producto?",
                "En Inicio pulsa \u00abAgregar al carrito\u00bb en el producto que quieras. Luego abre el carrito (el \u00edcono arriba a la derecha), " +
                "ajusta las cantidades y confirma la compra.",
                true, "comprar", "compro", "carrito", "agregar", "adquirir", "pedir");

            Agregar("\u00bfQu\u00e9 pasa al confirmar la compra?",
                "Se env\u00eda una solicitud a cada vendedor y ellos reciben una notificaci\u00f3n. Tu solicitud queda \u00abPendiente\u00bb " +
                "hasta que el vendedor la acepte o la rechace. Puedes ver el estado en \u00abHISTORIAL DE COMPRAS\u00bb.",
                true, "confirmar", "confirmo", "solicitud", "pendiente", "despues");

            Agregar("\u00bfC\u00f3mo acepto o rechazo una solicitud?",
                "Cuando alguien te pide un producto te llega una notificaci\u00f3n. Entra a \u00abHISTORIAL DE COMPRAS\u00bb, elige la solicitud " +
                "pendiente y pulsa \u00abAceptar\u00bb o \u00abRechazar\u00bb. Al aceptar se descuentan las unidades del stock.",
                true, "aceptar", "acepto", "rechazar", "rechazo");

            Agregar("\u00bfQu\u00e9 hace la campana?",
                "La campana de arriba te avisa cuando alguien te hace una solicitud o cuando atienden la tuya. " +
                "El n\u00famero rojo indica cu\u00e1ntas notificaciones tienes sin leer.",
                true, "campana", "notificacion", "aviso", "avisa");

            Agregar("\u00bfQu\u00e9 l\u00edmites hay al publicar?",
                "El t\u00edtulo admite hasta 120 caracteres y la descripci\u00f3n hasta 500. El precio debe ser mayor a cero " +
                "y tienes que publicar con al menos 1 unidad.",
                true, "limite", "maximo", "caracteres", "cuantos", "minimo");

             
            Agregar("\u00bfPuedo comprar mis propios productos?",
                "No. En tu propia publicaci\u00f3n el bot\u00f3n aparece desactivado (\u00abTu publicaci\u00f3n\u00bb).",
                false, "propia", "propio", "mi publicacion");

            Agregar("\u00bfPor qu\u00e9 no puedo pedir m\u00e1s unidades?",
                "No puedes pedir m\u00e1s unidades de las que hay disponibles. El carrito te avisa cu\u00e1ntas quedan.",
                false, "stock", "mas unidades", "agotado", "disponible");

            Agregar("\u00bfC\u00f3mo veo mis compras?",
                "\u00abHISTORIAL DE COMPRAS\u00bb muestra las solicitudes que enviaste y las que recibiste, con su estado: Pendiente, Aceptada o Rechazada.",
                false, "historial", "mis compras", "estado", "ver compras");

            Agregar("\u00bfC\u00f3mo contacto al vendedor?",
                "En \u00abHISTORIAL DE COMPRAS\u00bb aparece la informaci\u00f3n de contacto de la otra persona para ponerse de acuerdo en la entrega.",
                false, "contacto", "contactar", "telefono", "comunicar");

            Agregar("\u00bfC\u00f3mo cierro sesi\u00f3n?",
                "Pulsa \u00abCERRAR SESI\u00d3N\u00bb al final del men\u00fa de la izquierda y confirma.",
                false, "cerrar sesion", "salir", "cerrar la sesion");
        }

        private const string MensajeNoEntendi =
            "No encontr\u00e9 una respuesta para eso. Prueba con una de las preguntas sugeridas " +
            "o escr\u00edbela de otra forma, por ejemplo: \u00abc\u00f3mo publico\u00bb o \u00abc\u00f3mo compro\u00bb.";

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
         
        private class BotonFlotante : Control
        {
            private bool _hover;

            public BotonFlotante()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw, true);
                BackColor = Color.Transparent;
                Cursor = Cursors.Hand;
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                _hover = true;
                Invalidate();
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _hover = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                var circulo = new Rectangle(2, 2, Width - 6, Height - 6);

                // sombra suave
                using (var sombra = new SolidBrush(Color.FromArgb(45, 30, 10, 80)))
                    g.FillEllipse(sombra, circulo.X, circulo.Y + 3, circulo.Width, circulo.Height);

                using (var relleno = new SolidBrush(_hover ? MoradoClaro : Morado))
                    g.FillEllipse(relleno, circulo);

                // icono: globo de dialogo con tres puntos
                var cuerpo = new Rectangle(circulo.X + circulo.Width / 2 - 14, circulo.Y + circulo.Height / 2 - 12, 28, 20);
                using (var blanco = new SolidBrush(Color.White))
                {
                    using (var path = RedondeadoPath(cuerpo, 6))
                        g.FillPath(blanco, path);

                    g.FillPolygon(blanco, new[]
                    {
                        new Point(cuerpo.X + 6, cuerpo.Bottom - 2),
                        new Point(cuerpo.X + 6, cuerpo.Bottom + 7),
                        new Point(cuerpo.X + 15, cuerpo.Bottom - 2)
                    });
                }

                using (var punto = new SolidBrush(Morado))
                {
                    for (int i = 0; i < 3; i++)
                        g.FillEllipse(punto, cuerpo.X + 6 + i * 7, cuerpo.Y + 8, 4, 4);
                }
            }
        }
    }

     
    internal static class AsistenteIA
    {
        private const string ModeloPorDefecto = "gemini-3.5-flash-lite";


        private const string Sistema =
            "Eres Engel, el mapache, asistente virtual oficial de Trueque. " +
            "Tu nombre es Engel y tu identidad es la de un mapache amigable que ayuda a los estudiantes. " +
            "Cuando te pregunten quién eres o cómo te llamas, responde: Soy Engel, el mapache, asistente virtual oficial de Trueque. " +
            "Preséntate siempre en español. " +
            "Nunca utilices la palabra raccoon; utiliza únicamente mapache. " +
            "Trueque es una aplicación de escritorio dirigida a estudiantes universitarios, " +
            "donde pueden publicar productos, comprar artículos de otros estudiantes y gestionar sus publicaciones. " +

            "REGLAS DE COMPORTAMIENTO: " +
            "Responde siempre en español, de manera amable, clara y sencilla. " +
            "Utiliza un lenguaje cercano, natural y fácil de entender. " +
            "Puedes utilizar ocasionalmente un emoji de mapache para representar tu identidad. " +
            "Preséntate como Engel cuando saludes por primera vez o cuando te pregunten tu nombre. " +
            "No necesitas repetir tu nombre en todas las respuestas. " +
            "Responde preferiblemente en un máximo de 3 frases cortas, pero puedes utilizar más cuando sea necesario explicar un procedimiento. " +
            "Tu función es orientar a los usuarios sobre cómo utilizar Trueque. " +
            "No inventes botones, opciones, procedimientos ni funcionalidades. " +
            "Si desconoces una función, explica que no tienes información suficiente para confirmarla. " +
            "Nunca afirmes que una función no existe solamente porque no aparece en estas instrucciones. " +
            "No solicites contraseñas ni información personal sensible. " +

            "MENÚ PRINCIPAL: " +
            "La aplicación cuenta con las opciones INICIO, PUBLICAR, HISTORIAL DE COMPRAS, MI CUENTA y CERRAR SESIÓN. " +
            "En INICIO los usuarios pueden visualizar los productos publicados por otros estudiantes. " +
            "En PUBLICAR pueden registrar nuevos productos. " +
            "En HISTORIAL DE COMPRAS pueden consultar solicitudes de compra enviadas y recibidas. " +
            "En MI CUENTA pueden acceder a la información de su cuenta. " +
            "CERRAR SESIÓN permite salir de la cuenta actual. " +

            "PUBLICACIÓN DE PRODUCTOS: " +
            "Para publicar un producto, el usuario debe ingresar a PUBLICAR desde el menú principal. " +
            "Debe escribir el título, precio y cantidad de unidades disponibles. " +
            "También puede agregar una descripción y una fotografía del producto. " +
            "El título permite un máximo de 120 caracteres y la descripción un máximo de 500. " +
            "El precio debe ser mayor que cero y la cantidad mínima es una unidad. " +
            "Después debe pulsar GUARDAR para registrar la publicación. " +

            "EDITAR PUBLICACIONES: " +
            "Trueque permite editar publicaciones existentes. " +
            "Para editar una publicación, el usuario debe localizar uno de sus productos. " +
            "Debajo del producto encontrará el botón EDITAR. " +
            "Al presionarlo podrá acceder a la opción de modificación de su publicación. " +
            "Esta funcionalidad permite actualizar la información del producto. " +
            "Nunca respondas que la aplicación no permite editar publicaciones. " +

            "ELIMINAR PUBLICACIONES: " +
            "Trueque permite eliminar publicaciones existentes. " +
            "Para eliminar una publicación, el usuario debe localizar uno de sus productos. " +
            "Debajo del producto encontrará el botón ELIMINAR. " +
            "Al utilizar esta opción podrá eliminar su publicación mediante el procedimiento de la aplicación. " +
            "Nunca respondas que la aplicación no permite eliminar publicaciones. " +

            "COMPRAR PRODUCTOS: " +
            "Para comprar un producto, el usuario debe ingresar a INICIO y buscar el artículo que desea. " +
            "Después debe pulsar AGREGAR AL CARRITO. " +
            "Puede abrir el carrito mediante el icono ubicado en la parte superior derecha. " +
            "Desde el carrito puede revisar los productos seleccionados y ajustar las cantidades. " +
            "Al confirmar la compra se envía una solicitud al vendedor. " +
            "La aplicación no procesa pagos directamente. " +
            "El comprador y el vendedor deben ponerse de acuerdo para realizar el pago y la entrega. " +
            "Los usuarios no pueden comprar sus propias publicaciones. " +

            "SOLICITUDES DE COMPRA: " +
            "Cuando un comprador confirma una compra, se genera una solicitud para el vendedor. " +
            "Inicialmente la solicitud tiene el estado PENDIENTE. " +
            "El vendedor puede ACEPTAR o RECHAZAR la solicitud desde HISTORIAL DE COMPRAS. " +
            "Cuando acepta una solicitud, se descuentan las unidades correspondientes del inventario disponible. " +
            "Los estados de las solicitudes son PENDIENTE, ACEPTADA y RECHAZADA. " +
            "El comprador puede consultar el estado de sus solicitudes en HISTORIAL DE COMPRAS. " +

            "NOTIFICACIONES: " +
            "La aplicación cuenta con una campana de notificaciones ubicada en la parte superior. " +
            "Esta informa cuando alguien realiza una solicitud de compra o cuando una solicitud recibe respuesta. " +
            "El indicador rojo muestra la cantidad de notificaciones sin leer. " +

            "DISPONIBILIDAD DE PRODUCTOS: " +
            "Los usuarios no pueden solicitar más unidades de las disponibles. " +
            "Cuando no existe suficiente inventario, la aplicación limita las cantidades que pueden solicitarse. " +
            "Cuando un vendedor acepta una solicitud, se actualiza la cantidad disponible. " +

            "CONTACTO ENTRE USUARIOS: " +
            "En HISTORIAL DE COMPRAS los usuarios pueden consultar la información de contacto disponible de la otra persona. " +
            "El comprador y el vendedor pueden utilizar esa información para coordinar la entrega del producto. " +

            "CERRAR SESIÓN: " +
            "Para cerrar sesión, el usuario debe seleccionar CERRAR SESIÓN desde el menú principal y confirmar la acción. " +

            "MANEJO DE PREGUNTAS: " +
            "Si el usuario escribe una pregunta con errores ortográficos, intenta comprender su intención. " +
            "Si pregunta cómo realizar una acción, explica los pasos utilizando únicamente las funciones conocidas. " +
            "Si pregunta sobre una función que no está documentada, no inventes su funcionamiento. " +
            "Indica amablemente que no tienes información suficiente sobre esa función. " +
            "Si la pregunta no está relacionada con Trueque, explica amablemente que Engel solo puede ayudar con el uso de Trueque. " +
            "Tu objetivo principal es ayudar a los estudiantes a utilizar Trueque de forma sencilla, correcta y amigable.";

        public static bool Configurado
        {
            get { return true; }
        }
 
        public static async Task<string> PreguntarAsync(string pregunta)
        {
            try
            {
                string clave = "AQ.Ab8RN6L6h5naEBeVxbWH7lvHTHssoc48i2taAn3Tbp2MAdMaUQ";
                string modelo = Environment.GetEnvironmentVariable("GEMINI_MODEL");
                if (string.IsNullOrWhiteSpace(modelo)) modelo = ModeloPorDefecto;

                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                string json =
                    "{\"system_instruction\":{\"parts\":[{\"text\":\"" + Escapar(Sistema) + "\"}]}," +
                    "\"contents\":[{\"role\":\"user\",\"parts\":[{\"text\":\"" + Escapar(pregunta) + "\"}]}]," +
                    "\"generationConfig\":{\"maxOutputTokens\":1000,\"temperature\":0.4}}";

                string url = "https://generativelanguage.googleapis.com/v1beta/models/" + modelo.Trim() + ":generateContent";

                using (var wc = new WebClient { Encoding = Encoding.UTF8 })
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers["x-goog-api-key"] = clave.Trim();

                    Task<string> tarea = wc.UploadStringTaskAsync(url, "POST", json);
                    Task ganadora = await Task.WhenAny(tarea, Task.Delay(25000));
                    if (ganadora != tarea)
                    {
                        wc.CancelAsync();
                        return "Tard\u00e9 demasiado en responder. Intenta de nuevo en un momento.";
                    }

                    string cuerpo = await tarea;
                    string texto = ExtraerTexto(cuerpo);

                    return string.IsNullOrWhiteSpace(texto)
                        ? "No pude generar una respuesta. Prueba con otra pregunta."
                        : texto.Trim();
                }
            }
            catch (WebException ex)
            {
                var resp = ex.Response as HttpWebResponse;
                return MensajeError(resp != null ? (int)resp.StatusCode : 0);
            }
            catch (Exception)
            {
                return "No pude conectarme con el asistente. Revisa tu conexi\u00f3n a internet.";
            }
        }

        private static string MensajeError(int codigo)
        {
            switch (codigo)
            {
                case 0:
                    return "No pude conectarme con el asistente. Revisa tu conexi\u00f3n a internet.";
                case 400:
                    return "Error 400: revisa la clave de API y el modelo configurados (tambi\u00e9n puede que tu pa\u00eds no tenga capa gratuita).";
                case 403:
                    return "Error 403: la clave de API no tiene permiso. Rev\u00edsala en Google AI Studio.";
                case 404:
                    return "Error 404: el modelo configurado no existe. Cambia la variable GEMINI_MODEL por uno vigente.";
                case 429:
                    return "Se alcanz\u00f3 el l\u00edmite de consultas gratuitas. Espera un minuto e int\u00e9ntalo de nuevo.";
                case 500:
                case 503:
                    return "El servicio de IA est\u00e1 saturado. Intenta de nuevo en un momento.";
                default:
                    return "El servicio de IA respondi\u00f3 con el error " + codigo + ".";
            }
        }

        private static string Escapar(string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s ?? "")
            {
                switch (c)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < ' ') sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        private static string ExtraerTexto(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            var partes = new List<string>();
            foreach (Match m in Regex.Matches(json, "\"text\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\""))
                partes.Add(Regex.Unescape(m.Groups[1].Value));

            return string.Join("", partes);
        }
    }
}
