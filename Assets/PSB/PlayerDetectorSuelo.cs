using UnityEngine;

namespace Player
{
    public class PlayerDetectorSuelo : MonoBehaviour
    {
        // REFERENCIAS EDITABLES
        [SerializeField] LayerMask capaSuelo;

        // CAMPOS EDITABLES
       [SerializeField] float distanciaControladorSuelo = 0.08f;
        [SerializeField] Vector3 dimensionesControlador = new Vector3(0.5f, 0.16f, 0);

        // CAMPOS INTERNOS
        bool enSuelo;

        void Update()
        {
            enSuelo = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - distanciaControladorSuelo), dimensionesControlador, 0f, capaSuelo);
        }
        public bool PlayerTocandoSuelo() => enSuelo;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - distanciaControladorSuelo), dimensionesControlador);
        }
    }
}