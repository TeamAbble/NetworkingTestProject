using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform uiRoot;
    public CanvasGroup loadScreenGroup;
    public float fakeLoadTime;
    public TMP_Text loadScreenTips;
    public Image loadScreenImage;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            print("initialising UI Manager");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        loadScreenGroup.interactable = false;
        loadScreenGroup.blocksRaycasts = false;
        loadScreenGroup.ignoreParentGroups = true;
        loadScreenGroup.alpha = 0;
    }
    public List<UIModule> modules;

    public void AddModule(GameObject modulePrefab)
    {
        //Check if the prefab has a UIModule on the root.
        if(modulePrefab.TryGetComponent(out UIModule mod))
        {
            GameObject go = Instantiate(modulePrefab, uiRoot);
            go.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            mod = go.GetComponent<UIModule>();
            modules.Add(mod);
        }
        else
        {
            Debug.LogError($"Failed to create prefab of {modulePrefab.name}! This object does NOT have a valid UI Module!");
        }
    }

    public void ClearModule(UIModule module)
    {
        if (modules.Contains(module))
        {
            modules.Remove(module);
            Destroy(module.gameObject);
        }
    }

    public void SendLoadScreen(float loadTime, System.Action loadAction)
    {
        StartCoroutine(LoadingScreen(loadTime, loadAction));
    }
    public IEnumerator LoadingScreen(float loadTime, System.Action loadAction)
    {
        loadScreenGroup.blocksRaycasts = true;
        float speed = 1 / loadTime;
        float t = 0;
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            loadScreenGroup.alpha = t;
            yield return new WaitForEndOfFrame();
        }
        loadAction?.Invoke();
        yield return new WaitForSecondsRealtime(fakeLoadTime);
        while (t > 0)
        {
            t -= speed * Time.deltaTime;
            loadScreenGroup.alpha = t;
            yield return new WaitForEndOfFrame();
        }
        loadScreenGroup.blocksRaycasts = false;
    }
}
