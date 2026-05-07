using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MiniGamePhone : MonoBehaviour, IRegisterNumber
{
    public UIController textoController;
    public UIController ImageFragmento;
    public TMP_Text tmpText1;
    public TMP_Text tmpText2;
    public TMP_Text tmpText3;
    public TMP_Text tmpText4;

    [SerializeField] int RightDate = 1986;

    private List <int> numbers = new List<int>();

    public List<TMP_Text> textosNumeros = new List<TMP_Text>();

    private int countRegisterNumbers = 0;

    public void Start()
    {
        textosNumeros.Add(tmpText1);
        textosNumeros.Add(tmpText2);
        textosNumeros.Add(tmpText3);
        textosNumeros.Add(tmpText4);
    }
    public void RegisterNumber(int number)
    {
        countRegisterNumbers++;
        if(countRegisterNumbers <= 4)
        {
            numbers.Add(number);
            textoController.ChangeText(textosNumeros[countRegisterNumbers-1], number);

            if (countRegisterNumbers == 4)
            {
                VerifyDate();
            }
        }

    }

    public void VerifyDate()
    {
        int RegisterDate = 0;

        foreach (int n in numbers)
        {
            RegisterDate = RegisterDate * 10 + n;
        }

        if(RegisterDate == RightDate)
        {
            UnlockFragment();
        }
        else
        {
            foreach(var item in textosNumeros)
            {
                textoController.ChangeText(item, -1);
                countRegisterNumbers--;
            }

            countRegisterNumbers = 0;
            numbers.Clear();

        }
    }

    public void UnlockFragment()
    {
        Debug.Log("Interfaz fragmento");
        ImageFragmento.ShowImage();
    }
}
