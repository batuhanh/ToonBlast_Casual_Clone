using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GoalObject : MonoBehaviour
{

    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject markObj;
    [SerializeField] private ParticleSystem myEffect;
    [SerializeField] private int count;

    public int Count
    {
        get { return count; }
        set
        {
            if (value < count)
            {
                PlayEffect();
            }
            count = value;
            if (count <= 0)
            {
                count = 0;
                countText.gameObject.SetActive(false);
                markObj.gameObject.SetActive(true);

            }
            else
            {
                countText.text = count.ToString();
            }

        }
    }
    private void PlayEffect()
    {
        myEffect.Stop();
        myEffect.Play();
    }

}
