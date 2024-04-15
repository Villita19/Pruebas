 using UnityEngine;

namespace Player
{
    public class PlayerSaltar : MonoBehaviour
    {
        #region DECLARACIONES
        // REFERENCIAS AUTOMATICAS
        PlayerEntradasTeclado _PlayerEntradasTeclado;
        PlayerControlMecanicas _PlayerControlMecanicas;
        PlayerDetectorSuelo _PlayerDetectorSuelo;
        Rigidbody2D _Rigidbody2D;
        //---------------------------------------------------------------------------------------
        [Header("== SALTO ==")]
        // CAMPOS EDITABLES
        [SerializeField] float FuerzaSaltoSuelo = 10;
        [SerializeField] float FuerzaSaltoAire = 10;
        // CAMPOS INTERNOS
        bool activoSaltoAire;
        //---------------------------------------------------------------------------------------
        [Header("== AYUDA COYOTE TIME ==")]
        // CAMPOS EDITABLES
        [SerializeField] float tiempoCoyote = 0.1f;
        // CAMPOS INTERNOS
        float contadorCoyote;
        //---------------------------------------------------------------------------------------
        [Header("== AYUDA BUFFER TECLA SALTO ==")]
        // CAMPOS EDITABLES
        [SerializeField] float bufferSalto = 0.05f;
        // CAMPOS INTERNOS
        float contadorBufferTeclaSalto;
        //---------------------------------------------------------------------------------------
        [Header("== AYUDA SALTO CABEZA ESQUINAS ==")]
        // REFERENCIAS EDITABLES
        [SerializeField] LayerMask capaSuelo;
        // CAMPOS EDITABLES
        [SerializeField] float rayCastFuerzaDesplazamiento;
        [SerializeField] float rayCastAlturaDeteccion;
        [SerializeField] float rayCastOffsetX;
        [SerializeField] float rayCastOffsetY;
        // CAMPOS INTERNOS
        bool rayCastDerechaToca;
        bool rayCastIzquierdaToca;
        #endregion

        private void Awake()
        {
            _PlayerEntradasTeclado = GetComponent<PlayerEntradasTeclado>();
            _PlayerControlMecanicas = GetComponent<PlayerControlMecanicas>();
            _PlayerDetectorSuelo = GetComponent<PlayerDetectorSuelo>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            // inicializar coyote time
            contadorCoyote = tiempoCoyote;
            // inicializar el contador del buffer
            contadorBufferTeclaSalto = 0f;
        }
        void Update()
        {
            // si tiene activo el salto desde el suelo puede saltar
            // (si tiene activo el salto en el aire pero no el salto en el suelo no puede saltar)
            if (_PlayerControlMecanicas.PuedeSaltoSuelo()) Saltar();

            // si puede cortar el salto y la tecla de salto no esta pulsada y la velocidad en Y es mayor que cero...
            if (_PlayerControlMecanicas.PuedeCortarSalto() && _PlayerEntradasTeclado.TeclaSalto_S() && _Rigidbody2D.velocity.y > 0) CortarSalto();
        }
        void Saltar()
        {
            // AYUDAS INVISIBLES
            CoyoteTime(); // AYUDA COYOTE TIME            
            BufferTeclaSalto(); // AYUDA BUFFER TECLA SALTO         
            EmpujeCabezaEsquinas();
            
            // SALTAR
            // si esta dentro del buffer de tecla de salto
            if (contadorBufferTeclaSalto > 0f)
            {
                // si esta dentro del coyote time
                if (contadorCoyote > 0)
                {
                    // SALTO DESDE SUELO
                    _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, FuerzaSaltoSuelo);
                    print("estoysaltando");

                    // reinicia el contador del buffer de tecla de salto
                    contadorBufferTeclaSalto = 0f;
                }
                else if (activoSaltoAire && _PlayerControlMecanicas.PuedeSaltoAire())
                {
                    // SALTO DESDE EL AIRE
                    _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, FuerzaSaltoAire);
                    // desactiva el salto en el aire
                    //activoSaltoAire = false;
                }
            }
        }
        void CortarSalto()
        {
            // pone la velocidad del eje Y a cero
            _Rigidbody2D.velocity = new Vector2(_Rigidbody2D.velocity.x, 0);
        }

        #region ayudas invisibles
        void CoyoteTime()
        {
            // si esta tocando el suelo
            if (_PlayerDetectorSuelo.PlayerTocandoSuelo())
            {
                // inicializar coyote time
                contadorCoyote = tiempoCoyote;
                // activar el salto desde el aire
                activoSaltoAire = true;
            }
            // si no esta posado en el suelo
            else
            {
                // descontar tiempo del coyote time
                contadorCoyote -= Time.deltaTime;
            }
        }
        void BufferTeclaSalto()
        {
            // si pulsa la tecla de salto
            if (_PlayerEntradasTeclado.TeclaSalto_P())
            {
                // inicializar el contador del buffer
                contadorBufferTeclaSalto = bufferSalto;
            }
            else
            {
                contadorBufferTeclaSalto -= Time.deltaTime;
                // para evitar desbordamientos de memoria si no se pulsa la tecla en mucho tiempo (tiende a -infinito)
                if (contadorBufferTeclaSalto < 0) contadorBufferTeclaSalto = 0;
            }
        }
        void EmpujeCabezaEsquinas()
        {
            // AYUDA SALTO ESQUINAS
            // empuja al player si toca una esquina cuando casi ya la ha sobrepasado,
            // pero aun no y por fisicas deberia caer hacia abajo.
            // esta ayuda, en vez de dejarle caer, le empuja un poco hacia el lado al que esta mirando, para que esquive la esquina

            // si no esta tocando el suelo 
            if (!_PlayerDetectorSuelo.PlayerTocandoSuelo())
            {
                // lanzar los dos rayos de deteccion
                rayCastDerechaToca = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffsetX, transform.position.y + rayCastOffsetY),
                    Vector2.up, rayCastAlturaDeteccion, capaSuelo);
                rayCastIzquierdaToca = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffsetX, transform.position.y + rayCastOffsetY),
                    Vector2.up, rayCastAlturaDeteccion, capaSuelo);

                // si el rayo de la derecha no detecta nada y el rayo de la izquierda si y el player esta orientado hacia la izquierda, lo desplaza
                if (!rayCastDerechaToca && rayCastIzquierdaToca && transform.eulerAngles.y == 180f)
                {
                    _Rigidbody2D.position = new Vector2(transform.position.x - rayCastFuerzaDesplazamiento, transform.position.y);
                }
                // si el rayo de la izquierda no detecta nada y el rayo de la derecha si y el player esta orientado hacia la derecha, lo desplaza
                else if (rayCastDerechaToca && !rayCastIzquierdaToca && transform.eulerAngles.y == 0f)
                {
                    _Rigidbody2D.position = new Vector2(transform.position.x + rayCastFuerzaDesplazamiento, transform.position.y);
                }
            }
        }
        #endregion

        #region Ayudas visuales en el inspector
        private void OnDrawGizmosSelected()
        {
            // dibujar raycast izquierda cabeza       
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(new Vector2(transform.position.x + rayCastOffsetX, transform.position.y + rayCastOffsetY), Vector2.up * rayCastAlturaDeteccion);
            // dibujar raycast derecha cabeza
            Gizmos.color = Color.green;
            Gizmos.DrawRay(new Vector2(transform.position.x - rayCastOffsetX, transform.position.y + rayCastOffsetY), Vector2.up * rayCastAlturaDeteccion);
        }
        #endregion
    }
}
