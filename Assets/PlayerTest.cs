using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerTest : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Animate(transform.position, Vector3.zero));
    }
    
    private IEnumerator Animate(Vector3 start, Vector3 finish)
    {
        float journeyTime = 5.0f;
        float startTime = Time.time;
        float fracComplete = (Time.time - startTime) / journeyTime;
        while (fracComplete <= 1)
        {
            fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Lerp(start, finish, fracComplete);
            yield return null;
        }
    }
}