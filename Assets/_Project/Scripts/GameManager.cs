using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private MainAttackController mainAttackController;
    [SerializeField] private DefenceModeController defenceModeController;
    [SerializeField] private BossAttackController bossAttackController;

    [SerializeField] private UnityEvent onPlayerDeath;
    [SerializeField] private UnityEvent onBossStart;

    private Camera _camera;
    private CameraController _cameraController;
    
    private bool _bossTime;


    private void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }
    
    private void Start()
    {
        _bossTime = false;
        bossAttackController.gameObject.SetActive(false);
        StartAttack();
    }
    
    public void StartAttack()
    {
        if (_bossTime)
        {
            return;
        }
        playerInput.IsInDefenceMode = true;
        mainAttackController.IsAttackingStage = true;
        mainAttackController.StartAttack();
    }

    public void StartDefence()
    {
        if (_bossTime)
        {
            return;
        }
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
        StartCoroutine(StartBossTime());
    }

    private IEnumerator StartBossTime()
    {
        _bossTime = true;
        yield return defenceModeController.LowerRoots();
        _cameraController.ZoomOut();
        bossAttackController.gameObject.SetActive(true);
        onBossStart?.Invoke();
    }
}