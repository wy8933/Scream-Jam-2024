using UnityEngine;

public class CatTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlayMusic(SoundManager.instance.chaseMusic);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlayMusic(SoundManager.instance.screamJamMusic);
        }
    }
}
