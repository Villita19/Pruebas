using UnityEngine;

namespace Player
{
    public class PlayerMoverHorizontal : MonoBehaviour
    {
        // REFERENCIAS
        PlayerEntradasTeclado _PlayerEntradasTeclado;
        PlayerControlMecanicas _PlayerControlMecanicas;
        Rigidbody2D _Rigidbody2D;

        // CAMPOS
        [SerializeField] float velocidad = 8f;

        private void Awake()
        {
            _PlayerEntradasTeclado = GetComponent<PlayerEntradasTeclado>();
            _PlayerControlMecanicas = GetComponent<PlayerControlMecanicas>();
            _Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        void FixedUpdate()
        {
            if (_PlayerControlMecanicas.PuedeAndar()) MoverEnHorizontal();
        }
        void MoverEnHorizontal()
        {
            // si no esta pulsada ninguna de las dos teclas de direccion pone la velocidad en X a 0
            if (!_PlayerEntradasTeclado.TeclaIzquierda_PM() && !_PlayerEntradasTeclado.TeclaDerecha_PM())
            {
                _Rigidbody2D.velocity = new Vector2(0, _Rigidbody2D.velocity.y);
            }
            else // pulsada una de las dos direcciones
            {
                if (_PlayerEntradasTeclado.TeclaDerecha_PM())
                {
                    _Rigidbody2D.velocity = new Vector2(velocidad, _Rigidbody2D.velocity.y);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    _Rigidbody2D.velocity = new Vector2(-velocidad, _Rigidbody2D.velocity.y);
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
    }
}