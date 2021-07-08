using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    //Reference to AudioManger
    public AudioManager audioManager;

    private Material firstMaterial;
    private Material secondMaterial;

    private Quaternion currentRotation;


    [HideInInspector]
    public bool revealed = false;
    private CardManager cardManager;
    private bool clicked = false;
    private int index;

    public void SetIndex(int id)
    {
        index = id;
    }

    public int GetIndex()
    {
        return index;
    }

    //nothing is revealed or clicked when starting
    void Start()
    {
        revealed = false;
        clicked = false;
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        currentRotation = gameObject.transform.rotation;
    }


    private void OnMouseDown()
    {
        if(clicked == false)
        {
            //card is flipping
            cardManager.currentCardState = CardManager.CardState.CardRotating;
            StartCoroutine(LoopRotation(45, false));
            ///card is clicked
            clicked = true;

        }

        StartCoroutine(LoopRotation(45, false));
        clicked = true;
    }


    public void FlipBack()
    {
        //if card is active then flip back to backside
        if(gameObject.activeSelf)
        {
            cardManager.currentCardState = CardManager.CardState.CardRotating;
            revealed = false;
            StartCoroutine(LoopRotation(45, true));
        }
    }

    //Rotate Cards
    IEnumerator LoopRotation(float angle, bool firstMat)
    {
        var rot = 0f;
        const float dir = 1f;
        const float rotSpeed = 180.0f;
        const float rotSpeed1 = 90.0f;
        var startAngle = angle;
        var assigned = false;

        if(firstMat)
        {
            while(rot < angle)
            {
                var step = Time.deltaTime * rotSpeed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                if (rot >= (startAngle - 2) && assigned == false)
                {
                    ApplyFirstMaterial();
                    assigned = true;
                }

                rot += (1 * step * dir);
                yield return null;
            }
        }
        else
        {
            while(angle > 0)
            {
                float step = Time.deltaTime * rotSpeed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0, 2, 0) * step * dir);
                angle -= (1 * step * dir);
                yield return null;
            }
        }

        gameObject.GetComponent<Transform>().rotation = currentRotation;

        //if face of card is showing, apply image and fire CheckImage function
        if(!firstMat)
        {
            revealed = true;
            ApplySecondMaterial();
            cardManager.CheckImage();
        }
        else //
        {
            cardManager.cardRevealedNumber = CardManager.RevealedState.NoneRevealed;
            cardManager.currentCardState = CardManager.CardState.CanRotate;
        }

        clicked = false;
    }

    
    public void setFirstMaterial(Material mat, string texturePath)
    {
        firstMaterial = mat;
        firstMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void setSecondMaterial(Material mat, string texturePath)
    {
        secondMaterial = mat;
        secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }

    public void ApplyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = firstMaterial;
    }

    public void ApplySecondMaterial()
    {
        gameObject.GetComponent<Renderer>().material = secondMaterial;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCorutine());
    }

    private IEnumerator DeactivateCorutine()
    {
        revealed = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
