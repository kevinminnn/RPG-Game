using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Action : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] HPBar HPBar;

    Battle _unit;

    public void SetData(Battle unit)
    {
        _unit = unit;

        NameText.text = unit.Base.Name;
        LevelText.text = "Lvl: " + unit.Level.ToString();
        HPBar.SetHP((float)unit.HP / unit.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return HPBar.SetHPSmooth((float)_unit.HP / _unit.MaxHP);
    }
}
