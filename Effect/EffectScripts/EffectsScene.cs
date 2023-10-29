using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsScene : MonoBehaviour {

    public Transform[] m_effects;

    public static GameObject[] m_destroyObjects = new GameObject[30];
    public static int inputLocation;
    public Text m_effectName;
    int index = 0;

    void Awake()
    {
        inputLocation = 0;
        m_effectName.text = m_effects[index].name.ToString();
        MakeObject();

    }

	void Update ()
    {
        InputKey();
	}

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (index <= 0)
                index = m_effects.Length - 1;
            else
                index--;

            MakeObject();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (index >= m_effects.Length-1)
                index = 0;
            else
                index++;

            MakeObject();
        }

        if (Input.GetKeyDown(KeyCode.C))
            MakeObject();
    }

    void MakeObject()
    {
        DestroyGameObject();
        GameObject gm = Instantiate(m_effects[index],
            m_effects[index].transform.position,
            m_effects[index].transform.rotation).gameObject;
        m_effectName.text = (index+1) +" : "+m_effects[index].name.ToString();
        gm.transform.parent = this.transform;
        m_destroyObjects[inputLocation] = gm;

        inputLocation++;
    }

    void DestroyGameObject()
    {
        for(int i = 0; i < inputLocation; i++)
        {
            Destroy(m_destroyObjects[i]);
        }
        inputLocation = 0;
    }
}
