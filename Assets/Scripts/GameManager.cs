using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerMovementStats playerMovementStats;

    private int currentLevel;
    [SerializeField] private Diamond diamond;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource getDiamondSound;
    [SerializeField] private bool HasDoubleJump { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentLevel = playerMovementStats.currentLevel;

        if(currentLevel ==1)
        {
            playerMovementStats.numberOfJumpAllowed = 1;
        }
    }

    public void OnDiamondCollected()
    {
        if(currentLevel ==1)
        {
            getDiamondSound.Play();
            player.GetComponent<PlayerMovement>().EnableDoubleJump();
            HasDoubleJump = true;
        }

    }
    public void OnPlayerDeath()
    {
        if(currentLevel ==1)
        {
            HasDoubleJump = false;
            player.GetComponent<PlayerMovement>().DisableDoubleJump();
        }
    }

    public void OnPlayerRespawned()
    {
        if (diamond != null && currentLevel ==1)
        {
            diamond.ResetDiamond();
        }
    }
}
