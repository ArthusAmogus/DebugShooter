using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ObjectAutoScaleEffect : MonoBehaviour
{
    [SerializeField] float ScaleRate = 0.1f;
    [SerializeField] Vector3 og_size;
    [SerializeField] UnityEvent onEffectComplete;
    [SerializeField] AutoScaleType autoScaleType;

    public enum AutoScaleType
    {
        Shrink,
        Grow
    }


    private void OnEnable()
    {
        switch (autoScaleType)
        {
            case AutoScaleType.Shrink:
                transform.localScale = og_size;
                transform.LeanScale(Vector3.zero, ScaleRate).setOnComplete(() =>
                {
                    onEffectComplete?.Invoke();
                    gameObject.SetActive(false);
                });
                break;
            case AutoScaleType.Grow:
                transform.localScale = Vector3.zero;
                transform.LeanScale(og_size, ScaleRate).setOnComplete(() =>
                {
                    onEffectComplete?.Invoke();
                    gameObject.SetActive(false);
                });
                break;
        }
    }
}
