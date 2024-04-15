using UnityEngine;

namespace Player
{
    public class PlayerControlMecanicas : MonoBehaviour
    {
        // CAMPOS EDITABLES
        [Header("MECANICAS")]
        [SerializeField] bool mover;
        [SerializeField] bool saltoSuelo;
        [SerializeField] bool saltoAire;
        [SerializeField] bool cortarSalto;
        [SerializeField] bool dash;
        [SerializeField] bool saltoImpulso;

        // METODOS DE ACCESO
        public bool PuedeAndar() => mover;
        public bool PuedeSaltoSuelo() => saltoSuelo;
        public bool PuedeSaltoAire() => saltoAire;
        public bool PuedeCortarSalto() => cortarSalto;
        public bool PuedeDash() => dash;
        public bool PuedeImpulso() => saltoImpulso;
        // POCIMAS
        public void ActivarCortarSalto() { cortarSalto = true; }
        public void ActivarSaltoAire() { saltoAire = true; }
        public void DesactivarSaltoAire() { saltoAire = false; }
    }
}