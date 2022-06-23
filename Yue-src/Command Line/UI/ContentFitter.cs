using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFitter : MonoBehaviour
{
    [SerializeField] private GameObject content;

    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 400f;
    [SerializeField] private bool isSmoothResize = false;
    [SerializeField] private float resizeTime = 1f;

    private float previousYMin = 0f;

    private Vector2 height;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, 0f);
        height = new Vector2(0f, rectTransform.rect.height);
    }

    private void Start()
    {
        Resize();
    }

    public void Resize()
    {
        float yMin = 0f;

        foreach (RectTransform r in content.transform)
            yMin -= r.rect.height;

        if (!isSmoothResize)
        {
            rectTransform.offsetMin = height;
            float yMinClamp = Mathf.Clamp(-yMin, 0f, maxHeight);
            rectTransform.offsetMax = new Vector2(0f, yMinClamp);
            previousYMin = yMinClamp;
        }
        else
        {
            if (resize != null)
                StopCoroutine(resize);

            resize = StartCoroutine(ResizeSmooth(-yMin));
        }
    }

    private Coroutine resize = null;
    private IEnumerator ResizeSmooth(float yMin)
    {
        float time = 0f;
        rectTransform.offsetMin = height;
        while (time < resizeTime)
        {
            float lerp = Mathf.Lerp(previousYMin, yMin, time / resizeTime);
            float yMinClamp = Mathf.Clamp(lerp, minHeight, maxHeight);
            rectTransform.offsetMax = new Vector2(0f, yMinClamp);
            previousYMin = yMinClamp;
            time += Time.deltaTime;
            //Debug.Log(time);
            yield return null;
        }

        float clamp = Mathf.Clamp(yMin, 0f, maxHeight);
        rectTransform.offsetMax = new Vector2(0f, clamp);
        previousYMin = yMin;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        //Resize();
    }
#endif
}
