using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    RawImage raw;
    void Start()
    {
        StartCoroutine(TomcatLoad());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator TomcatLoad()
    {
        WWW wWW = new WWW("http://192.168.0.34:8087/MyAssetBundle/cube");
        yield return wWW;
        if (wWW.isDone)
        {
            if (string.IsNullOrEmpty(wWW.error))
            {
                GameObject game = wWW.assetBundle.LoadAsset<GameObject>("Cube");
                Instantiate(game);
            }
            else
            {
                Debug.LogError(wWW.error);
            }
        }
    }
}
