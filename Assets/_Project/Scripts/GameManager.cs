using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private MainAttackController mainAttackController;
    [SerializeField] private DefenceModeController defenceModeController;

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

    public void BossTime()
    {
        
    }
}