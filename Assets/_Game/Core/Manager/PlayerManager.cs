using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager m_inputManager;
    private PlayerMovement m_playerMovement;
    private PlayerCombat m_playerCombat;
    void Awake()
    {
        m_inputManager = GetComponent<InputManager>();

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerCombat = GetComponent<PlayerCombat>();

    }

    void Update()
    {
        m_inputManager.HandleAllInputs();
        m_playerCombat.HandleCombat();

    }
    void FixedUpdate()
    {
        if (m_playerMovement != null)
            m_playerMovement.HandleAllMovements();
    }

}
