using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private MainAttackController mainAttackController;
    [SerializeField] private DefenceModeController defenceModeController;

    [SerializeField] private UnityEvent onPlayerDeath;
    

    private void Start()
    {
        StartAttack();
    }
    
    public void StartAttack()
    {
        playerInput.IsInDefenceMode = true;
        mainAttackController.IsAttackingStage = true;
        mainAttackController.StartAttack();
    }

    public void StartDefence()
    {
        playerInput.IsInDefenceMode = false;
        mainAttackController.IsAttackingStage = false;
        defenceModeController.StartDefence();
    }

    public void PlayerDead()
    {
        onPlayerDeath?.Invoke();
    }

    public void BossTime()
    {
        
    }
}