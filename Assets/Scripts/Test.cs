using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        var dissolveEffect = GetComponent<SpineDissolveEffect>();
        if (dissolveEffect != null)
        {
            dissolveEffect.StartDissolve();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
