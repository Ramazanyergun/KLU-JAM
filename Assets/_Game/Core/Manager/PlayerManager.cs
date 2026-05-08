using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager m_inputManager;
    private PlayerMovement m_playerMovement;
    void Awake()
    {
        m_inputManager = GetComponent<InputManager>();

        m_playerMovement = GetComponent<PlayerMovement>();

    }

    void Update()
    {
        m_inputManager.HandleAllInputs();
    }
    void FixedUpdate()
    {
        if (m_playerMovement != null)
            m_playerMovement.HandleAllMovements();
    }

}
