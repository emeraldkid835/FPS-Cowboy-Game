using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    private Animator doorAnim;
    private bool doorOpen = false;

    [Header("Animation Names")]
    [SerializeField] private string openAnimationName = "DoorOpen";
    [SerializeField] private string closeAnimationName = "DoorClose";

    [SerializeField] private int timeToShowUI = 1;
    [SerializeField] private GameObject showDoorLockedUI = null;

    [SerializeField] private KeyInventory _keyInventory = null;

    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    private IEnumerator PauseInteraction()
    {
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void PlayAnimation()
    {
        if (_keyInventory.hasRedKey)
        {
            if (!doorOpen && !pauseInteraction)
            {
                doorAnim.Play(openAnimationName, 0, 0.0f);
                doorOpen = true;
                StartCoroutine(PauseInteraction());
            }
            else if (doorOpen && !pauseInteraction)
            {
                doorAnim.Play(closeAnimationName, 0, 0.0f);
                doorOpen = false;
                StartCoroutine(PauseInteraction());
            }
        }
        else
        {
            StartCoroutine(ShowDoorLocked());
        }
    }

    IEnumerator ShowDoorLocked()
    {
        showDoorLockedUI.SetActive(true);
        yield return new WaitForSeconds(timeToShowUI);
        showDoorLockedUI.SetActive(false);
    }
}
