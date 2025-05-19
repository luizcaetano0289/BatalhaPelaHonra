using UnityEngine;
using UnityEngine.UI;

public class SwingTimerUI : MonoBehaviour
{
    [SerializeField] private Slider swingBar;

    private float swingDuration;
    private float swingTimer;
    private bool isSwinging;

    public void StartSwing(float duration)
    {
        swingDuration = duration;
        swingTimer = 0f;
        isSwinging = true;
        swingBar.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isSwinging) return;

        swingTimer += Time.deltaTime;
        float percent = swingTimer / swingDuration;
        swingBar.value = percent;

        if (percent >= 1f)
        {
            isSwinging = false;
            swingBar.gameObject.SetActive(false);
        }
    }
}
