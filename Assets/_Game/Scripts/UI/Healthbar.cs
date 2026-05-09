using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    public Slider Slider;
    public Gradient gradient;
    public Image fill;

    public void SetmaxHealth(float Health)
    {
        Slider.maxValue = Health;
        Slider.value = Health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        Slider.value = health;
        fill.color = gradient.Evaluate(Slider.normalizedValue);
    }


}
