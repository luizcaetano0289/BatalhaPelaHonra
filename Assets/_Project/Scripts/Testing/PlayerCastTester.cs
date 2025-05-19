using UnityEngine;

[RequireComponent(typeof(CastController))]
public class PlayerCastTester : MonoBehaviour
{
    private CastController castController;

    private void Start()
    {
        castController = GetComponent<CastController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!castController.IsCasting)
            {
                castController.StartCast("Magia de Teste", 3f); // 3 segundos de cast
                Debug.Log("Iniciando cast de 3 segundos...");
            }
        }
    }
}
