using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panel de Instrucciones")]
    [SerializeField] private GameObject instruccionesPanel;

    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    public void Instrucciones()
    {
        if (instruccionesPanel != null)
            instruccionesPanel.SetActive(true);
    }

    public void CerrarInstrucciones()
    {
        if (instruccionesPanel != null)
            instruccionesPanel.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


