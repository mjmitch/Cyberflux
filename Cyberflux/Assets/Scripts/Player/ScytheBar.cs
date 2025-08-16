using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class ScytheBar : MonoBehaviour
{

    [SerializeField] Slider slider;

    [SerializeField] ScytheCombat scytheScript;

    private void Start()
    {
        scytheScript.OnSlash += ResetBar;
    }


    private void Update()
    {
        float timeSinceLastSlash = Time.time - (scytheScript.nextSlashTime - scytheScript.slashRechargeTime);
        slider.value = Mathf.Clamp01(timeSinceLastSlash / scytheScript.slashRechargeTime);
    }

    void ResetBar()
    {
        slider.value = 0f;
    }
}
