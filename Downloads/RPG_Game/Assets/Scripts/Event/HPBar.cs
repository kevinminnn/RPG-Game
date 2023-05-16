using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject HP;

    public void SetHP(float hpNormalized)
    {
        HP.transform.localScale = new Vector3(hpNormalized, 1f);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currentHP = HP.transform.localScale.x;
        float changeAMT = currentHP - newHP;

        while (currentHP - newHP > Mathf.Epsilon)
        {
            currentHP -= changeAMT * Time.deltaTime;
            HP.transform.localScale = new Vector3(currentHP, 1f);

            yield return null;
        }
        HP.transform.localScale = new Vector3(newHP, 1f);
    }
}
