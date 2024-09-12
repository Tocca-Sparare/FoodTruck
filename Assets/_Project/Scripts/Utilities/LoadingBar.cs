using UnityEngine;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] GameObject bar;

    Vector3 defaultScale;

    void Awake()
    {
        defaultScale = bar.transform.localScale;
    }

    public void Updatebar(float percentage)
    {
        if (percentage <= 0)
        {
            Hide();
            return;
        }

        Show();

        var inversePercentage = percentage * 0.01f;
        bar.transform.localScale = new Vector3(defaultScale.x, inversePercentage * defaultScale.y, defaultScale.z);
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
