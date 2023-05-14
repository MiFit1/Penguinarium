using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class miningСontroller : MonoBehaviour
{
    [SerializeField] private miningCollision handCollision;
    [SerializeField] private GameObject hand;
    [SerializeField] private float animationSpeed = 1f;


    [SerializeField] private AnimationCurve speedUp; //кривая поднятия
    [SerializeField] private AnimationCurve speedDown;// кривая опускания
    private AudioSource playerAudioSource;
    private bool HitInProcess = false;

    private bool minigButtonIsPressed = false;
    private bool animationIsStarted = false;

    [SerializeField] private GameObject EquipItem;//удалить

    public bool GetHitProcess()
    {
        return HitInProcess;
    }

    private void Start()
    {
        PlayerController.OnLeftCliked += MiningButtonIsPressed;
        PlayerController.OnLeftClikedUp += MiningButtonUp;
        playerAudioSource = GetComponent<AudioSource>();
        EquipItem.SetActive(false);//удалить
    }

    private void MiningButtonUp()
    {
        minigButtonIsPressed = false;
        EquipItem.SetActive(false);//удалить
    }

    private void MiningButtonIsPressed()
    {
        minigButtonIsPressed = true;
        EquipItem.SetActive(true);//удалить
        StartCoroutine(KickProcess());
    }


    private ExtractedObject chekHandCollision()
    {
        for (int i = 0; i < handCollision.collisionObjects.Count; i++)
        {
            if (handCollision.collisionObjects[i]?.GetComponent<ExtractedObject>())
            {
                ExtractedObject extObject = handCollision.collisionObjects[i].GetComponent<ExtractedObject>();
                if (extObject.GetMouseOnObject())
                    return extObject;
            }
        }
        return null;
    }
    private IEnumerator KickAnimation()
    {
        HitInProcess = true;
        animationIsStarted = true;
/*        float currentHandAngle = hand.transform.eulerAngles.z;
        Debug.Log(currentHandAngle);*/
        float time = 0;

        Quaternion StartRotation = hand.transform.localRotation;
        Quaternion Down = Quaternion.Euler(hand.transform.localRotation.x, hand.transform.localRotation.y, 320f);
        Quaternion Up = Quaternion.Euler(hand.transform.localRotation.x, hand.transform.localRotation.y, 40f);


        while (time <= 1)
        {
            hand.transform.localRotation = Quaternion.Lerp(Up, Down, speedUp.Evaluate(time));
            time += (Time.deltaTime * animationSpeed);
            yield return null;
        }
        time = 0;
        while (time <= 1)
        {
            hand.transform.localRotation = Quaternion.Lerp(Down, Up, speedDown.Evaluate(time));
            time += (Time.deltaTime * animationSpeed);
            yield return null;
        }
        animationIsStarted = false;
        HitInProcess = false;
    }
    private IEnumerator KickProcess()
    {
        while (minigButtonIsPressed)
        {
            if (!animationIsStarted)
            {
                yield return StartCoroutine(KickAnimation());

                ExtractedObject extractedObject = chekHandCollision();
                if (extractedObject != null)
                {
                    //добавить, чтобы урон брался с экипированного предмета
                    extractedObject.dealDamage(20f);
                    extractedObject.StartShake();
                    extractedObject.SpawnMiningParticles();
                    playerAudioSource.clip = extractedObject.soundOfMining;
                    playerAudioSource.Play();
                }
            }
            yield return null;
        }
    }


    /*    private IEnumerator HitProcess(float AnimationTime)
    {
        HitInProcess = true;
        if(chekHandCollision())
        {
            Debug.Log("Удар");
        }
        yield return null;
        HitInProcess = false;
    }*/
}//
