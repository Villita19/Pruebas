using UnityEngine;

namespace Player
{
    public class PlayerEntradasTeclado : MonoBehaviour
    {
        // CAMPOS EDITABLES
        [SerializeField] private KeyCode teclaIzquierda;
        [SerializeField] private KeyCode teclaDerecha;
        [SerializeField] private KeyCode teclaSaltar;
        [SerializeField] private KeyCode teclaDash;

        // CAMPOS INTERNOS
        bool teclaIzquierda_PM, teclaDerecha_PM, teclaSalto_P, teclaSalto_S, teclaDash_P;

        public void Update()
        {
            teclaIzquierda_PM = Input.GetKeyDown(teclaIzquierda) || Input.GetKey(teclaIzquierda);
            teclaDerecha_PM = Input.GetKeyDown(teclaDerecha) || Input.GetKey(teclaDerecha);
            teclaSalto_P = Input.GetKeyDown(teclaSaltar);
            teclaSalto_S = Input.GetKeyUp(teclaSaltar);
            teclaDash_P = Input.GetKeyUp(teclaDash);
        }

        // METODOS DE ACCESO  
        public bool TeclaIzquierda_PM() => teclaIzquierda_PM;
        public bool TeclaDerecha_PM() => teclaDerecha_PM;
        public bool TeclaSalto_P() => teclaSalto_P;
        public bool TeclaSalto_S() => teclaSalto_S;
        public bool TeclaDash_P() => teclaDash_P;
    }
}