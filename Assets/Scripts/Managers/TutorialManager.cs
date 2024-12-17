using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TutorialManager : Singleton<TutorialManager>
{
    [System.Serializable]
    public class TutorialStep
    {
        public int stepId;
        public string description;
        public string buttonName;
        public bool highlight;
        public float delayBeforeNextStep;
    }

    private List<TutorialStep> steps = new List<TutorialStep>();
    private int currentStepIndex = 0;

    public Text tutorialText; // 提示文本
    public GameObject highlightPrefab; // 高亮框
    public GameObject arrowPrefab; // 动态箭头
    public GameObject dimOverlayPrefab; // 遮罩覆盖

    private GameObject currentHighlight;
    private GameObject currentArrow;
    private GameObject dimOverlay;

    private const string SaveKey = "TutorialStepIndex";

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        LoadSteps();
        LoadProgress();
        StartCoroutine(StartTutorial());
    }

    void LoadSteps()
    {
        // 从 Resources 加载文本资源
        TextAsset textAsset = Resources.Load<TextAsset>("TutorialSteps"); // 资源路径不需要文件后缀
        if (textAsset != null)
        {
            string jsonContent = textAsset.text; // 读取文本内容

            // 使用 JsonHelper 来解析 JSON 数组
            TutorialStep[] stepArray = JsonHelper.FromJson<TutorialStep>(jsonContent);

            if (stepArray != null)
            {
                steps = new List<TutorialStep>(stepArray);
                Debug.Log("Steps loaded successfully from Resources!");
            }
            else
            {
                steps = new List<TutorialStep>();
                Debug.LogError("Failed to parse TutorialSteps JSON content!");
            }
        }
        else
        {
            Debug.LogError("Tutorial steps file not found in Resources!");
        }
    }


    void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            currentStepIndex = PlayerPrefs.GetInt(SaveKey);
        }
        else
        {
            currentStepIndex = 0;
        }
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt(SaveKey, currentStepIndex);
        PlayerPrefs.Save();
    }

    IEnumerator StartTutorial()
    {
        if (steps != null && steps.Count > 0)
        {
            dimOverlay = Instantiate(dimOverlayPrefab);

            while (currentStepIndex < steps.Count)
            {
                yield return StartCoroutine(ShowStep(steps[currentStepIndex]));

                currentStepIndex++;
                SaveProgress();
            }

            EndTutorial();
        }
        else
        {
            Debug.LogError("No tutorial steps found!");
        }
    }

    IEnumerator ShowStep(TutorialStep step)
    {
        tutorialText.text = step.description;

        GameObject targetButton = GameObject.Find(step.buttonName);
        if (targetButton == null)
        {
            Debug.LogError($"Button {step.buttonName} not found!");
            yield break;
        }

        if (currentHighlight != null) Destroy(currentHighlight);
        if (currentArrow != null) Destroy(currentArrow);

        if (step.highlight)
        {
            yield return StartCoroutine(HighlightButton(targetButton));
            ShowArrow(targetButton);
        }

        yield return new WaitUntil(() => IsInputOverButton(targetButton));

        if (step.delayBeforeNextStep > 0)
        {
            yield return new WaitForSeconds(step.delayBeforeNextStep);
        }
    }

    IEnumerator HighlightButton(GameObject button)
    {
        currentHighlight = Instantiate(highlightPrefab, button.transform.position, Quaternion.identity);
        currentHighlight.transform.SetParent(button.transform, false);

        CanvasGroup canvasGroup = currentHighlight.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = currentHighlight.AddComponent<CanvasGroup>();
        }

        float duration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    void ShowArrow(GameObject button)
    {
        currentArrow = Instantiate(arrowPrefab, button.transform.position, Quaternion.identity);
        currentArrow.transform.SetParent(button.transform, false);

        currentArrow.transform.localPosition += new Vector3(0, 50, 0);

        Animator arrowAnimator = currentArrow.GetComponent<Animator>();
        if (arrowAnimator != null)
        {
            arrowAnimator.SetTrigger("Bounce");
        }
    }

    bool IsInputOverButton(GameObject button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);

        bool isOverWithMouse = rectTransform.rect.Contains(localMousePosition);
        bool isTouched = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
                         rectTransform.rect.Contains(rectTransform.InverseTransformPoint(Input.GetTouch(0).position));

        return isOverWithMouse || isTouched;
    }

    void EndTutorial()
    {
        tutorialText.text = "教程完成！";
        Debug.Log("Tutorial Finished!");

        if (dimOverlay != null) Destroy(dimOverlay);
        if (currentHighlight != null) Destroy(currentHighlight);
        if (currentArrow != null) Destroy(currentArrow);

        PlayerPrefs.DeleteKey(SaveKey);
    }
}
